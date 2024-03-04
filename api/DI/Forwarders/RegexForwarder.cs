using System.Text.RegularExpressions;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Forwarders;

internal class RegexForwarder : IRegexForwarder
{
    public string Replace(string input, string pattern, string replacement)
    {
        return Regex.Replace(input, pattern, replacement);
    }
}
