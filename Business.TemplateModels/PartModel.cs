using Newtonsoft.Json;
using Public.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Business.TemplateModels
{
    public class PartModel : PartDbModel
    {
        [JsonIgnore]
        public PartCharatModel Parts { get; set; }
    }
}
