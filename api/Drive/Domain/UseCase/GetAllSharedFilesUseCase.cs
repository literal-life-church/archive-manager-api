using System.Collections.Generic;
using System.Linq;
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

    public async Task<List<FileDomainModel>> ExecuteAsync()
    {
        var myDriveId = await _driveNetworkRepository.GetMyDriveIdAsync();
        _logger.LogDebug("Obtained service account's drive ID.");
        var sharedFolderRootLevel = await _driveNetworkRepository.GetAllRootFilesSharedWithMeAsync(myDriveId);
        _logger.LogDebug("Obtained all root files within the service account's shared folder.");

        Queue<FileDomainModel> queue = new(sharedFolderRootLevel);
        List<FileDomainModel> output = new();

        while (queue.Any())
        {
            var file = queue.Dequeue();

            if (file.IsFolder)
            {
                var allFilesInSubfolder =
                    await _driveNetworkRepository.GetAllFilesInFolderAsync(file.DriveId, file.Id);
                _logger.LogDebug(
                    $"Discovered the folder named: {file.Name}. Adding its children to the queue for processing.");
                allFilesInSubfolder.ForEach(subFolderFile => queue.Enqueue(subFolderFile));
            }
            else
            {
                output.Add(file);
            }
        }

        _logger.LogInformation($"Completed crawling. Discovered {output.Count} files(s).");
        return output;
    }
}
