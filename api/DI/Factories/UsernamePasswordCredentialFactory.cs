using Azure.Identity;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Factories;

internal class UsernamePasswordCredentialFactory : IUsernamePasswordCredentialFactory
{
    public UsernamePasswordCredential NewInstance(string username, string password, string tenantId,
        string clientId, TokenCredentialOptions options)
    {
        return new UsernamePasswordCredential(username, password, tenantId, clientId, options);
    }
}
