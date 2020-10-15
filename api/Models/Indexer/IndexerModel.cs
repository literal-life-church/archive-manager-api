namespace LiteralLifeChurch.ArchiveManagerApi.Models.Indexer
{
    public abstract class IndexerModel<Error> : IModel
    {
        public Error ParseError { get; set; }
    }
}
