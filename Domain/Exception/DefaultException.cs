using System.Net;

namespace Domain.Exception
{
    public class DefaultException : System.Exception
    {
        public virtual HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;

        public DefaultException() : base("An unexpected error occurred.") { }

        public DefaultException(string message) : base(message) { }

        public DefaultException(string message, System.Exception innerException)
            : base(message, innerException) { }
    }
}
