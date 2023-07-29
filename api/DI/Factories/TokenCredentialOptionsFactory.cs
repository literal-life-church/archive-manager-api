using Azure.Identity;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Factories;

internal class TokenCredentialOptionsFactory : ITokenCredentialOptionsFactory
{
    public TokenCredentialOptions NewInstance()
    {
        return new TokenCredentialOptions();
    }
}
