using System;
using System.Collections.Generic;
using System.Text;

namespace CBP.Models
{
    public class BaseMapModel
    {
        public BaseMapModel()
        {
            this.RecordID = Public.Tools.Tool.GuidTo16String();
            this.CreateTime = DateTime.Now;
            this.UpdateTime = DateTime.Now;
            this.DelMarker = false;
            this.IsEnable = true;
        }

        /// <summary>
        /// 记录Id
        /// </summary>
        public string RecordID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        public bool DelMarker { get; set; }

        public string OperationPerson { get; set; }
    }
}
