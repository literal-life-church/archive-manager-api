namespace LiteralLifeChurch.ArchiveManagerApi.Clean.Data.Mapper;

internal interface IMapper<in TInput, out TOutput>
{
    TOutput Map(TInput input);
}
