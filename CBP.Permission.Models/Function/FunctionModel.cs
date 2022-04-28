using CBP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBP.Permission.Models.Function
{
    public class FunctionModel : BaseModel
    {
        /// <summary>
        /// 功能id
        /// </summary>
        public string FunctionValue { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        public string FunctionName { get; set; }
    }
}
