//using MongoDB.Driver;
using MongoDB.Driver;
using Public.Log;
using Public.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.DbHelper.Mongon
{
    public class MongoDBClient
    {
        public IMongoDatabase MongoDb { get; private set; }

        public MongoDBClient()
        {
            try
            {
                if (MongoDb != null)
                    return;

                //数据库链接
                var connString = Tool.GetConfig("ConnectionSetting:MongoDb");
                var client = new MongoDB.Driver.MongoClient(connString);

                //获取数据库
                MongoDb = client.GetDatabase("Shopping_CBP");
            }
            catch (Exception ex)
            {
                Logger.Error("MongoClient 构造函数", ex);
            }
        }
    }
}
