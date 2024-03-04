#nullable disable // Since it is populated by the Functions Configuration Builder in Startup.cs
using System.Collections.Generic;
using LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Config.Domain.Model;

internal class ConfigurationOptionsDomainModel : IDomainModel
{
    public List<string> Categories { get; set; }
}
