using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBP.Models;
using CBP.Permission.Services.Role;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CBP.Main.Controllers.CBPBusiness.Permission
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private IRoleService _service;

        public RoleController(IRoleService service)
        {
            _service = service;
        }

        [HttpGet, Route("create")]
        public Task<ResponseModel> Create(string roleName)
        {
            return _service.Create(roleName);
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
        public Task<ResponseModel> Update(string recordID, string roleName)
        {
            return _service.Update(recordID, roleName);
        }

        [HttpGet, Route("createrolefunc")]
        public Task<ResponseModel> CreateRoleFunc(string roleID, string funcID)
        {
            return _service.CreateRoleFunc(roleID, funcID);
        }

        [HttpGet, Route("deleterolefunc")]
        public Task<ResponseModel> DeleteRoleFunc(string roleID, string funcID)
        {
            return _service.DeleteRoleFunc(roleID, funcID);
        }

        [HttpGet, Route("searchfunctionbyroleid")]
        public Task<ResponseModel> SearchFunctionByRoleID(string roleID)
        {
            return _service.SearchFunctionByRoleID(roleID);
        }
   
    }
}