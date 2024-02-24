using System.Collections.Generic;
using LiteralLifeChurch.ArchiveManagerApi.Config.Domain.UseCase;
using LiteralLifeChurch.ArchiveManagerApi.Correlation.Data.Mapper;
using LiteralLifeChurch.ArchiveManagerApi.Correlation.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Correlation.Domain.UseCase;

internal class ExtractCorrelationsFromMetadataUseCase : IExtractCorrelationsFromMetadataUseCase
{
    private readonly IMetadataToCorrelationsMapper _metadataToCorrelationsMapper;

    public ExtractCorrelationsFromMetadataUseCase(
        IMetadataToCorrelationsMapper metadataToCorrelationsMapper,
        IGetConfigurationOptionsUseCase getConfigurationOptionsUseCase)
    {
        _metadataToCorrelationsMapper = metadataToCorrelationsMapper;
    }

    public CorrelationsDomainModel Execute(List<MediaMetadataDomainModel> input)
    {
        return _metadataToCorrelationsMapper.Map(input);
    }
}
