using System.Collections.Generic;
using LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.UseCase;
using LiteralLifeChurch.ArchiveManagerApi.Correlation.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Correlation.Domain.UseCase;

internal interface IExtractCorrelationsFromMetadataUseCase : IUseCase
{
    CorrelationsDomainModel Execute(List<MediaMetadataDomainModel> input);
}
