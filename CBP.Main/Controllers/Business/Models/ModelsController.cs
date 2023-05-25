using Business.TemplateModels;
using Business.TemplateServices;
using CBP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Public.Log;
using Public.Tools;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CBP.Main.Controllers.Business.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : Controller
    {
        private readonly ITemplateService _service;

        public ModelsController(ITemplateService service)
        {
            _service = service;
        }

        [HttpPost, Route("Create")]
        public Task<ResponseModel> Create(TemplateModel model)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var res = _service.Create(model);

                    return new ResponseModel() { Code = string.IsNullOrEmpty(res) ? ErrorCodeEnum.Success : ErrorCodeEnum.Error, Data = model, Message = res };
                }
                catch (Exception ex)
                {
                    Logger.Error("创建模型异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception, Message = "更新部位异常(0)" };
                }
            });
        }

        [HttpGet, Route("Delete")]
        public Task<ResponseModel> Delete(string recordID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var res = _service.Delete(recordID);

                    return new ResponseModel() { Code = string.IsNullOrEmpty(res) ? ErrorCodeEnum.Success : ErrorCodeEnum.Error, Message = res };
                }
                catch (Exception ex)
                {
                    Logger.Error("创建模型异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception, Message = "更新部位异常(0)" };
                }
            });
        }

        [HttpGet, Route("Search")]
        public Task<ResponseModel> Search(int pageIdx, int size, string areaID, string stationID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var res = _service.Search(pageIdx, size, areaID, stationID);

                    return new ResponseModel() { Code = res.Item1 >= 0 ? ErrorCodeEnum.Success : ErrorCodeEnum.Error, Data = res.Item2 };
                }
                catch (Exception ex)
                {
                    Logger.Error("查询模型异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception, Message = "查询模型异常(0)" };
                }
            });
        }

        [HttpPost, Route("Update")]
        public Task<ResponseModel> Update(TemplateModel model)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var res = _service.Update(model);

                    return new ResponseModel() { Code = string.IsNullOrEmpty(res) ? ErrorCodeEnum.Success : ErrorCodeEnum.Error, Message = res };
                }
                catch (Exception ex)
                {
                    Logger.Error("更新模型异常", ex);
                    return new ResponseModel() { Code = ErrorCodeEnum.Exception, Message = "更新模型异常(0)" };
                }
            });
        }

        /// <summary>
        /// 获取该站点配置的模型信息
        /// </summary>
        /// <param name="stationID"></param>
        /// <returns></returns>
        [HttpGet, Route("SearchByStationID")]
        public Task<TemplateMapModel> SearchByStationID(string stationID)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var res = _service.SearchByStationID(stationID);

                    return res;
                }
                catch (Exception ex)
                {
                    Logger.Error("更新模型异常", ex);
                    return null;
                }
            });
        } 

    }
}
