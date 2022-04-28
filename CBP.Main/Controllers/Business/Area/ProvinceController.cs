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
    public class ProvinceController : Controller
    {
        private readonly IAreaService _service;

        public ProvinceController(IAreaService service)
        {
            _service = service;
        }

        [HttpGet, Route("create")]
        public Task<ResponseModel> Create(string name)
        {
            return _service.CreateProvince(name);
        }

        [HttpGet, Route("update")]
        public Task<ResponseModel> Update(string name, string id)
        {
            return _service.UpdateProvince(name, id);
        }

        [HttpPost, Route("search")]
        public Task<ResponseModel> Search(RequestModel req)
        {
            return _service.SearchProvince();
        }

        [HttpGet, Route("delete")]
        public Task<ResponseModel> Delete(string recordID)
        {
            return _service.DeleteProvince(recordID);
        }
    }
}
