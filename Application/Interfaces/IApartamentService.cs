using Core.Domain.DTOs;
using Core.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IApartamentService
{
    Task<object?> GetByIdAsync(string id, HttpRequest request);
    Task<List<Apartament>> SearchAsync(SearchApartamentDTO dto);
    Task<List<object>> GetAllAsync(HttpRequest request);
    Task<bool> AddAsync(ApartamentDTO dto, string uploadRootPath);
}
