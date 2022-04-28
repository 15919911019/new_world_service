using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBP.Models;
using CBP.Permission.Services.UserRole;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CBP.Main.Controllers.CBPBusiness.Permission
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private IUserRoleService _service;

        public UserRoleController(IUserRoleService service)
        {
            _service = service;
        }

        [HttpGet, Route("create")]
        public Task<ResponseModel> Create(string userID, string roleID)
        {
            return _service.Create(userID, roleID);
        }

        [HttpGet, Route("delete")]
        public Task<ResponseModel> Delete(string userID, string roleID)
        {
            return _service.Delete(userID, roleID);
        }

        [HttpPost, Route("search")]
        public Task<ResponseModel> Search(RequestModel request)
        {
            return _service.Search(request);
        }
    }
}