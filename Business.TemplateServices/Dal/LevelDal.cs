using Business.TemplateModels;
using log4net.Core;
using Public.DbHelper;
using Public.Log;
using Public.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TemplateServices.Dal
{
    public class LevelDal
    {
        private AbsDatabaseClient _database;

        private string _tabName;

        public LevelDal(AbsDatabaseClient database, string tabName)
        {
            _database = database;
            _tabName = tabName;
        }

        public string Create(LevelDbModel level)
        {
            try
            {
                var res = _database.Insert(level, _tabName);

                return res == 1 ? null : "插入失败";
            }
            catch(Exception ex)
            {
                Logger.Error("创建模型-等级异常", ex);
                return "创建模型-等级异常";
            }
        }

        public string Delete(string levelID)
        {
            try
            {

                return null;
            }
            catch (Exception ex)
            {
                Logger.Error("删除模型-等级异常", ex);
                return "删除模型-等级异常";
            }
        }

        public (int, List<LevelModel>) Search(int pageIdx, int size, string levelID = null)
        {
            try
            {
                var where = $"{(string.IsNullOrWhiteSpace(levelID) ? " 1=1 " : $" recordid = '{levelID}'")}";
                var con = $"select count(0) from {_tabName} where {where}";
                var sql = $"select * from {_tabName} where {where} limit {pageIdx * size}, {size}";
                var data = _database.Query<LevelModel>(con, sql);
                data.Item2?.ForEach(q => q.Levels = Tool.JsonToObject<List<LevelCharatModel>>(q.Content));

                return data;
            }
            catch (Exception ex)
            {
                Logger.Error("创建模型-等级异常", ex);
                return (0, null);
            }
        }

        public string Update(LevelDbModel level)
        {
            try
            {
                var sql = $"update {_tabName} set name = '{level.Name}', content = '{level.Content}' " +
                    $"where recordid = '{level.RecordID}'";

                var res = _database.ExecuteSql(sql);
                return res == 1 ? null : "更新失败";
            }
            catch (Exception ex)
            {
                Logger.Error("更新模型-等级异常", ex);
                return "更新模型-等级异常";
            }
        }

        public LevelModel SearchByrecordID(string recordID)
        {
            try
            {
                var sql = $"select * from {_tabName} where recordid = '{recordID}' limit 0,1";

                var datas = _database.Query<LevelModel>(sql);
                if(datas?.Count() > 0)
                {
                    datas[0].Levels = Tool.JsonToObject<List<LevelCharatModel>>(datas[0].Content);
                    datas[0].Content = null;
                    return datas[0];
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.Error("根据ID查询等级特性异常", ex);
                return null;
            }
        }
    }
}
