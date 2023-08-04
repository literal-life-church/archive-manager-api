using System.IO;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Factories;

internal class DirectoryInfoFactory : IDirectoryInfoFactory
{
    public DirectoryInfo NewInstance(string path)
    {
        return new DirectoryInfo(path);
    }
}
