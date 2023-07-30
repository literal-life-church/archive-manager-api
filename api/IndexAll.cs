using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.UseCase;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.UseCase;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace LiteralLifeChurch.ArchiveManagerApi;

internal class IndexAll
{
    private readonly IExtractMediaMetadataFromFilesUseCase _extractMediaMetadataFromFilesUseCase;
    private readonly IGetAllSharedFilesUseCase _getAllSharedFilesUseCase;

    public IndexAll(IExtractMediaMetadataFromFilesUseCase extractMediaMetadataFromFilesUseCase,
        IGetAllSharedFilesUseCase getAllSharedFilesUseCase)
    {
        _extractMediaMetadataFromFilesUseCase = extractMediaMetadataFromFilesUseCase;
        _getAllSharedFilesUseCase = getAllSharedFilesUseCase;
    }

    [FunctionName("IndexAll")]
    public async Task<List<MediaMetadataDomainModel>> RunOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
    {
        return await context.CallActivityAsync<List<MediaMetadataDomainModel>>(nameof(CrawlAllSharedFiles), null);
    }

    [FunctionName(nameof(CrawlAllSharedFiles))]
    public async Task<List<MediaMetadataDomainModel>> CrawlAllSharedFiles(
        [ActivityTrigger] IDurableActivityContext context)
    {
        var files = await _getAllSharedFilesUseCase.ExecuteAsync();
        var mediaMetadata = _extractMediaMetadataFromFilesUseCase.Execute(files);
        return mediaMetadata;
    }

    [FunctionName("IndexAll_Start")]
    public async Task<HttpResponseMessage> HttpStart(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "management/index")]
        HttpRequestMessage req,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log)
    {
        var instanceId = await starter.StartNewAsync("IndexAll");
        log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}
