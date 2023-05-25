using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TemplateModels
{
    public class PartCharatModel
    {
        /// <summary>
        /// 判读顺序
        /// </summary>
        public List<SortModel> Index { get; set; } = new List<SortModel>();

        /// <summary>
        /// 部位集合
        /// </summary>
        public List<PartTierModel> PartTiers { get; set; } = new List<PartTierModel>();
    }

    public class PartTierModel
    {
        public string TierName { get; set; }

        public string PartCode
        {
            get
            {
                if (TierName?.Length > 0 == false)
                    return TierName;

                return TierName.Substring(0, 1);
            }
        }

        /// <summary>
        /// 部位特征集合
        /// </summary>
        public List<CharactModel> PartCharsct { get; set; }
    }
}
