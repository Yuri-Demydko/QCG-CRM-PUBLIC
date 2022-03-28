using System;
using CRM.ServiceCommon.Services.CodeService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.ServiceCommon.Configurations
{
    public static class EmailCodeConfiguration
    {
        public static void ConfigureEmailCodes(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<ICodeService>(provider =>
                new CodeService(
                    new Services.CodeService.EmailCodeConfiguration(
                        TimeSpan.FromMinutes(configuration.GetValue<int>("emailverifycodelifetime"))),
                    options => options.UseNpgsql(configuration.GetConnectionString("CRM"))));
        }
    }
}