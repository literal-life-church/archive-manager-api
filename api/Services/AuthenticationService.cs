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
        private static string[] scopes = new string[] { "Files.Read.All", "Sites.Read.All" };

        public static async Task<Drive> GetDriveAsync(ConfigurationModel config)
        {
            GraphServiceClient client = GetClientAsync(config);

            return await client.Me.Drive.Request().WithUsernamePassword(
                config.Username,
                config.Password
            ).GetAsync();
        }

        private static GraphServiceClient GetClientAsync(ConfigurationModel config)
        {
            IPublicClientApplication application = PublicClientApplicationBuilder
                .Create(config.ClientId)
                .WithTenantId(config.TenantId)
                .Build();

            UsernamePasswordProvider provider = new UsernamePasswordProvider(application, scopes);
            GraphServiceClient client = new GraphServiceClient(provider);

            LoggerService.Info("Created credential client", LoggerService.Bootstrapping);
            return client;
        }
    }
}
