using LiteralLifeChurch.ArchiveManagerApi.Config.Data.DataSource;
using LiteralLifeChurch.ArchiveManagerApi.Config.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Config.Data.Repository;

internal class AuthenticationOptionsEnvironmentVariableRepository : IAuthenticationOptionsEnvironmentVariableRepository
{
    public AuthenticationOptionsEnvironmentVariableRepository(
        IAuthenticationOptionsEnvironmentVariableDataSource authenticationOptionsEnvironmentVariableDataSource)
    {
        GetAuthenticationOptions = authenticationOptionsEnvironmentVariableDataSource.GetAuthenticationOptions;
    }

    public AuthenticationOptionsDomainModel GetAuthenticationOptions { get; }
}
