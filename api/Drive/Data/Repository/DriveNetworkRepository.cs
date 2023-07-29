using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Data.DataSource;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Mapper;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Repository;

internal class DriveNetworkRepository : IDriveNetworkRepository
{
    private readonly IDriveNetworkDataSource _dataSource;
    private readonly IDriveItemToItemMapper _driveItemToItemMapper;
    private readonly IDriveToDriveIdMapper _driveToDriveIdMapper;

    public DriveNetworkRepository(IDriveToDriveIdMapper driveIdToStringMapper,
        IDriveItemToItemMapper driveItemToItemMapper,
        IDriveNetworkDataSource driveNetworkDataSource)
    {
        _driveToDriveIdMapper = driveIdToStringMapper;
        _driveItemToItemMapper = driveItemToItemMapper;
        _dataSource = driveNetworkDataSource;
    }

    public async Task<List<ItemDomainModel>> GetAllFilesInFolderAsync(string driveId, string fileId)
    {
        var driveItem = await _dataSource.GetAllFilesInFolder(driveId, fileId).GetAsync();
        if (driveItem?.Value == null) return new List<ItemDomainModel>();

        return driveItem
            .Value
            .Select(item => _driveItemToItemMapper.Map(item))
            .Where(item => item != null)
            .ToList();
    }

    public async Task<List<ItemDomainModel>> GetAllRootFilesSharedWithMeAsync(string myDriveId)
    {
        var sharedWithMe = await _dataSource.GetAllRootFilesSharedWithMe(myDriveId).GetAsync();
        if (sharedWithMe?.Value == null) return new List<ItemDomainModel>();

        return sharedWithMe
            .Value
            .Select(item => _driveItemToItemMapper.Map(item))
            .Where(item => item != null)
            .ToList();
    }

    public async Task<string> GetMyDriveIdAsync()
    {
        var drive = await _dataSource.GetMyDrive().GetAsync();
        return _driveToDriveIdMapper.Map(drive);
    }
}
