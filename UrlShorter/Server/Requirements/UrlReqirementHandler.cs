using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Requirements
{
    public class UrlReqirementHandler : AuthorizationHandler<UrlRequirements>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UrlRequirements requirement)
        {
            var httpContext = (HttpContext)context.Resource!;
            var urlExist = httpContext.Request.RouteValues.TryGetValue("id", out object? urlId);

            if (urlExist)
            {

                var userRole = httpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

                if (userRole == "Admin")
                {
                    context.Succeed(requirement);
                    return;
                }

                var userIdJwt = httpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;

                if (string.IsNullOrEmpty(userIdJwt))
                {
                    return;
                }

                var services = httpContext.RequestServices;
                var dataContext = services.GetRequiredService<DataContext>();
                Console.WriteLine(urlId);
                var url = await dataContext.Urls.SingleOrDefaultAsync(x => x.Id ==  Int32.Parse(urlId.ToString()));
                if (url?.UserId == userIdJwt)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
