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
    public class CityController : Controller
    {
        private readonly IAreaService _service;

        public CityController(IAreaService service)
        {
            _service = service;
        }

        [HttpGet, Route("create")]
        public Task<ResponseModel> Create(string name, string proRecID)
        {
            return _service.CreateCity(name, proRecID);
        }

        [HttpGet, Route("update")]
        public Task<ResponseModel> Update(string name, string proRecID, string recordID)
        {
            return _service.UpdateCity(name, proRecID, recordID);
        }

        [HttpPost, Route("search")]
        public Task<ResponseModel> Search(RequestModel req)
        {
            return _service.SearchCity(req.Param?.Count > 0 ? req.Param[0].ToString() : null);
        }

        [HttpGet, Route("delete")]
        public Task<ResponseModel> Delete(string recordID)
        {
            return _service.DeleteCity(recordID);
        }
    }
}
