using Infrastructure.Persistence;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Seed;
using Infrastructure.Service;
using Microsoft.Extensions.FileProviders;
using WhereAndHow.Server;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddInfrastructurePersistenceService(builder.Configuration);
builder.Services.AddInfrastructureService();
builder.Services.AddInfrastructureWeb(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "MyPolice",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    );
});
//Seed Data command `dotnet run seed`
if(args.Contains("seed"))
{
    var tempApp = builder.Build();
    using var scope = tempApp.Services.CreateScope();
    var services = scope.ServiceProvider;

    var userContext = services.GetRequiredService<UserContext>();
    SeedData.Seed(userContext);
    Console.WriteLine("✅ Seeding complete!");

    return;
}
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
var webRoot = builder.Environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

var uploadsPath = Path.Combine(webRoot, "uploads");

if(Directory.Exists(uploadsPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(uploadsPath),
        RequestPath = "/uploads",
        ServeUnknownFileTypes = true
    });
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("MyPolice");

app.UseAuthentication();   
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
