using System.Collections.Generic;
using LiteralLifeChurch.ArchiveManagerApi.Clean.Data.Mapper;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Extraction.Data.Mapper;

internal interface IFileToMediaMetadataMapper : IMapper<List<FileDomainModel>, List<MediaMetadataDomainModel>>
{
}
