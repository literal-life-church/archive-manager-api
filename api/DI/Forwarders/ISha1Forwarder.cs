using System.Security.Cryptography;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Forwarders;

internal interface ISha1Forwarder : IForwarder
{
    SHA1 Create();
}
