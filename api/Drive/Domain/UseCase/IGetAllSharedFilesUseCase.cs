using System.Collections.Generic;
using System.Threading.Tasks;
using LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.UseCase;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.UseCase;

internal interface IGetAllSharedFilesUseCase : IUseCase
{
    Task<List<FileDomainModel>> ExecuteAsync();
}
