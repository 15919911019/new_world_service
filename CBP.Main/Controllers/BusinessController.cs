using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

using CBP.Main.Core.Service;
using CBP.Main.Filter;
using CBP.Models;
using CBP.BaseServices;

namespace CBP.Main.Controllers
{
    [Route("api/[controller]"), ParamFilter, SqlFilter, StatisticsFilter]
    [ApiController]
    public class BusinessController : BaseController
    {
        private IMapper _mapper;

        public BusinessController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpPost, Route("requestnoparam")]
        public Task<ResponseModel> RequestCommand()
        {
            try
            {
                var url = Request.Method;

                var modu = Request.Headers["module"];
                var inte = Request.Headers["service"];
                var comm = Request.Headers["command"];
                var token = Request.Headers["token"];
                var admin = Request.Headers["admin"];

                //if(string.IsNullOrEmpty(admin) == true)
                //{
                //    FilterDefinitionBuilder<TokenModel> builderFilter = Builders<TokenModel>.Filter;
                //    var filter = Builders<TokenModel>.Filter.Eq("TokenGuid", token);
                //    var key = TokenMogonCollection.Find(filter).ToList();
                //    if (key.Count == 0)
                //        return Task.Factory.StartNew(() => { return ResponseModel.Fail("token失效"); });
                //}

                var instance = ModuleManager.GetInstance(modu, inte);
                if (instance == null)
                    return Task.Factory.StartNew(() => { return ResponseModel.Fail("组件没有注册"); });
                instance.Mapper = _mapper;

                var method = ModuleManager.GetMethodInfo(modu, inte, comm);
                if (method == null)
                    return Task.Factory.StartNew(() => { return ResponseModel.Fail($"{modu}没有该实现：{instance}/{comm}"); });

                //return Task.Factory.StartNew(() =>
                //{
                //    return (ResponseModel)method.Invoke(instance, null);
                //});

                var result = method.Invoke(instance, null);
                return (Task<ResponseModel>)result;
            }
            catch (Exception ex)
            {
                return Task.Factory.StartNew(() => { return ResponseModel.Fail(ex.Message); });
            }
        }


        [HttpPost, Route("requestcommand")]
        public Task<ResponseModel> RequestCommand(RequestModel request)
        {
            try
            {
                if (ParamChech(request.Param) == true)
                    return Task.Factory.StartNew(() => { return ResponseModel.Fail("包含敏感参数"); }); 

                var modu = Request.Headers["module"];
                var inte = Request.Headers["service"];
                var comm = Request.Headers["command"].ToString().ToLower();
                var token = Request.Headers["token"];

                //if ((comm == "login" | comm == "register") == false)
                //{
                //    FilterDefinitionBuilder<TokenModel> builderFilter = Builders<TokenModel>.Filter;
                //    var filter = Builders<TokenModel>.Filter.Eq("TokenGuid", token);
                //    var key = TokenMogonCollection.Find(filter).ToList();
                //    if (key.Count == 0)
                //        return Task.Factory.StartNew(() => { return ResponseModel.Fail("token失效"); });
                //    else
                //        request.UserID = key[0].UserID;
                //}

                var instance = ModuleManager.GetInstance(modu, inte);
                if (instance == null)
                    return Task.Factory.StartNew(() => { return ResponseModel.Fail("组件没有注册"); });
                instance.Mapper = _mapper;

                var methodInfo = ModuleManager.GetMethodInfo(modu, inte, comm);
                if (methodInfo == null)
                    return Task.Factory.StartNew(() => { return ResponseModel.Fail($"{modu}没有该实现：{instance}/{comm}"); });

                var result = methodInfo.Invoke(instance, new object[] { request });
                return (Task<ResponseModel>)result;
            }
            catch (Exception ex)
            {
                return Task.Factory.StartNew(() => { return ResponseModel.Fail(ex.Message); });
            }
        }

        public bool ParamChech(List<object> param)
        {
            if (param == null)
                return false;

            foreach (var p in param)
            {
                var temp = p.ToString().Trim().ToLower();
                if (temp.Contains(" select ") ||
                    temp.Contains(" * ") ||
                    temp.Contains(" delete ") ||
                    temp.Contains(" where ") ||
                    temp.Contains(" or ") ||
                    temp.Contains(" and ") 
                   )
                    return true;
            }

            return false;
        }

       
        [HttpGet, Route("test")]
        public string Test(string msg)
        {
            return msg + DateTime.Now;
        }
    }
}