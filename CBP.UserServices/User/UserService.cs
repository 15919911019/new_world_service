using CBP.BaseServices;
using CBP.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Public.Log;
using System.Threading.Tasks;
using AutoMapper;
using CBP.UserModels.User;
using Public.Tools;
using MongoDB.Driver;

namespace CBP.UserServices.User
{
    public class UserService : BaseService, IUserService
    {
        public override string ServiceName => "User";

        public string UserTName { get; } = "User";

        public UserService() { }

        public UserService(IMapper mapper)
        {
            Mapper = mapper;
        }

        public Task<ResponseModel> Register(RequestModel request)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var con = CheckRequestCount(request, 3);
                    if (con.Item1 == false)
                        return con.Item2;

                    var name = request.Param?[0].ToString();
                    var mobile = request.Param?[1].ToString();
                    var password = request.Param?[2].ToString();

                    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
                        return ResponseModel.Fail("用户名或密码不能为空");

                    var exists = $"select count(0) from {UserTName} where Username = '{name}'";
                    var exi = Database.Exists(exists);
                    if (exi == true)
                        return ResponseModel.Fail($"用户名 {name} 已存在");

                    UserMapModel model = new UserMapModel();
                    model.UserName = name;
                    model.Mobile = mobile;
                    model.Password = Tool.Md5Encrypt(password);

                    var data = Mapper.Map<UserModel>(model);
                    var temp = Database.Insert(data, UserTName);
                    if (temp == 0)
                        return ResponseModel.Fail("创建用户失败");

                    var guid = Tool.GuidTo16String();
                    TokenCollection.InsertOne(new TokenModel() { UserID =model.RecordID, TokenGuid = guid });

                    return ResponseModel.Success(new { model.Mobile, Token = guid });
                }
                catch (Exception ex)
                {
                    Logger.Error("注册用户异常", ex);
                    return ResponseModel.Excetption("注册用户异常", ex);
                }
            });
        }

        public Task<ResponseModel> Delete(string userID)
        {
            return Task.Factory.StartNew(() =>
            {
                ResponseModel response = null;
                try
                {
                    var del = $"update {UserTName} set DelMarker = 1 where UserID = '{userID}'";
                    var temp = Database.ExecuteSql(del);
                    if (temp == 0)
                        return ResponseModel.Fail("删除用户失败");

                    response = ResponseModel.Success();
                }
                catch (Exception ex)
                {
                    Logger.Error("创建用户异常", ex);
                    response = ResponseModel.Excetption("创建用户异常", ex);
                }
                return response;
            });
        }

        public Task<ResponseModel> Login(RequestModel request)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var userName = request.Param?[0].ToString();
                    var time = request.Param?[1].ToString();
                    var sign = request.Param?[2].ToString();

                    if (string.IsNullOrEmpty(userName))
                        return ResponseModel.Fail("用户名为空");

                    var sql = $"select * from {UserTName} " +
                    $"where DelMarker = 0 and IsEnable = 1 and " +
                    $"({(string.IsNullOrEmpty(userName) ? "1=1" : $"UserName = '{userName}'")} or " +
                    $"{(string.IsNullOrEmpty(userName) ? "1=1" : $"Mobile = '{userName}'")}) ";

                    var data = Database.Query<UserModel>(sql);
                    if (data.Count == 0)
                        return ResponseModel.Fail("用户不存在");

                    var psw = Tool.Md5Decrypt(data[0].Password);
                    var temp = Tool.GenerateMD5($"{psw}{time}{Skey}");
                    if (psw != temp)
                        return ResponseModel.Fail("密码错误");

                    FilterDefinitionBuilder<TokenModel> builderFilter = Builders<TokenModel>.Filter;
                    var filter = Builders<TokenModel>.Filter.Eq("UserID", data[0].RecordID);
                    var token = TokenCollection.Find(filter).ToList();
                    var guid = Tool.GuidTo16String();
                    if (token.Count == 0)
                        TokenCollection.InsertOne(new TokenModel() { UserID = data[0].RecordID, TokenGuid = guid });
                    else
                    {
                        var filUpdate = Builders<TokenModel>.Filter.Eq("UserID", data[0].RecordID);
                        var update = Builders<TokenModel>.Update.Set("TokenGuid", guid);
                        var result = TokenCollection.UpdateOne(filUpdate, update);
                    }

                    return ResponseModel.Success(new { data[0].Mobile, Token = guid });
                }
                catch (Exception ex)
                {
                    Logger.Error("用户登录异常", ex);
                    return ResponseModel.Excetption("用户登录异常", ex);
                }
            });
        }

        public Task<ResponseModel> Search(RequestModel request)
        {
            return Task.Factory.StartNew(() =>
            {
                ResponseModel response = null;
                try
                {

                }
                catch (Exception ex)
                {
                    Logger.Error("创建用户异常", ex);
                }
                return response;
            });
        }

        public Task<ResponseModel> Update(string userID, string userName)
        {
            return Task.Factory.StartNew(() =>
            {
                ResponseModel response = null;
                try
                {

                }
                catch (Exception ex)
                {
                    Logger.Error("创建用户异常", ex);
                }
                return response;
            });
        }
    }
}
