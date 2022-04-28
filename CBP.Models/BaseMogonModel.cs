using System;
using System.Collections.Generic;
using System.Text;

namespace CBP.Models
{
    public class BaseMongoModel
    {
        /// <summary>
        /// 记录id
        /// </summary>
        public string RecordID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
