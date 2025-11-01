using Application.Command;
using Domain.Dto.DataBase;
using Domain.Dto.Response;
using Domain.Exception;
using Infrastructure.Repository.Interface;
using MediatR;

namespace Application.Handler
{
    public class RegisterClientHandler : IRequestHandler<RegisterClientCommand, RegisterClientResponseDto>
    {
        private readonly IClientRepository _clientRepository;

        public RegisterClientHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<RegisterClientResponseDto> Handle(RegisterClientCommand request, CancellationToken cancellationToken)
        {
            var cpfDigits = new string((request.Cpf ?? "").Where(char.IsDigit).ToArray());

            if (await _clientRepository.ExistsByCpf(cpfDigits))
            {
                throw new ClientAlreadyExistsException();

            }

            if (request.ValorLimite < 0)
            {
                throw new NegativeLimitException();
            }

            var client = new Client
            {
                Id = Guid.NewGuid(),
                Nome = request.Nome,
                Cpf = cpfDigits,
                ValorLimite = request.ValorLimite
            };

            await _clientRepository.Add(client);

            return new RegisterClientResponseDto
            {
                IdCliente = client.Id.ToString(),
                Status = "OK"
            };
        }
    }

}
