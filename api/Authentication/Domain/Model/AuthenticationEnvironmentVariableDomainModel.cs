namespace LiteralLifeChurch.ArchiveManagerApi.Authentication.Domain.Model
{
    internal class AuthenticationEnvironmentVariableDomainModel
    {
        public string ClientId { get; set; }
        public string TenantId { get; set; }
        public string ServiceAccountUsername { get; set; }
        public string ServiceAccountPassword { get; set; }
    }
}
