using System;
using System.Collections.Generic;
using LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.Model;

internal class MediaMetadataDomainModel : IDomainModel
{
    public DateTime Date { get; set; }
    public List<string> Speakers { get; set; }
    public string Title { get; set; }
}
