using LiteralLifeChurch.ArchiveManagerApi.DI.Factories;
using LiteralLifeChurch.ArchiveManagerApi.Global.Domain.Model;
using Microsoft.Extensions.Options;
using Microsoft.Graph;

namespace LiteralLifeChurch.ArchiveManagerApi.Authentication.Domain.UseCase;

internal class GetAuthenticatedClientUseCase : IGetAuthenticatedClientUseCase
{
    private readonly AuthenticationEnvironmentVariableDomainModel _authenticationEnvironmentVariableDomainModel;
    private readonly IGraphServiceClientFactory _graphServiceClientFactory;
    private readonly ITokenCredentialOptionsFactory _tokenCredentialOptionsFactory;
    private readonly IUsernamePasswordCredentialFactory _usernamePasswordCredentialFactory;

    public GetAuthenticatedClientUseCase(
        IOptions<AuthenticationEnvironmentVariableDomainModel> environmentVariableDomainModel,
        IGraphServiceClientFactory graphServiceClientFactory,
        ITokenCredentialOptionsFactory tokenCredentialOptionsFactory,
        IUsernamePasswordCredentialFactory usernamePasswordCredentialFactory)
    {
        _authenticationEnvironmentVariableDomainModel = environmentVariableDomainModel.Value;
        _graphServiceClientFactory = graphServiceClientFactory;
        _tokenCredentialOptionsFactory = tokenCredentialOptionsFactory;
        _usernamePasswordCredentialFactory = usernamePasswordCredentialFactory;
    }

    public GraphServiceClient Execute()
    {
        var options = _tokenCredentialOptionsFactory.NewInstance();
        options.AuthorityHost = AuthenticationConfig.AuthorityHost;

        var clientId = _authenticationEnvironmentVariableDomainModel.ClientId;
        var password = _authenticationEnvironmentVariableDomainModel.ServiceAccountPassword;
        var tenantId = _authenticationEnvironmentVariableDomainModel.TenantId;
        var username = _authenticationEnvironmentVariableDomainModel.ServiceAccountUsername;

        var credential =
            _usernamePasswordCredentialFactory.NewInstance(username, password, tenantId, clientId, options);

        return _graphServiceClientFactory.NewInstance(credential, AuthenticationConfig.Scopes);
    }
}
