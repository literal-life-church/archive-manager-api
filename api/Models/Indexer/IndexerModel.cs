namespace LiteralLifeChurch.ArchiveManagerApi.Models.Indexer
{
    public abstract class IndexerModel<Error> : IModel
    {
        public bool AssumedValue { get; set; }

        public string Id { get; set; }

        public Error ParseError { get; set; }
    }
}
