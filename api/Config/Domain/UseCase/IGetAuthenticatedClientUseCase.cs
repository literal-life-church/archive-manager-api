using LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.UseCase;
using Microsoft.Graph;

namespace LiteralLifeChurch.ArchiveManagerApi.Config.Domain.UseCase;

internal interface IGetAuthenticatedClientUseCase : IUseCase
{
    GraphServiceClient Execute();
}
