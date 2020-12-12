using LiteralLifeChurch.ArchiveManagerApi.Exceptions.IndexerWorkflow.Reduce;
using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow;
using System;

namespace LiteralLifeChurch.ArchiveManagerApi.Services.IndexerWorkflow.Steps.Reduce
{
    public class NameReduceStep : IndexerWorkflowStep<ReducedMediaModel, ReducedMediaModel, ReduceException>
    {
        public NameReduceStep(ConfigurationModel config) : base(config)
        {
        }

        public override ReducedMediaModel Run(ReducedMediaModel item)
        {
            throw new NotImplementedException("Use the two parameter implementation of this method");
        }

        public ReducedMediaModel Run(ReducedMediaModel item, FullMediaModel fullModel)
        {
            item.Name = fullModel.Name.Normalized;
            return item;
        }
    }
}
