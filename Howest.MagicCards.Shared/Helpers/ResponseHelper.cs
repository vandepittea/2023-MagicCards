using Howest.MagicCards.Shared.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Howest.MagicCards.WebAPI.Helpers
{
    public static class ResponseHelper
    {
        public static Response<T> NotFound<T>(string message) where T : class
        {
            return new Response<T>
            {
                Errors = new string[] { "404" },
                Message = message
            };
        }

        public static Response<T> InternalServerError<T>(string message) where T : class
        {
            return new Response<T>
            {
                Succeeded = false,
                Errors = new string[] { $"Status code: {StatusCodes.Status500InternalServerError}" },
                Message = message
            };
        }
    }
}