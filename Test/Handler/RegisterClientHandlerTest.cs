using Application.Command;
using Application.Handler;
using Domain.Dto.DataBase;
using Domain.Exception;
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
    public class RegisterClientHandlerTest
    {
        private readonly Mock<IClientRepository> _clientRepository = new();
        private readonly RegisterClientHandler _handler;

        public RegisterClientHandlerTest()
        {
            _handler = new RegisterClientHandler(_clientRepository.Object);
        }

        [Fact]
        public async Task RegisterClientSuccess()
        {
            var cmd = new RegisterClientCommand
            {
                Nome = "Maria",
                Cpf = "123.456.789-09",
                ValorLimite = 1500m
            };

            _clientRepository.Setup(r => r.ExistsByCpf("12345678909"))
                 .ReturnsAsync(false);

            Client? captured = null;
            _clientRepository.Setup(r => r.Add(It.IsAny<Client>()))
                 .Callback<Client>(c => captured = c)
                 .ReturnsAsync((Client c) => c);

            var result = await _handler.Handle(cmd, CancellationToken.None);

            result.Should().NotBeNull();
            result.Status.Should().Be("OK");
            result.IdCliente.Should().NotBeNullOrWhiteSpace();
            Guid.TryParse(result.IdCliente, out var parsedId).Should().BeTrue();

            captured.Should().NotBeNull();
            captured!.Nome.Should().Be("Maria");
            captured.Cpf.Should().Be("12345678909");
            captured.ValorLimite.Should().Be(1500m);
            captured.Id.Should().Be(parsedId);

            _clientRepository.Verify(r => r.ExistsByCpf("12345678909"), Times.Once);
            _clientRepository.Verify(r => r.Add(It.IsAny<Client>()), Times.Once);
        }

        [Fact]
        public async Task ClientAlreadyExist()
        {
            var cmd = new RegisterClientCommand
            {
                Nome = "João",
                Cpf = "111.222.333-44",
                ValorLimite = 1000m
            };

            _clientRepository.Setup(r => r.ExistsByCpf("11122233344"))
                 .ReturnsAsync(true);

            var act = async () => await _handler.Handle(cmd, CancellationToken.None);

            await act.Should().ThrowAsync<ClientAlreadyExistsException>();
            _clientRepository.Verify(r => r.Add(It.IsAny<Client>()), Times.Never);
        }

        [Fact]
        public async Task NegativeLimitError()
        {
            var cmd = new RegisterClientCommand
            {
                Nome = "Ana",
                Cpf = "987.654.321-00",
                ValorLimite = -10m
            };

            _clientRepository.Setup(r => r.ExistsByCpf("98765432100"))
                 .ReturnsAsync(false);

            var act = async () => await _handler.Handle(cmd, CancellationToken.None);

            await act.Should().ThrowAsync<NegativeLimitException>();
            _clientRepository.Verify(r => r.Add(It.IsAny<Client>()), Times.Never);
        }

        [Fact]
        public async Task InternalError()
        {
            var cmd = new RegisterClientCommand
            {
                Nome = "Carlos",
                Cpf = "555.666.777-88",
                ValorLimite = 200m
            };

            _clientRepository.Setup(r => r.ExistsByCpf("55566677788"))
                 .ReturnsAsync(false);

            _clientRepository.Setup(r => r.Add(It.IsAny<Client>()))
                 .ThrowsAsync(new Exception("db write failed"));

            var act = async () => await _handler.Handle(cmd, CancellationToken.None);

            var ex = await act.Should().ThrowAsync<Exception>();
            ex.Which.Message.Should().Contain("db write failed");
        }

        [Fact]
        public async Task AdjustCPF()
        {
            var cmd = new RegisterClientCommand
            {
                Nome = "Norma",
                Cpf = "  012.345.678-90  ",
                ValorLimite = 50m
            };

            _clientRepository.Setup(r => r.ExistsByCpf("01234567890"))
                 .ReturnsAsync(false);

            Client? captured = null;
            _clientRepository.Setup(r => r.Add(It.IsAny<Client>()))
                 .Callback<Client>(c => captured = c)
                 .ReturnsAsync((Client c) => c);

            var result = await _handler.Handle(cmd, CancellationToken.None);

            _clientRepository.Verify(r => r.ExistsByCpf("01234567890"), Times.Once);
            captured!.Cpf.Should().Be("01234567890");
            result.Status.Should().Be("OK");
        }
    }
}
