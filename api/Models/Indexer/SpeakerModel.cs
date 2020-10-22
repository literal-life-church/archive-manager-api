using System.Collections.Generic;
using static LiteralLifeChurch.ArchiveManagerApi.Models.Indexer.SpeakerModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Models.Indexer
{
    public class SpeakerModel : IndexerModel<ErrorType>
    {
        public List<SpeakerNameModel> Names { get; set; }

        public class SpeakerNameModel
        {
            public string Given { get; set; }

            public string Id { get; set; }

            public string Normalized { get; set; }
        }

        public enum ErrorType
        {
            EmptyName,
            EmptyNormalizedName,
            NoNames,
            None
        }
    }
}
