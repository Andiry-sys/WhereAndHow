using Application.Interfaces;
using Core.Domain.DTOs;
using Core.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WhereAndHow.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AddressController(IAddressService addressService) : ControllerBase
{
    private readonly IAddressService _service = addressService;
    [HttpGet]
    public async Task<ActionResult<List<AddressDTO>>> GetAddress() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Address>> GetAddress(string id)
    {
        var address = await _service.GetByIdAsync(id);
        return address == null ? NotFound() : Ok(address);
    }

    [HttpPost]
    public async Task<ActionResult<Address>> PostAddress(Address address)
    {
        await _service.AddAsync(address);
        return CreatedAtAction(nameof(GetAddress),new { id = address.Id },address);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAddress(string id,Address address)
    {
        if (!await _service.UpdateAsync(id,address))
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAddress(string id)
    {
        if (!await _service.DeleteAsync(id))
            return NotFound();
        return NoContent();
    }
}
