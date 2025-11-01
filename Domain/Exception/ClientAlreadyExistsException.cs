using System.Net;

namespace Domain.Exception
{
    public class ClientAlreadyExistsException : DefaultException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public ClientAlreadyExistsException()
            : base("Client already exists.") { }
    }
}
