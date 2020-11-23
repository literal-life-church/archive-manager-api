namespace LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract
{
    public abstract class ExtractModel<Error> : IIndexerWorkflowModel
    {
        public bool AssumedValue { get; set; }

        public string Id { get; set; }

        public Error ParseError { get; set; }
    }
}
