using LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.UseCase;
using Microsoft.Graph;

namespace LiteralLifeChurch.ArchiveManagerApi.Authentication.Domain.UseCase
{
    internal interface IGetAuthenticatedClientUseCase : IUseCase
    {
        GraphServiceClient Execute();
    }
}
