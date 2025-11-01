using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto.Response
{
    public class ErrorResponseDto
    {
        public string Status { get; set; } = "ERRO";
        public string DetalheErro { get; set; }
    }
}
