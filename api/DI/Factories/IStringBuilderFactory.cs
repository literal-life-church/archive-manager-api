using System.Text;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Factories;

internal interface IStringBuilderFactory : IFactory
{
    StringBuilder NewInstance(int capacity);
}
