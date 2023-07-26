using LiteralLifeChurch.ArchiveManagerApi.Authentication.Domain.UseCase;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LiteralLifeChurch.ArchiveManagerApi
{
    internal class IndexAll
    {
        private readonly GraphServiceClient _client;

        public IndexAll(IGetAuthenticatedClientUseCase getAuthenticatedClientUseCase)
        {
            _client = getAuthenticatedClientUseCase.Execute();
        }

        [FunctionName("IndexAll")]
        public async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            return new()
            {
                await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo")
            };
        }

        [FunctionName(nameof(SayHello))]
        public async Task<string> SayHello([ActivityTrigger] string name, ILogger log)
        {
            // See: https://github.com/microsoftgraph/msgraph-sdk-dotnet/issues/1737#issuecomment-1475774056
            var driveItem = await _client.Me.Drive.GetAsync();
            var sharedFiles = await _client.Drives[driveItem.Id].SharedWithMe.GetAsync();

            var stringBuilder = new StringBuilder();


            foreach (var file in sharedFiles.Value)
            {
                stringBuilder.AppendLine($"Name: {file.Name}, Id: {file.Id}, Drive ID: {file.RemoteItem.ParentReference.DriveId}");

                try
                {

                    var firstLevel = await _client.Drives[file.RemoteItem.ParentReference.DriveId].Items[file.Id]
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
        public async Task<HttpResponseMessage> HttpStart(
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