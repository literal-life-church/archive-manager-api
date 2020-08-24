using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Services.Common;
using System;
using System.Security;

namespace LiteralLifeChurch.ArchiveManagerApi.Services
{
    public static class ConfigurationService
    {

        private static readonly string ClientIdName = "ARCHIVE_MANAGER_API_CLIENT_ID";
        private static readonly string PasswordName = "ARCHIVE_MANAGER_API_PASSWORD";
        private static readonly string TenantIdName = "ARCHIVE_MANAGER_API_TENANT_ID";
        private static readonly string UsernameName = "ARCHIVE_MANAGER_API_USERNAME";

        public static ConfigurationModel GetConfiguration()
        {
            ConfigurationModel model = new ConfigurationModel
            {
                ClientId = Environment.GetEnvironmentVariable(ClientIdName),
                Password = ConvertToSecureString(Environment.GetEnvironmentVariable(PasswordName)),
                TenantId = Environment.GetEnvironmentVariable(TenantIdName),
                Username = Environment.GetEnvironmentVariable(UsernameName)
            };

            LoggerService.Info("Extracted configuration", LoggerService.Bootstrapping);
            return model;
        }

        private static SecureString ConvertToSecureString(string input)
        {
            SecureString secure = new SecureString();

            foreach (char character in input)
            {
                secure.AppendChar(character);
            }

            return secure;
        }
    }
}
