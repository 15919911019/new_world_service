using CBP.Models;
using Newtonsoft.Json;
using Public.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TemplateModels
{
    public class PartDbModel : BaseModel
    {
        public string Name { get; set; }

        public string Content { get; set; }
    }
}
