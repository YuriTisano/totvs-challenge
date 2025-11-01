using Domain.Dto;
using Domain.Dto.DataBase;
using Domain.Dto.Entities;

namespace Infrastructure.Repository.Interface
{
    public interface IClientRepository
    {
        Task<bool> ExistsByCpf(string cpf);
        Task<Client> Add(Client client);
        Task<Client?> GetById(Guid id);
    }
}
