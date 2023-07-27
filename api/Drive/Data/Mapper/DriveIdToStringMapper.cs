#nullable enable
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Mapper;

internal class DriveIdToStringMapper : IDriveIdToStringMapper
{
    public string Map(Microsoft.Graph.Models.Drive? input)
    {
        return input?.Id ?? throw new MissingDriveIdException();
    }
}
