using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Models.Indexer;
using LiteralLifeChurch.ArchiveManagerApi.Services;
using LiteralLifeChurch.ArchiveManagerApi.Services.Common;
using LiteralLifeChurch.ArchiveManagerApi.Services.Crawler;
using LiteralLifeChurch.ArchiveManagerApi.Services.Indexer;
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
                IndexerPipelineService indexer = new IndexerPipelineService(config);
                List<DriveItem> media = await fileListService.ListAllMediaItems();
                List<MediaModel> transformedMedia = indexer.Transform(media);

                IEnumerable<string> dates = transformedMedia.Select(x => {
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
