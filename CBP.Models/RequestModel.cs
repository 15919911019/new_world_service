using System;
using System.Collections.Generic;

namespace CBP.Models
{
    public class RequestModel
    {
        /// <summary>
        /// 请求页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 请求数据长度
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 条件
        /// </summary>
        public List<object> Param { get; set; }

        public object Data { get; set; }

        public string UserID { get; set; }
    }
}
