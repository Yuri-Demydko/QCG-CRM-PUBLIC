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

        public async Task Match(Guid id)
        {
            // db.Query("AspNetUsers")
            //     .
        }
        
    }
}