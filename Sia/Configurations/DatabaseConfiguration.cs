using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sia.Models;

namespace Sia.Configurations
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<SiaDbContext>((serviceProvider, options) =>
            {
                var connectionString = configuration.GetConnectionString("CRM");
                options.UseNpgsql(connectionString, builder => builder.UseNetTopologySuite());
            });

            return services;
        }
    }
}