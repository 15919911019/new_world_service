using CBP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Driver;
using Public.DbHelper.Mongon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBP.Main.Filter
{
    public class StatisticsFilter : ActionFilterAttribute
    {
        private IMongoCollection<BaseMongoModel> _collection;

        public bool IsEnable;

        public StatisticsFilter(bool isEnable = true)
        {
            _collection = new MongoDBClient().MongoDb.GetCollection<BaseMongoModel>("User_Merchant_ID");
            IsEnable = isEnable;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //if (IsEnable == false)
                return;

            var id = context.HttpContext.Request.Headers["userid"];
            var path = context.HttpContext.Request.Path;

            if(string.IsNullOrEmpty(id) == true)
                context.Result = new RedirectResult($"fail?code={(int)ErrorCodeEnum.NoUser_Mer_ID}");

            var find = _collection.Find(q => q.RecordID == id);
            if(find == null)
                context.Result = new RedirectResult($"fail?code={(int)ErrorCodeEnum.NoUser_Mer_ID}");
        }
    }
}
