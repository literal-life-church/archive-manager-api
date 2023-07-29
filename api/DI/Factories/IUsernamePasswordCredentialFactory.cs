using Azure.Identity;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Factories;

internal interface IUsernamePasswordCredentialFactory : IFactory
{
    UsernamePasswordCredential NewInstance(string username, string password, string tenantId, string clientId,
        TokenCredentialOptions options);
}
