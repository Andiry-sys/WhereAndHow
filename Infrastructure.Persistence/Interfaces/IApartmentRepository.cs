using Core.Domain.DTOs;
using Core.Domain.Models;


namespace Infrastructure.Persistence.Interfaces;
public interface IApartamentRepository
{
    Task<ApartamentResponseDTO?> GetByIdAsync(string id);
    Task<List<ApartamentResponseDTO>> SearchAsync(SearchApartamentDTO dto);
    Task<List<ApartamentResponseDTO>> GetAllAsync();
    Task AddAsync(Apartament apartament);
    Task<User?> GetUserByIdAsync(string id);
    Task<Address?> GetAddressByIdAsync(string id);
    Task SaveChangesAsync();
}
