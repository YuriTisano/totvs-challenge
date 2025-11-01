using Domain.Dto;
using Domain.Dto.Entities;

namespace Infrastructure.Repository.Interface
{
    public interface IUserRepository
    {
        Task UserRegister(UserRegisterDto userRegisterDto);
        Task<bool> ExistUserRegister(string email);
        Task<User?> GetByEmailAndPassword(string email, string password);
    }
}
