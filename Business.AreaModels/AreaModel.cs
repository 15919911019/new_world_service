using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.AreaModels
{
    public class AreaModel
    {
        public string Value {get;set;}

        public string Label { get;set;}

        public List<AreaModel> Children { get;set; }
    }
}
