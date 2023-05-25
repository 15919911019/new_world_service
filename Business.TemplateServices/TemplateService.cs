using AutoMapper;
using Business.TemplateModels;
using Business.TemplateServices.Dal;
using CBP.BaseServices;
using Public.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TemplateServices
{
    public class TemplateService : BaseService , ITemplateService
    {
        private TemplateDal _tmpDal;
        private PartDal _partDal;
        private LevelDal _levelDal;

        private string ModelTabName => "Template";

        public override string ServiceName => "Template";

        public ITemplateService _tempService;

        public TemplateService() 
        {
        }

        public TemplateService(IMapper mapper)
        {
            Mapper = mapper;
            _tmpDal = new TemplateDal(Database, "model_template");
            _partDal = new PartDal(Database, "model_part");
            _levelDal = new LevelDal(Database, "model_level");
        }

        public string Create(TemplateModel template)
        {
            return _tmpDal.Create(template);
        }

        public string Delete(string templateID)
        {
            return _tmpDal.Delete(templateID);
        }

        public (int, List<TemplateModel>) Search(int pageIdx, int size, string areaID, string stationID)
        {
            return _tmpDal.Search(pageIdx, size, areaID, stationID);
        }

        public string Update(TemplateModel template)
        {
            return _tmpDal.Update(template);
        }

        public TemplateMapModel SearchByStationID(string stationID)
        {
            try
            {
                var tmp = _tmpDal.SearchByStationID(stationID);
                if (tmp == null)
                    return null;

                var part = _partDal.SearchByrecordID(tmp.PartRcdID);
                var level = _levelDal.SearchByrecordID(tmp.LevelRcdID);

                return new TemplateMapModel() { Name = tmp.Name, Part = part, Level = level };
            }
            catch(Exception ex)
            {
                Logger.Error("获取站点模型异常", ex);
                return null;
            }
        }
    }
}
