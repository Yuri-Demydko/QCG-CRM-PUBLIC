using EasyCaching.Core.Configurations;
using EasyCaching.InMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.User.WebApp.Configurations
{
    public static class CacheConfiguration
    {
        public const string ProviderMemory = "Memory";
        public const string ProviderRedis = "Redis";

        public static void ConfigureMemoryCache(this IServiceCollection services)
        {
            // Add an in-memory cache service provider
            // More info: https://easycaching.readthedocs.io/en/latest/In-Memory/
            services.AddEasyCaching(options =>
            {
                // use memory cache with your own configuration

                options.UseInMemory(config =>
                {
                    config.DBConfig = new InMemoryCachingOptions
                    {
                        // scan time, default value is 60s
                        ExpirationScanFrequency = 60,
                        // total count of cache items, default value is 10000
                        SizeLimit = 1000,

                        // enable deep clone when reading object from cache or not, default value is true.
                        EnableReadDeepClone = false,
                        // enable deep clone when writing object to cache or not, default value is false.
                        EnableWriteDeepClone = false,
                    };
                    // the max random second will be added to cache's expiration, default value is 120
                    config.MaxRdSecond = 120;
                    // whether enable logging, default is false
                    config.EnableLogging = true;
                    // mutex key's alive time(ms), default is 5000
                    config.LockMs = 500;
                    // when mutex key alive, it will sleep some time, default is 300
                    config.SleepMs = 100;
                }, ProviderMemory);
            });
        }

        public static void ConfigureRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            var endpoint = new ServerEndPoint(configuration.GetSection("Cache:Redis:Host").Value, 6379);

            services.AddEasyCaching(options =>
            {
                options.UseRedis(config => { config.DBConfig.Endpoints.Add(endpoint); }, ProviderRedis)
                    .UseRedisLock();
            });
        }
    }
}