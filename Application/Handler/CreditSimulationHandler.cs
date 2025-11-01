using Application.Command;
using Domain.Dto.Response;
using Infrastructure.Repository.Interface;
using MediatR;

namespace Application.Handler
{
    public class CreditSimulationHandler : IRequestHandler<CreditSimulationCommand, CreditSimulationResponseDto>
    {
        private readonly IClientRepository _clientRepository;

        public CreditSimulationHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<CreditSimulationResponseDto> Handle(CreditSimulationCommand command, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetById(command.IdCliente);
            if (client is null)
            {
                return new CreditSimulationResponseDto
                {
                    Status = "NEGADO",
                    IdTransacao = null
                };
            }

            if (command.ValorSimulacao > client.ValorLimite)
            {
                return new CreditSimulationResponseDto
                {
                    Status = "NEGADO",
                    IdTransacao = null
                };
            }

            return new CreditSimulationResponseDto
            {
                Status = "APROVADO",
                IdTransacao = Guid.NewGuid()
            };

        }
    }
}
