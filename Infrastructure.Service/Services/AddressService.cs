using Application.Interfaces;
using Core.Domain.DTOs;
using Core.Domain.Models;
using Infrastructure.Persistence.Interfaces;

namespace Infrastructure.Service.Services;
public class AddressService(IAddressRepository repository) : IAddressService
{
    private readonly IAddressRepository _repository = repository;

    public async Task<List<AddressDTO>> GetAllAsync()
    {
        var all = await _repository.GetAllAsync();
        return all
            .Where(a => a.ApartamentId == null)
            .Select(a => new AddressDTO
            {
                Id = a.Id,
                City = a.City,
                Street = a.Street,
                NumberHouse = a.NumberHouse
            })
            .ToList();
    }

    public async Task<Address?> GetByIdAsync(string id) =>
        await _repository.GetByIdAsync(id);

    public async Task AddAsync(Address address) =>
        await _repository.AddAsync(address);

    public async Task<bool> UpdateAsync(string id,Address address)
    {
        if (id != address.Id || !await _repository.ExistsAsync(id))
            return false;

        await _repository.UpdateAsync(address);
        return true;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            return false;

        await _repository.DeleteAsync(entity);
        return true;
    }
}
