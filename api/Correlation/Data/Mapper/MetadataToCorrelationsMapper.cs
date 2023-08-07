using System;
using System.Collections.Generic;
using System.Linq;
using LiteralLifeChurch.ArchiveManagerApi.Config.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.Config.Domain.UseCase;
using LiteralLifeChurch.ArchiveManagerApi.Correlation.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Correlation.Data.Mapper;

internal class MetadataToCorrelationsMapper : IMetadataToCorrelationsMapper
{
    private readonly ConfigurationOptionsDomainModel _configurationOptions;
    private readonly IStringToHashMapper _stringToHashMapper;

    public MetadataToCorrelationsMapper(IGetConfigurationOptionsUseCase configurationOptionsUseCase,
        IStringToHashMapper stringToHashMapper)
    {
        _configurationOptions = configurationOptionsUseCase.Execute();
        _stringToHashMapper = stringToHashMapper;
    }

    public CorrelationsDomainModel Map(List<MediaMetadataDomainModel> input)
    {
        throw new NotImplementedException();
    }

    private List<CorrelationsDomainModel.CategoryModel> GetCategoriesFromConfiguration()
    {
        return _configurationOptions
            .Categories
            .Select((category, index) => new CorrelationsDomainModel.CategoryModel(
                index + 1,
                category,
                _stringToHashMapper.Map(category),
                0
            ))
            .ToList();
    }
}
