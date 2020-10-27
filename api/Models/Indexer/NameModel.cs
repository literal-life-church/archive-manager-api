using static LiteralLifeChurch.ArchiveManagerApi.Models.Indexer.NameModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Models.Indexer
{
    public class NameModel : IndexerModel<ErrorType>
    {
        public string Given { get; set; }

        public string Normalized { get; set; }

        public enum ErrorType
        {
            NoName,
            None
        }
    }
}
