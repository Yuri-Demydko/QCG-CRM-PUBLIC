using System;
using System.Linq;
using CRM.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CRM.ServiceCommon.Services
{
    public class DatabaseConfigurationProvider : ConfigurationProvider
    {
        public DatabaseConfigurationProvider(Action<DbContextOptionsBuilder> optionsAction)
        {
            OptionsAction = optionsAction;
        }

        private Action<DbContextOptionsBuilder> OptionsAction { get; }

        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<GeneralDbContext>();

            OptionsAction(builder);

            using var dbContext = new GeneralDbContext(builder.Options);

            Data = dbContext.Configurations.ToDictionary(c => c.Name, c => c.Value);
        }
    }

    public class DatabaseConfigurationSource : IConfigurationSource
    {
        private readonly Action<DbContextOptionsBuilder> optionsAction;

        public DatabaseConfigurationSource(Action<DbContextOptionsBuilder> optionsAction)
        {
            this.optionsAction = optionsAction;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DatabaseConfigurationProvider(optionsAction);
        }
    }
}