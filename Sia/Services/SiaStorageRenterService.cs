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
             
         }
         
         
    }
}