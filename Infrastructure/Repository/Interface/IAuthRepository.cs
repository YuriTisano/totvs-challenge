using Domain.Dto;
using Domain.Dto.Entities;

namespace Infrastructure.Repository.Interface
{
    public interface IAuthRepository
    {
        Task<User?> GetByEmailAndPassword(string email, string password);
    }
}
