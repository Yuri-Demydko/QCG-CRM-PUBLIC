using System;
using CRM.ServiceCommon.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CRM.ServiceCommon.Configurations
{
    public static class DatabaseConfigsConfiguration
    {
        public static void AddDatabaseConfigs(this IConfigurationBuilder configurationBuilder,
            Action<DbContextOptionsBuilder> optionsAction)
        {
            configurationBuilder.Add(new DatabaseConfigurationSource(optionsAction));
        }
    }
}