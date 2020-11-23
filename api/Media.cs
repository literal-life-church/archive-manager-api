using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow;
using LiteralLifeChurch.ArchiveManagerApi.Services;
using LiteralLifeChurch.ArchiveManagerApi.Services.Common;
using LiteralLifeChurch.ArchiveManagerApi.Services.IndexerWorkflow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteralLifeChurch.ArchiveManagerApi
{
    public class Media
    {
        /*private readonly TelemetryClient TelemetryClient;

        public Media(TelemetryConfiguration telemetryConfiguration)
        {
            TelemetryClient = new TelemetryClient(telemetryConfiguration);
        }*/

        [FunctionName("Media")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "consumer/media")] HttpRequest req,
            ILogger log)
        {
            //TelemetryClient.TrackEvent("Start");

            using (LoggerService.Init(log))
            {
                ConfigurationModel config = ConfigurationService.GetConfiguration();
                IndexerWorkflowPipeline indexer = new IndexerWorkflowPipeline(config);
                List<MediaModel> transformedMedia = await indexer.Run();

                IEnumerable<string> dates = transformedMedia.Select(x =>
                {
                    return "Sermon Name: " + x.Name.Normalized + "\n" +
                    "Speakers: " + string.Join(", ", x.Speakers.Names.Select(y => y.Normalized)) + "\n" +
                    "Date: " + x.Date.DateString.Normalized + x.Date.Modifier.Symbol + " (" + x.Date.Stamp.ToString() + ")\n" +
                    "Type: " + x.Type.Name + "\n";
                });

                return new OkObjectResult(string.Join("\n", dates));
            }
        }
    }
}
