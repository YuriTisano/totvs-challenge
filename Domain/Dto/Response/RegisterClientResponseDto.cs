using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto.Response
{
    public class RegisterClientResponseDto
    {
        public string IdCliente { get; set; }
        public string Status { get; set; } = "OK";
    }
}
