using Core.Configuration;
using Infrastructure.Configuration;

namespace WebApi.Configuration
{
    public static class ConfigureOptions
    {
        public static IServiceCollection AddOptions(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<AwsCredentials>(configuration.GetSection("AwsCredentials"));
            services.Configure<HashGeneratorSettings>(configuration.GetSection("HashGeneratorSettings"));

            return services;
        }
    }
}
