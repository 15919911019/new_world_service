using CBP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TemplateModels.Old
{
    public class TemplateValueModel : BaseModel
    {
        public string TemplateID { get; set; }

        public string ParentID { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public int Type { get; set; }
    }
}
