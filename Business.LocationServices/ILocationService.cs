using CBP.Models;
using System;
using System.Threading.Tasks;

namespace Business.LocationServices
{
    public interface ILocationService
    {
        Task<ResponseModel> GetCityCode(double longitude, double latitude);
    }
}
