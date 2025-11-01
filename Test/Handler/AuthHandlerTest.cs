using Application.Command;
using Application.Handler;
using Domain.Dto.Entities;
using Domain.Exception;
using FluentAssertions;
using Infrastructure.Repository.Interface;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Handler
{
    public class AuthHandlerTest
    {
        private readonly Mock<IAuthRepository> _authRepository;
        private readonly Mock<IJwtTokenService> _jwtTokenService;
        private readonly IConfiguration _configuration;
        private readonly AuthHandler _handler;
        public AuthHandlerTest()
        {
            _authRepository = new Mock<IAuthRepository>();
            _jwtTokenService = new Mock<IJwtTokenService>();

            var inMemorySettings = new Dictionary<string, string?>
            {
                {"AdminUser:Email", "admin@test.com"},
                {"AdminUser:Password", "admin123"}
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();

            _handler = new AuthHandler(_authRepository.Object, _jwtTokenService.Object, _configuration);

        }

        [Fact]
        public async Task UserValid()
        {
            var user = new User { 
                Id = 1, 
                Email = "john@doe.com", 
                Password = "123" 
            };
            var expiresAt = DateTime.UtcNow.AddMinutes(30);

            _authRepository.Setup(x => x.GetByEmailAndPassword(user.Email, user.Password))
                           .ReturnsAsync(user);
            _jwtTokenService.Setup(x => x.Generate(user))
                            .Returns(("token-123", expiresAt));

            var command = new AuthCommand { Email = user.Email, Password = user.Password };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Token.Should().Be("token-123");
            var expected = (int)(expiresAt - DateTime.UtcNow).TotalSeconds;
            result.ExpiresIn.Should().BeInRange(expected - 5, expected + 5);
        }

        [Fact]
        public async Task UserNotFound()
        {
            _authRepository.Setup(x => x.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                           .ReturnsAsync((User?)null);

            var command = new AuthCommand { Email = "x@y.com", Password = "wrong" };

            var act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<UserUnauthorizedException>();
            _jwtTokenService.Verify(x => x.Generate(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task InternalError()
        {
            _authRepository
                .Setup(r => r.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("database connection failed"));

            var cmd = new AuthCommand { Email = "fail@test.com", Password = "123" };

            var act = async () => await _handler.Handle(cmd, CancellationToken.None);

            var ex = await act.Should().ThrowAsync<Exception>();
            ex.Which.Message.Should().Contain("database connection failed");

            _jwtTokenService.Verify(j => j.Generate(It.IsAny<User>()), Times.Never);
        }


        [Fact]
        public void AdminCredentials()
        {
            _configuration["AdminUser:Email"].Should().Be("admin@test.com");
            _configuration["AdminUser:Password"].Should().Be("admin123");
        }
    }
}
