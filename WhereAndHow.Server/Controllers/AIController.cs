using Application.Interfaces;
using Core.Domain.DTOs;
using Infrastructure.Service.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace WhereAndHow.Server.Controllers;

[ApiController]
[Route("api/ai")]
public class AIController(IAIService aiService, ILogger<AIController> logger) : ControllerBase
{
    private readonly IAIService _aiService = aiService;
    private readonly ILogger<AIController> _logger = logger;

    [Authorize]
    [HttpPost("analyze")]
    [EnableRateLimiting("ai-analyze")]
    public async Task<IActionResult> Analyze([FromBody] AIAnalyzeRequestDTO dto)
    {
        if (dto is null || string.IsNullOrWhiteSpace(dto.Description))
            return BadRequest("Description is required");

        try
        {
            var result = await _aiService.AnalyzeDescriptionAsync(dto.Description);
            return Ok(result);
        }
        catch (AIServiceException ex)
        {
            _logger.LogError(ex, "AI analysis failed.");
            return StatusCode(StatusCodes.Status502BadGateway, "AI service unavailable");
        }
    }
}
