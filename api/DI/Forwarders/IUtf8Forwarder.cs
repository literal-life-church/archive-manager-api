namespace LiteralLifeChurch.ArchiveManagerApi.DI.Forwarders;

internal interface IUtf8Forwarder : IForwarder
{
    byte[] GetBytes(string s);
}
