using Domain.Dto.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public class RegisterClientCommand : IRequest<RegisterClientResponseDto>
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public decimal ValorLimite { get; set; }
    }
}
