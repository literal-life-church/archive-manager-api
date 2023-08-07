using LiteralLifeChurch.ArchiveManagerApi.Config.Domain.Model;
using Microsoft.Extensions.Options;

namespace LiteralLifeChurch.ArchiveManagerApi.Config.Data.DataSource;

internal class AuthenticationOptionsEnvironmentVariableDataSource : IAuthenticationOptionsEnvironmentVariableDataSource
{
    public AuthenticationOptionsEnvironmentVariableDataSource(
        IOptions<AuthenticationOptionsDomainModel> authenticationOptionsDomainModel)
    {
        GetAuthenticationOptions = authenticationOptionsDomainModel.Value;
    }

    public AuthenticationOptionsDomainModel GetAuthenticationOptions { get; }
}
