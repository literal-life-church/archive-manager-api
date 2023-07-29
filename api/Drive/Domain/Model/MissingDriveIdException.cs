using LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;

internal class MissingDriveIdException : CustomException
{
    public MissingDriveIdException() : base("The Drive cannot be used since it does not contain a populated Drive ID.")
    {
    }
}
