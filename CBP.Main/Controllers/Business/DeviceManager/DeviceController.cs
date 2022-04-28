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
    public class DeviceController : Controller
    {
        private readonly IDeviceService _service;

        public DeviceController(IDeviceService service)
        {
            _service = service;
        }

        [HttpGet, Route("create")]
        public Task<ResponseModel> Create(string name, string deviceID)
        {
            return _service.CreateDevice(name, deviceID);
        }

        [HttpGet, Route("update")]
        public Task<ResponseModel> Update(string name, string unitID, string recordID)
        {
            return _service.UpdateDevice(name, unitID, recordID);
        }

        [HttpPost, Route("search")]
        public Task<ResponseModel> Search(RequestModel req)
        {
            return _service.SearchDevice(req); 
        }

        [HttpGet, Route("delete")]
        public Task<ResponseModel> Delete(string recordID)
        {
            return _service.DeleteDevice(recordID);
        }

        [HttpGet, Route("setdeviceunit")]
        public Task<ResponseModel> SetDeviceUnit(string devID, string unitID)
        {
            return _service.SetDeviceUnit(devID, unitID);
        }
    }
}
