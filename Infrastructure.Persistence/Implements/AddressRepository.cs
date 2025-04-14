using Infrastructure.Persistence.Interfaces;
using Core.Domain.Models;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Implements;
public class AddressRepository(UserContext userContext) : IAddressRepository
{
    private readonly UserContext _context = userContext;  

    public async Task AddAsync(Address address)
    {
        if(address is null)
        {
            return;
        }
        if (string.IsNullOrEmpty(address.Id))
        {
            address.Id = Guid.NewGuid().ToString();
        }
        await _context.Address.AddAsync(address);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Address>> GetAllAsync() =>
        await _context.Address.ToListAsync();

    public async Task<Address?> GetByIdAsync(string id) =>
        await _context.Address.FindAsync(id);

    public async Task UpdateAsync(Address address)
    {
        _context.Entry(address).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Address address)
    {
        _context.Address.Remove(address);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(string id) =>
        await _context.Address.AnyAsync(e => e.Id == id);
}
