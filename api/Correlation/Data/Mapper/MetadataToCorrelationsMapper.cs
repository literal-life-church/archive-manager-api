using LiteralLifeChurch.ArchiveManagerApi.Config.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.Config.Domain.UseCase;
using LiteralLifeChurch.ArchiveManagerApi.Correlation.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.Extensions;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace LiteralLifeChurch.ArchiveManagerApi.Correlation.Data.Mapper;

internal class MetadataToCorrelationsMapper : IMetadataToCorrelationsMapper
{
    private readonly ConfigurationOptionsDomainModel _configurationOptions;
    private readonly IStringToStableIdMapper _stringToStableIdMapper;

    public MetadataToCorrelationsMapper(IGetConfigurationOptionsUseCase configurationOptionsUseCase,
        IStringToStableIdMapper stringToStableIdMapper)
    {
        _configurationOptions = configurationOptionsUseCase.Execute();
        _stringToStableIdMapper = stringToStableIdMapper;
    }

    public CorrelationsDomainModel Map(List<MediaMetadataDomainModel> input)
    {
        var categories = PreLoadCategoriesFromConfiguration(); // Known in advance and cached now
        var events = new Dictionary<string, CorrelationsDomainModel.EventModel>();
        var series = new Dictionary<string, CorrelationsDomainModel.SeriesModel>();
        var speakers = new Dictionary<string, CorrelationsDomainModel.SpeakerModel>();

        input
            .ForEach(mediaMetadata => { LoadEventIntoCache(ref events, mediaMetadata); });
        
        var sortedCategoryList = categories
            .Values
            .OrderBy(category => category.DisplayOrder)
            .ToList();

        var sortedEventsList = events
            .Values
            .OrderBy(eventModel => eventModel.Date)
            .ToList();

        return new CorrelationsDomainModel(
            new List<CorrelationsDomainModel.CategoryModel>(),
            sortedEventsList,
            new List<CorrelationsDomainModel.SeriesModel>(),
            new List<CorrelationsDomainModel.SpeakerModel>()
        );
    }

    private void LoadEventIntoCache(ref Dictionary<string, CorrelationsDomainModel.EventModel> events,
        MediaMetadataDomainModel mediaMetadata)
    {
        // For the format code, see:
        // https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings#RFC1123
        var eventId = _stringToStableIdMapper.Map(mediaMetadata.Date.ToString("R"));
        
        if (!events.ContainsKey(eventId))
        {
            events[eventId] = new CorrelationsDomainModel.EventModel(
                mediaMetadata.Date,
                new List<CorrelationsDomainModel.MediaEntryModel>()
            );
        }

        events[eventId].Media.Add(new CorrelationsDomainModel.MediaEntryModel(
            mediaMetadata.FileId,
            mediaMetadata.Title
        ));
    }

    private Dictionary<string, CorrelationsDomainModel.CategoryModel> PreLoadCategoriesFromConfiguration()
    {
        var cachedCategories = new Dictionary<string, CorrelationsDomainModel.CategoryModel>();

        _configurationOptions
            .Categories
            .ForEachIndexed((index, categoryName) =>
            {
                var id = _stringToStableIdMapper.Map(categoryName);
                if (cachedCategories.ContainsKey(id)) return;

                cachedCategories[id] = new CorrelationsDomainModel.CategoryModel(
                    index,
                    categoryName,
                    id,
                    0
                );
            });

        return cachedCategories;
    }

    private Dictionary<string, CorrelationsDomainModel.SpeakerModel> CacheSpeakersFromConfiguration(
        List<MediaMetadataDomainModel> mediaMetadata)
    {
        var speakerIndex = 0;
        var cachedSpeakers = new Dictionary<string, CorrelationsDomainModel.SpeakerModel>();

        //mediaMetadata
        //    .ForEach(mediaItem =>
        //    {
        //        var id: string = _stringToStableIdMapper.Map(mediaItem.Speakers);
        //    });

        return cachedSpeakers;
    }
}
