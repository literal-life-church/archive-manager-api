using LiteralLifeChurch.ArchiveManagerApi.Exceptions.IndexerWorkflow.Extract.Date;
using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow;
using LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract;
using Microsoft.Graph;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using static LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract.DateModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Services.IndexerWorkflow.Steps.Extract
{
    // Extract something like: 200901AM - John Smith - Overcoming the World.mp4
    //                         ^^^^^^^^
    // or
    // 20200901AM - John Smith - Overcoming the World.mp4
    // ^^^^^^^^^^
    // 
    // or
    // 20200901M - John Smith - Overcoming the World.mp4
    // ^^^^^^^^^
    // 
    // or
    // 200901M - John Smith - Overcoming the World.mp4
    // ^^^^^^^

    public class DateExtractStep : IndexerWorkflowStep<DriveItem, DateModel, DateException>
    {
        private const string DateFormat = "yyMMdd";
        private readonly DateTime ValidDateLowerBound;
        private readonly DateTime ValidDateUpperBound;

        private readonly DateStringModel DefaultDateStringModel;
        private readonly DateTime DefaultDateTime;
        private readonly ModifierModel DefaultModifierModel;

        private const int modifierHoursAfternoon = 12;
        private const int modifierHoursBreakfast = 8;
        private const int modifierHoursEvening = 17;
        private const int modifierHoursMorning = 10;
        private const int modifierHoursSunrise = 6;

        private ErrorType Error = ErrorType.None;
        private readonly int Index;
        private readonly string Split;

        public DateExtractStep(ConfigurationModel config, string split, int index) : base(config)
        {
            Index = index;
            Split = split;

            string dateString = DateTime.Now.ToString(DateFormat, CultureInfo.InvariantCulture);

            DefaultDateTime = DateTime.Now;
            ValidDateLowerBound = new DateTime(config.ValidYearLowerBound, 1, 1);
            ValidDateUpperBound = DateTime.Today.AddDays(1);

            DefaultDateStringModel = new DateStringModel
            {
                Given = dateString,
                Normalized = dateString
            };

            DefaultModifierModel = new ModifierModel
            {
                Symbol = "",
                Type = DateModifierType.None
            };
        }

        public override DateModel Run(DriveItem item)
        {
            Error = ErrorType.None;

            string dateOnly = item.Name.Split(Split).ElementAtOrDefault(Index).Trim();
            DateStringModel dateString = ExtractDateString(dateOnly);
            ModifierModel modifier = ExtractModifier(dateOnly);
            DateTime date = ExtractDateTime(dateString.Normalized, modifier.Type);

            string fullDate;

            if (modifier.Type == DateModifierType.None)
            {
                fullDate = dateString.Normalized;
            }
            else
            {
                fullDate = $"{dateString.Normalized}{modifier.Symbol}";
            }

            return new DateModel
            {
                AssumedValue = Error != ErrorType.None,
                DateString = dateString,
                Id = GenerateId(fullDate),
                Modifier = modifier,
                ParseError = Error,
                Stamp = date
            };
        }

        // region Helper Methods

        private DateStringModel ExtractDateString(string dateOnly)
        {
            string datePart = Regex.Replace(dateOnly, "[^0-9]*", "");

            if (datePart.Length == 6)
            {
                return new DateStringModel
                {
                    Given = datePart,
                    Normalized = datePart
                };
            }
            else if (datePart.Length == 8)
            {
                string normalized = datePart.Substring(2);

                return new DateStringModel
                {
                    Given = datePart,
                    Normalized = normalized
                };
            }

            Error = ErrorType.InvalidDateFormat;

            return ErrorHandler
                .SetExceptionType(new DateStringFormatException())
                .SetDeveloperMessage($"The given date string '{dateOnly}' does not fit the 6-digit yyMMdd or 8-digit yyyyMMdd date formats.")
                .SetPublicMessage($"The date '{dateOnly}' is unreadable.")
                .ThrowIfAllowed()
                .OrDefault(DefaultDateStringModel);
        }

        private DateTime ExtractDateTime(string dateString, DateModifierType modifierType)
        {
            if (!DateTime.TryParseExact(dateString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                Error = ErrorType.UnreadableDate;

                return ErrorHandler
                    .SetExceptionType(new DateParseException())
                    .SetDeveloperMessage($"The given date string '{dateString}' does not resolve to a valid date.")
                    .SetPublicMessage($"The date '{dateString}' is invalid.")
                    .ThrowIfAllowed()
                    .OrDefault(DefaultDateTime);
            }

            switch (modifierType)
            {
                case DateModifierType.Afternoon:
                    parsedDate = parsedDate.AddHours(modifierHoursAfternoon);
                    break;

                case DateModifierType.Breakfast:
                    parsedDate = parsedDate.AddHours(modifierHoursBreakfast);
                    break;

                case DateModifierType.Evening:
                    parsedDate = parsedDate.AddHours(modifierHoursEvening);
                    break;

                case DateModifierType.Morning:
                    parsedDate = parsedDate.AddHours(modifierHoursMorning);
                    break;

                case DateModifierType.Sunrise:
                    parsedDate = parsedDate.AddHours(modifierHoursSunrise);
                    break;
            }

            if (!(ValidDateLowerBound <= parsedDate && parsedDate <= ValidDateUpperBound))
            {
                Error = ErrorType.TimeOutOfBounds;

                return ErrorHandler
                    .SetExceptionType(new DateParseException())
                    .SetDeveloperMessage($"The given date string '{dateString}' is out of bounds. Valid dates and times are between Jan 1st, {Config.ValidYearLowerBound} and tomorrow at midnight, local time.")
                    .SetPublicMessage($"The date '{dateString}' is out of bounds.")
                    .ThrowIfAllowed()
                    .OrDefault(DefaultDateTime);
            }

            return parsedDate;
        }

        private ModifierModel ExtractModifier(string dateOnly)
        {
            string modifierPart = Regex.Replace(dateOnly, "[^A-Za-z]*", "");

            if (modifierPart.Length == 0)
            {
                return DefaultModifierModel;
            }
            else if (modifierPart.Length == 1 || modifierPart.Length == 2)
            {
                string symbol = modifierPart.ToUpperInvariant();
                DateModifierType type;

                switch (symbol)
                {
                    case "A":
                        type = DateModifierType.Afternoon;
                        break;

                    case "B":
                        type = DateModifierType.Breakfast;
                        break;

                    case "E":
                    case "PM":
                        type = DateModifierType.Evening;
                        break;

                    case "AM":
                    case "M":
                        type = DateModifierType.Morning;
                        break;

                    case "S":
                        type = DateModifierType.Sunrise;
                        break;

                    case "X":
                        type = DateModifierType.Miscellaneous;
                        break;

                    default:
                        Error = ErrorType.InvalidModifierCode;

                        return ErrorHandler
                            .SetExceptionType(new ModifierCodeFormatException())
                            .SetDeveloperMessage($"The given modifier code '{symbol}' is not a supported code.")
                            .SetPublicMessage($"Cannot interpret the meaning of the '{symbol}' modifier code.")
                            .ThrowIfAllowed()
                            .OrDefault(DefaultModifierModel);
                }

                return new ModifierModel
                {
                    Symbol = symbol,
                    Type = type
                };
            }

            Error = ErrorType.ModifierCodeLengthInvalid;

            return ErrorHandler
                .SetExceptionType(new ModifierCodeFormatException())
                .SetDeveloperMessage($"The given modifier code '{modifierPart}' must be a supported 1 or 2 letter symbol.")
                .SetPublicMessage($"Cannot interpret the meaning of the '{modifierPart}' modifier code.")
                .ThrowIfAllowed()
                .OrDefault(DefaultModifierModel);
        }

        // endregion
    }
}
