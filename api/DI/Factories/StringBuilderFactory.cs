using System.Text;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Factories;

internal class StringBuilderFactory : IStringBuilderFactory
{
    public StringBuilder NewInstance(int capacity)
    {
        return new StringBuilder(capacity);
    }
}
