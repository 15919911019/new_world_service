using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CBP.Main.Core.Service;
using CBP.Main.Filter;
using CBP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Public.DbHelper.Mongon;

namespace CBP.Main.Controllers
{
    [Route("api/[controller]")]
    [ApiController, ParamFilter, SqlFilter, StatisticsFilter]
    public class BaseController : ControllerBase
    {
        protected IMongoCollection<TokenModel> TokenMogonCollection { get; private set; }

        public BaseController()
        {
            //TokenMogonCollection = new MongoDBClient().MongoDb.GetCollection<TokenModel>("UserToken");
        }

        [HttpGet]
        [Route("fail")]
        public ActionResult<ResponseModel> Fail(ErrorCodeEnum code)
        {
            var model = new ResponseModel();
            model.Code = code;
            model.Message = "请求异常";

            return model;
        }
    }
}