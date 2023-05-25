using Business.TemplateModels;
using Business.TemplateServices;
using CBP.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Utilities.Date;
using Public.Log;
using Public.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace CBP.Main.Controllers.Business.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartController : Controller
    {
        private readonly IPartService _service;

        public PartController(IPartService service)
        {
            _service = service;
        }

        [HttpPost, Route("Create")]
        public Task<ResponseModel> Create(PartModel part)
        {
            return Task.Factory.StartNew(() =>
            {
                if (part == null)
                    return new ResponseModel() { Code = ErrorCodeEnum.Error, Message = "参数为空" };

                part.RecordID = Tool.GuidTo16String();
                part.CreateTime = part.UpdateTime = DateTime.Now;
                part.Content = Tool.ObjectToJson(part.Parts);
                var res = _service.Create(part);

                return new ResponseModel() { Code = string.IsNullOrEmpty(res) ? ErrorCodeEnum.Success : ErrorCodeEnum.Error, Message = res };
            });
        }

        [HttpGet, Route("Delete")]
        public Task<ResponseModel> Delete(string partid)
        {
            return Task.Factory.StartNew(() =>
            {
                var res = _service.Delete(partid);

                return new ResponseModel() { Code = string.IsNullOrEmpty(res) ? ErrorCodeEnum.Success : ErrorCodeEnum.Error, Message = res };
            });
        }

        [HttpGet, Route("Search")]
        public Task<ResponseModel> Search(int pageIdx, int size, string partID)
        {
            return Task.Factory.StartNew(() =>
            {
                var res = _service.Search(pageIdx, size, partID);

                return new ResponseModel() { Code = ErrorCodeEnum.Success, Data = new { total = res.Item1, data = res.Item2 } };

                var charat = new PartCharatModel();
                //part.Name = "下部烟_部位区分_通用";
                //part.Parts.PartTiers = new List<PartTierModel>();
                charat.Index = new List<SortModel>() { new SortModel() { Name = "X", Index = 1 }, new SortModel() { Name = "C", Index = 2 }, new SortModel() { Name = "B", Index = 3 } };

                charat.PartTiers.Add(new PartTierModel()
                {
                    TierName = "X_1",
                    PartCharsct = new List<CharactModel>() { new CharactModel() { Name = "_L_correct", Values = new List<double>() { 0, 40 } } }
                });
                charat.PartTiers.Add(new PartTierModel()
                {
                    TierName = "X_2",
                    PartCharsct = new List<CharactModel>()
                    {
                        new CharactModel() { Name = "_crimp", Values = new List<double>() { 0, 0.65 } } ,
                        new CharactModel() { Name = "color", Values = new List<double>() { 220, 1000 } } ,
                        new CharactModel() { Name = "_L_correct", Values = new List<double>() { 0, 550 } } ,
                        new CharactModel() { Name = "_T", Values = new List<double>() { 55, 1000 } } ,
                    }
                });
                charat.PartTiers.Add(new PartTierModel()
                {
                    TierName = "C_1",
                    PartCharsct = new List<CharactModel>() { new CharactModel() { Name = "_L_correct", Values = new List<double>() { 0, 1000 } } }
                });
                charat.PartTiers.Add(new PartTierModel()
                {
                    TierName = "B_1",
                    PartCharsct = new List<CharactModel>() {
                        new CharactModel() { Name = "_L_correct", Values = new List<double>() { 0, 680 } } ,
                        new CharactModel() { Name = "_T", Values = new List<double>() { 0, 60 } } ,
                        new CharactModel() { Name = "color", Values = new List<double>() { 0, 150 } } ,
                    }
                });
                charat.PartTiers.Add(new PartTierModel()
                {
                    TierName = "B_2",
                    PartCharsct = new List<CharactModel>()
                    {
                        new CharactModel() { Name = "_L_correct", Values = new List<double>() { 0, 680 } } ,
                        new CharactModel() { Name = "_T", Values = new List<double>() { 0, 35 } }
                    }
                });
                charat.PartTiers.Add(new PartTierModel()
                {
                    TierName = "B_3",
                    PartCharsct = new List<CharactModel>()
                    {
                        new CharactModel() { Name = "_L_correct", Values = new List<double>() { 0, 680 } } ,
                        new CharactModel() { Name = "stalkrate2", Values = new List<double>() { 0.15, 10 } }
                    }
                });

                PartDbModel part = new PartDbModel() { Name = "下部_通用", Content = Tool.ObjectToJson(charat) };

                //var view = Tool.JsonToObject<PartModel>(Tool.ObjectToJson(part));


                //var d = _service.Create(part);

                return new ResponseModel() { Code = ErrorCodeEnum.Success, Data = new { total = res.Item1, data = res.Item2 } };
            });
        }

        [HttpPost, Route("Update")]
        public Task<ResponseModel> Update(PartModel part)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    for (int i = 0; i < part.Parts.PartTiers.Count; i++)
                    {
                        var f = part.Parts.PartTiers[i];
                        for (int j = 0; j < f.PartCharsct.Count; j++)
                        {
                            var v = f.PartCharsct[j];
                            if (v.Values.Count == 0 || v.Values.Sum() <= 0)
                            {
                                f.PartCharsct.Remove(v);
                                j--;
                            }
                        }
                    }
                    part.Content = Tool.ObjectToJson(part.Parts);
                    var res = _service.Update(part);

                    return new ResponseModel() { Code = string.IsNullOrEmpty(res) ? ErrorCodeEnum.Success : ErrorCodeEnum.Error, Message = res };
                }
                catch(Exception ex)
                {
                    Logger.Error("更新部位异常", ex);
                    return new ResponseModel() { Code =  ErrorCodeEnum.Exception , Message = "更新部位异常(0)" };
                }
            });
        }
    }
}
