using System;
using Azure.Identity;

namespace LiteralLifeChurch.ArchiveManagerApi.Authentication;

internal static class AuthenticationConfig
{
    public static readonly Uri AuthorityHost = AzureAuthorityHosts.AzurePublicCloud;
    public static readonly string[] Scopes = { "Files.Read.All" };
}
