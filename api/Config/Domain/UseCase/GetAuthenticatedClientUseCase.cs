using LiteralLifeChurch.ArchiveManagerApi.Config.Data.Repository;
using LiteralLifeChurch.ArchiveManagerApi.Config.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.DI.Factories;
using Microsoft.Graph;

namespace LiteralLifeChurch.ArchiveManagerApi.Config.Domain.UseCase;

internal class GetAuthenticatedClientUseCase : IGetAuthenticatedClientUseCase
{
    private readonly AuthenticationOptionsDomainModel _authenticationOptionsDomainModel;
    private readonly IGraphServiceClientFactory _graphServiceClientFactory;
    private readonly ITokenCredentialOptionsFactory _tokenCredentialOptionsFactory;
    private readonly IUsernamePasswordCredentialFactory _usernamePasswordCredentialFactory;

    public GetAuthenticatedClientUseCase(
        IAuthenticationOptionsEnvironmentVariableRepository authenticationOptionsEnvironmentVariableRepository,
        IGraphServiceClientFactory graphServiceClientFactory,
        ITokenCredentialOptionsFactory tokenCredentialOptionsFactory,
        IUsernamePasswordCredentialFactory usernamePasswordCredentialFactory)
    {
        _authenticationOptionsDomainModel = authenticationOptionsEnvironmentVariableRepository.GetAuthenticationOptions;
        _graphServiceClientFactory = graphServiceClientFactory;
        _tokenCredentialOptionsFactory = tokenCredentialOptionsFactory;
        _usernamePasswordCredentialFactory = usernamePasswordCredentialFactory;
    }

    public GraphServiceClient Execute()
    {
        var options = _tokenCredentialOptionsFactory.NewInstance();
        options.AuthorityHost = GlobalConfig.AuthorityHost;

        var clientId = _authenticationOptionsDomainModel.ClientId;
        var password = _authenticationOptionsDomainModel.ServiceAccountPassword;
        var tenantId = _authenticationOptionsDomainModel.TenantId;
        var username = _authenticationOptionsDomainModel.ServiceAccountUsername;

        var credential =
            _usernamePasswordCredentialFactory.NewInstance(username, password, tenantId, clientId, options);

        return _graphServiceClientFactory.NewInstance(credential, GlobalConfig.Scopes);
    }
}
