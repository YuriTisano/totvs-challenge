using Domain.Dto.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public class CreditSimulationCommand : IRequest<CreditSimulationResponseDto>
    {
        public Guid IdCliente { get; set; }
        public decimal ValorSimulacao { get; set; }

    }
}
