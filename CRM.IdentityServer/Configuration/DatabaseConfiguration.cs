using CRM.IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.IdentityServer.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContextPool<IdentityServerDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("CRM");
                options.UseNpgsql(connectionString);
            });

            return services;
        }
    }
}