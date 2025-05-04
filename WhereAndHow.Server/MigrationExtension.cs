using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace WhereAndHow.Server;

public static class MigrationExtension
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using UserContext dbContext = scope.ServiceProvider.GetRequiredService<UserContext>();

        dbContext.Database.Migrate();
    }
}
