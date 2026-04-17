using Application.Interfaces;
using Infrastructure.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Service;

public static class ConfigurationService
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
    {
        // Add infrastructure services here
        services.AddScoped<IAddressService,AddressService>();
        services.AddScoped<IApartamentService, ApartamentService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPartnerService, PartnerService>();
        return services;
    }
}
