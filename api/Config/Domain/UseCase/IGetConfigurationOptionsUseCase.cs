using LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.UseCase;
using LiteralLifeChurch.ArchiveManagerApi.Config.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Config.Domain.UseCase;

internal interface IGetConfigurationOptionsUseCase : IUseCase
{
    ConfigurationOptionsDomainModel Execute();
}
