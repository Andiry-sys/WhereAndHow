using Core.Domain.Models;

namespace Infrastructure.Persistence.Interfaces;
public interface IAddressRepository
{
    Task<List<Address>> GetAllAsync();
    Task<Address?> GetByIdAsync(string id);
    Task AddAsync(Address address);
    Task UpdateAsync(Address address);
    Task DeleteAsync(Address address);
    Task<bool> ExistsAsync(string id);
}
