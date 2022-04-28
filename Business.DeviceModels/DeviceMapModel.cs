using CBP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DeviceModels
{
    public class DeviceMapModel : BaseMapModel
    {
        public string DeviceName { get; set; }

        public string DeviceID { get; set; }

        public string UnitID { get; set; }

        public string UnitName { get; set; }

        public string SiteID { get; set; }

        public string SiteName { get; set; }
    }
}
