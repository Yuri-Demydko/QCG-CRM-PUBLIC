using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;

namespace CRM.ServiceCommon.Services.DbEventHandlerService
{
    public class DbEventHostedService : BackgroundService
    {
        private readonly IEnumerable<DbEventHandler> listEventDelegates;

        private readonly string connectionString;

        private readonly ILogger<DbEventHostedService> logger;

        private const string Channel = "db_notifications";

        public DbEventHostedService(IEnumerable<DbEventHandler> listEventDelegates, string connectionString,
            ILogger<DbEventHostedService> logger)
        {
            this.listEventDelegates = listEventDelegates;
            this.connectionString = connectionString;
            this.logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync(stoppingToken);

            conn.Notification += (o, e) =>
            {
                var dbEvent = JsonConvert.DeserializeObject<DbEvent>(e.Payload);

                if (dbEvent == null)
                {
                    logger.LogError("Invalid db event message: {0}", e.Payload);
                    return;
                }

                if (listEventDelegates == null || !listEventDelegates.Any())
                {
                    logger.LogInformation("emply handlers list");
                    return;
                }

                var handlers = listEventDelegates.Where(i =>
                    i.Pattern(dbEvent));

                foreach (var handler in handlers)
                {
                    logger.LogInformation($"handle {handler.Handler.GetType().FullName}");
                    handler.Handler.Invoke(dbEvent);
                }
            };

            await using (var cmd = new NpgsqlCommand($"LISTEN {Channel}", conn))
            {
                await cmd.ExecuteNonQueryAsync(stoppingToken);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await conn.WaitAsync(stoppingToken);
            }

            logger.LogInformation("event handler stoped");
        }
    }
}