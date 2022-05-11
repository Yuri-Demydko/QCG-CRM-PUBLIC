using System.Threading.Tasks;
using SqlKata.Execution;

namespace Sia.Services
{
    public class UserBalanceUpdateService
    {
        private readonly QueryFactory db;
        
        public async Task Update(string userId, decimal coins)
        {
            await db.StatementAsync("update [AspNetUsers]" +
                                    $"set [SiaCoinBalance]=[SiaCoinBalance]+{coins}" +
                                    $"where [Id]=\'{userId}\'");

        }
    }
}