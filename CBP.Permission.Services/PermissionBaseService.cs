using CBP.BaseServices;
using CBP.Models;
using CBP.Permission.Services.Function;
using CBP.Permission.Services.Permission;
using CBP.Permission.Services.Role;
using CBP.Permission.Services.UserRole;
using System.Threading.Tasks;

namespace CBP.Permission.Services
{
    public class PermissionBaseService : BaseService
    {
        //public override string ServiceName => "Permission";

        //private IFunctionService _function;

        //private IRoleService _role;

        //private IUserRoleService _userRole;

        //private IPermissionService _permission;

        //public PermissionBaseService(
        //    IFunctionService fun, 
        //    IRoleService role, 
        //    IUserRoleService userRole, 
        //    IPermissionService perssion)
        //{
        //    _function = fun;
        //    _role = role;
        //    _userRole = userRole;
        //    _permission = perssion;
        //}

        //public override Task<ResponseModel> Command(string commandName, RequestModel request)
        //{
        //    return base.Command(commandName, request);
        //}
    }
}
