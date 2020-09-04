using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Services.Common;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace LiteralLifeChurch.ArchiveManagerApi.Services
{
    public class AuthenticationService
    {
        private static GraphServiceClient Client = null;
        private static readonly string[] Scopes = new string[] { "Files.Read.All", "Sites.Read.All" };

        public static async Task<Drive> GetDriveAsync(ConfigurationModel config)
        {
            GraphServiceClient client = GetClientAsync(config);

            return await client
                .Me
                .Drive
                .Request()
                .WithUsernamePassword(
                    config.Username,
                    config.Password
                )
                .GetAsync();
        }

        public static async Task<IDriveItemChildrenCollectionPage> GetChildrenAsync(ConfigurationModel config, DriveItem item)
        {
            GraphServiceClient client = GetClientAsync(config);
            string driveId;
            string itemId;

            if (item.RemoteItem != null)
            {
                driveId = item.RemoteItem.ParentReference.DriveId;
                itemId = item.RemoteItem.Id;
            }
            else
            {
                driveId = item.ParentReference.DriveId;
                itemId = item.Id;
            }

            return await client
                .Drives[driveId]
                .Items[itemId]
                .Children
                .Request()
                .WithUsernamePassword(
                    config.Username,
                    config.Password
                )
                .GetAsync();
        }

        public static async Task<IDriveSharedWithMeCollectionPage> GetSharedDrivesAsync(ConfigurationModel config)
        {
            GraphServiceClient client = GetClientAsync(config);

            return await client
                .Me
                .Drive
                .SharedWithMe()
                .Request()
                .WithUsernamePassword(
                    config.Username,
                    config.Password
                )
                .GetAsync();
        }

        private static GraphServiceClient GetClientAsync(ConfigurationModel config)
        {
            if (Client != null)
            {
                return Client;
            }

            IPublicClientApplication application = PublicClientApplicationBuilder
                .Create(config.ClientId)
                .WithTenantId(config.TenantId)
                .Build();

            UsernamePasswordProvider provider = new UsernamePasswordProvider(application, Scopes);
            GraphServiceClient client = new GraphServiceClient(provider);
            Client = client;

            LoggerService.Info("Created credential client", LoggerService.Bootstrapping);
            return client;
        }
    }
}
