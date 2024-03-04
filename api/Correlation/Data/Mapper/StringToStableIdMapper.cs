using LiteralLifeChurch.ArchiveManagerApi.Config;
using LiteralLifeChurch.ArchiveManagerApi.DI.Factories;
using LiteralLifeChurch.ArchiveManagerApi.DI.Forwarders;

namespace LiteralLifeChurch.ArchiveManagerApi.Correlation.Data.Mapper;

internal class StringToStableIdMapper : IStringToStableIdMapper
{
    private readonly IRegexForwarder _regexForwarder;
    private readonly ISha1Forwarder _sha1Forwarder;
    private readonly IStringBuilderFactory _stringBuilderFactory;
    private readonly IUtf8Forwarder _utf8Forwarder;

    public StringToStableIdMapper(IRegexForwarder regexForwarder, ISha1Forwarder sha1Forwarder,
        IStringBuilderFactory stringBuilderFactory,
        IUtf8Forwarder utf8Forwarder)
    {
        _regexForwarder = regexForwarder;
        _sha1Forwarder = sha1Forwarder;
        _stringBuilderFactory = stringBuilderFactory;
        _utf8Forwarder = utf8Forwarder;
    }

    public string Map(string input)
    {
        var cleanedInput = _regexForwarder
            .Replace(input, CorrelationConfig.HashingCleaningRegexPattern,
                CorrelationConfig.HashingCleaningRegexReplacement)
            .ToLower(GlobalConfig.LanguageCulture);

        var inputBytes = _utf8Forwarder.GetBytes(cleanedInput);
        var hashedInputStringBuilder =
            _stringBuilderFactory.NewInstance(CorrelationConfig.HashingStringBuilderCapacity);

        using (var hash = _sha1Forwarder.Create())
        {
            var hashedInputBytes = hash.ComputeHash(inputBytes);

            foreach (var inputByte in hashedInputBytes)
                hashedInputStringBuilder.Append(inputByte.ToString(CorrelationConfig.HashingStringFormat));
        }

        return hashedInputStringBuilder.ToString();
    }
}
