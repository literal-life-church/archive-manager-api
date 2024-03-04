using System.Security.Cryptography;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Forwarders;

internal class Sha1Forwarder : ISha1Forwarder
{
    public SHA1 Create()
    {
        return SHA1.Create();
    }
}
