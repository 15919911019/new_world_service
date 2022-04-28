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
    public class DeviceDal
    {
        private AbsDatabaseClient _database;

        private string _tabName;

        public DeviceDal(AbsDatabaseClient database, string tabName)
        {
            _database = database;
            _tabName = tabName;
        }

        #region Device

        public bool CreateDevice(string name, string deviceID)
        {
            DeviceModel model = new DeviceModel();
            model.DeviceID = deviceID;
            model.DeviceName = name;
            var res = _database.Insert(model, _tabName);
            return res == 1;
        }

        public bool UpdateDevice(string name, string unitID, string recordID)
        {
            var sql = $"update {_tabName} set " +
                    $"DeviceName = '{name}', UnitID = '{unitID}' " +
                    $"where " +
                    $"RecordID = '{recordID}'";
            var count = _database.ExecuteSql(sql);
            return count == 1;
        }

        public bool SetDeviceUnit(string devID, string unitID)
        {
            var sql = $"update {_tabName} set " +
                    $"UnitRecordID = {unitID} where" +
                    $"DeviceID = '{devID}'";
            var count = _database.ExecuteSql(sql);
            return count == 1;
        }

        public List<DeviceMapModel> SearchDevice(RequestModel req)
        {
            string countyID = req.Param?.Count > 0 ? req.Param[0].ToString() : null;
            string siteID = req.Param?.Count > 1 ? req.Param[1].ToString() : null;

            var sql = $"select dev.*, " +
                $"unit.unitname, unit.recordid as unitid, " +
                $"site.sitename, site.recordid as siteid " +
                $"from {_tabName} dev " +
                $"left join dev_unit unit on dev.unitid = unit.recordid " +
                $"left join dev_site site on unit.siteid = site.recordid " +
                $"where " +
                $"{(string.IsNullOrWhiteSpace(countyID) ? "1=1" : $"site.countyid = '{countyID}'")} and " +
                $"{(string.IsNullOrWhiteSpace(siteID) ? "1=1" : $"site.recordID = '{siteID}'")} ";
            var data = _database.Query<DeviceMapModel>(sql);
            return data;
        }

        public bool DeleteDevice(string id)
        {
            var sql = $"delete from {_tabName} where" +
                    $"RecordID = '{id}'";
            var count = _database.ExecuteSql(sql);
            return count == 1;
        }


        #endregion
    }
}
