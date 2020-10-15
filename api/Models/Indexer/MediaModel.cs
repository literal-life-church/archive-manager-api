using LiteralLifeChurch.ArchiveManagerApi.Services.Indexer;

namespace LiteralLifeChurch.ArchiveManagerApi.Models.Indexer
{
    public class MediaModel : IIndexerService
    {
        public DateModel Date { get; set; }
    }
}
