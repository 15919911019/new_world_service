using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TemplateModels
{
    public class LevelCharatModel
    {
        /// <summary>
        /// 小等级代码
        /// </summary>
        public string LitLevCode { get; set; }

        /// <summary>
        /// 小等级所属类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 判定顺序
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 所属部位
        /// </summary>
        public string Part { get; set; }

        /// <summary>
        /// 国标等级代码
        /// </summary>
        public string GBLevCode { get; set; }

        /// <summary>
        /// 等级特征集合
        /// </summary>
        public List<CharactModel> LevCharat { get; set; }
    }
}
