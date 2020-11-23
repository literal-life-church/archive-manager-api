using LiteralLifeChurch.ArchiveManagerApi.Exceptions.IndexerWorkflow.Extract.MediaType;
using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract;
using Microsoft.Graph;
using System.Linq;
using static LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow.Extract.MediaTypeModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Services.IndexerWorkflow.Steps.Extract
{
    public class MediaTypeStep : IndexerWorkflowStep<DriveItem, MediaTypeModel, MediaTypeException>
    {
        public MediaTypeStep(ConfigurationModel config) : base(config)
        {
        }

        public override MediaTypeModel Run(DriveItem item)
        {
            string name = item.ParentReference.Path.Split("/").Last().Trim();

            return new MediaTypeModel
            {
                AssumedValue = false,
                Id = GenerateId(name),
                Name = name,
                ParseError = ErrorType.None
            };
        }
    }
}
