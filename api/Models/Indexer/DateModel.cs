using System;
using static LiteralLifeChurch.ArchiveManagerApi.Models.Indexer.DateModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Models.Indexer
{
    public class DateModel : IndexerModel<ErrorType>
    {
        public DateStringModel DateString { get; set; }

        public ModifierModel Modifier { get; set; }

        public DateTime Stamp { get; set; }

        public class DateStringModel
        {
            public string Given { get; set; }

            public string Normalized { get; set; }
        }

        public enum ErrorType
        {
            InvalidDateFormat,
            InvalidModifierCode,
            ModifierCodeLengthInvalid,
            None,
            TimeOutOfBounds,
            UnreadableDate
        }

        public class ModifierModel
        {
            public ModifierType Type { get; set; }

            public string Symbol { get; set; }

            public enum ModifierType
            {
                Afternoon,
                Breakfast,
                Evening,
                Miscellaneous,
                Morning,
                None,
                Sunrise
            }
        }
    }
}
