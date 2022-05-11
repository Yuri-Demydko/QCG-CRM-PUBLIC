using System;
using System.Linq;
using System.Threading.Tasks;
using CRM.DAL.Models.DatabaseModels.SiaMonitoredBlock;
using CRM.ServiceCommon.Clients;
using Hangfire;
using Microsoft.Extensions.Logging;
using Sia.Models;

namespace Sia.Services
{
    [Queue("sia")]
    public class SiaBackgroundService
    {
         private readonly SiaDbContext siaDbContext;
         private readonly SiaApiClient siad;
         private readonly ILogger<SiaBackgroundService> logger;

         public SiaBackgroundService(SiaDbContext siaDbContext, ILogger<SiaBackgroundService> logger, SiaApiClient siad)
         {
             this.siaDbContext = siaDbContext;
             this.logger = logger;
             this.siad = siad;
         }

         public async Task MonitorConsensus()
         {
             var consensus = await siad.GetConsensusAsync();

             var lastMonitoredBlock = GetLastMonitoredBlock();

             if (lastMonitoredBlock?.Hash == consensus.Currentblock||!consensus.Synced)
             {
                 return;
             }

             await siaDbContext.AddAsync(new SiaMonitoredBlock()
             {
                 Hash = consensus.Currentblock,
                 Height = consensus.Height,
                 MonitoringTime = DateTime.Now
             });

             await siaDbContext.SaveChangesAsync();
         }

         private SiaMonitoredBlock? GetLastMonitoredBlock() =>
             siaDbContext
                 .SiaMonitoredBlocks
                 .OrderByDescending(i => i.MonitoringTime)
                 .FirstOrDefault();

         public async Task MonitorReceives()
         {
             //@TODO calc blocks, calc confirmations
             var lastBlock = GetLastMonitoredBlock();

             var startHeight = lastBlock != null ? lastBlock.Height - 1 : 0;
             
             var transactionsResponse = await siad.GetTransactionsAsync(startHeight,-1);

             var processedTransactionSet = transactionsResponse.ConfirmedTransactions?
                 .Select
                 (t =>
                     new{
                         innerObject= t?.Outputs
                     .Where(r => r.Walletaddress)
                     .Select(r => 
                         new
                         {
                             r.Relatedaddress, 
                             r.Value,
                             r.Id, 
                             Output = true
                         }).ToList()
                     .Union(t.Inputs
                         .Where(f => f.Walletaddress)
                         .Select(r => 
                             new
                             {
                                 r.Relatedaddress, 
                                 r.Value, 
                                 Id = r.Parentid, 
                                 Output = false
                             })),
                         ConfirmationHeight = t?.Confirmationheight
                     }
                     )
                 .Where(r => r.innerObject.Any())
                 .Select(r => 
                     new
                     {
                         Address=r.innerObject.First().Relatedaddress,
                         r.innerObject.First().Value,
                         r.innerObject.First().Output,
                         r.innerObject.First().Id,
                         ConfirmHeight = r.ConfirmationHeight
                     })
                 .ToList();

             processedTransactionSet = processedTransactionSet?
                 .Where(r => 
                     !processedTransactionSet.Any(f => f.Output == !r.Output && f.Id == r.Id))
                 .ToList();
             
             
         }
         
    }
}