using System.Collections.Generic;
using LiteralLifeChurch.ArchiveManagerApi.Clean.Data.Repository;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Extraction.Data.Repository;

internal interface IMediaMetadataMemoryRepository : IRepository
{
    List<MediaMetadataDomainModel> ExtractMetadata(List<FileDomainModel> files);
}
