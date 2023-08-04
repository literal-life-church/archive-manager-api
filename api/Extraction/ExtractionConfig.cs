using System.Collections.Generic;
using System.Globalization;

namespace LiteralLifeChurch.ArchiveManagerApi.Extraction;

internal static class ExtractionConfig
{
    public static readonly string DateTimeFormat = "yyyy.MM.dd";
    public static readonly DateTimeStyles DateTimeStyles = DateTimeStyles.AssumeLocal;

    public static readonly Dictionary<string, int> SupportedModifiers = new()
    {
        // Maps the modifier to the number of hours to add to the recorded date
        // Modifiers are compared using lowercase, so keep all entries lower case here, as well
        { "am", 9 }, // AM => 9am
        { "pm", 17 } // PM => 5pm
    };

    public static readonly string MediaFileNameDelimiter = " - ";
    public static readonly int MaxNumberOfPartsInMediaFileName = 4;
    public static readonly int MinNumberOfPartsInMediaFileName = 3;
    public static readonly string SpeakerDelimiter = ";";
}
