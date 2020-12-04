using static LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract.SeriesModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract
{
    public class SeriesModel : ExtractModel<ErrorType>
    {
        public string Given { get; set; }

        public bool IsPartOfSeries { get; set; }

        public string Normalized { get; set; }

        public int Number { get; set; }

        public enum ErrorType
        {
            None,
            NoSeriesName
        }
    }
}
