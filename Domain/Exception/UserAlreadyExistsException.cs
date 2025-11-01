using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exception
{
    public class UserAlreadyExistsException : DefaultException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public UserAlreadyExistsException()
            : base("User already exists.") { }
    }
}
