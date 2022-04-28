//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using CBP.Models;
//using CBP.Permission.Services.Permission;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace CBP.Main.Controllers.CBPBusiness.Permission
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PermissionController : BaseController
//    {
//        private readonly IPermissionService _service;

//        public PermissionController(IPermissionService service)
//        {
//            _service = service;
//        }

//        [HttpGet, Route("getpermission")]
//        public Task<ResponseModel> GetPermission(string userID)
//        {
//            return _service.GetPermission(userID);
//        }
//    }
//}