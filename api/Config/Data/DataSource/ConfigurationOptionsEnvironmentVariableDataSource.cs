using LiteralLifeChurch.ArchiveManagerApi.Config.Domain.Model;
using Microsoft.Extensions.Options;

namespace LiteralLifeChurch.ArchiveManagerApi.Config.Data.DataSource;

internal class ConfigurationOptionsEnvironmentVariableDataSource : IConfigurationOptionsEnvironmentVariableDataSource
{
    public ConfigurationOptionsEnvironmentVariableDataSource(
        IOptions<ConfigurationOptionsDomainModel> authenticationOptionsDomainModel)
    {
        GetConfigurationOptions = authenticationOptionsDomainModel.Value;
    }

    public ConfigurationOptionsDomainModel GetConfigurationOptions { get; }
}
