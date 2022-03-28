using Microsoft.Extensions.DependencyInjection;

namespace CRM.User.WebApp.Configurations
{
    public static class AutoMapperConfiguration
    {
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(config =>
            {
                
            }, typeof(Startup));
        }
    }
}
