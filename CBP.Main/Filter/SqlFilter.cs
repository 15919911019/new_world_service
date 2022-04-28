using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CBP.Models;

namespace CBP.Main.Filter
{
    public class SqlFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var value = context.HttpContext.Request.QueryString.Value;
            if(string.IsNullOrEmpty(value) == false)
            {
                var paras = value.TrimStart('/').Split('&').ToList();
                paras.ForEach(p =>
                {
                    var temp = p.Split('=')[1].Trim().ToLower();
                    if (temp.Contains("%20or%20") ||
                    temp.Contains("%20delete%20") ||
                    temp.Contains("%20select%20") || 
                    temp.Replace(" ","").Contains("1=1"))
                        context.Result = new RedirectResult($"fail?code={(int)ErrorCodeEnum.ParamereError}");
                });
            }
        }
    }
}
