using static LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract.NameModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract
{
    public class NameModel : ExtractModel<ErrorType>
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
