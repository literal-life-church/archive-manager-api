using System.Collections.Generic;
using LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.UseCase;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.UseCase;

internal interface IExtractMediaMetadataFromFilesUseCase : IUseCase
{
    List<MediaMetadataDomainModel> Execute(List<FileDomainModel> input);
}
