using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CRM.DAL.Models.DatabaseModels.PayCards;
using CRM.DAL.Models.DatabaseModels.Products;
using CRM.DAL.Models.DatabaseModels.ProductsUsers;
using CRM.DAL.Models.RequestModels.ProductBuy;
using CRM.User.WebApp.Models.Basic;
using CRM.User.WebApp.Services.PayCardValidationService;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Z.EntityFramework.Plus;

namespace CRM.User.WebApp.Controllers
{
    [ODataRoutePrefix(nameof(PayCard))]
    public class PayCardController : BaseController<PayCardController>
    {

        private readonly IMapper mapper;
        private readonly UserDbContext userDbContext;
        private readonly IPayCardValidationService payCardValidationService;

        public PayCardController(ILogger<PayCardController> logger, UserDbContext userDbContext,
            UserManager<DAL.Models.DatabaseModels.Users.User> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper, IPayCardValidationService payCardValidationService) : base(
            logger, userDbContext,
            userManager, httpContextAccessor)
        {
            this.userDbContext = userDbContext;
            this.mapper = mapper;
            this.payCardValidationService = payCardValidationService;
        }


        /// <summary>
        ///     Get ProductUsers.
        /// </summary>
        /// <returns>The requested ProductUsers.</returns>
        /// <response code="200">The ProductUsers was successfully retrieved.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PayCard>), StatusCodes.Status200OK)]
        public IEnumerable<PayCard> Get()
        {
            return userDbContext.PayCards
                .Where(r => r.UserId == userManager.GetUserId(User));
        }
        
        /// <summary>
        ///     Get ProductUsers.
        /// </summary>
        /// <returns>The requested ProductUsers.</returns>
        /// <response code="200">The ProductUsers was successfully retrieved.</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody]PayCard item)
        {
            if (!ModelState.IsValid)
            {
                //@TODO: Pass errors
                return BadRequest("Invalid PayCard Model");
            }

            //In future will use some API to validate card (withdraw and return little amount of money, as example)
            var validationResult = await payCardValidationService.ValidateAsync(item);

            if (!validationResult.IsSuccess)
            {
                return BadRequest(validationResult.Errors.Aggregate("Errors:", (a, b) => $"{a}\n{b}"));
            }

            item.UserId = userManager.GetUserId(User);
            await userDbContext.PayCards.AddAsync(item);
            await userDbContext.SaveChangesAsync();

            return NoContent();
        }
        
    }
}