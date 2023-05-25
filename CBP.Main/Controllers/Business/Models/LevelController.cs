using Business.TemplateModels;
using Business.TemplateServices;
using CBP.Models;
using Microsoft.AspNetCore.Mvc;
using Public.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBP.Main.Controllers.Business.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelController : Controller
    {
        private readonly ILevelService _service;

        public LevelController(ILevelService service)
        {
            _service = service;
        }

        [HttpPost, Route("Create")]
        public Task<ResponseModel> Create(LevelModel level)
        {
            return Task.Factory.StartNew(() =>
            {
                if (level == null)
                    return new ResponseModel() { Code = ErrorCodeEnum.Error, Message = "参数为空" };


                level.RecordID = Tool.GuidTo16String();
                level.CreateTime = level.UpdateTime = DateTime.Now;
                level.Content = Tool.ObjectToJson(level.Levels);
                var res = _service.Create(level);
                return new ResponseModel() { Code = string.IsNullOrEmpty(res) ? ErrorCodeEnum.Success : ErrorCodeEnum.Error, Message = res };
            });
        }


        [HttpGet, Route("search")]
        public Task<ResponseModel> Search(int pageIdx, int size, string levID)
        {
            return Task.Factory.StartNew(() =>
            {
                #region
                //LevelModel model = new LevelModel();
                //model.Name = "楚雄--下部等级通用";
                //model.Levels = new List<LevelCharatModel>();
                //model.Levels.Add(new LevelCharatModel()
                //{
                //    LitLevCode = "CGY1",
                //    LevCharat = new List<CharactModel>()
                //    {
                //        new CharactModel(){Name = "4V_area", Values = new List<double>(){500, 10000} }
                //    },
                //    Index = 1,
                //    GBLevCode = "GY1",
                //    Type = "ZK",
                //    Part = "C",
                //});
                //model.Levels.Add(new LevelCharatModel()
                //{
                //    LitLevCode = "CGY2",
                //    LevCharat = new List<CharactModel>()
                //    {
                //        new CharactModel(){Name = "1HB_correct", Values = new List<double>(){0.1, 1} },
                //        new CharactModel(){Name = "4V_area", Values = new List<double>(){350, 10000} },
                //    },
                //    Index = 2,
                //    GBLevCode = "GY1",
                //    Type = "ZK",
                //    Part = "C",
                //});
                //model.Levels.Add(new LevelCharatModel()
                //{
                //    LitLevCode = "CK1",
                //    LevCharat = new List<CharactModel>()
                //    {
                //        new CharactModel(){Name = "1HB_correct", Values = new List<double>(){0.17, 1} },
                //        new CharactModel(){Name = "color", Values = new List<double>(){0, 210} },
                //    },
                //    Index = 3,
                //    GBLevCode = "CX1K",
                //    Type = "ZK",
                //    Part = "C",
                //});
                //model.Levels.Add(new LevelCharatModel()
                //{
                //    LitLevCode = "CK2",
                //    LevCharat = new List<CharactModel>()
                //    {
                //        new CharactModel(){Name = "1HB_correct", Values = new List<double>(){0.21, 1} },
                //        new CharactModel(){Name = "color", Values = new List<double>(){0, 210} },
                //    },
                //    Index = 4,
                //    GBLevCode = "CX1K",
                //    Type = "ZK",
                //    Part = "C",
                //});
                //model.Levels.Add(new LevelCharatModel()
                //{
                //    LitLevCode = "CK3",
                //    LevCharat = new List<CharactModel>()
                //    {
                //        new CharactModel(){Name = "1HB_correct", Values = new List<double>(){0.15, 1} },
                //        new CharactModel(){Name = "color", Values = new List<double>(){0, 210} },
                //    },
                //    Index = 5,
                //    GBLevCode = "CX1K",
                //    Type = "ZK",
                //    Part = "C",
                //});
                //model.Levels.Add(new LevelCharatModel()
                //{
                //    LitLevCode = "CV1",
                //    LevCharat = new List<CharactModel>()
                //    {
                //        new CharactModel(){Name = "1HB_correct", Values = new List<double>(){0, 0.05} },
                //        new CharactModel(){Name = "2GH_correct", Values = new List<double>(){0, 0.2} },
                //        new CharactModel(){Name = "_L_correct", Values = new List<double>(){500, 1000} },
                //        new CharactModel(){Name = "_T", Values = new List<double>(){0, 75} },
                //        new CharactModel(){Name = "4V_area", Values = new List<double>(){1000, 10000} },
                //    },
                //    Index = 6,
                //    GBLevCode = "C4F",
                //    Type = "ZK",
                //    Part = "C",
                //});
                //model.Levels.Add(new LevelCharatModel()
                //{
                //    LitLevCode = "C1H",
                //    LevCharat = new List<CharactModel>()
                //    {
                //        new CharactModel(){Name = "1HB_correct", Values = new List<double>(){0, 0.01} },
                //        new CharactModel(){Name = "2GH_correct", Values = new List<double>(){0.11, 0.21} },
                //        new CharactModel(){Name = "_L_correct", Values = new List<double>(){680, 1000} },
                //        new CharactModel(){Name = "_T", Values = new List<double>(){30, 79} },
                //        new CharactModel(){Name = "4V_area", Values = new List<double>(){1150, 10000} },
                //    },
                //    Index = 7,
                //    GBLevCode = "C1F",
                //    Type = "ZC",
                //    Part = "C",
                //});
                //model.Levels.Add(new LevelCharatModel()
                //{
                //    LitLevCode = "C1F",
                //    LevCharat = new List<CharactModel>()
                //    {
                //        new CharactModel(){Name = "1HB_correct", Values = new List<double>(){0, 0.01} },
                //        new CharactModel(){Name = "2GH_correct", Values = new List<double>(){0.1, 0.2} },
                //        new CharactModel(){Name = "_L_correct", Values = new List<double>(){700, 800} },
                //        new CharactModel(){Name = "_T", Values = new List<double>(){30, 75} },
                //        new CharactModel(){Name = "4V_area", Values = new List<double>(){1200, 10000} },
                //    },
                //    Index = 8,
                //    GBLevCode = "C1F",
                //    Type = "ZC",
                //    Part = "C",
                //});
                //model.Levels.Add(new LevelCharatModel()
                //{
                //    LitLevCode = "C1L",
                //    LevCharat = new List<CharactModel>()
                //    {
                //        new CharactModel(){Name = "1HB_correct", Values = new List<double>(){0, 0.03} },
                //        new CharactModel(){Name = "2GH_correct", Values = new List<double>(){0.1, 0.3} },
                //        new CharactModel(){Name = "_L_correct", Values = new List<double>(){600, 900} },
                //        new CharactModel(){Name = "_T", Values = new List<double>(){20, 75} },
                //        new CharactModel(){Name = "4V_area", Values = new List<double>(){1100, 11000} },
                //    },
                //    Index = 9,
                //    GBLevCode = "C2F",
                //    Type = "ZC",
                //    Part = "C",
                //});

                //LevelDbModel db = new LevelDbModel();
                //db.Name = model.Name;
                //db.Content = Tool.ObjectToJson(model.Levels);

                //_service.Create(db);
                #endregion

                var data = _service.Search(pageIdx, size, levID);
                data.Item2?.ForEach(q => q.Content = null);
                return new ResponseModel() { Code = ErrorCodeEnum.Success , Total = data.Item1, Data = data.Item2};
            });
        }


        [HttpPost, Route("update")]
        public Task<ResponseModel> Update(LevelModel level)
        {
            return Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < level.Levels.Count; i++)
                {
                    var f = level.Levels[i];
                    for (int j = 0; j < f.LevCharat.Count; j++)
                    {
                        var v = f.LevCharat[j];
                        if (v.Values.Count == 0 || v.Values.Sum() <= 0)
                        {
                            f.LevCharat.Remove(v);
                            j--;
                        }
                    }
                }
                level.Content = Tool.ObjectToJson(level.Levels);
                var res = _service.Update(level);

                return new ResponseModel() { Code = string.IsNullOrEmpty(res) ? ErrorCodeEnum.Success : ErrorCodeEnum.Error, Message = res };
            });
        }

    }
}
