using System.Collections.Generic;
using CRM.DAL.Models.DatabaseModels.SiaTransaction;
using CRM.ServiceCommon.Services.DbEventHandlerService;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sia.Services;

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
                        BackgroundJob.Enqueue<UserTransactionMatchingService>(j=>j.Match(e.EntryId))
                    },
                    new DbEventHandler()
                    {
                        Pattern = p =>
                            p.TableName == "SiaTransactions" && new List<DbOperation>()
                                {DbOperation.Update}.Contains(p.Operation) &&
                            p.GetNew<SiaTransaction>().UserId != null && p.GetNew<SiaTransaction>().Confirmations >= 1 , //@TODO CONFIG
                        Handler = e =>
                        {
                            var item = e.GetNew<SiaTransaction>();
                            BackgroundJob.Enqueue<UserBalanceUpdateService>(j => j.Update(item.UserId,item.CoinsValue));
                        }
                    },
                }, connectionString, logger);
            });
        }
    }
}