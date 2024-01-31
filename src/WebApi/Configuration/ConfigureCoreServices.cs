using Core.Entities;
using Core.Interfaces;
using Core.Services;
using Infrastructure.Caching;
using Infrastructure.Data;
using Infrastructure.Logging;
using Infrastructure.Services;

namespace WebApi.Configuration
{
    public static class ConfigureCoreServices
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            
            services.AddScoped<IObjectStorageService<TextObject>, S3TextStorageService>();
            
            services.AddSingleton(typeof(IAppCache<>), typeof(CacheAdapter<>));
            services.AddSingleton<IHashGenerator, HashGenerator>();

            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            return services;
        }
    }
}
