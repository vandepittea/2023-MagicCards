namespace Howest.MagicCards.WebAPI.Helpers
{
    public static class ResponseHelper
    {
        public static Response<T> NotFound<T>(string message) where T : class
        {
            return new Response<T>(Data: null, Succeeded: false, Errors: new string[] { "404" }, Message: message);
        }

        public static Response<T> InternalServerError<T>(string message) where T : class
        {
            return new Response<T>(Data: null, Succeeded: false, Errors: new string[] { $"Status code: {StatusCodes.Status500InternalServerError}" }, Message: message);
        }
    }
}