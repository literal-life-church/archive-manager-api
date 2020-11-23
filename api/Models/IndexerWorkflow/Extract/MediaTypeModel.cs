using static LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract.MediaTypeModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract
{
    public class MediaTypeModel : ExtractModel<ErrorType>
    {
        public string Name { get; set; }

        public enum ErrorType
        {
            None
        }
    }
}
