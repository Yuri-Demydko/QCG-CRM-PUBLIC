using System;
using System.Linq;
using System.Threading.Tasks;
using CRM.DAL.Models.DatabaseModels.Users;
using CRM.IdentityServer.Models;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Z.EntityFramework.Plus;

namespace CRM.IdentityServer.Services
{
    [Queue("identity")]
    public class AccountsService
    {
        private readonly IdentityServerDbContext identityServerDbContext;

        public AccountsService(UserManager<User> userManager, IdentityServerDbContext identityServerDbContext)
        {
            this.identityServerDbContext = identityServerDbContext;
        }

        public async Task CleanUnconfirmedAccounts()
        {
            //@TODO: Move Days before delete value to CommonConfigs
            await identityServerDbContext.Users
                .Where(i => 
                    !i.EmailConfirmed
                    &&
                    DateTime.Now.Date-i.RegistrationDate.Date>TimeSpan.FromDays(7)
                ).DeleteAsync();
            
            await identityServerDbContext.SaveChangesAsync();
        }
    }
}