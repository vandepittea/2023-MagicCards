namespace Howest.MagicCards.Shared.Wrappers
{
    public record Response<T>(T Data, bool Succeeded = true, string[]? Errors = null, string Message = "");
}