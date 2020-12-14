using LiteralLifeChurch.ArchiveManagerApi.Exceptions.IndexerWorkflow.Extract.Series;
using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow;
using LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using static LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract.SeriesModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Services.IndexerWorkflow.Steps.Extract
{
    // Extract something like: 200901AM - John Smith - Overcoming the World - Greater Is He That Is In You.mp4
    //                                                                        ^^^^^^^^^^^^^^^^^^^^^^^^^^^^

    public class SeriesStep : IndexerWorkflowStep<MediaModel, SeriesModel, SeriesException>
    {
        private readonly SeriesModel DefaultSeries;

        private Dictionary<string, int> ExistingSeries;
        private readonly int Index;
        private readonly string Split;

        public SeriesStep(ConfigurationModel config, string split, int index, Dictionary<string, int> existingSeries) : base(config)
        {
            ExistingSeries = existingSeries;
            Index = index;
            Split = split;

            DefaultSeries = new SeriesModel
            {
                AssumedValue = true,
                Given = null,
                Id = null,
                IsPartOfSeries = false,
                Normalized = null,
                Number = 0,
                ParseError = ErrorType.NoSeriesName
            };
        }

        public override SeriesModel Run(MediaModel item)
        {
            string givenName = item.OneDriveMetadata.Name.Split(Split).ElementAtOrDefault(Index);

            if (string.IsNullOrWhiteSpace(givenName))
            {
                return DefaultSeries;
            }

            givenName = RemoveExtension(givenName.Trim());

            TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
            string normalizedName = textinfo.ToTitleCase(givenName);
            string id = GenerateId(NameWithLettersOnly(givenName));

            return new SeriesModel
            {
                AssumedValue = false,
                Given = givenName,
                Id = id,
                IsPartOfSeries = true,
                Normalized = normalizedName,
                Number = GenerateSeriesNumber(id),
                ParseError = ErrorType.None
            };
        }

        // region Helper Methods

        private int GenerateSeriesNumber(string id)
        {
            if (ExistingSeries.ContainsKey(id))
            {
                int seriesNumber = ExistingSeries[id];
                ++seriesNumber;

                ExistingSeries[id] = seriesNumber;
                return seriesNumber;
            }

            ExistingSeries[id] = 1;
            return 1;
        }

        private string NameWithLettersOnly(string name)
        {
            return Regex.Replace(name, "[^A-Za-z]*", "").ToLowerInvariant();
        }

        // endregion
    }
}
