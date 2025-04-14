using Core.Domain.DTOs;
using Core.Domain.Models;


namespace Application.Interfaces;
public interface IAddressService
{
    Task<List<AddressDTO>> GetAllAsync();
    Task<Address?> GetByIdAsync(string id);
    Task AddAsync(Address address);
    Task<bool> UpdateAsync(string id,Address address);
    Task<bool> DeleteAsync(string id);
}
