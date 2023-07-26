using System.Collections.Generic;
using Azure.Core;
using Microsoft.Graph;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Factories;

internal class GraphServiceClientFactory : IGraphServiceClientFactory
{
    public GraphServiceClient NewInstance(TokenCredential tokenCredential, IEnumerable<string> scopes = null,
        string baseUrl = null)
    {
        return new GraphServiceClient(tokenCredential, scopes, baseUrl);
    }
}
