using LiteralLifeChurch.ArchiveManagerApi.Exceptions;
using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow;
using LiteralLifeChurch.ArchiveManagerApi.Services.IndexerWorkflow.Steps.Crawl;
using LiteralLifeChurch.ArchiveManagerApi.Services.IndexerWorkflow.Steps.Extract;
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
        private readonly DateStep Date;
        private readonly MediaTypeStep MediaType;
        private readonly NameStep Name;
        private readonly OneDriveMetadataStep OneDriveMetadata;
        private readonly SpeakerStep Speaker;

        public IndexerWorkflowPipeline(ConfigurationModel config)
        {
            Config = config;
            Crawl = new CrawlStep(config);
            Date = new DateStep(config, " - ", 0);
            MediaType = new MediaTypeStep(config);
            Name = new NameStep(config, " - ", 2);
            OneDriveMetadata = new OneDriveMetadataStep(config);
            Speaker = new SpeakerStep(config, " - ", 1);
        }

        public async Task<List<MediaModel>> Run()
        {
            List<DriveItem> driveItems = await Crawl.Run(null);
            List<MediaModel> models = new List<MediaModel>();

            foreach (DriveItem item in driveItems)
            {
                try
                {
                    models.Add(new MediaModel
                    {
                        Date = Date.Run(item),
                        Name = Name.Run(item),
                        OneDriveMetadata = OneDriveMetadata.Run(item),
                        Speakers = Speaker.Run(item),
                        Type = MediaType.Run(item)
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
    }
}
