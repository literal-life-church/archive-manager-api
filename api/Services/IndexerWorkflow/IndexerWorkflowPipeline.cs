using LiteralLifeChurch.ArchiveManagerApi.Exceptions;
using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow;
using LiteralLifeChurch.ArchiveManagerApi.Services.IndexerWorkflow.Steps.Crawl;
using LiteralLifeChurch.ArchiveManagerApi.Services.IndexerWorkflow.Steps.Extract;
using LiteralLifeChurch.ArchiveManagerApi.Services.IndexerWorkflow.Steps.Reduce;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;
using static LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping.ConfigurationModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Services.IndexerWorkflow
{
    public class IndexerWorkflowPipeline : IIndexerWorkflow
    {
        private readonly ConfigurationModel Config;
        private readonly CrawlStep Crawl;

        private readonly DateExtractStep DateExtract;
        private readonly MediaTypeExtractStep MediaTypeExtract;
        private readonly NameExtractStep NameExtract;
        private readonly OneDriveMetadataExtractStep OneDriveMetadataExtract;
        private readonly SpeakerExtractStep SpeakerExtract;

        private readonly DateReduceStep DateReduce;
        private readonly MediaTypeReduceStep MediaTypeReduce;
        private readonly NameReduceStep NameReduce;
        private readonly SpeakerReduceStep SpeakerReduce;

        public IndexerWorkflowPipeline(ConfigurationModel config)
        {
            Config = config;
            Crawl = new CrawlStep(config);

            DateExtract = new DateExtractStep(config, " - ", 0);
            MediaTypeExtract = new MediaTypeExtractStep(config);
            NameExtract = new NameExtractStep(config, " - ", 2);
            OneDriveMetadataExtract = new OneDriveMetadataExtractStep(config);
            SpeakerExtract = new SpeakerExtractStep(config, " - ", 1);

            DateReduce = new DateReduceStep(config);
            MediaTypeReduce = new MediaTypeReduceStep(config);
            NameReduce = new NameReduceStep(config);
            SpeakerReduce = new SpeakerReduceStep(config);
        }

        public async Task<List<ReducedMediaModel>> Run()
        {
            List<DriveItem> driveItems = await RunCrawl();
            List<FullMediaModel> rawExtract = RunExtract(driveItems);
            List<ReducedMediaModel> reduced = RunReduce(rawExtract);

            return reduced;
        }

        // region Helper Methods

        private async Task<List<DriveItem>> RunCrawl()
        {
            return await Crawl.Run(null);
        }

        private List<FullMediaModel> RunExtract(List<DriveItem> driveItems)
        {
            List<FullMediaModel> models = new List<FullMediaModel>();

            foreach (DriveItem item in driveItems)
            {
                try
                {
                    models.Add(new FullMediaModel
                    {
                        Date = DateExtract.Run(item),
                        Name = NameExtract.Run(item),
                        OneDriveMetadata = OneDriveMetadataExtract.Run(item),
                        Speakers = SpeakerExtract.Run(item),
                        Type = MediaTypeExtract.Run(item)
                    });
                }
                catch (AppException ex)
                {
                    if (Config.FaultResponse == FaultResponseType.Terminate)
                    {
                        throw ex;
                    }
                }
            }

            return models;
        }

        private List<ReducedMediaModel> RunReduce(List<FullMediaModel> rawExtract)
        {
            List<ReducedMediaModel> models = new List<ReducedMediaModel>();

            foreach (FullMediaModel item in rawExtract)
            {
                ReducedMediaModel model = new ReducedMediaModel();
                model = DateReduce.Run(model, item);
                model = MediaTypeReduce.Run(model, item);
                model = NameReduce.Run(model, item);
                model = SpeakerReduce.Run(model, item);

                models.Add(model);
            }

            return models;
        }

        // endregion
    }
}
