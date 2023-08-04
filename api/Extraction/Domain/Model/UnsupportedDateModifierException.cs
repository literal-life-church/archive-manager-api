using LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.Model;

internal class UnsupportedDateModifierException : CustomException
{
    public UnsupportedDateModifierException(string modifier, string dateString) : base(
        $"The modifier '{modifier}' on the date '{dateString}' is not supported. Supported modifiers are: {string.Join(", ", ExtractionConfig.SupportedModifiers.Keys)}.")
    {
    }
}
