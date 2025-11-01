using Application.Command;
using Application.Handler;
using AutoMapper;
using Domain.Dto;
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
    public class RegisterUserHandlerTest
    {
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly RegisterUserHandler _handler;

        public RegisterUserHandlerTest()
        {
            _handler = new RegisterUserHandler(_mapper.Object, _userRepository.Object);

        }

        [Fact]
        public async Task Success()
        {
            var cmd = new RegisterUserCommand { Email = "new@user.com", Password = "123" };

            _userRepository
                .Setup(r => r.ExistUserRegister(cmd.Email))
                .ReturnsAsync(false);

            var dto = new UserRegisterDto { Email = cmd.Email, Password = cmd.Password };

            _mapper
                .Setup(m => m.Map<UserRegisterDto>(cmd))
                .Returns(dto);

            _userRepository
                .Setup(r => r.UserRegister(It.IsAny<UserRegisterDto>()))
                .Returns(Task.CompletedTask);

            await _handler.Handle(cmd, CancellationToken.None);

            _userRepository.Verify(r => r.ExistUserRegister(cmd.Email), Times.Once);
            _mapper.Verify(m => m.Map<UserRegisterDto>(cmd), Times.Once);
            _userRepository.Verify(r =>
                r.UserRegister(It.Is<UserRegisterDto>(u =>
                    u.Email == cmd.Email && u.Password == cmd.Password)),
                Times.Once);
        }

        [Fact]
        public async Task ErrorUserAlreradyExist()
        {
            var cmd = new RegisterUserCommand { Email = "exists@user.com", Password = "123" };

            _userRepository
                .Setup(r => r.ExistUserRegister(cmd.Email))
                .ReturnsAsync(true);

            var act = async () => await _handler.Handle(cmd, CancellationToken.None);

            await act.Should().ThrowAsync<UserAlreadyExistsException>();

            _mapper.Verify(m => m.Map<UserRegisterDto>(It.IsAny<RegisterUserCommand>()), Times.Never);
            _userRepository.Verify(r => r.UserRegister(It.IsAny<UserRegisterDto>()), Times.Never);
        }
    }
}
