using LiteralLifeChurch.ArchiveManagerApi.DI.Factories;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;
using Microsoft.Graph.Models;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Mapper;

internal class DriveItemToFileMapper : IDriveItemToFileMapper
{
    private readonly IDirectoryInfoFactory _directoryInfoFactory;

    public DriveItemToFileMapper(IDirectoryInfoFactory directoryInfoFactory)
    {
        _directoryInfoFactory = directoryInfoFactory;
    }

    public FileDomainModel? Map(DriveItem? input)
    {
        // It must have an ID and a name
        if (input?.Id == null) return null;
        if (input.Name == null) return null;

        // It must be associated with a drive
        string driveId;

        if (input.RemoteItem?.ParentReference?.DriveId != null)
            driveId = input.RemoteItem.ParentReference.DriveId;
        else if (input.ParentReference?.DriveId != null)
            driveId = input.ParentReference.DriveId;
        else
            return null;

        // Check for a parent folder name
        string? fullDrivePath;

        if (input.RemoteItem?.ParentReference?.Path != null)
            fullDrivePath = input.RemoteItem.ParentReference.Path;
        else if (input.ParentReference?.Path != null)
            fullDrivePath = input.ParentReference.Path;
        else
            fullDrivePath = null;

        var lastFolderInFullPath = fullDrivePath != null ? _directoryInfoFactory.NewInstance(fullDrivePath).Name : null;

        // Get the name without the extension
        var lastExtensionIndex = input.Name.LastIndexOf(DriveConfig.FileExtensionDelimiter);
        var nameWithoutExtension = lastExtensionIndex > 0 ? input.Name[..lastExtensionIndex] : input.Name;

        return new FileDomainModel(
            driveId,
            input.Id,
            input.Folder != null,
            input.Name,
            nameWithoutExtension,
            lastFolderInFullPath
        );
    }
}
