using Infrastructure.Persistence;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Seed;
using Infrastructure.Service;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructurePersistenceService(builder.Configuration);
builder.Services.AddInfrastructureService();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Seed Data command `dotnet run seed`
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    if (args.Contains("seed"))
    {
        var userContext = services.GetRequiredService<UserContext>();
        SeedData.Seed(userContext);
        Console.WriteLine("✅ Seeding complete!");
    }
    else
    {
        app.Run();
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
