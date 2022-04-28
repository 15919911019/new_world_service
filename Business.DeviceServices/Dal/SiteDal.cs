using Business.Area;
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
    public class SiteDal
    {
        private AbsDatabaseClient _database;
        private string _tabName;
        private IAreaService _area;
        public SiteDal(AbsDatabaseClient database, string tabName, IAreaService area)
        {
            _database = database;
            _tabName = tabName;
            _area = area;
        }

        #region Site

        public bool CreateSite(string name, string countyID)
        {
            SiteModel model = new SiteModel();
            model.SiteName = name;
            model.CountyID = countyID;
            var res = _database.Insert(model, _tabName);
            return res == 1;
        }

        public bool UpdateSite(string name, string countyID, string recordID)
        {
            var sql = $"update {_tabName} set " +
                    $"SiteName = '{name}', CountyID = '{countyID}' " +
                    $"where " +
                    $"RecordID = '{recordID}'";
            var res = _database.ExecuteSql(sql);
            return res == 1;
        }

        public List<SiteMapModel> SearchSite(RequestModel req)
        {
            string proID = req.Param?.Count > 0 ? req.Param[0].ToString() : null;
            string cityID = req.Param?.Count > 1 ? req.Param[1].ToString() : null;
            string coutID = req.Param?.Count > 2 ? req.Param[2].ToString() : null;

            var sql = $"select site.*, " +
                $"county.name as countyName, county.recordID as countyID," +
                $"city.name as cityName, city.recordID as cityID, " +
                $"pro.name as provinceName, pro.recordID as provinceID " +
                $"from {_tabName} site " +
                $"left join {_area.CountyTName} county on site.countyID = county.recordID " +
                $"left join {_area.CityTName} city on county.parent = city.recordID " +
                $"left join {_area.ProvinceTName} pro on city.parent = pro.recordID " +
                $"where " +
                $"{(string.IsNullOrWhiteSpace(proID) ? "1=1" : $"pro.recordid = '{proID}'")} and " +
                $"{(string.IsNullOrWhiteSpace(cityID) ? "1=1" : $"city.recordid = '{cityID}'")} and " +
                $"{(string.IsNullOrWhiteSpace(coutID) ? "1=1" : $"county.recordid = '{coutID}'")} ";
            var res = _database.Query<SiteMapModel>(sql);
            return res;
        }

        public bool DeleteSite(string id)
        {
            var sql = $"delete from {_tabName} where RecordID = '{id}'";
            var res = _database.ExecuteSql(sql);
            return res == 1;
        }

        #endregion
    }
}
