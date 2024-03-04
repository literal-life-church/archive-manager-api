using System.Collections.Generic;
using LiteralLifeChurch.ArchiveManagerApi.Clean.Data.Mapper;
using LiteralLifeChurch.ArchiveManagerApi.Correlation.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Correlation.Data.Mapper;

internal interface IMetadataToCorrelationsMapper : IMapper<List<MediaMetadataDomainModel>, CorrelationsDomainModel>
{
}
