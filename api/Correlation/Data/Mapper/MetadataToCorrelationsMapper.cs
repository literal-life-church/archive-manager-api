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
        var categories = LoadCategoriesIntoCache(); // Known in advance and cached now
        var events = new Dictionary<string, CorrelationsDomainModel.EventModel>();
        var series = new Dictionary<string, CorrelationsDomainModel.SeriesModel>();
        var speakers = new Dictionary<string, CorrelationsDomainModel.SpeakerModel>();

        input
            .ForEach(mediaMetadata => LoadEventsIntoCache(ref events, mediaMetadata));

        input
            .ForEach(mediaMetadata => LoadSpeakersIntoCache(ref speakers, mediaMetadata));

        var sortedCategoryList = categories
            .Values
            .OrderBy(category => category.DisplayOrder)
            .ToList();

        var sortedEventsList = events
            .Values
            .OrderBy(eventModel => eventModel.Date)
            .ToList();
        
        var sortedSpeakersList = speakers
            .Values
            .OrderBy(speaker => speaker.Name)
            .ToList();

        return new CorrelationsDomainModel(
            new List<CorrelationsDomainModel.CategoryModel>(),
            sortedEventsList,
            new List<CorrelationsDomainModel.SeriesModel>(),
            sortedSpeakersList
        );
    }

    private Dictionary<string, CorrelationsDomainModel.CategoryModel> LoadCategoriesIntoCache()
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

    private void LoadEventsIntoCache(
        ref Dictionary<string, CorrelationsDomainModel.EventModel> events,
        MediaMetadataDomainModel mediaMetadata)
    {
        // For the format code, see:
        // https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings#RFC1123
        var eventId = _stringToStableIdMapper.Map(mediaMetadata.Date.ToString("R"));
        var speakerIds = mediaMetadata
            .Speakers
            .Select(speaker => _stringToStableIdMapper.Map(speaker))
            .ToList();

        if (!events.ContainsKey(eventId))
        {
            events[eventId] = new CorrelationsDomainModel.EventModel(
                mediaMetadata.Date,
                new List<CorrelationsDomainModel.MediaEntryModel>()
            );
        }

        events[eventId].Media.Add(new CorrelationsDomainModel.MediaEntryModel(
            mediaMetadata.FileId,
            speakerIds,
            mediaMetadata.Title
        ));
    }


    private void LoadSpeakersIntoCache(
        ref Dictionary<string, CorrelationsDomainModel.SpeakerModel> speakers,
        MediaMetadataDomainModel mediaMetadata)
    {
        foreach (var speaker in mediaMetadata.Speakers)
        {
            var speakerId = _stringToStableIdMapper.Map(speaker);
            if (speakers.ContainsKey(speakerId)) continue;

            speakers[speakerId] = new CorrelationsDomainModel.SpeakerModel(
                speakerId,
                speaker
            );
        }
    }
}
