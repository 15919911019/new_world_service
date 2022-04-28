using Public.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBP.Models
{
    public class GlobalConfigModel
    {
        /// <summary>
        /// 签名秘钥
        /// </summary>
        public static string SKey
        {
            get
            {
                return _sKey;
            }
        }
        private static string _sKey = Tool.GetConfig("SKey");
    }
}
