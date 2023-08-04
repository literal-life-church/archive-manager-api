using LiteralLifeChurch.ArchiveManagerApi.Clean.Data.Mapper;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;
using Microsoft.Graph.Models;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Mapper;

internal interface IDriveItemToFileMapper : IMapper<DriveItem?, FileDomainModel?>
{
}
