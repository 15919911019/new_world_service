using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBP.Models;
using CBP.Permission.Services.Function;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CBP.Main.Controllers.CBPBusiness.Permission
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunctionController : BaseController
    {
        private IFunctionService _service;

        public FunctionController(IFunctionService service)
        {
            _service = service;
        }

        [HttpGet, Route("create")]
        public Task<ResponseModel> Create(string functionName, string functionValue)
        {
            return _service.Create(functionName, functionValue);
        }

        [HttpGet, Route("delete")]
        public Task<ResponseModel> Delete(string recordID)
        {
            return _service.Delete(recordID);
        }

        [HttpPost, Route("search")]
        public Task<ResponseModel> Search(RequestModel request)
        {
            return _service.Search(request);
        }

        [HttpGet, Route("update")]
        public Task<ResponseModel> Update(string functionName, string functionValue, string recordID)
        {
            return _service.Update(functionName, functionValue, recordID);
        }
    }
}