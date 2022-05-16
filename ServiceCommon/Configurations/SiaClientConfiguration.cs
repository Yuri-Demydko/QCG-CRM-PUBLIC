using System.Net.Http;
using CRM.ServiceCommon.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace CRM.ServiceCommon.Configurations
{
    public static class SiaClientConfiguration
    {
        public static void ConfigureSiaClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped(option =>
            {
                var siaSection = configuration.GetSection("Sia");
                
                var addr = siaSection.GetValue<string>("SiadAddress");

                var apiPass = siaSection.GetValue<string>("ApiPassword");
                
                var encPass = siaSection.GetValue<string>("EncPassword");

                var renterSection = siaSection.GetSection("Renter");

                var renterPeriod = renterSection.GetValue<string>("Period");
                var renterRenew = renterSection.GetValue<string>("RenewWindow");
                var renterFunds = renterSection.GetValue<string>("Funds");
                var renterHosts = renterSection.GetValue<string>("Hosts");
                
                return new SiaApiClient(new HttpClient(),addr,apiPass,encPass,renterPeriod,renterRenew,renterFunds,renterHosts); 
            });
        }
    }
}