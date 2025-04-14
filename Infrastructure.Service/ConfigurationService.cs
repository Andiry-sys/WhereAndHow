using Application.Interfaces;
using Infrastructure.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Service;

public static class ConfigurationService
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
    {
        // Add your infrastructure services here
        services.AddScoped<IAddressService,AddressService>();
        return services;
    }
}
