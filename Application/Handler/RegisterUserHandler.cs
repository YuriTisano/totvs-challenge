using Application.Command;
using AutoMapper;
using Domain.Dto;
using Domain.Exception;
using Infrastructure.Repository.Interface;
using MediatR;

namespace Application.Handler
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public RegisterUserHandler(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task Handle(RegisterUserCommand command, CancellationToken cancellation)
        {
            try
            {
                if (await _userRepository.ExistUserRegister(command.Email))
                {
                    throw new UserAlreadyExistsException();
                }

                var model = _mapper.Map<UserRegisterDto>(command);
                var repository = _userRepository.UserRegister(model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
