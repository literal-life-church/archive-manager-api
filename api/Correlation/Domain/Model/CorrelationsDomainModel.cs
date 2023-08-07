using System;
using System.Collections.Generic;
using LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Correlation.Domain.Model;

internal class CorrelationsDomainModel : IDomainModel
{
    public CorrelationsDomainModel(List<CategoryModel> categories, List<EventModel> events, List<SeriesModel> series,
        List<SpeakerModel> speakers)
    {
        Categories = categories;
        Events = events;
        Series = series;
        Speakers = speakers;
    }

    public List<CategoryModel> Categories { get; set; }

    public List<EventModel> Events { get; set; }

    public List<SeriesModel> Series { get; set; }

    public List<SpeakerModel> Speakers { get; set; }

    internal class CategoryModel
    {
        public CategoryModel(int displayOrder, string name, string id, int occurrences)
        {
            DisplayOrder = displayOrder;
            Name = name;
            Id = id;
            Occurrences = occurrences;
        }

        public int DisplayOrder { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public int Occurrences { get; set; }
    }

    internal class CategoryOccurrenceModel
    {
        public CategoryOccurrenceModel(int displayOrder, string name, string id)
        {
            DisplayOrder = displayOrder;
            Name = name;
            Id = id;
        }

        public int DisplayOrder { get; set; }

        public string Name { get; set; }
        public string Id { get; set; }
    }

    internal class EventModel
    {
        public EventModel(DateTime date, string id, List<MediaEntryModel> mediaEntries)
        {
            Date = date;
            Id = id;
            MediaEntries = mediaEntries;
        }

        public DateTime Date { get; set; }
        public string Id { get; set; }

        public List<MediaEntryModel> MediaEntries { get; set; }
    }

    internal class MediaEntryModel
    {
        public MediaEntryModel(CategoryOccurrenceModel category, List<SpeakerOccurrenceModel> speakers,
            SeriesOccurrenceModel? series,
            string title)
        {
            Category = category;
            Speakers = speakers;
            Series = series;
            Title = title;
        }

        public CategoryOccurrenceModel Category { get; set; }
        public List<SpeakerOccurrenceModel> Speakers { get; set; }
        public SeriesOccurrenceModel? Series { get; set; }
        public string Title { get; set; }
    }

    internal class SeriesModel
    {
        public SeriesModel(string name, string id, int totalParts)
        {
            Name = name;
            Id = id;
            TotalParts = totalParts;
        }

        public string Name { get; set; }
        public string Id { get; set; }
        public int TotalParts { get; set; }
    }

    internal class SeriesOccurrenceModel
    {
        public SeriesOccurrenceModel(string? name, string id, int occurrencePart, int totalParts)
        {
            Name = name;
            Id = id;
            OccurrencePart = occurrencePart;
            TotalParts = totalParts;
        }

        public string? Name { get; set; }
        public string Id { get; set; }
        public int OccurrencePart { get; set; }
        public int TotalParts { get; set; }
    }

    internal class SpeakerModel
    {
        public SpeakerModel(string name, string id, string occurrences)
        {
            Name = name;
            Id = id;
            Occurrences = occurrences;
        }

        public string Name { get; set; }
        public string Id { get; set; }
        public string Occurrences { get; set; }
    }

    internal class SpeakerOccurrenceModel
    {
        public SpeakerOccurrenceModel(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; set; }
        public string Id { get; set; }
    }
}
