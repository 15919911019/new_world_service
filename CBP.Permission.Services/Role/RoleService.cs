using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Public.Log;
using Public.Tools;
using CBP.Models;
using CBP.Permission.Models.Role;
using CBP.BaseServices;
using CBP.Permission.Services.Function;
using CBP.Permission.Models.Function;
using System.Linq;
using CBP.Permission.Models.Menu;
using AutoMapper;

namespace CBP.Permission.Services.Role
{
    public class RoleService : BaseService, IRoleService
    {
        public override string ServiceName => "Permission";

        public string RoleTabName { get => "Role"; }

        public string RoleFunctionTabName { get => "RoleFunction"; }

        public string RoleMenuTabName { get => "RoleMenu"; }

        private readonly IFunctionService _funService;

        public RoleService() { }

        public RoleService(IFunctionService functionService, IMapper mapper) 
        {
            _funService = functionService;
            Mapper = mapper;
        }

        public RoleService(IFunctionService fun)
        {
            _funService = fun;
        }

        public Task<ResponseModel> Create(string name)
        {
            return Task.Factory.StartNew(() =>
            {
                ResponseModel response = null;
                try
                {
                    if (string.IsNullOrEmpty(name) == true)
                        return ResponseModel.Fail("名称为空");

                    var na = $"select RecordID from `{RoleTabName}` where RoleName = '{name}'";

                    var exist = Database.Exists(na);
                    if (exist == true)
                        return ResponseModel.Fail($"角色名称：{name} 已经存在");

                    var model = new RoleMapModel();
                    model.RoleName = name;

                    var data = Mapper.Map<RoleModel>(model);
                    var temp = Database.Insert(data, RoleTabName);
                    if (temp == 0)
                        return ResponseModel.Fail("创建失败");

                    response = new ResponseModel();
                    response.Data = temp;
                }
                catch (Exception ex)
                {
                    response = ResponseModel.Excetption("创建角色异常", ex);
                    Logger.Error("创建角色异常", ex);
                }

                return response;
            });
        }

        public Task<ResponseModel> Delete(string recordID)
        {
            return Task.Factory.StartNew(() =>
            {
                ResponseModel response = null;
                try
                {
                    var delete = $"delete `{RoleTabName}` where RecordID = '{recordID}'";
                    var temp = Database.ExecuteSql(delete);
                    if (temp == 0)
                        return ResponseModel.Fail("删除失败");

                    response = new ResponseModel();
                    response.Data = temp;
                }
                catch (Exception ex)
                {
                    response = ResponseModel.Excetption("删除角色异常", ex);
                    Logger.Error("删除角色异常", ex);
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
                        return ResponseModel.Fail("参数错误");

                    //var id = request.Param[0].ToString().Trim();
                    var name = request.Param[0].ToString().Trim();
                    //var idwhere = string.IsNullOrEmpty(id) ? null : $"r.recordid = '{id}'";
                    var namewhere = string.IsNullOrEmpty(name) ? "1=1" : $"r.rolename like '%{name}%'";
                    //var where = string.IsNullOrEmpty(idwhere) ? namewhere : idwhere;
                    var countSql = $"select count(0) from `{RoleTabName}` as r where {namewhere}";

                    var dataSql = $"select r.recordid as roleid, r.rolename, " +
                                  $"f.recordid as functionid, f.functionname, f.functionvalue " +
                                  $"from tc.role as r " +
                                  $"left join tc.rolefunction as rf on r.recordid = rf.roleid " +
                                  $"left join tc.function as f on rf.functionid = f.recordid " +
                                  $"where {namewhere} " +
                                  $"limit {(request.PageIndex - 1) * request.PageSize}, {request.PageIndex * request.PageSize}";
                    var data = Database.Query(countSql, dataSql);
                    if (data.Item1 == 0)
                        return ResponseModel.Fail("没有数据");

                    var rf = Tool.DataTableToList<RoleFunctionMapModel>(data.Item2).Where(q => q.FunctionID != null || q.RoleID != null).ToList();
                    return ResponseModel.Success(rf);
                }
                catch (Exception ex)
                {
                    Logger.Error("RoleService search", ex);
                    return ResponseModel.Excetption("RoleService search", ex);
                }
            });
        }

        public Task<ResponseModel> Update(string id, string name)
        {
            return Task.Factory.StartNew(() =>
            {
                ResponseModel response = null;
                try
                {
                    var update = $"update `{RoleTabName}` set RoleName = '{name}' " +
                    $" where RecodeID = '{id}'";
                    var temp = Database.ExecuteSql(update);
                    if (temp == 0)
                        return ResponseModel.Fail("修改失败");

                    response = new ResponseModel();
                    response.Data = temp;
                }
                catch (Exception ex)
                {
                    response = ResponseModel.Excetption("修改角色异常", ex);
                    Logger.Error("修改角色异常", ex);
                }

                return response;
            });
        }


        public Task<ResponseModel> CreateRoleFunc(string roleID, string functionID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var data = new RoleFunctionMapModel();
                    data.RoleID = roleID;
                    data.FunctionID = functionID;

                    var model = Mapper.Map<RoleFunctionModel>(data);
                    var temp = Database.Insert(model, RoleFunctionTabName);
                    if (temp == 1)
                        return ResponseModel.Success();
                    else
                        return ResponseModel.Fail("创建失败");
                }
                catch (Exception ex)
                {
                    Logger.Error("创建角色-功能异常", ex);
                    return ResponseModel.Excetption("创建角色-功能异常", ex);
                }
            });
        }

        public Task<ResponseModel> DeleteRoleFunc(string roleID, string funcID)
        {
            return Task.Factory.StartNew(() =>
            {
                ResponseModel response = null;
                try
                {
                    var delete = $"delete {RoleFunctionTabName} where RoleID = '{roleID}' and FunctionID = '{funcID}'";
                    var temp = Database.ExecuteSql(delete);
                    if (temp == 0)
                        return ResponseModel.Fail("删除失败");

                    response = new ResponseModel();
                    response.Data = temp;
                }
                catch (Exception ex)
                {
                    response = ResponseModel.Excetption("删除角色-功能异常", ex);
                    Logger.Error("删除角色-功能异常", ex);
                }

                return response;
            });
        }

        public Task<ResponseModel> SearchFunctionByRoleID(string roleID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var where = string.IsNullOrEmpty(roleID) ? "1=1" : $"r.RecordID = '{roleID}'";
                    var sql = $"select rf.RoleID, r.RoleName, f.RecordID, f.FunctionName, f.FunctionValue " +
                    $"from tc.{RoleTabName} r " +
                    $"left join tc.{RoleFunctionTabName} rf on r.RecordID = rf.RoleID " +
                    $"left join tc.{_funService.FunctionTabName} f on rf.FunctionID = f.RecordID " +
                    $"where {where}";
                    var data = Database.Query<RoleFunctionMapModel>(sql);
                    if (data.Count == 0)
                        return ResponseModel.Success();

                    var roleIDs = data.Select(f => f.RoleID).Distinct().ToList();

                    List<RoleMapModel> roles = new List<RoleMapModel>();
                    roleIDs.ForEach(q =>
                    {
                        var r = data.FirstOrDefault();
                        roles.Add(new RoleMapModel() 
                        { 
                            RoleID = r.RoleID, 
                            RoleName = r.RoleName,
                            FunctionList = data.Where(f => f.RoleID == r.RoleID).Select(s =>
                                new FunctionMapModel()
                                {
                                    RecordID = s.FunctionID,
                                    FunctionName = s.FunctionName,
                                    FunctionValue = s.FunctionValue
                                }).ToList()
                        });
                    });

                    return ResponseModel.Success(roles);
                }
                catch (Exception ex)
                {
                    Logger.Error("根据角色id查询功能异常", ex);
                    return ResponseModel.Error(ex.Message);
                }
            });
        }

        public Task<ResponseModel> CreateRoleMenu(string roleID, string menuID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var data = new RoleMenuModel();
                    data.RoleID = roleID;
                    data.MenuID = menuID;

                    var temp = Database.Insert(data, RoleMenuTabName);
                    if (temp == 1)
                        return ResponseModel.Success();
                    else
                        return ResponseModel.Fail("创建失败");
                }
                catch (Exception ex)
                {
                    Logger.Error("创建角色-菜单异常", ex);
                    return ResponseModel.Excetption("创建角色-菜单异常", ex);
                }
            });
        }

        public Task<ResponseModel> DeleteRoleMenu(string roleID, string menuID)
        {
            return Task.Factory.StartNew(() =>
            {
                ResponseModel response = null;
                try
                {
                    var delete = $"delete {RoleMenuTabName} where RoleID = '{roleID}' and MenuID = '{menuID}'";
                    var temp = Database.ExecuteSql(delete);
                    if (temp == 0)
                        return ResponseModel.Fail("删除失败");

                    response = new ResponseModel();
                    response.Data = temp;
                }
                catch (Exception ex)
                {
                    response = ResponseModel.Excetption("删除角色-菜单异常", ex);
                    Logger.Error("删除角色-菜单异常", ex);
                }

                return response;
            });
        }

        public Task<ResponseModel> SearchRoleMenu(RequestModel request)
        {
            return Task.Factory.StartNew(() =>
            {
                ResponseModel response = null;
                try
                {

                }
                catch (Exception ex)
                {
                    response = ResponseModel.Excetption("RoleService create", ex);
                    Logger.Error("RoleService create", ex);
                }

                return response;
            });
        }


        private List<FunctionModel> GetFunction(List<RoleModel> roleIDs)
        {
            StringBuilder sb = new StringBuilder();
            roleIDs.ForEach(q =>
            {
                sb.Append($"'{q.RecordID}',");
            });
            var view = sb.ToString();
            var sql = $"select FunctionName, FunctionValue from CBP_Function  where " +
                $"RoleID in ({sb.ToString().TrimEnd(',')}) " +
                $"left jion SST_Function on " +
                $"FunctionValue";

            return null;
        }

        public Task<ResponseModel> GetAllRole()
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var sql = $"select * from `{RoleTabName}` where DelMarker = 0 and IsEnable = 1";
                    var data = Database.Query<RoleModel>(sql);
                    return ResponseModel.Success(data);
                }
                catch (Exception ex)
                {
                    Logger.Error("获取全部角色信息失败", ex);
                    return ResponseModel.Excetption("获取全部角色信息失败", ex);
                }
            });
        }

    }
}
