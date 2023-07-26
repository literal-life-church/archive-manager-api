using System.Collections.Generic;
using Azure.Core;
using Microsoft.Graph;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Factories;

internal interface IGraphServiceClientFactory : IFactory
{
    GraphServiceClient NewInstance(TokenCredential tokenCredential, IEnumerable<string> scopes = null,
        string baseUrl = null);
}
