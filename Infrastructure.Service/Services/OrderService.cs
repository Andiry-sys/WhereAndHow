using Application.Interfaces;
using Core.Domain.DTOs;
using Core.Domain.Models;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.Services;
internal class OrderService(UserContext context,IEmailSender emailSender): IOrderService
{
    private readonly UserContext _context = context;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task<bool> ProcessOrderAndSendEmailAsync(OrderDTO order)
    {
        var apartament = await _context.Apartaments
         .Include(a => a.Address)
         .Include(a => a.Owner)
         .FirstOrDefaultAsync(a => a.Id == order.ApartamentId);

        if(apartament == null || apartament.Owner == null)
            return false;

        var customer = await _context.Users.FindAsync(order.UserId);
        if(customer == null)
            return false;

        if(!DateTime.TryParse(order.CheckInDate, out var startDate) ||
            !DateTime.TryParse(order.CheckOutDate, out var endDate))
            return false;

        var total = apartament.Price * (endDate - startDate).Days;

        string message = string.Join("\n", new[]
        {
        $"В'їзд: {order.CheckInDate}",
        $"Виїзд: {order.CheckOutDate}",
        $"Ваше ім'я: {customer.Name}",
        $"Ваша електронна пошта: {customer.Email}",
        $"Ваш номер: {customer.PhoneNumber}",
        $"Адреса помешкання: {apartament.Address?.City} {apartament.Address?.Street} {apartament.Address?.NumberHouse}",
        $"Контакти адміністрації: {apartament.Owner.PhoneNumber}",
        $"Електронна пошта: {apartament.Owner.Email}",
        $"Сума до сплати: {total} грн"
    });

        _context.Histories.Add(new HistoryApartament
        {
            Id = Guid.NewGuid().ToString(),
            ApartamentId = apartament.Id,
            UserId = customer.Id,
            DateArrival = order.CheckInDate,
            DateDeparture = order.CheckOutDate
        });

        await _context.SaveChangesAsync();

        await _emailSender.SendEmailAsync(customer.Email, "Ваше бронювання помешкання: " + apartament.Name, message);

        return true;
    }
}
