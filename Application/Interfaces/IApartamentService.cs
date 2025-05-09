using Core.Domain.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IApartamentService
{
    Task<ApartamentResponseDTO> GetByIdAsync(string id);
    Task<List<ApartamentResponseDTO>> SearchAsync(SearchApartamentDTO dto);
    Task<List<ApartamentResponseDTO>> GetAllApartamentsAsync();
    Task<bool> AddAsync(ApartamentRequestDTO dto, string uploadRootPath);
}
