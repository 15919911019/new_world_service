using CBP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.AreaModels
{
    public class CountyMapModel : BaseMapModel
    {
        public string Name { get; set; }

        public string Parent { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string ProvinceID { get; set; }
    }
}
