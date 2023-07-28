using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Repository;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;
using Microsoft.Extensions.Logging;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.UseCase;

internal class GetAllSharedFilesUseCase : IGetAllSharedFilesUseCase
{
    private readonly IDriveNetworkRepository _driveNetworkRepository;
    private readonly ILogger<GetAllSharedFilesUseCase> _logger;

    public GetAllSharedFilesUseCase(IDriveNetworkRepository driveNetworkRepository,
        ILogger<GetAllSharedFilesUseCase> logger)
    {
        _driveNetworkRepository = driveNetworkRepository;
        _logger = logger;
    }

    public async Task<List<ItemDomainModel>> ExecuteAsync()
    {
        var myDriveId = await _driveNetworkRepository.GetMyDriveIdAsync();
        _logger.LogDebug("Obtained service account's drive ID.");
        var sharedFolderRootLevel = await _driveNetworkRepository.GetAllRootFilesSharedWithMeAsync(myDriveId);
        _logger.LogDebug("Obtained all root files within the service account's shared folder.");

        Queue<ItemDomainModel> queue = new(sharedFolderRootLevel);
        List<ItemDomainModel> output = new();
        var stringBuilder = new StringBuilder();

        while (queue.Any())
        {
            var item = queue.Dequeue();
            output.Add(item);

            if (item.IsFolder)
            {
                var allFilesInSubfolder = await _driveNetworkRepository.GetAllFilesInFolderAsync(item.DriveId, item.Id);
                allFilesInSubfolder.ForEach(subFolderItem => queue.Enqueue(subFolderItem));
            }
        }

        return output;
    }
}
