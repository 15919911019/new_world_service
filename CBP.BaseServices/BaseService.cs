using AutoMapper;
using CBP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Public.DbHelper;
using Public.DbHelper.Mongon;
using Public.DbHelper.MySql;
using Public.DbHelper.Sql;
using Public.Tools;
using System;
using System.Threading.Tasks;

namespace CBP.BaseServices
{
    public abstract class BaseService 
    {
        public virtual string ServiceName { get; }

        public IMapper Mapper { get; set; }

        private AbsDatabaseClient _dataBase;

        #region 接口访问管理

        public virtual Task<ResponseModel> Command(string commandName, RequestModel request)
        {
            return Task.Factory.StartNew(() => { return ResponseModel.Success(); });
        }
        #endregion


        #region 数据库

        public virtual AbsDatabaseClient Database
        {
            get
            {
                if(_dataBase == null)
                    _dataBase = new MySqlClient(SqlConnectionString ?? _sqlConnectionString);
                return _dataBase;

                //return new SqlClient(SqlConnectionString ?? _sqlConnectionString);
            }
        }

        public virtual string SqlConnectionString { get; set; }
        //private static string _sqlConnectionString = Tool.GetConfig("ConnectionSetting:SQLConnectionString"); 
        private static string _sqlConnectionString = Tool.GetConfig("ConnectionSetting:MySQLConnectionString"); 

        public virtual void InitDb(IServiceCollection services, IConfiguration configuration)
        {
        }

        #endregion


        #region 日志

        protected void LoggerError(string log, Exception ex = null)
        {
            Public.Log.Logger.Error(log, ex);
        }

        protected void LoggerDebug(string log, Exception ex = null)
        {
            Public.Log.Logger.Debug(log, ex);
        }

        #endregion


        #region Token PasswordKey

        protected IMongoCollection<TokenModel> TokenCollection;

        protected static string Skey = GlobalConfigModel.SKey;

        public BaseService()
        {
            TokenCollection = new MongoDBClient().MongoDb.GetCollection<TokenModel>("UserToken");
        }

        #endregion


        protected (bool, ResponseModel) CheckRequestCount(RequestModel request, int count)
        {
            var bol = request?.Param?.Count == count;
            if (bol == false)
                return (false, ResponseModel.Fail("参数异常"));

            return (true, null);
        }


        protected (bool, Task<ResponseModel>, string) ExecuteMethod(string moduleName, string interfaceName, string commandName, RequestModel request)
        {
            var instance = ModuleManager.GetInstance(moduleName, interfaceName);
            var method = ModuleManager.GetMethodInfo(moduleName, interfaceName, commandName);
            if (instance == null || method == null)
                return (false, null, instance == null ? "组件没有注册" : "方法没有实现");

            var result = (Task<ResponseModel>)method.Invoke(instance, request == null ? null : new object[] { request });
            return (true, result, string.Empty);
        }
    }
}
