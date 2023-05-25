using CBP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TemplateModels
{
    public class LevelDbModel : BaseModel
    {
        public string Name { get; set; }

        public string Content { get; set; }
    }
}
