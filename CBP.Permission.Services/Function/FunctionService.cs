using System;
using System.Threading.Tasks;

using CBP.BaseServices;
using CBP.Models;
using CBP.Permission.Models.Function;
using Public.Tools;
using Public.Log;
using AutoMapper;
using System.Linq;

namespace CBP.Permission.Services.Function
{
    public class FunctionService : BaseService, IFunctionService
    {
        public override string ServiceName => "Permission";

        public string FunctionTabName { get => "Function"; }


        public FunctionService() { }

        public FunctionService(IMapper mapper)
        {
            Mapper = mapper;
        }


        public Task<ResponseModel> Create(string name, string value)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
                        return ResponseModel.Fail("参数错误");

                    var na = $"select RecordID from `{FunctionTabName}` where FunctionName = '{name}'";
                    var va = $"select RecordID from `{FunctionTabName}` where FunctionValue = '{value}'";

                    var exist = Database.Exists(na);
                    if (exist == true)
                        return ResponseModel.Fail($"功能名称：{name} 已经存在");
                    exist = Database.Exists(va);
                    if (exist == true)
                        return ResponseModel.Fail($"功能值：{value} 已经存在");

                    FunctionMapModel model = new FunctionMapModel();
                    model.FunctionName = name;
                    model.FunctionValue = value;

                    var insert = Mapper.Map<FunctionModel>(model);
                    var temp = Database.Insert(insert, FunctionTabName);
                    if (temp < 1)
                        return ResponseModel.Fail($"创建{name}失败");

                    return ResponseModel.Success(temp);
                }
                catch (Exception ex)
                {
                    Logger.Error("创建权限功能失败", ex);
                    return ResponseModel.Error(ex.Message);
                }

            });
        }

        public Task<ResponseModel> Delete(string id)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(id))
                        return ResponseModel.Fail("参数错误");

                    var temp = Database.Delete(FunctionTabName, recordId:id);
                    if (temp == 0)
                        return ResponseModel.Fail("删除权限功能失败");

                    return ResponseModel.Success(temp);
                }
                catch (Exception ex)
                {
                    Logger.Error("删除权限功能异常", ex);
                    return ResponseModel.Excetption(ex.Message);
                }
            });
        }

        public Task<ResponseModel> GetAllFunction()
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var data = Database.Query<FunctionModel>(FunctionTabName, 1, int.MaxValue);
                    if (data.Item1 == 0)
                        return ResponseModel.Fail("没有数据");

                    var temp = data.Item2.Select(q => Mapper.Map<FunctionMapModel>(q)).ToList();
                    return ResponseModel.Success(temp);
                }
                catch (Exception ex)
                {
                    Logger.Error("更新权限功能异常", ex);
                    return ResponseModel.Excetption("更新权限功能异常", ex);
                }
            });
        }

        public Task<ResponseModel> Search(RequestModel request)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (request?.Param?.Count != 1)
                        return ResponseModel.Error("参数错误");

                    var name = request.Param[0]?.ToString().Trim();
                    var where = string.IsNullOrEmpty(name) ? null : $"FunctionName like '%{name}%'";

                    var data = Database.Query<FunctionModel>(FunctionTabName, request.PageIndex, request.PageSize, where);
                    if (data.Item1 == 0)
                        return ResponseModel.Fail("没有数据");

                    var temp = data.Item2.Select(q => Mapper.Map<FunctionMapModel>(q)).ToList();
                    return ResponseModel.Success(temp);
                }
                catch (Exception ex)
                {
                    Logger.Error("更新权限功能异常", ex);
                    return ResponseModel.Excetption("更新权限功能异常", ex);
                }
            });
        }

        public Task<ResponseModel> Update(string id, string name, string value)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
                        return ResponseModel.Fail("参数异常");

                    var sql = $"update `{FunctionTabName}` set " +
                    $"UpdateTime = '{DateTime.Now}'," +
                    $"FunctionName = '{name}', " +
                    $"FunctionValue = '{value}' " +
                    $"where RecordID = '{id}'";

                    var temp = Database.ExecuteSql(sql);
                    if (temp == 0)
                        return ResponseModel.Fail("修改失败");
                    return ResponseModel.Success(temp);
                }
                catch (Exception ex)
                {
                    Logger.Error("更新权限功能失败", ex);
                    return ResponseModel.Error(ex.Message);
                }
            });
        }
    }
}
