using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Mapper;

internal class DriveToDriveIdMapper : IDriveToDriveIdMapper
{
    public string Map(Microsoft.Graph.Models.Drive? input)
    {
        return input?.Id ?? throw new MissingDriveIdException();
    }
}
