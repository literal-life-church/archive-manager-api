using System.Threading.Tasks;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Data.DataSource;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Mapper;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Repository;

internal class DriveNetworkRepository : IDriveNetworkRepository
{
    private readonly IDriveNetworkDataSource _dataSource;
    private readonly IDriveIdToStringMapper _driveIdMapper;

    public DriveNetworkRepository(IDriveNetworkDataSource driveNetworkDataSource,
        IDriveIdToStringMapper driveIdToStringMapper)
    {
        _dataSource = driveNetworkDataSource;
        _driveIdMapper = driveIdToStringMapper;
    }

    public async Task<string> GetMyDriveIdAsync()
    {
        var drive = await _dataSource.GetMyDrive().GetAsync();
        return _driveIdMapper.Map(drive);
    }
}
