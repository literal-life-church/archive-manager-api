using System;
using System.Globalization;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Forwarders;

internal class DateTimeForwarder : IDateTimeForwarder
{
    public bool TryParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style,
        out DateTime result)
    {
        return DateTime.TryParseExact(s, format, provider, style, out result);
    }
}
