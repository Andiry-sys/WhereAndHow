using Core.Domain.DTOs;
using Core.Domain.Models;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Implements;

public class ApartamentRepository : IApartamentRepository
{
    private readonly UserContext _context;

    public ApartamentRepository(UserContext context)
    {
        _context = context;
    }

    public async Task<Apartament?> GetByIdAsync(string id) =>
        await _context.Apartaments
            .Include(x => x.Photos)
            .Include(x => x.Address)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<Apartament>> SearchAsync(SearchApartamentDTO dto)
    {
        return await _context.Apartaments
            .Include(a => a.Address)
            .Where(
                s =>
                    (string.IsNullOrEmpty(dto.Name) || s.Name == dto.Name)
                    && (!dto.minValue.HasValue || s.Price >= dto.minValue.Value)
                    && (!dto.maxValue.HasValue || s.Price <= dto.maxValue.Value)
                    && (
                        string.IsNullOrEmpty(dto.TypeApartament) || s.TypeRoom == dto.TypeApartament
                    )
                    && (string.IsNullOrEmpty(dto.City) || s.Address.City == dto.City)
            )
            .ToListAsync();
    }

    public async Task<List<Apartament>> GetAllAsync() =>
        await _context.Apartaments.Include(x => x.Photos).Include(x => x.Address).ToListAsync();

    public async Task AddAsync(Apartament apartament)
    {
        _context.Apartaments.Add(apartament);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByIdAsync(string id) => await _context.Users.FindAsync(id);

    public async Task<Address?> GetAddressByIdAsync(string id) =>
        await _context.Address.FindAsync(id);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
