using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Services.Common;
using System;
using System.Security;
using static LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping.ConfigurationModel;

namespace LiteralLifeChurch.ArchiveManagerApi.Services
{
    public static class ConfigurationService
    {

        private static readonly string ClientIdName = "ARCHIVE_MANAGER_API_CLIENT_ID";
        private static readonly string FaultResponseName = "ARCHIVE_MANAGER_API_FAULT_RESPONSE";
        private static readonly string FolderFullServiceName = "ARCHIVE_MANAGER_API_FOLDER_FULL_SERVICE";
        private static readonly string FolderFullServiceAudioName = "ARCHIVE_MANAGER_API_FOLDER_FULL_SERVICE_AUDIO";
        private static readonly string FolderPreachingName = "ARCHIVE_MANAGER_API_FOLDER_PREACHING";
        private static readonly string FolderSpecialsName = "ARCHIVE_MANAGER_API_FOLDER_SPECIALS";
        private static readonly string PasswordName = "ARCHIVE_MANAGER_API_PASSWORD";
        private static readonly string TenantIdName = "ARCHIVE_MANAGER_API_TENANT_ID";
        private static readonly string UsernameName = "ARCHIVE_MANAGER_API_USERNAME";
        private static readonly string ValidYearLowerBoundName = "ARCHIVE_MANAGER_API_VALID_YEAR_LOWER_BOUND";

        public static ConfigurationModel GetConfiguration()
        {
            ConfigurationModel model = new ConfigurationModel
            {
                ClientId = Environment.GetEnvironmentVariable(ClientIdName),
                FaultResponse = ConvertToFaultReponse(Environment.GetEnvironmentVariable(FaultResponseName).Trim().ToLowerInvariant()),
                FolderFullService = Environment.GetEnvironmentVariable(FolderFullServiceName).Trim().ToLowerInvariant(),
                FolderFullServiceAudio = Environment.GetEnvironmentVariable(FolderFullServiceAudioName).Trim().ToLowerInvariant(),
                FolderPreaching = Environment.GetEnvironmentVariable(FolderPreachingName).Trim().ToLowerInvariant(),
                FolderSpecials = Environment.GetEnvironmentVariable(FolderSpecialsName).Trim().ToLowerInvariant(),
                Password = ConvertToSecureString(Environment.GetEnvironmentVariable(PasswordName)),
                TenantId = Environment.GetEnvironmentVariable(TenantIdName),
                Username = Environment.GetEnvironmentVariable(UsernameName),
                ValidYearLowerBound = Convert.ToInt32(Environment.GetEnvironmentVariable(ValidYearLowerBoundName))
            };

            LoggerService.Info("Extracted configuration", LoggerService.Bootstrapping);
            return model;
        }

        // region Helper Methods

        private static FaultResponseType ConvertToFaultReponse(string input)
        {
            return input switch
            {
                "default" => FaultResponseType.Default,
                "skip" => FaultResponseType.Skip,
                _ => FaultResponseType.Terminate,
            };
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

        // endregion
    }
}
