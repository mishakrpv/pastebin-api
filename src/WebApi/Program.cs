using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using WebApi.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

if (builder.Environment.IsDevelopment() || builder.Environment.EnvironmentName == "Docker")
{
    builder.Services.AddDbContext<AppDbContext>(c => c.UseInMemoryDatabase("pastebindb"));
}
else
{
    builder.Services.AddDbContext<AppDbContext>(c =>
    {
        var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
        c.UseNpgsql(connectionString, options => options.EnableRetryOnFailure());
    });
}

builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddOptions(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseStaticFiles();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
