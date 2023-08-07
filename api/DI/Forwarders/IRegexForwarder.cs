namespace LiteralLifeChurch.ArchiveManagerApi.DI.Forwarders;

internal interface IRegexForwarder : IForwarder
{
    string Replace(string input, string pattern, string replacement);
}
