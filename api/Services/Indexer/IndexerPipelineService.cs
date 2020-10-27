using LiteralLifeChurch.ArchiveManagerApi.Exceptions;
using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Models.Indexer;
using LiteralLifeChurch.ArchiveManagerApi.Services.Indexer.Steps;
using Microsoft.Graph;
using System.Collections.Generic;
using static LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping.ConfigurationModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Services.Indexer
{
    public class IndexerPipelineService : IIndexerService
    {
        private readonly ConfigurationModel Config;
        private readonly DateStepService DateStepService;
        private readonly MediaTypeStepService MediaTypeStepService;
        private readonly NameStepService NameStepService;
        private readonly SpeakerStepService SpeakerStepService;

        public IndexerPipelineService(ConfigurationModel config)
        {
            Config = config;
            DateStepService = new DateStepService(config);
            MediaTypeStepService = new MediaTypeStepService(config);
            NameStepService = new NameStepService(config);
            SpeakerStepService = new SpeakerStepService(config);
        }

        public List<MediaModel> Transform(List<DriveItem> driveItems)
        {
            List<MediaModel> models = new List<MediaModel>();

            foreach (DriveItem item in driveItems)
            {
                try
                {
                    models.Add(new MediaModel
                    {
                        Date = DateStepService.Transform(item, " - ", 0),
                        Name = NameStepService.Transform(item, " - ", 2),
                        Speakers = SpeakerStepService.Transform(item, " - ", 1),
                        Type = MediaTypeStepService.Transform(item, "", 0)
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
