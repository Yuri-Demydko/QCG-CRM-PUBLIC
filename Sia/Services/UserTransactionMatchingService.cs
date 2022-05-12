using System;
using System.Threading.Tasks;
using SqlKata.Execution;

namespace Sia.Services
{
    public class UserTransactionMatchingService
    {
        private readonly QueryFactory db;

        public UserTransactionMatchingService(QueryFactory db)
        {
            this.db = db;
        }

        public async Task Match(string id)
        {
           await db.StatementAsync("update \"SiaTransactions\"" +
                              "set \"UserId\"=(select \"AspNetUsers\".\"Id\" from \"AspNetUsers\"" +
                              "join \"UserSiaAddresses\" on \"AspNetUsers\".\"Id\" = \"UserSiaAddresses\".\"UserId\"" +
                              "where \"UserSiaAddresses\".\"Address\"=\"SiaTransactions\".\"DestinationAddress\")" +
                              $"where \"SiaTransactions\".\"Id\"=\'{id}\'");
        }
        
    }
}