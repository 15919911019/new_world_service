using AutoMapper;
using Business.Area;
using Business.AreaModels;
using Business.AreaService.Dal;
using CBP.BaseServices;
using CBP.Models;
using Public.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.AreaService
{
    public class AreaService : BaseService, IAreaService
    {
        private ProvinceDal _proDal;
        private CityDal _cityDal;
        private CountyDal _countyDal;

        public string ProvinceTName => "Area_Province";

        public string CityTName => "Area_City";

        public string CountyTName => "Area_County";

        public override string ServiceName => "Area";

        public AreaService() { }

        public AreaService(IMapper mapper)
        {
            Mapper = mapper;

            _proDal = new ProvinceDal(Database, ProvinceTName);
            _cityDal = new CityDal(Database, CityTName);
            _countyDal = new CountyDal(Database, CountyTName);
        }


        #region Province

        public Task<ResponseModel> CreateProvince(string name)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var bol = _proDal.Create(name);
                    return new ResponseModel() { Code = bol ? ErrorCodeEnum.Success : ErrorCodeEnum.Fail };
                }
                catch (Exception ex)
                {
                    Logger.Error("创建省份信息异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        public Task<ResponseModel> UpdateProvince(string name, string recordID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var bol = _proDal.Update(name, recordID);
                    return new ResponseModel() { Code = bol ? ErrorCodeEnum.Success : ErrorCodeEnum.Fail };
                }
                catch (Exception ex)
                {
                    Logger.Error("更新省份信息异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        public Task<ResponseModel> SearchProvince(string name = null)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var data = _proDal.Search();
                    return new ResponseModel() { Code = ErrorCodeEnum.Success, Data = data };
                }
                catch (Exception ex)
                {
                    Logger.Error("查询省份信息异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        public Task<ResponseModel> DeleteProvince(string recordID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var bol = _proDal.Delete(recordID);
                    return new ResponseModel() { Code = bol ? ErrorCodeEnum.Success : ErrorCodeEnum.Fail };
                }
                catch (Exception ex)
                {
                    Logger.Error("删除省份信息异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        #endregion

        #region City

        public Task<ResponseModel> CreateCity(string name, string proRecID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var bol = _cityDal.Create(name, proRecID);
                    return new ResponseModel() { Code = bol ? ErrorCodeEnum.Success : ErrorCodeEnum.Fail };
                }
                catch (Exception ex)
                {
                    Logger.Error("创建城市信息异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        public Task<ResponseModel> UpdateCity(string name, string proRecID, string recordID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var bol = _cityDal.Update(name, proRecID, recordID);
                    return new ResponseModel() { Code = bol ? ErrorCodeEnum.Success : ErrorCodeEnum.Fail };
                }
                catch (Exception ex)
                {
                    Logger.Error("更新城市信息异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        public Task<ResponseModel> SearchCity(string proRecID = null, string name = null)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var data = _cityDal.Search(proRecID);
                    return new ResponseModel() { Code = ErrorCodeEnum.Success, Data = data };
                }
                catch (Exception ex)
                {
                    Logger.Error("查询城市信息异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        public Task<ResponseModel> DeleteCity(string recordID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var bol = _cityDal.Delete(recordID);
                    return new ResponseModel() { Code = bol ? ErrorCodeEnum.Success : ErrorCodeEnum.Fail };
                }
                catch (Exception ex)
                {
                    Logger.Error("删除城市信息异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        #endregion

        #region County

        public Task<ResponseModel> CreateCounty(string name, string cityRecID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var bol = _countyDal.Create(name, cityRecID);
                    return new ResponseModel() { Code = bol ? ErrorCodeEnum.Success : ErrorCodeEnum.Fail };
                }
                catch (Exception ex)
                {
                    Logger.Error("创建县区数据异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        public Task<ResponseModel> UpdateCounty(string name, string cityRecID, string recordID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var bol = _countyDal.Update(name, cityRecID, recordID);
                    return new ResponseModel() { Code = bol ? ErrorCodeEnum.Success : ErrorCodeEnum.Fail };
                }
                catch (Exception ex)
                {
                    Logger.Error("更新县区数据异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        public Task<ResponseModel> SearchCounty(string cityRecID = null, string name = null)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var data = _countyDal.Search(cityRecID);
                    return new ResponseModel() { Code = ErrorCodeEnum.Success, Data = data };
                }
                catch (Exception ex)
                {
                    Logger.Error("查询县区数据异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }

        public Task<ResponseModel> DeleteCounty(string recordID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var bol = _countyDal.Delete(recordID);
                    return new ResponseModel() { Code = bol ? ErrorCodeEnum.Success : ErrorCodeEnum.Fail };
                }
                catch (Exception ex)
                {
                    Logger.Error("删除县区数据异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }


        #endregion

        public Task<ResponseModel> SearchArea()
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    List<AreaModel> result = new List<AreaModel>();

                    var temp = _countyDal.Search();
                    foreach(var item in temp)
                    {
                        if (string.IsNullOrWhiteSpace(item.Province) == true)
                            continue;

                        if (result.Exists(p => p.Label == item.Province) == false)
                        {
                            result.Add(new AreaModel()
                            {
                                Label = item.Province,
                                Value = item.ProvinceID,
                                Children = new List<AreaModel>()
                            });
                        }

                        if (string.IsNullOrWhiteSpace(item.City) == true)
                            continue;

                        var pro = result.Find(q => q.Value == item.ProvinceID);
                        if (pro.Children.Exists(q => q.Value == item.Parent) == false)
                            pro.Children.Add(new AreaModel()
                            {
                                Label = item.City,
                                Value = item.Parent,
                                Children = new List<AreaModel>()
                            });

                        var city = result.Find(q => q.Value == item.ProvinceID)?.Children?.Find(q => q.Value == item.Parent);
                        if (city?.Children?.Exists(q => q.Value == item.RecordID) == false)
                            city.Children.Add(new AreaModel() 
                            { 
                                Label = item.Name,
                                Value = item.RecordID,
                            });
                    };
                    return new ResponseModel() { Code = ErrorCodeEnum.Success, Data = result };
                }
                catch (Exception ex)
                {
                    Logger.Error("查询地址信息异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception };
                }
            });
        }
    }
}
