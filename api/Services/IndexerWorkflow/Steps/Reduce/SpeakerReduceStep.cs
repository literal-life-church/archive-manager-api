using LiteralLifeChurch.ArchiveManagerApi.Exceptions.IndexerWorkflow.Reduce;
using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow;
using System;
using System.Linq;

namespace LiteralLifeChurch.ArchiveManagerApi.Services.IndexerWorkflow.Steps.Reduce
{
    public class SpeakerReduceStep : IndexerWorkflowStep<ReducedMediaModel, ReducedMediaModel, ReduceException>
    {
        public SpeakerReduceStep(ConfigurationModel config) : base(config)
        {
        }

        public override ReducedMediaModel Run(ReducedMediaModel item)
        {
            throw new NotImplementedException("Use the two parameter implementation of this method");
        }

        public ReducedMediaModel Run(ReducedMediaModel item, FullMediaModel fullModel)
        {
            item.Speakers = fullModel.Speakers.Names.Select(speaker => speaker.Normalized).ToList();
            return item;
        }
    }
}
