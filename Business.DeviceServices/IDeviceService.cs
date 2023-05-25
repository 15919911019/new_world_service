using CBP.Models;
using System;
using System.Threading.Tasks;

namespace Business.DeviceServices
{
    public interface IDeviceService
    {
        public string DeviceTName { get; }
        public string UnitTName { get; }
        public string SiteTName { get; }

        #region Device

        Task<ResponseModel> CreateDevice(string name, string deviceID);

        Task<ResponseModel> UpdateDevice(string name, string unitID, string recordID);

        Task<ResponseModel> SetDeviceUnit(string devID, string unitID);

        Task<ResponseModel> SearchDevice(RequestModel request);

        Task<ResponseModel> DeleteDevice(string id);


        #endregion


        #region Unit

        Task<ResponseModel> CreateUnit(string name, string siteID);

        Task<ResponseModel> UpdateUnit(string name, string siteID, string id);

        Task<ResponseModel> SearchUnit(RequestModel req);

        Task<ResponseModel> DeleteUnit(string id);

        #endregion


        #region Site

        Task<ResponseModel> CreateSite(string name, string siteID);

        Task<ResponseModel> UpdateSite(string name, string countyID, string recordID);

        Task<ResponseModel> SearchSite(RequestModel req);

        Task<ResponseModel> DeleteSite(string id);

        #endregion
    }
}
