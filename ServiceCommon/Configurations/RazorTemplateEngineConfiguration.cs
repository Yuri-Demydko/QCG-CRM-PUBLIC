using Microsoft.Extensions.DependencyInjection;
using RazorLight;

namespace CRM.ServiceCommon.Configurations
{
    public static class RazorTemplateEngineConfiguration
    {
        public static void ConfigureRazorTemplateEngine(this IServiceCollection services,
            string location)
        {
            services.AddSingleton(new RazorLightEngineBuilder()
                .UseFileSystemProject(location)
                .UseMemoryCachingProvider()
                .Build());
        }
    }
}