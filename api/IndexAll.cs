using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LiteralLifeChurch.ArchiveManagerApi.Correlation.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.Correlation.Domain.UseCase;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.UseCase;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.UseCase;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace LiteralLifeChurch.ArchiveManagerApi;

internal class IndexAll
{
    private readonly IExtractCorrelationsFromMetadataUseCase _extractCorrelationsFromMetadataUseCase;
    private readonly IExtractMediaMetadataFromFilesUseCase _extractMediaMetadataFromFilesUseCase;
    private readonly IGetAllSharedFilesUseCase _getAllSharedFilesUseCase;

    public IndexAll(
        IExtractCorrelationsFromMetadataUseCase extractCorrelationsFromMetadataUseCase,
        IExtractMediaMetadataFromFilesUseCase extractMediaMetadataFromFilesUseCase,
        IGetAllSharedFilesUseCase getAllSharedFilesUseCase)
    {
        _extractCorrelationsFromMetadataUseCase = extractCorrelationsFromMetadataUseCase;
        _extractMediaMetadataFromFilesUseCase = extractMediaMetadataFromFilesUseCase;
        _getAllSharedFilesUseCase = getAllSharedFilesUseCase;
    }

    [FunctionName("IndexAll")]
    public async Task<CorrelationsDomainModel> RunOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
    {
        return await context.CallActivityAsync<CorrelationsDomainModel>(nameof(CrawlAllSharedFiles),
            null);
    }

    [FunctionName(nameof(CrawlAllSharedFiles))]
    public async Task<CorrelationsDomainModel> CrawlAllSharedFiles(
        [ActivityTrigger] IDurableActivityContext context)
    {
        var files = await _getAllSharedFilesUseCase.ExecuteAsync();
        var mediaMetadata = _extractMediaMetadataFromFilesUseCase.Execute(files);
        var categories = _extractCorrelationsFromMetadataUseCase.Execute(mediaMetadata);
        return categories;
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
