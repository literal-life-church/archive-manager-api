using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Services;
using LiteralLifeChurch.ArchiveManagerApi.Services.Common;
using LiteralLifeChurch.ArchiveManagerApi.Services.Crawler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
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
                FileListService fileListService = new FileListService(config);
                List<DriveItem> media = await fileListService.ListAllMediaItems();

                IEnumerable<string> names = media.Select(x => x.Name);
                return new OkObjectResult(string.Join("\n", names));
            }
        }
    }
}
