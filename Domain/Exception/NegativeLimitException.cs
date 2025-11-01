using System.Net;

namespace Domain.Exception
{
    public class NegativeLimitException : DefaultException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public NegativeLimitException()
            : base("The limit cannot be negative.") { }
    }
}
