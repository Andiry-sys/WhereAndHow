using Core.Domain.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IApartamentService
{
    Task<object?> GetByIdAsync(string id, HttpRequest request);
    Task<List<ApartamentResponseDTO>> SearchAsync(SearchApartamentDTO dto);
    Task<List<object>> GetAllAsync(HttpRequest request);
    Task<bool> AddAsync(ApartamentRequestDTO dto, string uploadRootPath);
}
