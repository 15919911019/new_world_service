using System;
using System.Collections.Generic;
using System.Text;

namespace CBP.Models
{
    public enum ErrorCodeEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 异常
        /// </summary>
        Exception = 1,

        /// <summary>
        /// 失败
        /// </summary>
        Fail = 2,

        /// <summary>
        /// 错误
        /// </summary>
        Error =3,

        /// <summary>
        /// 签名错误
        /// </summary>
        SignError = 4,

        /// <summary>
        /// 参数错误
        /// </summary>
        ParamereError = 5,

        /// <summary>
        /// 没有用户、商户id
        /// </summary>
        NoUser_Mer_ID = 6,

        /// <summary>
        /// 没有模块名称（服务模块）
        /// </summary>
        NoModuleOrCommand = 7,
    }
}
