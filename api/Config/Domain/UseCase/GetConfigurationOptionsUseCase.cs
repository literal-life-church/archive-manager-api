using LiteralLifeChurch.ArchiveManagerApi.Config.Data.Repository;
using LiteralLifeChurch.ArchiveManagerApi.Config.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Config.Domain.UseCase;

internal class GetConfigurationOptionsUseCase : IGetConfigurationOptionsUseCase
{
    private readonly IConfigurationOptionsEnvironmentVariableRepository
        _configurationOptionsEnvironmentVariableRepository;

    public GetConfigurationOptionsUseCase(
        IConfigurationOptionsEnvironmentVariableRepository configurationOptionsEnvironmentVariableRepository)
    {
        _configurationOptionsEnvironmentVariableRepository = configurationOptionsEnvironmentVariableRepository;
    }

    public ConfigurationOptionsDomainModel Execute()
    {
        return _configurationOptionsEnvironmentVariableRepository.GetConfigurationOptions;
    }
}
