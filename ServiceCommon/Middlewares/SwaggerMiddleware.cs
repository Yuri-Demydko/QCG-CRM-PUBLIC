using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace CRM.ServiceCommon.Middlewares
{
    public class SwaggerMiddleware
    {
        private readonly RequestDelegate next;

        public SwaggerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger") && !context.User.Identity.IsAuthenticated)
            {
                await context.ChallengeAsync();
            }
            else
            {
                await next.Invoke(context);
            }
        }
    }
}