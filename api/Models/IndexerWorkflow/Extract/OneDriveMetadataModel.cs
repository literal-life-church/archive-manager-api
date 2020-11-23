using static LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract.OneDriveMetadataModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract
{
    public class OneDriveMetadataModel : ExtractModel<ErrorType>
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
