using System.Security;

namespace LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping
{
    public class ConfigurationModel : IBootstrappingModel
    {
        public string ClientId { get; set; }

        public string FolderFullService { get; set; }

        public string FolderFullServiceAudio { get; set; }

        public string FolderPreaching { get; set; }

        public string FolderSpecials { get; set; }

        public SecureString Password { get; set; }

        public string TenantId { get; set; }

        public string Username { get; set; }
    }
}
