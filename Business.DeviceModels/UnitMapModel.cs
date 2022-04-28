using CBP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DeviceModels
{
    public class UnitMapModel : BaseMapModel
    {
        public string UnitName { get; set; }

        public string SiteID { get; set; }

        public string SiteName { get; set; }
    }
}
