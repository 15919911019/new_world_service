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
    public class PartService : BaseService, IPartService
    {
        private string ModelTabName => "model_part";

        public override string ServiceName => "Template_Part";

        private PartDal _dal;
        private string tab_Name = "model_part";

        public PartService()
        {
        }

        public PartService(IMapper mapper)
        {
            Mapper = mapper;
            _dal = new PartDal(Database, tab_Name);
        }


        public string Create(PartDbModel part)
        {
            return _dal.Create(part);
        }

        public string Delete(string partID)
        {
            throw new NotImplementedException();
        }

        public (int, List<PartModel>) Search(int pageIdx, int size, string partID = null)
        {
            try
            {
                return _dal.Search(pageIdx, size, partID);
            }
            catch(Exception ex)
            {
                Logger.Error("查询部位数据异常", ex);
                return (0, null);
            }
        }

        public string Update(PartDbModel part)
        {
            return _dal.Update(part);
        }

        public PartModel SearchByrecordID(string recordID)
        {
            return _dal.SearchByrecordID(recordID);
        }
    }
}
