
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CBP.Main.Filter
{
    public class AuthorizationFilter : AuthorizeFilter
    {
        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            return null;
        }
    }
}
