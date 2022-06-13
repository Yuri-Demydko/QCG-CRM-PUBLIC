using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CRM.DAL.Models.DatabaseModels.SiaMonitoredBlock;
using CRM.DAL.Models.DatabaseModels.SiaRenterAllowances;
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
    [Obsolete]
    public class SiaStorageRenterService
    {
         private readonly SiaDbContext siaDbContext;
         private readonly SiaApiClient siad;
         private readonly ILogger<SiaStorageRenterService> logger;
         

         public SiaStorageRenterService(SiaDbContext siaDbContext, ILogger<SiaStorageRenterService> logger, SiaApiClient siad)
         {
             this.siaDbContext = siaDbContext;
             this.logger = logger;
             this.siad = siad;
         }

         public async Task SetupRenter()
         {
             var init = await siaDbContext.SiaRenterAllowances
                 .AnyAsync(i => i.Type == SiaRenterAllowanseRequestType.Init);

             if (!init)
             {
               var result= await siad.PostInitRenterAsync();
               await siaDbContext.SiaRenterAllowances.AddAsync(new SiaRenterAllowance()
               {
                   Funds = result.RequestData["Funds"],
                   Hosts = result.RequestData["Hosts"],
                   RenewWindow = result.RequestData["RenewWindow"],
                   Period = result.RequestData["Period"],
                   Type = SiaRenterAllowanseRequestType.Init,
                   RegistrationTime = DateTime.Now
               });
               await siaDbContext.SaveChangesAsync();
                 return;
             }
             
         }
         
         
    }
}