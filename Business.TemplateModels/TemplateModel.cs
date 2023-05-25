using CBP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TemplateModels
{
    public class TemplateModel : BaseModel
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

        public string PartName { get; set; }

        /// <summary>
        /// 部位特性关联
        /// </summary>
        public string PartRcdID { get; set; }

        public string LevelName { get; set; }

        /// <summary>
        /// 等级特性关联
        /// </summary>
        public string LevelRcdID { get; set; }

    }
}
