using CBP.Models;
using System;

namespace Business.DeviceModels
{
    public class DeviceModel : BaseMapModel
    {
        public string DeviceName { get; set; }

        public string DeviceID { get; set; }

        public string UnitID { get; set; }
    }
}
