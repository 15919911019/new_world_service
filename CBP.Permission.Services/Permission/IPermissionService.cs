using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using CBP.Models;

namespace CBP.Permission.Services.Permission
{
    public interface IPermissionService
    {
        Task<ResponseModel> GetPermission(RequestModel request);

    }
}
