using Core.Domain.DTOs;


namespace Application.Interfaces;
public interface IOrderService
{
    Task<bool> ProcessOrderAndSendEmailAsync(OrderDTO order);

}
