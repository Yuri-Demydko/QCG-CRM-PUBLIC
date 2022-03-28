using System;
using CRM.ServiceCommon.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

namespace CRM.User.WebApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((_, config) =>
                {
                    var configs = config.Build();

                    LogManager.Configuration = new NLogLoggingConfiguration(configs.GetSection("NLog"));
                    ConfigSettingLayoutRenderer.DefaultConfiguration = configs;

                    config.AddDatabaseConfigs(
                        options => options.UseNpgsql(configs.GetConnectionString("CRM")));
                });

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Production ||
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Staging)
            {
                host = host
                    .ConfigureLogging(logging => logging.ClearProviders())
                    .UseNLog();
            }


            return host.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseSentry();
                webBuilder.UseStartup<Startup>();
            });
        }
    }
}