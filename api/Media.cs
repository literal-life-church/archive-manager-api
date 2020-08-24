using LiteralLifeChurch.ArchiveManagerApi.Models.Bootstrapping;
using LiteralLifeChurch.ArchiveManagerApi.Services;
using LiteralLifeChurch.ArchiveManagerApi.Services.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
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
                Drive drive = await AuthenticationService.GetDriveAsync(config);

                string first = drive.Name;
                string responseMessage = $"Drive name is: {first}";

                return new OkObjectResult(responseMessage);
            }
        }
    }
}
