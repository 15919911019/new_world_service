using Business.TemplateModels;
using System;
using System.Collections.Generic;

namespace Business.TemplateServices
{
    public interface ITemplateService
    {

        string Create(TemplateModel template);

        string Delete(string templateID);

        (int, List<TemplateModel>) Search(int pageIdx, int size, string areaID, string stationID);

        string Update(TemplateModel template);

        TemplateMapModel SearchByStationID(string stationID);
    }
}
