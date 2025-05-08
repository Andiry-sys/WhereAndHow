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

    public async Task<ApartamentResponseDTO?> GetByIdAsync(string id)
    {
        var apartament = await _context.Apartaments
            .Include(x => x.Photos)
            .Include(x => x.Address)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (apartament == null)
            return null;

        return new ApartamentResponseDTO
        {
            Id = apartament.Id,
            Name = apartament.Name,
            TypeRoom = apartament.TypeRoom,
            Price = apartament.Price,
            Description = apartament.Description,
            Address =
                apartament.Address == null
                    ? null
                    : new AddressDTO
                    {
                        Id = apartament.Address.Id,
                        City = apartament.Address.City,
                        Street = apartament.Address.Street,
                        NumberHouse = apartament.Address.NumberHouse
                    },
            Photos = apartament.Photos.Select(p => new PhotoDTO { ImagePath = p.ImagePath }).ToList()
        };
    }

    public async Task<List<ApartamentResponseDTO>> SearchAsync(SearchApartamentDTO dto)
    {
        var apartaments = await _context.Apartaments
            .Include(a => a.Photos)
            .Include(a => a.Address)
            .Where(
                s =>
                    (string.IsNullOrEmpty(dto.Name) || s.Name == dto.Name)
                    && (!dto.minValue.HasValue || s.Price >= dto.minValue.Value)
                    && (!dto.maxValue.HasValue || s.Price <= dto.maxValue.Value)
                    && (string.IsNullOrEmpty(dto.TypeRoom) || s.TypeRoom == dto.TypeRoom)
                    && (string.IsNullOrEmpty(dto.City) || s.Address.City == dto.City)
            )
            .ToListAsync();

        return apartaments
            .Select(
                a =>
                    new ApartamentResponseDTO
                    {
                        Id = a.Id,
                        Name = a.Name,
                        TypeRoom = a.TypeRoom,
                        Price = a.Price,
                        Description = a.Description,
                        Address =
                            a.Address == null
                                ? null
                                : new AddressDTO
                                {
                                    Id = a.Address.Id,
                                    City = a.Address.City,
                                    Street = a.Address.Street,
                                    NumberHouse = a.Address.NumberHouse
                                },
                        Photos = a.Photos.Select(p => new PhotoDTO { ImagePath = p.ImagePath }).ToList()
                    }
            )
            .ToList();
    }

    public async Task<List<ApartamentResponseDTO>> GetAllAsync()
    {
        var apartaments = await _context.Apartaments
            .Include(x => x.Photos)
            .Include(x => x.Address)
            .ToListAsync();

        return apartaments
            .Select(
                a =>
                    new ApartamentResponseDTO
                    {
                        Id = a.Id,
                        Name = a.Name,
                        TypeRoom = a.TypeRoom,
                        Price = a.Price,
                        Description = a.Description,
                        Address =
                            a.Address == null
                                ? null
                                : new AddressDTO
                                {
                                    Id = a.Address.Id,
                                    City = a.Address.City,
                                    Street = a.Address.Street,
                                    NumberHouse = a.Address.NumberHouse
                                },
                        Photos = a.Photos.Select(p => new PhotoDTO { ImagePath = p.ImagePath }).ToList()
                    }
            )
            .ToList();
    }

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
