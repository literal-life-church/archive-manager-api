using LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract;

namespace LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Correlate
{
    public class CorrelationModel : IIndexerWorkflowModel
    {
        public DateModel Date { get; set; }

        public NameModel Name { get; set; }
    }
}
