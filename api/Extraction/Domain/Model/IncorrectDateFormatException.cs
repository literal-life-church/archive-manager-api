using LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.Model;

internal class IncorrectDateFormatException : CustomException
{
    public IncorrectDateFormatException(string dateString) : base(
        $"Could not extract the date from the given date string '{dateString}' because it does not match this exact date format pattern: {ExtractionConfig.DateTimeFormat}.")
    {
    }
}
