using LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;

internal class FileDomainModel : IDomainModel
{
    public FileDomainModel(string driveId, string id, bool isFolder, string name, string nameWithoutExtension,
        string? parentFolderName)
    {
        DriveId = driveId;
        Id = id;
        IsFolder = isFolder;
        Name = name;
        NameWithoutExtension = nameWithoutExtension;
        ParentFolderName = parentFolderName;
    }

    public string DriveId { get; set; }
    public string Id { get; set; }
    public bool IsFolder { get; set; }
    public string Name { get; set; }
    public string NameWithoutExtension { get; set; }
    public string? ParentFolderName { get; set; }
}
