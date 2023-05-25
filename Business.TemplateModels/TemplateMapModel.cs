using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TemplateModels
{
    public class TemplateMapModel
    {
        /// <summary>
        /// 模型明细
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 地区ID
        /// </summary>
        public string AreaID { get; set; }

        public string StationID { get; set; }

        public PartModel Part { get; set; }

        public LevelModel Level { get; set; }
    }
}
