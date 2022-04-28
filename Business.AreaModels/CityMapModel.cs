using CBP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.AreaModels
{
    public class CityMapModel : BaseMapModel
    {
        public string Name { get; set; }

        public string Parent { get; set; }

        public string Province { get; set; }
    }
}
