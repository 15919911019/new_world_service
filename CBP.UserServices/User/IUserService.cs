using CBP.Models;
using System;
using System.Threading.Tasks;

namespace CBP.UserServices.User
{
    public interface IUserService
    {
        public string UserTName { get; }

        Task<ResponseModel> Login(RequestModel request);

        Task<ResponseModel> Register(RequestModel request);
    }
}
