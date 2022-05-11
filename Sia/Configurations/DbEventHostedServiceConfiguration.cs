using System.Collections.Generic;
using CRM.ServiceCommon.Services.DbEventHandlerService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Sia.Configurations
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
                    new DbEventHandler()
                    {
                        Pattern = p =>
                            p.TableName == "SiaTransactions" && new List<DbOperation>()
                                { DbOperation.Insert }.Contains(p.Operation),
                        Handler = e =>
                        {
                            var a = 1;
                        }
                    },
                }, connectionString, logger);
            });
        }
    }
}