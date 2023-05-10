namespace Howest.MagicCards.DAL.Models
{
    public class Response<T>
    {
        public T Data { get; set; }
        public bool Succeeded { get; set; } = true;
        public string[]? Errors { get; set; }
        public string Message { get; set; } = String.Empty;

        public Response(): this(default(T))
        {

        }

        public Response(T data, bool succeeded = true, string[] errors = null, string message = "")
        {
            Data = data;
            Succeeded = succeeded;
            Errors = errors;
            Message = message;
        }
    }
}
