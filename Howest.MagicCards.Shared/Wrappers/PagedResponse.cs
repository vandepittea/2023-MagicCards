namespace Howest.MagicCards.Shared.Wrappers
{
    public record PagedResponse<T>(T Data, int PageNumber, int PageSize, int TotalCount, int TotalPages);
}
