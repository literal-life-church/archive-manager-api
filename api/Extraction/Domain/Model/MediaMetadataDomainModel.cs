using System;
using System.Collections.Generic;
using LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.Model;

internal class MediaMetadataDomainModel : IDomainModel
{
    public MediaMetadataDomainModel(
        string category,
        DateTime date,
        string fileId,
        List<string> speakers,
        string? series,
        string title)
    {
        Category = category;
        Date = date;
        FileId = fileId;
        Speakers = speakers;
        Series = series;
        Title = title;
    }

    public string Category { get; set; }

    public DateTime Date { get; set; }
    public string FileId { get; set; }
    public List<string> Speakers { get; set; }
    public string? Series { get; set; }
    public string Title { get; set; }
}
