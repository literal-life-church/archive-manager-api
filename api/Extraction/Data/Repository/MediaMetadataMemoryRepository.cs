using System.Collections.Generic;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Data.Mapper;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Extraction.Data.Repository;

internal class MediaMetadataMemoryRepository : IMediaMetadataMemoryRepository
{
    private readonly IFileToMediaMetadataMapper _fileToMediaMetadataMapper;

    public MediaMetadataMemoryRepository(IFileToMediaMetadataMapper fileToMediaMetadataMapper)
    {
        _fileToMediaMetadataMapper = fileToMediaMetadataMapper;
    }

    public List<MediaMetadataDomainModel> ExtractMetadata(List<FileDomainModel> files)
    {
        return _fileToMediaMetadataMapper.Map(files);
    }
}
