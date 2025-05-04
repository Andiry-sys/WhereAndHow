using Application.Interfaces;
using Core.Domain.DTOs;
using Core.Domain.Models;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Service.Services;
internal class OrderService(UserContext context,IEmailSender emailSender): IOrderService
{
    private readonly UserContext _context = context;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task<bool> ProcessOrderAndSendEmailAsync(OrderDTO order)
    {
        var owner = _context.Users.FirstOrDefault(u => u.ApartamentId == order.ApartamentId);
        if(owner == null)
            return false;

        var apartament = _context.Apartaments.FirstOrDefault(a => a.Id == order.ApartamentId);
        if(apartament == null)
            return false;

        apartament.Address = _context.Address.FirstOrDefault(a => a.ApartamentId == order.ApartamentId);

        var customer = _context.Users.Find(order.UserId);
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
            $"Контакти адміністрації: {owner.PhoneNumber}",
            $"Електронна пошта: {owner.Email}",
            $"Сума до сплати: {total} грн"
        });

        _context.Histories.Add(new HistoryApartament
        {
            Id = Guid.NewGuid().ToString(),
            ApartamentId = order.ApartamentId,
            UserId = customer.Id,
            DateArrival = order.CheckInDate,
            DateDeparture = order.CheckOutDate,
            User = customer,
            Apartament = apartament
        });

        await _context.SaveChangesAsync();

        await _emailSender.SendEmailAsync(customer.Email, "Ваше бронювання помешкання: " + apartament.Name, message);

        return true;
    }
}
