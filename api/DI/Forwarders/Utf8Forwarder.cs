using System.Text;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Forwarders;

internal class Utf8Forwarder : IUtf8Forwarder
{
    public byte[] GetBytes(string s)
    {
        return Encoding.UTF8.GetBytes(s);
    }
}
