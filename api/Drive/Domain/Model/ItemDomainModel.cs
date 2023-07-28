using LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;

internal class ItemDomainModel : IDomainModel
{
    public string DriveId { get; set; }
    public string Id { get; set; }
    public bool IsFolder { get; set; }
    public string Name { get; set; }
}
