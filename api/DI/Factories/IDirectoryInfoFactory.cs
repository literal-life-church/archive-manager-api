using System.IO;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Factories;

internal interface IDirectoryInfoFactory : IFactory
{
    DirectoryInfo NewInstance(string path);
}
