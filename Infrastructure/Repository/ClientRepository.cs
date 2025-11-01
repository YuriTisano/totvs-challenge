using Domain.Dto;
using Domain.Dto.DataBase;
using Domain.Dto.Entities;
using Infrastructure.Database;
using Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByCpf(string cpf)
        {
            return await _context.Clients.AnyAsync(c => c.Cpf == cpf);
        }

        public async Task<Client?> GetById(Guid id)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Client> Add(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }
    }
}
