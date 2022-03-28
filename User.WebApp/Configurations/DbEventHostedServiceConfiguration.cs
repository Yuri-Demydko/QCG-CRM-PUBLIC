using System.Collections.Generic;
using CRM.ServiceCommon.Services.DbEventHandlerService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CRM.User.WebApp.Configurations
{
    public static class DbEventHostedServiceConfiguration
    {
        public static void ConfigureDbEventHostedService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService(provider =>
            {
                var logger = provider.GetService<ILogger<DbEventHostedService>>();
                var connectionString = configuration.GetConnectionString("CRM");

                return new DbEventHostedService(new List<DbEventHandler>()
                {
                    
                }, connectionString, logger);
            });
        }
    }
}