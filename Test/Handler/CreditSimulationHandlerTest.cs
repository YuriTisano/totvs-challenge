using Application.Command;
using Application.Handler;
using Domain.Dto.DataBase;
using FluentAssertions;
using Infrastructure.Repository.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Handler
{
    public class CreditSimulationHandlerTest
    {
        private readonly Mock<IClientRepository> _clientRepository = new();
        private readonly CreditSimulationHandler _handler;

        public CreditSimulationHandlerTest()
        {
            _handler = new CreditSimulationHandler(_clientRepository.Object);
        }

        [Fact]
        public async Task ClientNotFoundError()
        {
            var cmd = new CreditSimulationCommand { IdCliente = Guid.NewGuid(), ValorSimulacao = 100m };
            _clientRepository.Setup(r => r.GetById(cmd.IdCliente)).ReturnsAsync((Client?)null);

            var result = await _handler.Handle(cmd, CancellationToken.None);

            result.Status.Should().Be("NEGADO");
            result.IdTransacao.Should().BeNull();
            _clientRepository.Verify(r => r.GetById(cmd.IdCliente), Times.Once);
        }

        [Fact]
        public async Task ExceedsLimitError()
        {
            var client = new Client { Id = Guid.NewGuid(), Nome = "Ana", Cpf = "123", ValorLimite = 500m };
            var cmd = new CreditSimulationCommand { IdCliente = client.Id, ValorSimulacao = 600m };

            _clientRepository.Setup(r => r.GetById(client.Id)).ReturnsAsync(client);

            var result = await _handler.Handle(cmd, CancellationToken.None);

            result.Status.Should().Be("NEGADO");
            result.IdTransacao.Should().BeNull();
            _clientRepository.Verify(r => r.GetById(client.Id), Times.Once);
        }

        [Fact]
        public async Task InternalErrorGetById()
        {
            var cmd = new CreditSimulationCommand { IdCliente = Guid.NewGuid(), ValorSimulacao = 100m };
            _clientRepository.Setup(r => r.GetById(cmd.IdCliente))
                 .ThrowsAsync(new Exception("db unavailable"));

            var act = async () => await _handler.Handle(cmd, CancellationToken.None);

            var ex = await act.Should().ThrowAsync<Exception>();
            ex.Which.Message.Should().Contain("db unavailable");
            _clientRepository.Verify(r => r.GetById(cmd.IdCliente), Times.Once);
        }

    }
}