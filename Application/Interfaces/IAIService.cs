using Core.Domain.DTOs;

namespace Application.Interfaces;

public interface IAIService
{
    Task<AIAnalyzeResponseDTO> AnalyzeDescriptionAsync(string description);
}
