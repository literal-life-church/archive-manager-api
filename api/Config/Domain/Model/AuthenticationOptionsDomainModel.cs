#nullable disable // Since it is populated by the Functions Configuration Builder in Startup.cs
using LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.Model;

namespace LiteralLifeChurch.ArchiveManagerApi.Config.Domain.Model;

internal class AuthenticationOptionsDomainModel : IDomainModel
{
    public string ClientId { get; set; }
    public string ServiceAccountPassword { get; set; }
    public string ServiceAccountUsername { get; set; }
    public string TenantId { get; set; }
}
