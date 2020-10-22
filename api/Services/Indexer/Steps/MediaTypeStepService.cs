using LiteralLifeChurch.ArchiveManagerApi.Exceptions.Indexer.MediaType;
using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Models.Indexer;
using Microsoft.Graph;
using System.Linq;
using static LiteralLifeChurch.ArchiveManagerApi.Models.Indexer.MediaTypeModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Services.Indexer.Steps
{
    public class MediaTypeStepService : IndexerStepService<ErrorType, IndexerMediaTypeException, MediaTypeModel>
    {
        public MediaTypeStepService(ConfigurationModel config) : base(config)
        {
        }

        public override MediaTypeModel Transform(DriveItem item, string split, int index)
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
