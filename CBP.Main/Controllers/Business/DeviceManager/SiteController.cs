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
    public class SiteController : Controller
    {
        private readonly IDeviceService _service;

        public SiteController(IDeviceService service)
        {
            _service = service;
        }

        [HttpGet, Route("create")]
        public Task<ResponseModel> Create(string name, string countyID)
        {
            return _service.CreateSite(name, countyID);
        }

        [HttpGet, Route("update")]
        public Task<ResponseModel> Update(string name, string countyID, string recordID)
        {
            return _service.UpdateSite(name, countyID, recordID);
        }

        [HttpPost, Route("Search")]
        public Task<ResponseModel> Search(RequestModel req)
        {
            return _service.SearchSite(req);
        }

        [HttpGet, Route("delete")]
        public Task<ResponseModel> Delete(string recordID)
        {
            return _service.DeleteSite(recordID);
        }
    }
}
