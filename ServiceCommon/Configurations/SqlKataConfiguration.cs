using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace CRM.ServiceCommon.Configurations
{
    public static class SqlKataConfiguration
    {
        public static void ConfigureSqlKata(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped(option =>
            {
                var connection = new NpgsqlConnection(configuration.GetConnectionString("CRM"));

                var compiler = new PostgresCompiler();

                return new QueryFactory(connection, compiler);
            });
        }
    }
}