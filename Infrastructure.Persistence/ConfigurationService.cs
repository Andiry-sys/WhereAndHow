using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.Persistence;

public static class ConfigurationService
{
    public static IServiceCollection AddInfrastructurePersistenceService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserContext>(
                option =>
                    option.UseNpgsql(configuration["ConnectionStrings:PostgresConnect"])
            );
        return services;
    }
}
