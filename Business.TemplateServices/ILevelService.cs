using Business.TemplateModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TemplateServices
{
    public interface ILevelService
    {
        string Create(LevelDbModel level);

        string Delete(string levelID);

        (int, List<LevelModel>) Search(int pageIdx, int size, string levelID = null);

        string Update(LevelDbModel level);

        LevelModel SearchByrecordID(string recordID);
    }
}
