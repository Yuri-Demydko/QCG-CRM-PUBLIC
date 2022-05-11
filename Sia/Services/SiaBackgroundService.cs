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
             
             
             var tSet = await siad.GetTransactionsAsync(0,-1);

             var processedTSet = tSet.ConfirmedTransactions
                 .Select
                 (t => t.Outputs
                     .Where(r => r.Walletaddress)
                     .Select(r => new {r.Relatedaddress, r.Value, Id = r.Id, Output = true})
                     .Union(t.Inputs.Where(f => f.Walletaddress)
                         .Select(r => new {r.Relatedaddress, r.Value, Id = r.Parentid, Output = false})))
                 .Where(r => r.Any())
                 .Select(r => r.First())
                 .ToList();

             processedTSet = processedTSet
                 .Where(r => !processedTSet.Any(f => f.Output == !r.Output && f.Id == r.Id))
                 .ToList();
             
         }
         
    }
}