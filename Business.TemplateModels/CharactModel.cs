using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TemplateModels
{
    public class CharactModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// eg: [23] 、 [10, 56]
        /// </summary>
        public List<double> Values { get; set; }
    }
}
