using static LiteralLifeChurch.ArchiveManagerApi.Models.Indexer.OneDriveMetadataModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Models.Indexer
{
    public class OneDriveMetadataModel : IndexerModel<ErrorType>
    {
        public string DriveId { get; set; }

        public long Duration { get; set; }

        public long FileSize { get; set; }

        public string FullPath { get; set; }

        public string MimeType { get; set; }

        public string Name { get; set; }

        public string NameWithoutExtension { get; set; }

        public MediaType Type { get; set; }

        public string WebUrl { get; set; }

        public enum ErrorType
        {
            None
        }

        public enum MediaType
        {
            Audio,
            Other,
            Video
        }
    }
}
