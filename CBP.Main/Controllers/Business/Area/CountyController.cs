using Business.Area;
using CBP.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBP.Main.Controllers.Business.Area
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountyController : Controller
    {
        private readonly IAreaService _service;

        public CountyController(IAreaService service)
        {
            _service = service;
        }

        [HttpGet, Route("create")]
        public Task<ResponseModel> Create(string name, string cityRecID)
        {
            return _service.CreateCounty(name, cityRecID);
        }

        [HttpGet, Route("update")]
        public Task<ResponseModel> Update(string name, string cityRecID, string recordID)
        {
            return _service.UpdateCounty(name, cityRecID, recordID);
        }

        [HttpPost, Route("search")]
        public Task<ResponseModel> Search(RequestModel req)
        {
            return _service.SearchCounty(req.Param?.Count > 0 ? req.Param[0].ToString(): null);
        }

        [HttpGet, Route("delete")]
        public Task<ResponseModel> Delete(string recordID)
        {
            return _service.DeleteCounty(recordID);
        }

        [HttpGet, Route("area")]
        public Task<ResponseModel> SearchArea()
        {
            return _service.SearchArea();
        }
    }
}
