using Application.Command;
using Domain.Dto;
using Domain.Exception;
using Infrastructure.Repository.Interface;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Handler
{
    public class AuthHandler : IRequestHandler<AuthCommand, AuthResponseDto>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IConfiguration _configuration;
        private readonly string _adminEmail;
        private readonly string _adminPassword;

        public AuthHandler(IAuthRepository authRepository, IJwtTokenService jwtTokenService, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _jwtTokenService = jwtTokenService;
            _adminEmail = configuration["AdminUser:Email"];
            _adminPassword = configuration["AdminUser:Password"];
        }

        public async Task<AuthResponseDto> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            var user = await _authRepository.GetByEmailAndPassword(request.Email, request.Password);
            if (user == null)
            {
                throw new UserUnauthorizedException();
            }

            var (token, expiresAt) = _jwtTokenService.Generate(user);

            return new AuthResponseDto
            {
                Token = token,
                ExpiresIn = (int)(expiresAt - DateTime.UtcNow).TotalSeconds
            };
        }
    }
}
