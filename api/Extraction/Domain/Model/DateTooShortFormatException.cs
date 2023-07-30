using LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.Model;

internal class DateTooShortFormatException : CustomException
{
    public DateTooShortFormatException(string dateString) : base(
        $"Could not extract the date from the given date string '{dateString}' because the length of the string must be at least {ExtractionConfig.DateTimeFormat.Length} character(s).")
    {
    }
}
