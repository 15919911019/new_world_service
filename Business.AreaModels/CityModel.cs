using CBP.Models;
using System;

namespace Business.AreaModels
{
    public class CityModel : BaseModel
    {
        public string Name { get; set; }

        public string Parent { get; set; }
    }
}
