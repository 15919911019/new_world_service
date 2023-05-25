using Business.TemplateModels;
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
    public class PartDal
    {
        private AbsDatabaseClient _database;

        private string _tabName;

        public PartDal(AbsDatabaseClient database, string tabName)
        {
            _database = database;
            _tabName = tabName;
        }

        public string Create(PartDbModel part)
        {
            try
            {
                var res = _database.Insert(part, _tabName);
                if (res != 1)
                    return "创建部位数据失败";

                return null;
            }
            catch (Exception ex)
            {
                Logger.Error("创建部位数据异常", ex);
                return "创建部位数据异常" + ex.Message;
            }
        }

        public (int, List<PartModel>) Search(int pageIdx, int size, string partID = null)
        {
            try
            {
                var where = string.IsNullOrWhiteSpace(partID) == true ? " 1=1 " : $" RecordID = '{partID}'";

                var sqlCount = $"select count(0) from {_tabName} where {where}";
                var sqlData = $"select * from {_tabName} where {where} limit {pageIdx * size}, {size}";

                var data = _database.Query<PartModel>(sqlCount, sqlData);
                data.Item2?.ForEach(q => { q.Parts = Tool.JsonToObject<PartCharatModel>(q.Content); q.Content = null; });

                return data;
            }
            catch (Exception ex)
            {
                Logger.Error("查询部位数据异常", ex);
                return (0, null);
            }
        }

        public string Update(PartDbModel part)
        {
            try
            {
                var sql = $"update {_tabName} set name = '{part.Name}', content = '{part.Content}' " +
                    $"where recordid = '{part.RecordID}'";

                var con = _database.ExecuteSql(sql);
                return con == 1 ? null : "更新失败";
            }
            catch (Exception ex)
            {
                Logger.Error("更新部位信息异常", ex);
                return ex.Message;
            }
        }

        public PartModel SearchByrecordID(string recordID)
        {
            try
            {
                var sql = $"select * from {_tabName} where recordid = '{recordID}' limit 0,1";
                var datas = _database.Query<PartModel>(sql);
                if(datas?.Count > 0)
                {
                    datas[0].Parts = Tool.JsonToObject<PartCharatModel>(datas[0].Content);
                    datas[0].Content = null;
                    return datas[0];
                }

                return null;
            }
            catch(Exception ex)
            {
                Logger.Error("通过ID获取部位信息异常", ex);
                return null;
            }
        }
    }
}
