using System.Linq;
using System.Threading.Tasks;
using CRM.ServiceCommon.Clients;
using Hangfire;
using Microsoft.Extensions.Logging;
using Sia.Models;

namespace Sia.Services
{
    [Queue("sia")]
    public class SiaBackgroundService
    {
         private readonly SiaDbContext userDbContext;
         private readonly SiaApiClient siad;
         private readonly ILogger<SiaBackgroundService> logger;

         public SiaBackgroundService(SiaDbContext userDbContext, ILogger<SiaBackgroundService> logger, SiaApiClient siad)
         {
             this.userDbContext = userDbContext;
             this.logger = logger;
             this.siad = siad;
         }

         public async Task MonitorReceives()
         {
             //@TODO calc blocks, calc confirmations
             var tSet = await siad.GetTransactionsAsync(0,-1);

             var processedTSet = tSet.ConfirmedTransactions
                 .Select
                 (t => t.Outputs
                     .Where(r => r.Walletaddress)
                     .Select(r => new {r.Relatedaddress, r.Value, Id = r.Id, Out = true})
                     .Union(t.Inputs.Where(f => f.Walletaddress)
                         .Select(r => new {r.Relatedaddress, r.Value, Id = r.Parentid, Out = false})))
                 .Where(r => r.Any())
                 .Select(r => r.First())
                 .ToList();

             processedTSet = processedTSet
                 .Where(r => !processedTSet.Any(f => f.Out == !r.Out && f.Id == r.Id))
                 .ToList();
             
         }
         
    }
}