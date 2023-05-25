using Business.TemplateModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TemplateServices
{
    public interface IPartService
    {
        string Create(PartDbModel part);

        string Delete(string partID);

        (int, List<PartModel>) Search(int pageIdx, int size, string partID = null);

        string Update(PartDbModel part);

        PartModel SearchByrecordID(string recordID);
    }
}
