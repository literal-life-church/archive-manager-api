using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Data.DataSource;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Mapper;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.Extensions;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Repository;

internal class DriveNetworkRepository : IDriveNetworkRepository
{
    private readonly IDriveNetworkDataSource _dataSource;
    private readonly IDriveItemToFileMapper _driveItemToFileMapper;
    private readonly IDriveToDriveIdMapper _driveToDriveIdMapper;

    public DriveNetworkRepository(IDriveToDriveIdMapper driveIdToStringMapper,
        IDriveItemToFileMapper driveItemToFileMapper,
        IDriveNetworkDataSource driveNetworkDataSource)
    {
        _driveToDriveIdMapper = driveIdToStringMapper;
        _driveItemToFileMapper = driveItemToFileMapper;
        _dataSource = driveNetworkDataSource;
    }

    public async Task<string> GetMyDriveIdAsync()
    {
        var drive = await _dataSource.GetMyDrive().GetAsync();
        return _driveToDriveIdMapper.Map(drive);
    }

    public async Task<List<FileDomainModel>> GetAllFilesInFolderAsync(string driveId, string fileId)
    {
        var driveItem = await _dataSource.GetAllFilesInFolder(driveId, fileId).GetAsync();
        if (driveItem?.Value == null) return new List<FileDomainModel>();

        return driveItem
            .Value
            .Select(item => _driveItemToFileMapper.Map(item))
            .WhereNotNull()
            .ToList();
    }

    public async Task<List<FileDomainModel>> GetAllRootFilesSharedWithMeAsync(string myDriveId)
    {
        var sharedWithMe = await _dataSource.GetAllRootFilesSharedWithMe(myDriveId).GetAsync();
        if (sharedWithMe?.Value == null) return new List<FileDomainModel>();

        return sharedWithMe
            .Value
            .Select(item => _driveItemToFileMapper.Map(item))
            .WhereNotNull()
            .ToList();
    }
}
