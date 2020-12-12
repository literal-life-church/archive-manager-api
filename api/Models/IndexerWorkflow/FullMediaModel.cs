using LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract;

namespace LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow
{
    public class FullMediaModel : IIndexerWorkflowModel
    {
        public DateModel Date { get; set; }

        public NameModel Name { get; set; }

        public OneDriveMetadataModel OneDriveMetadata { get; set; }

        public SpeakerModel Speakers { get; set; }

        public MediaTypeModel Type { get; set; }
    }
}
