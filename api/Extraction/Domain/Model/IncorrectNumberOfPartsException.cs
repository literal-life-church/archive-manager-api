using LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.Model;

internal class IncorrectNumberOfPartsException : CustomException
{
    public IncorrectNumberOfPartsException(string fileName, int numberOfParts) : base(
        $"The file '{fileName}' must have at least ${ExtractionConfig.MinNumberOfPartsInMediaFileName} and at most ${ExtractionConfig.MaxNumberOfPartsInMediaFileName} in the name when separated by the delimiter '{ExtractionConfig.MediaFileNameDelimiter}'. This file has {numberOfParts} parts(s).")
    {
    }
}
