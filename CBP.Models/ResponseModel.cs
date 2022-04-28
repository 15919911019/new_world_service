using Public.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBP.Models
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            Code = ErrorCodeEnum.Success;
        }

        /// <summary>
        /// 
        /// </summary>
        public ErrorCodeEnum Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ResponseModel Success(object data = null, int total = 0, string msg = null, bool isLog = false)
        {
            ResponseModel response = new ResponseModel();
            response.Code = ErrorCodeEnum.Success;
            response.Data = data;
            response.Total = total;
            response.Message = msg;

            if (isLog == true)
                Logger.Debug(msg);

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ResponseModel Fail(string msg = null, bool isLog = false)
        {
            ResponseModel response = new ResponseModel();
            response.Code = ErrorCodeEnum.Fail;
            response.Message = msg;

            if (isLog == true)
                Logger.Error(msg);

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static ResponseModel Excetption(string msg = null, Exception ex = null, bool isLog = true)
        {
            ResponseModel response = new ResponseModel();
            response.Code = ErrorCodeEnum.Exception;
            response.Message = $"{msg}\r\n{ex?.Message}".Trim();

            if (isLog == true)
                Logger.Error(response.Message);

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ResponseModel Error(string msg = null, ErrorCodeEnum code = ErrorCodeEnum.Error, bool isLog = true)
        {
            ResponseModel response = new ResponseModel();
            response.Code = code;
            response.Message = msg;

            if (isLog == true)
                Logger.Error(response.Message);

            return response;
        }
    }
}
