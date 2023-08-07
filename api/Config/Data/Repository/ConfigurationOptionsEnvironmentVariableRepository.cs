using LiteralLifeChurch.ArchiveManagerApi.Config.Data.DataSource;
using LiteralLifeChurch.ArchiveManagerApi.Config.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Config.Data.Repository;

internal class ConfigurationOptionsEnvironmentVariableRepository : IConfigurationOptionsEnvironmentVariableRepository
{
    public ConfigurationOptionsEnvironmentVariableRepository(
        IConfigurationOptionsEnvironmentVariableDataSource configurationOptionsEnvironmentVariableDataSource)
    {
        GetConfigurationOptions = configurationOptionsEnvironmentVariableDataSource.GetConfigurationOptions;
    }

    public ConfigurationOptionsDomainModel GetConfigurationOptions { get; }
}
