#nullable enable
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;
using Microsoft.Graph.Models;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Mapper;

internal class DriveItemToFileMapper : IDriveItemToFileMapper
{
    public FileDomainModel? Map(DriveItem? input)
    {
        if (input?.Name == null) return null;

        string driveId;

        if (input.RemoteItem?.ParentReference?.DriveId != null)
            driveId = input.RemoteItem.ParentReference.DriveId;
        else if (input.ParentReference?.DriveId != null)
            driveId = input.ParentReference.DriveId;
        else
            return null;

        var lastIndex = input.Name.LastIndexOf('.');
        var nameWithoutExtension = lastIndex > 0 ? input.Name[..lastIndex] : input.Name;

        return new FileDomainModel
        {
            DriveId = driveId,
            Id = input.Id,
            IsFolder = input.Folder != null,
            Name = input.Name,
            NameWithoutExtension = nameWithoutExtension
        };
    }
}
