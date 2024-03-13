using System;
using System.Collections.Generic;
using LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Correlation.Domain.Model;

/// <summary>
///     <para>
///         Creates a representation of all of the metadata that is extracted from
///         a media file in OneDrive and structures it in a way that clearly shows
///         the relationships that is formed between each of the media files.
///     </para>
///     <para>
///         For example, it is completely possible to have several media files that
///         were generated on the same day. Thus, they would be listed under the same
///         <see cref="Events">event</see> entry, but would likely be cataloged into
///         individual <see cref="Categories">categories</see>.
///     </para>
///     <para>
///         Another common  scenario would be to have one
///         <see cref="Speakers">speaker</see> who is featured in multiple media
///         files. This would result in the speaker being listed under multiple
///         <see cref="Events">events</see> and also within the
///         <see cref="Speakers">speakers</see> list.
///     </para>
///     <list type="bullet">
///         <item>
///             <term>Categories</term>
///             <description>
///                 <see cref="CategoryModel" />
///             </description>
///         </item>
///         <item>
///             <term>Events</term>
///             <description>
///                 <see cref="EventModel" />
///             </description>
///         </item>
///         <item>
///             <term>Series</term>
///             <description>
///                 <see cref="SeriesModel" />
///             </description>
///         </item>
///         <item>
///             <term>Speakers</term>
///             <description>
///                 <see cref="SpeakerModel" />
///             </description>
///         </item>
///     </list>
/// </summary>
internal class CorrelationsDomainModel : IDomainModel
{
    /// <summary>
    ///     A bulk payload of all possible metadata that can be extracted from
    ///     a media file in OneDrive.
    /// </summary>
    /// <param name="categories">All of the event category types that the media was cataloged into.</param>
    /// <param name="events">All points in time during which one or more media entries were produced.</param>
    /// <param name="series">A listing of all series into which 2 or more <paramref name="events" /> are cataloged.</param>
    /// <param name="speakers">Names of people who participated in each of the <paramref name="events" />.</param>
    /// <seealso cref="CategoryModel" />
    /// <seealso cref="EventModel" />
    /// <seealso cref="SeriesModel" />
    /// <seealso cref="SpeakerModel" />
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

    /// <summary>
    ///     <para>
    ///         A bucket into which a media file is cataloged.
    ///     </para>
    ///     <para>
    ///         Broadly speaking, there are only going to be a handful of categories
    ///         given within OneDrive, these are represented as the name of the parent
    ///         folder from which the media file was extracted.
    ///     </para>
    ///     <para>
    ///         The <see cref="DisplayOrder" /> is ultimately given by the application's
    ///         configuration options. Thus, this order can be specified at install time
    ///         or on a development machine, within the <c>local.settings.json</c> file.
    ///     </para>
    /// </summary>
    internal class CategoryModel
    {
        /// <summary>
        ///     Represents a bucket into which a media file is cataloged.
        /// </summary>
        /// <param name="displayOrder">The order in which the category should be displayed in the UI.</param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="occurrences"></param>
        public CategoryModel(int displayOrder, string name, string id, int occurrences)
        {
            DisplayOrder = displayOrder;
            Name = name;
            Id = id;
            Occurrences = occurrences;
        }

        /// <summary>
        ///     The order in which the category should be displayed in the UI. Also indicates
        ///     the presentation order of the media files when more than one file is mapped to
        ///     a single <see cref="Events">event</see>, but within a different
        ///     <see cref="CategoryModel">category</see>.
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        ///     The friendly name of the category, as it should be displayed in the UI.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The unique identifier for the category. This is used for searches and filtering.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     The number of times that this category has been used to catalog a media file.
        /// </summary>
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

    /// <summary>
    ///     This is an example.
    /// </summary>
    internal class EventModel
    {
        public EventModel(DateTime date, List<MediaEntryModel> media)
        {
            Date = date;
            Media = media;
        }

        public DateTime Date { get; set; }

        public List<MediaEntryModel> Media { get; set; }
    }

    internal class MediaEntryModel
    {
        public MediaEntryModel(
            // CategoryOccurrenceModel category,
            // SeriesOccurrenceModel? series,
            string id,
            List<string> speakers,
            string title)
        {
            // Category = category;
            // Series = series;
            Id = id;
            Speakers = speakers;
            Title = title;
        }

        // public CategoryOccurrenceModel Category { get; set; }
        // public SeriesOccurrenceModel? Series { get; set; }
        public string Id { get; set; }
        public List<string> Speakers { get; set; }
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
        public SpeakerModel(string id, string name)
        {
            Id = id;
            Name = name;
        }
        
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
