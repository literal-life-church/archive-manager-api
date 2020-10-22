using static LiteralLifeChurch.ArchiveManagerApi.Models.Indexer.MediaTypeModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Models.Indexer
{
    public class MediaTypeModel : IndexerModel<ErrorType>
    {
        public string Name { get; set; }

        public enum ErrorType
        {
            None
        }
    }
}
