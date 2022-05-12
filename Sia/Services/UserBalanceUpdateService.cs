using System;
using System.Linq;
using System.Threading.Tasks;
using CRM.DAL.Models.DatabaseModels.SiaTransaction;
using SqlKata.Execution;

namespace Sia.Services
{
    public class UserBalanceUpdateService
    {
        private readonly QueryFactory db;

        public UserBalanceUpdateService(QueryFactory db)
        {
            this.db = db;
        }

        public async Task Update(string transactionId)
        {
            var item = await db.Query("SiaTransactions")
                .Where("Id", Guid.Parse(transactionId))
                .GetAsync<SiaTransaction>();

            await db.StatementAsync("update \"AspNetUsers\"" +
                                    $"set \"SiaCoinBalance\"=\"SiaCoinBalance\"+{item.First().CoinsValue} " +
                                    $"where \"Id\"=\'{item.First().UserId}\'");
            
            await db.Query("SiaTransactions")
                .Where("Id", item.First().Id)
                .UpdateAsync(new
                {
                    OnBalance = true
                });
            
        }
    }
}