using Application.Interfaces;
using Core.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WhereAndHow.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ApartmentController (IApartamentService service, IWebHostEnvironment env): ControllerBase
{
    private readonly IApartamentService _service = service;
    private readonly IWebHostEnvironment _env = env;

    [HttpGet("room/{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var result = await _service.GetByIdAsync(id, Request);
        return result == null ? BadRequest("Not found Apartament") : Ok(result);
    }

    [HttpGet("apartaments")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync(Request);
        return result.Count == 0 ? NotFound("No apartaments found") : Ok(new { aparataments = result });
    }

    [HttpGet("searchapartament")]
    public async Task<IActionResult> Search([FromQuery] SearchApartamentDTO dto)
    {
        if(dto == null)
            return BadRequest("Missing parameters");

        var result = await _service.SearchAsync(dto);
        return result.Count == 0 ? NotFound("No results") : Ok(result);
    }

    [HttpPost("addroom")]
    public async Task<IActionResult> Add([FromForm] ApartamentDTO dto)
    {
        var uploadRootPath = Path.Combine(_env.WebRootPath, "uploads");

        var success = await _service.AddAsync(dto, uploadRootPath);
        return success ? Ok("Apartment added") : BadRequest("Could not add apartment");
    }
}
