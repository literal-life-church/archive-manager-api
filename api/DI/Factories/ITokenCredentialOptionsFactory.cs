using Azure.Identity;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Factories;

internal interface ITokenCredentialOptionsFactory : IFactory
{
    TokenCredentialOptions NewInstance();
}
