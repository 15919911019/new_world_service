
using CBP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Public.Tools;

namespace CBP.Main.Filter
{
    public class ParamFilter : ActionFilterAttribute
    {
        private bool isSign;

        public ParamFilter(bool bol = true)
        {
            isSign = bol;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (isSign == false)
                return;

            var guid = context.HttpContext.Request.Headers["a"];
            var time = context.HttpContext.Request.Headers["b"];
            var sign = context.HttpContext.Request.Headers["c"];
            var mName = context.HttpContext.Request.Headers["module"];
            var cName = context.HttpContext.Request.Headers["command"];
            var token = context.HttpContext.Request.Headers["token"];
            var admin = context.HttpContext.Request.Headers["admin"];

            if (admin == "admin")
                return;

            if (string.IsNullOrEmpty(mName) == true || string.IsNullOrEmpty(cName) == true)
            {
                context.Result = new RedirectResult($"fail?code={(int)ErrorCodeEnum.NoModuleOrCommand}");
                return;
            }

            if (string.IsNullOrEmpty(token) == true)
            {
                context.Result = new RedirectResult($"fail?code={(int)ErrorCodeEnum.NoUser_Mer_ID}");
                return;
            }

            var temp = Tool.GenerateMD5(guid + time + GlobalConfigModel.SKey);
            if (temp != sign)
            {
                context.Result = new RedirectResult($"fail?code={(int)ErrorCodeEnum.SignError}");
                return;
            }
        }
    }
}
