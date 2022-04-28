using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBP.Main.Filter;
using CBP.Models;
using CBP.UserServices;
using CBP.UserServices.User;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CBP.Main.Controllers.CBPBusiness.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost, Route("login"), StatisticsFilter(false)]
        public Task<ResponseModel> Login(RequestModel request)
        {
            return _service.Login(request);
        }


        [HttpPost, Route("register")]
        public Task<ResponseModel> Register(RequestModel request)
        {
            return _service.Register(request);
        }

        [HttpGet, Route("test")]
        public string Test(string msg)
        {
            return msg;
        }
    }
}