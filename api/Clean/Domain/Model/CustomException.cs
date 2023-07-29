using System;

namespace LiteralLifeChurch.ArchiveManagerApi.Clean.Domain.Model;

internal abstract class CustomException : Exception
{
    protected CustomException(string message) : base(message)
    {
    }
}
