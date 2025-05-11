using Application.Interfaces;
using Core.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WhereAndHow.Server.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class OrderController(IOrderService orderService): ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [HttpPost("send-order-email")]
    public async Task<IActionResult> SendOrderEmail([FromBody] OrderDTO order)
    {
        var success = await _orderService.ProcessOrderAndSendEmailAsync(order);
        return success ? Ok() : BadRequest("Failed to process order.");
    }

}
