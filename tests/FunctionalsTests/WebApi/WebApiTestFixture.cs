using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Controllers;

namespace FunctionalsTests.WebApi
{
    public class TestApplication : WebApplicationFactory<TextController>
    {
        private string _environment = "Development";

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment(_environment);

            // Add mock/test services to the builder here
            builder.ConfigureServices(services => 
            {
                var descriptors = services.Where(d =>
                                                    d.ServiceType == typeof(DbContextOptions<AppDbContext>))
                                                .ToList();

                foreach (var descriptor in descriptors)
                {
                    services.Remove(descriptor);
                }

                services.AddScoped(sp =>
                {
                    return new DbContextOptionsBuilder<AppDbContext>()
                        .UseInMemoryDatabase("InMemoryDbForTesting")
                        .UseApplicationServiceProvider(sp)
                        .Options;
                });
            });

            return base.CreateHost(builder);
        }
    }
}
