using LiteralLifeChurch.ArchiveManagerApi.Clean.Data.DataSource;
using LiteralLifeChurch.ArchiveManagerApi.Config.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Config.Data.DataSource;

internal interface IConfigurationOptionsEnvironmentVariableDataSource : IDataSource
{
    ConfigurationOptionsDomainModel GetConfigurationOptions { get; }
}
