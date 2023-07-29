#nullable enable
using LiteralLifeChurch.ArchiveManagerApi.Clean.Data.Mapper;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Mapper;

internal interface IDriveToDriveIdMapper : IMapper<Microsoft.Graph.Models.Drive?, string>
{
}
