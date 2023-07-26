using Azure.Identity;
using LiteralLifeChurch.ArchiveManagerApi.Authentication.Domain.Model;
using Microsoft.Extensions.Options;
using Microsoft.Graph;

namespace LiteralLifeChurch.ArchiveManagerApi.Authentication.Domain.UseCase
{
    internal class GetAuthenticatedClientUseCase : IGetAuthenticatedClientUseCase
    {
        private readonly AuthenticationEnvironmentVariableDomainModel _authenticationEnvironmentVariableDomainModel;

        public GetAuthenticatedClientUseCase(IOptions<AuthenticationEnvironmentVariableDomainModel> authenticationEnvironmentVariableDomainModel)
        {
            _authenticationEnvironmentVariableDomainModel = authenticationEnvironmentVariableDomainModel.Value;
        }

        public GraphServiceClient Execute()
        {
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AuthenticationConfig.AuthorityHost
            };

            string username = _authenticationEnvironmentVariableDomainModel.ServiceAccountUsername;
            string password = _authenticationEnvironmentVariableDomainModel.ServiceAccountPassword;
            string tenantId = _authenticationEnvironmentVariableDomainModel.TenantId;
            string clientId = _authenticationEnvironmentVariableDomainModel.ClientId;

            var credential = new UsernamePasswordCredential(username, password, tenantId, clientId, options);
            var graphClient = new GraphServiceClient(credential, AuthenticationConfig.Scopes);
            return graphClient;
        }
    }
}
