using AutoMapper;
using Business.Area;
using Business.DeviceModels;
using Business.DeviceServices.Dal;
using CBP.BaseServices;
using CBP.Models;
using Public.Log;
using Public.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DeviceServices
{
    public class DeviceService : BaseService, IDeviceService
    {
        public string DeviceTName => "Dev_Device";

        public string UnitTName => "Dev_Unit";

        public string SiteTName => "Dev_Site";

        public override string ServiceName => "Device";

        private DeviceDal _deviceDal;
        private UnitDal _unitDal;
        private SiteDal _siteDal;

        private IAreaService _areaService;

        public DeviceService() { }

        public DeviceService(IMapper mapper, IAreaService areaService)
        {
            Mapper = mapper;
            _areaService = areaService;

            _deviceDal = new DeviceDal(Database, DeviceTName);
            _unitDal = new UnitDal(Database, UnitTName);
            _siteDal = new SiteDal(Database, SiteTName, _areaService);
        }

        #region Device

        public Task<ResponseModel> CreateDevice(string name, string deviceID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var res = _deviceDal.CreateDevice(name, deviceID);
                    return new ResponseModel()
                    {
                        Code = res ? ErrorCodeEnum.Success : ErrorCodeEnum.Fail
                    };
                }
                catch (Exception ex)
                {
                    Logger.Error("创建站点信息异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        public Task<ResponseModel> UpdateDevice(string name, string unitID, string recordID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var count = _deviceDal.UpdateDevice(name,unitID, recordID);
                    return new ResponseModel() 
                    { 
                        Code = count ? ErrorCodeEnum.Success : ErrorCodeEnum.Fail
                    };
                }
                catch(Exception ex)
                {
                    Logger.Error("更新设备信息异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception};
                }
            });
        }

        public Task<ResponseModel> SetDeviceUnit(string devID, string unitID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var count = _deviceDal.SetDeviceUnit(devID, unitID);
                    return new ResponseModel()
                    {
                        Code = count ? ErrorCodeEnum.Success : ErrorCodeEnum.Fail
                    };
                }
                catch (Exception ex)
                {
                    Logger.Error("设置设备-单元异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        public Task<ResponseModel> SearchDevice(RequestModel request)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var data = _deviceDal.SearchDevice(request);
                    return new ResponseModel()
                    {
                        Code = ErrorCodeEnum.Success,
                        Data = data,
                    };
                }
                catch (Exception ex)
                {
                    Logger.Error("查询设备异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception, Message = ex.Message };
                }
            });
        }

        public Task<ResponseModel> DeleteDevice(string id)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var count = _deviceDal.DeleteDevice(id);
                    return new ResponseModel()
                    {
                        Code = count ? ErrorCodeEnum.Success : ErrorCodeEnum.Fail
                    };
                }
                catch (Exception ex)
                {
                    Logger.Error("删除设备异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }


        #endregion


        #region Unit


        public Task<ResponseModel> CreateUnit(string name, string siteID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var count = _unitDal.CreateUnit(name, siteID);
                    return new ResponseModel() { Code = count ? ErrorCodeEnum.Success : ErrorCodeEnum.Fail };
                }
                catch (Exception ex)
                {
                    Logger.Error("创建单元异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        public Task<ResponseModel> UpdateUnit(string name, string siteID, string id)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var count = _unitDal.UpdateUnit(name,siteID, id);
                    return new ResponseModel() { Code = count ? ErrorCodeEnum.Success : ErrorCodeEnum.Fail };
                }
                catch (Exception ex)
                {
                    Logger.Error("更新单元异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        public Task<ResponseModel> SearchUnit(RequestModel req)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var data = _unitDal.SearchUnit(req);
                    return new ResponseModel() 
                    { 
                        Code = data != null ?  ErrorCodeEnum.Success : ErrorCodeEnum.Fail,
                        Data = data
                    };
                }
                catch (Exception ex)
                {
                    Logger.Error("查询单元异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        public Task<ResponseModel> DeleteUnit(string id)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var count = _unitDal.DeleteUnit(id);
                    return new ResponseModel() { Code = count ? ErrorCodeEnum.Success: ErrorCodeEnum.Fail };
                }
                catch (Exception ex)
                {
                    Logger.Error("删除单元异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        #endregion


        #region Site

        public Task<ResponseModel> CreateSite(string name, string countyID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var res = _siteDal.CreateSite(name, countyID);
                    return new ResponseModel()
                    {
                        Code = res ? ErrorCodeEnum.Success : ErrorCodeEnum.Fail
                    };
                }
                catch(Exception ex)
                {
                    Logger.Error("创建站点信息异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception};
                }
            });
        }

        public Task<ResponseModel> UpdateSite(string name, string countyID, string recordID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var res = _siteDal.UpdateSite(name, countyID, recordID);
                    return new ResponseModel()
                    {
                        Code = res ? ErrorCodeEnum.Success : ErrorCodeEnum.Fail
                    };
                }
                catch (Exception ex)
                {
                    Logger.Error("更新站点信息异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        public Task<ResponseModel> SearchSite(RequestModel req)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var res = _siteDal.SearchSite(req); 
                    return new ResponseModel()
                    {
                        Code = ErrorCodeEnum.Success,
                        Data = res,
                    };
                }
                catch (Exception ex)
                {
                    Logger.Error("查询站点信息异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        public Task<ResponseModel> DeleteSite(string id)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var res = _siteDal.DeleteSite(id);
                    return new ResponseModel()
                    {
                        Code = res ? ErrorCodeEnum.Success : ErrorCodeEnum.Fail,
                    };
                }
                catch (Exception ex)
                {
                    Logger.Error("删除站点信息异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        #endregion
    }
}
