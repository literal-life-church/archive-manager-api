using System.Globalization;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Forwarders;

internal interface ICultureInfoForwarder : IForwarder
{
    TextInfo TextInfo { get; }
}
