using LiteralLifeChurch.ArchiveManagerApi.Clean.Data.Repository;
using LiteralLifeChurch.ArchiveManagerApi.Config.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Config.Data.Repository;

internal interface IConfigurationOptionsEnvironmentVariableRepository : IRepository
{
    ConfigurationOptionsDomainModel GetConfigurationOptions { get; }
}
