namespace LiteralLifeChurch.ArchiveManagerApi.Correlation;

internal static class CorrelationConfig
{
    public const string HashingCleaningRegexPattern = @"[^a-zA-Z0-9]+";
    public const string HashingCleaningRegexReplacement = "";
    public const int HashingStringBuilderCapacity = 40;
    public const string HashingStringFormat = "x2";
}
