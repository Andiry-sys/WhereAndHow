using Core.Domain.Models;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Implements;
using Infrastructure.Persistence.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class ConfigurationService
{
    public static IServiceCollection AddInfrastructurePersistenceService(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IApartamentRepository, ApartamentRepository>();
        services.AddDbContext<UserContext>(
            option => option.UseNpgsql(configuration["ConnectionStrings:PostgresConnect"])
        );
        services
            .AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<UserContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}
