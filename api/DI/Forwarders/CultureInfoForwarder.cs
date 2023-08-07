using System.Globalization;
using LiteralLifeChurch.ArchiveManagerApi.Config;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Forwarders;

internal class CultureInfoForwarder : ICultureInfoForwarder
{
    // For cases when you aren't referring to the config value itself,
    // but, rather, a property of it. Specifically used to extract the
    // TextInfo property for converting strings to title case.
    // All other cases which just need to pass the language culture
    // through should use the GlobalConfig.LanguageCulture property.
    public TextInfo TextInfo =>
        GlobalConfig.LanguageCulture.TextInfo;
}
