using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CBP.BaseServices;
using CBP.Models;
using Public.Log;
using Public.Tools;
using CBP.Permission.Models.UserRole;
using CBP.Permission.Services.Function;
using CBP.Permission.Services.Role;
using CBP.UserServices;
using System.Linq;
using CBP.UserModels;
using CBP.Permission.Models.Role;
using CBP.UserServices.User;
using CBP.UserModels.User;
using CBP.Permission.Models.Function;

namespace CBP.Permission.Services.UserRole
{
    public class UserRoleService : BaseService, IUserRoleService
    {
        public override string ServiceName => "Permission";

        public string UserRoleTName { get; } = "UserRole";

        private readonly IRoleService _roleService;
        private readonly IFunctionService _funService;
        private readonly IUserService _userService;

        public UserRoleService() { }

        public UserRoleService(IRoleService role, IFunctionService fun, IUserService user)
        {
            _roleService = role;
            _funService = fun;
            _userService = user;
        }

        public Task<ResponseModel> Create(string userID, string roleID)
        {
            return Task.Factory.StartNew(() =>
            {
                ResponseModel response = null;
                try
                {
                    UserRoleModel model = new UserRoleModel()
                    {
                        UserID = userID,
                        RoleID = roleID,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        DelMarker = false,
                        IsEnable = true,
                        RecordID = Tool.GuidTo16String()
                    };
                    var temp = Database.Insert(model, UserRoleTName);
                    if (temp == 0)
                        return ResponseModel.Fail("创建失败");

                    response = new ResponseModel();
                    response.Data = temp;
                }
                catch (Exception ex)
                {
                    response = ResponseModel.Excetption("创建用户-角色异常", ex);
                    Logger.Error("创建用户-角色异常", ex);
                }

                return response;
            });
        }

        public Task<ResponseModel> Delete(string userID, string roleID)
        {
            return Task.Factory.StartNew(() =>
            {
                ResponseModel response = null;
                try
                {
                    var where = $"UserID = '{userID}' and RoleID = '{roleID}'";
                    var temp = Database.Delete(UserRoleTName, where);
                    if (temp == 0)
                        return ResponseModel.Fail("删除失败");

                    response = new ResponseModel();
                    response.Data = temp;
                }
                catch (Exception ex)
                {
                    response = ResponseModel.Excetption("删除用户-角色异常", ex);
                    Logger.Error("删除用户-角色异常", ex);
                }

                return response;
            });
        }

        public Task<ResponseModel> Search(RequestModel request)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (request?.Param?.Count != 1)
                        return ResponseModel.Fail("参数异常");

                    var id = request.Param[0].ToString().Trim();
                    var where = string.IsNullOrEmpty(id) ? null : $" ur.userid = '{id}'";

                    var countSql = $"select count(0) " +
                                  $"from tc.user as u " +
                                  $"left join tc.userrole as ur  on u.recordid = ur.userid " +
                                  $"left join tc.role as r on ur.roleid = r.recordid " +
                                  $"left join tc.rolefunction as rf on r.recordid = rf.roleid " +
                                  $"left join tc.function as f on rf.functionid = f.recordid " +
                                  $"where {where} ";

                    var dataSql = $"select u.recordid, u.username, " +
                                  $"r.recordid as roleid, r.rolename, " +
                                  $"f.recordid as functionid, f.functionname, f.functionvalue " +
                                  $"from tc.user as u " +
                                  $"left join tc.userrole as ur  on u.recordid = ur.userid " +
                                  $"left join tc.role as r on ur.roleid = r.recordid " +
                                  $"left join tc.rolefunction as rf on r.recordid = rf.roleid " +
                                  $"left join tc.function as f on rf.functionid = f.recordid " +
                                  $"where {where} " +
                                  $"limit {(request.PageIndex - 1) * request.PageSize}, {request.PageIndex * request.PageSize}";

                    var data = Database.Query(countSql, dataSql);
                    if (data.Item1 == 0)
                        return ResponseModel.Fail("没有数据");

                    var u = Tool.DataTableToList<UserModel>(data.Item2);
                    var rf = Tool.DataTableToList<RoleFunctionMapModel>(data.Item2);

                    var roleIDs = rf.Select(q => q.RoleID).Distinct().ToList();
                    var result = new UserRoleMapModel();
                    result.UserID = u.FirstOrDefault()?.RecordID;
                    result.UserName = u.FirstOrDefault()?.UserName;

                    List<RoleMapModel> roles = new List<RoleMapModel>();
                    roleIDs.ForEach(q =>
                    {
                        var r = rf.FirstOrDefault();
                        roles.Add(new RoleMapModel()
                        {
                            RoleID = r.RoleID,
                            RoleName = r.RoleName,
                            FunctionList = rf.Where(f => f.RoleID == r.RoleID).Select(s =>
                                new FunctionMapModel()
                                {
                                    RecordID = s.FunctionID,
                                    FunctionName = s.FunctionName,
                                    FunctionValue = s.FunctionValue
                                }).ToList()
                        });
                    });

                    result.RoleList = roles;
                    return ResponseModel.Success(result);
                }
                catch (Exception ex)
                {
                    Logger.Error("查询用户权限异常", ex);
                    return ResponseModel.Excetption("查询用户权限异常", ex);
                }
            });
        }

        public Task<ResponseModel> GetPermission(string userID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var sql = $"select u.recordid, u.username, ur.roleid, f.functionvalue" +
                    $" from (select * from `{_userService.UserTName}`  where recordid = '{userID}') u " +
                    $"left join `{UserRoleTName}` ur on u.recordid = ur.userid " +
                    $"left join `{_roleService.RoleFunctionTabName}` rf on ur.roleid = rf.roleid " +
                    $"left join `{_funService.FunctionTabName}` f on rf.functionid = f.recordid";

                    var data = Database.QueryDt(sql);
                    var temp = Tool.DataTableToList<UserRoleMapModel>(data);

                    return ResponseModel.Success(temp);
                }
                catch (Exception ex)
                {
                    return ResponseModel.Excetption("获取权限异常", ex);
                };
            });
        }
    }
}
