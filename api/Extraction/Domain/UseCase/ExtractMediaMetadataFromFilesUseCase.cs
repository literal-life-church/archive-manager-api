using System.Collections.Generic;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Data.Repository;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.UseCase;

internal class ExtractMediaMetadataFromFilesUseCase : IExtractMediaMetadataFromFilesUseCase
{
    private readonly IMediaMetadataMemoryRepository _mediaMetadataMemoryRepository;

    public ExtractMediaMetadataFromFilesUseCase(IMediaMetadataMemoryRepository mediaMetadataMemoryRepository)
    {
        _mediaMetadataMemoryRepository = mediaMetadataMemoryRepository;
    }

    public List<MediaMetadataDomainModel> Execute(List<FileDomainModel> input)
    {
        return _mediaMetadataMemoryRepository.ExtractMetadata(input);
    }
}
