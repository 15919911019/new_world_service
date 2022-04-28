using Business.AreaModels;
using CBP.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Area
{
    public interface IAreaService
    {
        string ProvinceTName { get; }

        string CityTName { get; }

        string CountyTName { get; }


        #region Province

        Task<ResponseModel> CreateProvince(string name);

        Task<ResponseModel> UpdateProvince(string name, string recordID);

        Task<ResponseModel> SearchProvince(string name = null);

        Task<ResponseModel> DeleteProvince(string recordID);

        #endregion


        #region City

        Task<ResponseModel> CreateCity(string name, string proRecID);

        Task<ResponseModel> UpdateCity(string name, string proRecID, string recordID);

        Task<ResponseModel> SearchCity(string proRecID = null,string name = null);

        Task<ResponseModel> DeleteCity(string recordID);

        #endregion


        #region County

        Task<ResponseModel> CreateCounty(string name, string cityRecID);

        Task<ResponseModel> UpdateCounty(string name, string cityRecID, string recordID);

        Task<ResponseModel> SearchCounty(string cityRecID = null, string name = null);

        Task<ResponseModel> DeleteCounty(string recordID);

        #endregion


        Task<ResponseModel> SearchArea();
    }
}
