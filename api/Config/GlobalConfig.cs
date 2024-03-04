using System;
using System.Globalization;
using Azure.Identity;

namespace LiteralLifeChurch.ArchiveManagerApi.Config;

internal class GlobalConfig
{
    // Authentication
    public static readonly Uri AuthorityHost = AzureAuthorityHosts.AzurePublicCloud;
    public static readonly string[] Scopes = { "Files.Read.All" };

    // Language
    public static readonly CultureInfo LanguageCulture = new("en-US", false);
}
