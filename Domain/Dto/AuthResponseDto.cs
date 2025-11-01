using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; } 
    }
}
