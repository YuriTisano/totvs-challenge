using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exception
{
    public class UserUnauthorizedException : DefaultException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

        public UserUnauthorizedException()
            : base("Invalid credentials.") { }
    }
}
