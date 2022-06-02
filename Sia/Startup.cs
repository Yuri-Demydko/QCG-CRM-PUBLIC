using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.ServiceCommon.Configurations;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Sia.Configurations;
using Sia.Services;

namespace Sia
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Sia", Version = "v1"}); });
            services.ConfigureDatabase(Configuration);

            services.ConfigureSqlKata(Configuration);
            services.ConfigureSiaClient(Configuration);
            services.AddHangfire(config =>
            {
                config.UseNLogLogProvider();

                if (!Env.IsDevelopment())
                {
                    
                    //@TODO: CREATE SCHEMA FOR PROD MODE
                    var connectionString = Configuration.GetConnectionString("CRM");
                    config.UsePostgreSqlStorage(connectionString, new PostgreSqlStorageOptions()
                    {
                        SchemaName = "hangfire-crm"
                    });
                }
                else
                {
                    config.UseInMemoryStorage();
                }
            });
            
            services.ConfigureDbEventHostedService(Configuration);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sia v1"));
            }

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                Queues = new[] {"digital", "sia", "default"},
                WorkerCount = 30
            });
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            //BackgroundJob.Enqueue<SiaStorageRenterService>(j => j.SetupRenter());
            
            //BackgroundJob.Enqueue<SiaBackgroundService>(j => j.MonitorTransactions());
            
             RecurringJob.AddOrUpdate<SiaBackgroundService>(j => j.MonitorTransactions(), Cron.Minutely);
        }
    }
}