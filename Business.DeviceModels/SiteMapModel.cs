using CBP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DeviceModels
{
    public class SiteMapModel : BaseMapModel
    {
        public string ProvinceName { get; set; }

        public string ProvinceID { get; set; }

        public string CityName { get; set; }

        public string CityID { get; set; }

        public string CountyName { get; set; }

        public string CountyID { get; set; }

        public string SiteName { get; set; }
    }
}
