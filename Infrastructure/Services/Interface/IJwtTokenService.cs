using Domain.Dto;
using Domain.Dto.Entities;

namespace Infrastructure.Repository.Interface
{
    public interface IJwtTokenService
    {
        (string token, DateTime expiresAt) Generate(User user);
    }
}
