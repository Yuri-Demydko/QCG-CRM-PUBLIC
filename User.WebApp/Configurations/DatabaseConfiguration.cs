using CRM.User.WebApp.Models.Basic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.User.WebApp.Configurations
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<UserDbContext>((serviceProvider, options) =>
            {
                var connectionString = configuration.GetConnectionString("CRM");
                options.UseNpgsql(connectionString, builder => builder.UseNetTopologySuite());
            });

            return services;
        }
    }
}