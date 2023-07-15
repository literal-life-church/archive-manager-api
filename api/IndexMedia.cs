using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Graph;
using System;
using System.Text;

namespace LiteralLifeChurch.ArchiveManagerApi
{
    public static class IndexMedia
    {
        private const string ClientId = "FAKE";
        private const string TenantId = "FAKE";
        private const string Username = "FAKE";
        private const string Password = "FAKE";

        private static readonly string[] Scopes = { "Files.Read.All" };

        [FunctionName("IndexMedia")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            // See: https://learn.microsoft.com/en-us/graph/sdks/choose-authentication-providers?tabs=CS#usernamepassword-provider
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            var credential = new UsernamePasswordCredential(Username, Password, TenantId, ClientId, options);
            var graphClient = new GraphServiceClient(credential, Scopes);

            // See: https://github.com/microsoftgraph/msgraph-sdk-dotnet/issues/1737#issuecomment-1475774056
            var driveItem = await graphClient.Me.Drive.GetAsync();
            var sharedFiles = await graphClient.Drives[driveItem.Id].SharedWithMe.GetAsync();

            var stringBuilder = new StringBuilder();


            foreach (var file in sharedFiles.Value)
            {
                stringBuilder.AppendLine($"Name: {file.Name}, Id: {file.Id}, Drive ID: {file.RemoteItem.ParentReference.DriveId}");

                try
                {

                    var firstLevel = await graphClient.Drives[file.RemoteItem.ParentReference.DriveId].Items[file.Id]
                        .Children.GetAsync();

                    foreach (var subfolders in firstLevel.Value)
                    {
                        stringBuilder.AppendLine($"Name: {subfolders.Name}, Id: {subfolders.Id}");
                    }
                }
                catch (Exception e)
                {
                    stringBuilder.AppendLine($"Error: {e.Message}");
                }

                stringBuilder.AppendLine("");
            }

            return new OkObjectResult(stringBuilder.ToString());
        }
    }
}
