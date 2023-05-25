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
    public class TemplateDal
    {
        private AbsDatabaseClient _database;

        private string _tabName;

        public TemplateDal(AbsDatabaseClient database, string tabName)
        {
            _database = database;
            _tabName = tabName;
        }

        public string Create(TemplateModel template)
        {
            try
            {
                var res = _database.Insert(template, _tabName);
                return res == 1 ? null : "新增失败";
            }
            catch(Exception ex)
            {
                Logger.Error("新增模型异常", ex);
                return "新增模型异常:"+ ex.Message;
            }
        }

        public string Delete(string templateID)
        {
            try
            {
                var sql = $"update {_tabName} set isenable = 0 where recordid = '{templateID}'"; 
                var res = _database.ExecuteSql(sql);

                return res == 1 ? null : "删除失败";
            }
            catch (Exception ex)
            {
                Logger.Error("删除模型异常", ex);
                return "新增模型异常:" + ex.Message;
            }
        }

        public(int, List<TemplateModel>) Search(int pageIdx, int size, string areaID, string stationID)
        {
            try
            {
                var where = string.IsNullOrWhiteSpace(areaID) ? " 1=1 " : $" areaid = '{areaID}' ";
                where += string.IsNullOrWhiteSpace(stationID) ? "" : $" and stationID = '{stationID}'";

                var con = $"select count(0) from {_tabName} where {where}";
                var sql = $"select * from {_tabName} where {where} limit {(pageIdx - 1)* size}, {size}";

                var datas = _database.Query<TemplateModel>(con, sql);

                return datas;
            }
            catch (Exception ex)
            {
                Logger.Error("查询模型异常", ex);
                return (-1, null);
            }
        }

        public TemplateModel SearchByStationID(string stationID)
        {
            try
            {
                var sql = $"select * from {_tabName} where stationid = '{stationID}'";
                var data = _database.Query<TemplateModel>(sql);

                return data?.Count > 0 ? data[0] : null;
            }
            catch(Exception ex)
            {
                Logger.Error("根据站点ID获取模型信息异常", ex);
                return null;
            }
        }

        public string Update(TemplateModel template)
        {
            try
            {
                var sql = $"update {_tabName} set " +
                    $"partname = '{template.PartName}', " +
                    $"partrcdid = '{template.PartRcdID}', " +
                    $"levelname = '{template.LevelName}'," +
                    $"levelrcdid = '{template.LevelRcdID}' " +
                    $"where " +
                    $"recordid = '{template.RecordID}'";

                var res = _database.ExecuteSql(sql);
                return res == 1 ? null : "更新失败";
            }
            catch (Exception ex)
            {
                Logger.Error("更新模型异常", ex);
                return "更新模型异常:" + ex.Message;
            }
        }
    }
}
