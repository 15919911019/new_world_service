using Business.DeviceServices;
using CBP.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBP.Main.Controllers.Business.DeviceManager
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : Controller
    {
        private readonly IDeviceService _service;

        public UnitController(IDeviceService service)
        {
            _service = service;
        }

        [HttpGet, Route("create")]
        public Task<ResponseModel> Create(string name, string siteID)
        {
            return _service.CreateUnit(name, siteID);
        }

        [HttpGet, Route("update")]
        public Task<ResponseModel> Update(string name, string siteID, string recordID)
        {
            return _service.UpdateUnit(name, siteID, recordID);
        }

        [HttpPost, Route("search")]
        public Task<ResponseModel> Search(RequestModel request)
        {
            return _service.SearchUnit(request);
        }

        [HttpGet, Route("delete")]
        public Task<ResponseModel> Delete(string recordID)
        {
            return _service.DeleteUnit(recordID);
        }
    }
}
