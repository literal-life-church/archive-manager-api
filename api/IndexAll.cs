using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using static System.Formats.Asn1.AsnWriter;

namespace LiteralLifeChurch.ArchiveManagerApi
{
    public static class IndexAll
    {
        private static readonly string ClientId = Environment.GetEnvironmentVariable("ARCHIVE_MANAGER_API_CLIENT_ID");
        private static readonly string TenantId = Environment.GetEnvironmentVariable("ARCHIVE_MANAGER_API_TENANT_ID");
        private static readonly string Username = Environment.GetEnvironmentVariable("ARCHIVE_MANAGER_API_SERVICE_ACCOUNT_USERNAME");
        private static readonly string Password = Environment.GetEnvironmentVariable("ARCHIVE_MANAGER_API_SERVICE_ACCOUNT_PASSWORD");

        private static readonly string[] Scopes = { "Files.Read.All" };

        [FunctionName("IndexAll")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            return new()
            {
                await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo")
            };
        }

        [FunctionName(nameof(SayHello))]
        public static async Task<string> SayHello([ActivityTrigger] string name, ILogger log)
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

            return stringBuilder.ToString();
        }

        [FunctionName("IndexAll_Start")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "management/index")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            string instanceId = await starter.StartNewAsync("IndexAll");
            log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}