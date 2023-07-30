using System;
using System.Globalization;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Forwarders;

internal interface IDateTimeForwarder : IForwarder
{
    bool TryParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style,
        out DateTime result);
}
