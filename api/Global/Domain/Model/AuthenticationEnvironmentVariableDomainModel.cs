#nullable disable // Since it is populated by the Functions Configuration Builder in Startup.cs
namespace LiteralLifeChurch.ArchiveManagerApi.Global.Domain.Model;

internal class AuthenticationEnvironmentVariableDomainModel
{
    public string ClientId { get; set; }
    public string ServiceAccountPassword { get; set; }
    public string ServiceAccountUsername { get; set; }
    public string TenantId { get; set; }
}
