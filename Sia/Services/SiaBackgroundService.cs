using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.DAL.Models.DatabaseModels.SiaMonitoredBlock;
using CRM.DAL.Models.DatabaseModels.SiaTransaction;
using CRM.DAL.Models.ResponseModels.Sia;
using CRM.ServiceCommon.Clients;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sia.Helpers;
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
         

         private async Task<ConsensusResponse> RegisterBlockAsync()
         {
             var consensus = await siad.GetConsensusAsync();
             
             logger.LogInformation($"Consensus monitoring (lastblock - {consensus.Currentblock})");
             
             var lastMonitoredBlock = GetLastMonitoredBlock();

             if (lastMonitoredBlock?.Hash == consensus.Currentblock || !consensus.Synced)
             {
                 return consensus;
             }

             await siaDbContext.AddAsync(new SiaMonitoredBlock()
             {
                 Hash = consensus.Currentblock,
                 Height = consensus.Height,
                 MonitoringTime = DateTime.Now
             });

             await siaDbContext.SaveChangesAsync();
             
             logger.LogInformation($"Registered block - {consensus.Currentblock}");
             
             return consensus;
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

             var processedTransactionIds = processedTransactionSet.Select(r=>r.Id).ToList();
             var registeredTransactions = await siaDbContext.SiaTransactions
                 .Where(r => processedTransactionIds.Contains(r.SiaId))
                 .ToListAsync();

             var consensus = await RegisterBlockAsync();
             
             var currentHeight = consensus.Height;
             
             registeredTransactions.ForEach(r=>r.Confirmations=currentHeight-r.InitialHeight);

             processedTransactionSet = processedTransactionSet?
                 .Where(r => registeredTransactions.All(rt => rt.SiaId != r.Id)).ToList();

             var newTransactions = processedTransactionSet.Select(r =>
                 new SiaTransaction
                 {
                     SiaId = r.Id,
                     CoinsValue = HastingsHelper.HastingsToCoins(r.Value),
                     InitialHeight = r.ConfirmHeight ?? 0,
                     Confirmations = currentHeight-r.ConfirmHeight??0,
                     RegistrationTime = DateTime.Now,
                     DestinationAddress = r.Address
                 }).ToList()
                 ;

             await siaDbContext.SiaTransactions.AddRangeAsync(newTransactions);
             await siaDbContext.SaveChangesAsync();
             logger.LogInformation($"Transactions monitoring. New - {newTransactions.Count}. Old - {registeredTransactions.Count}");
         }
         
    }
}