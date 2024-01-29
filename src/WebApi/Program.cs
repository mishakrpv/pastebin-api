using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using WebApi.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

//builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);

builder.Services.AddDbContext<AppDbContext>(c =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    c.UseNpgsql(connectionString, options => options.EnableRetryOnFailure());
});

builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddOptions(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
