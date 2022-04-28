using Business.DeviceModels;
using CBP.Models;
using Public.DbHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DeviceServices.Dal
{
    public class UnitDal
    {
        private AbsDatabaseClient _database;

        private string _tabName;

        public UnitDal(AbsDatabaseClient database, string tabName)
        {
            _database = database;
            _tabName = tabName;
        }

        #region Unit


        public bool CreateUnit(string name, string siteID)
        {
            UnitModel unit = new UnitModel();
            unit.UnitName = name;
            unit.SiteID = siteID;
            var count = _database.Insert(unit, _tabName);
            return count == 1;
        }

        public bool UpdateUnit(string name, string siteID, string id)
        {
            var sql = $"update {_tabName} set " +
                   $"UnitName = '{name}', SiteID = '{siteID}' " +
                   $"where" +
                   $"RecordID = '{id}'";
            var count = _database.ExecuteSql(sql);
            return count == 1;
        }

        public List<UnitMapModel> SearchUnit(RequestModel req)
        {
            var siteID = req.Param?.Count > 0 ? req.Param[0].ToString() : null;
            var sql = $"select unit.*, site.sitename from {_tabName} unit " +
                $"left join dev_site site on unit.siteid = site.recordid " +
                $"where " +
                $"{(string.IsNullOrWhiteSpace(siteID) ? "1=1" : $"unit.siteid = '{siteID}'" )}";
            var data = _database.Query<UnitMapModel>(sql);
            return data;
        }

        public bool DeleteUnit(string id)
        {
            var sql = $"delete  from {_tabName} where RecordID = '{id}'";
            var count = _database.ExecuteSql(sql);
            return count == 1;
        }

        #endregion
    }
}
