using System;
using System.Linq;
using System.Threading.Tasks;
using CRM.DAL.Models.DatabaseModels.Users;
using CRM.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

namespace CRM.IdentityServer.Services
{
    public class AccountsService
    {
        private readonly IdentityServerDbContext identityServerDbContext;

        public AccountsService(UserManager<User> userManager, IdentityServerDbContext identityServerDbContext)
        {
            this.identityServerDbContext = identityServerDbContext;
        }

        public async Task CleanUnconfirmedAccounts()
        {
            int DaysBeforeDel = 7;
            var items = identityServerDbContext.Users
                .Where(i => 
                    !i.EmailConfirmed
                    &&
                    i.RegistrationDate.Date-DateTime.Now.Date>TimeSpan.FromDays(DaysBeforeDel)
                );

            await identityServerDbContext.BulkDeleteAsync(items);
            await identityServerDbContext.SaveChangesAsync();
        }
    }
}