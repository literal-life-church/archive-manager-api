#nullable enable
using LiteralLifeChurch.ArchiveManagerApi.Clean.Data.Mapper;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Mapper;

internal interface IDriveIdToStringMapper : IMapper<Microsoft.Graph.Models.Drive?, string>
{
}
