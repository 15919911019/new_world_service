using CBP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TemplateModels.Old
{
    public class TemplateParamModel : BaseModel
    {
        public string Name { get; set; }

        public TemParaType Type { get; set; }
    }

    public enum TemParaType
    {
        Header = 0,

        Feature = 1,

        Feature_Detail = 2,
    }
}
