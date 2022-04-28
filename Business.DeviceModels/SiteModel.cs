using CBP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DeviceModels
{
    public class SiteModel : BaseMapModel
    {
        public string SiteName { get; set; }

        public string CountyID { get; set; }
    }
}
