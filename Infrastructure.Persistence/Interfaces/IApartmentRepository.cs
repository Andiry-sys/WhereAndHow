using Core.Domain.DTOs;
using Core.Domain.Models;


namespace Infrastructure.Persistence.Interfaces;
public interface IApartamentRepository
{
    Task<Apartament?> GetByIdAsync(string id);
    Task<List<Apartament>> SearchAsync(SearchApartamentDTO dto);
    Task<List<Apartament>> GetAllAsync();
    Task AddAsync(Apartament apartament);
    Task<User?> GetUserByIdAsync(string id);
    Task<Address?> GetAddressByIdAsync(string id);
    Task SaveChangesAsync();
}
