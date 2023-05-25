using AutoMapper;
using Business.TemplateModels;
using Business.TemplateServices.Dal;
using CBP.BaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TemplateServices
{
    public class LevelService : BaseService, ILevelService
    {
        private string ModelTabName => "model_level";

        public override string ServiceName => "Template_Level";

        private LevelDal _dal;

        public LevelService()
        {
        }

        public LevelService(IMapper mapper)
        {
            Mapper = mapper;
            _dal = new LevelDal(Database, ModelTabName);
        }

        public string Create(LevelDbModel level)
        {
            return _dal.Create(level);
        }

        public string Delete(string levelID)
        {
            throw new NotImplementedException();
        }

        public (int, List<LevelModel>) Search(int pageIdx, int size, string levelID = null)
        {
            return _dal.Search(pageIdx, size, levelID);
        }

        public string Update(LevelDbModel level)
        {
            return _dal.Update(level);
        }

        public LevelModel SearchByrecordID(string recordID)
        {
            return _dal.SearchByrecordID(recordID);
        }
    }
}
