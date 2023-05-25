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
    public class LevelModel : LevelDbModel
    {
        [JsonIgnore]
        public List<LevelCharatModel> Levels { get; set; }
    }
}
