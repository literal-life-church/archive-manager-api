using System.Globalization;

namespace LiteralLifeChurch.ArchiveManagerApi.Extraction;

internal static class ExtractionConfig
{
    public static readonly string DateTimeFormat = "yyyy.MM.dd";
    public static readonly DateTimeStyles DateTimeStyles = DateTimeStyles.AssumeLocal;
    public static readonly CultureInfo LanguageCulture = new("en-US");

    public static readonly string MediaFileNameDelimiter = " - ";
    public static readonly int MaxNumberOfPartsInMediaFileName = 4;
    public static readonly int MinNumberOfPartsInMediaFileName = 3;
    public static readonly string SpeakerDelimiter = ";";
}
