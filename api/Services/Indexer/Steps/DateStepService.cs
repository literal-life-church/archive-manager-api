using LiteralLifeChurch.ArchiveManagerApi.Exceptions.Indexer.Date;
using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Models.Indexer;
using Microsoft.Graph;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using static LiteralLifeChurch.ArchiveManagerApi.Models.Indexer.DateModel;
using static LiteralLifeChurch.ArchiveManagerApi.Models.Indexer.DateModel.ModifierModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Services.Indexer.Steps
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

    public class DateStepService : IndexerStepService<ErrorType, IndexerDateException, DateModel>
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

        public DateStepService(ConfigurationModel config) : base(config)
        {
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
                Type = ModifierType.None
            };
        }

        public override DateModel Transform(DriveItem item, string split, int index)
        {
            Error = ErrorType.None;

            string dateOnly = item.Name.Split(split).ElementAtOrDefault(index).Trim();
            DateStringModel dateString = ExtractDateString(dateOnly);
            ModifierModel modifier = ExtractModifier(dateOnly);
            DateTime date = ExtractDateTime(dateString.Normalized, modifier.Type);

            return new DateModel
            {
                AssumedValue = Error != ErrorType.None,
                DateString = dateString,
                Id = GenerateId(dateString.Normalized),
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

        private DateTime ExtractDateTime(string dateString, ModifierType modifierType)
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
                case ModifierType.Afternoon:
                    parsedDate = parsedDate.AddHours(modifierHoursAfternoon);
                    break;

                case ModifierType.Breakfast:
                    parsedDate = parsedDate.AddHours(modifierHoursBreakfast);
                    break;

                case ModifierType.Evening:
                    parsedDate = parsedDate.AddHours(modifierHoursEvening);
                    break;

                case ModifierType.Morning:
                    parsedDate = parsedDate.AddHours(modifierHoursMorning);
                    break;

                case ModifierType.Sunrise:
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
                ModifierType type;

                switch (symbol)
                {
                    case "A":
                        type = ModifierType.Afternoon;
                        break;

                    case "B":
                        type = ModifierType.Breakfast;
                        break;

                    case "E":
                    case "PM":
                        type = ModifierType.Evening;
                        break;

                    case "AM":
                    case "M":
                        type = ModifierType.Morning;
                        break;

                    case "S":
                        type = ModifierType.Sunrise;
                        break;

                    case "X":
                        type = ModifierType.Miscellaneous;
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
