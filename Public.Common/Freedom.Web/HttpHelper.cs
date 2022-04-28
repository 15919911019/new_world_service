using System;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;
using System.Web;

namespace Public.Common
{
    /// <summary>
    /// HttpHelper
    /// </summary>
    public class HttpHelper
    {
        private CookieContainer m_Cookie = new CookieContainer();

        public HttpHelper()
        {
        }

        public string HttpGet(string Url, string postDataStr, string contentType)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + ((postDataStr == "") ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = contentType;
            request.CookieContainer = this.m_Cookie;
            request.KeepAlive = false;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            this.m_Cookie = request.CookieContainer;
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        public string HttpPost(string Url, string postDataStr, string contentType)
        {
            Encoding code = Encoding.GetEncoding("utf-8");
            string strRequestData = postDataStr;
            byte[] bytesRequestData = code.GetBytes(strRequestData);
            string strUrl = Url;
            string strResult = "";
            try
            {
                //设置HttpWebRequest基本信息
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                myReq.Method = "post";
                myReq.ContentType = "application/x-www-form-urlencoded";
                myReq.CookieContainer = this.m_Cookie;
                //填充POST数据
                myReq.ContentLength = bytesRequestData.Length;
                Stream requestStream = myReq.GetRequestStream();
                requestStream.Write(bytesRequestData, 0, bytesRequestData.Length);
                requestStream.Close();

                //发送POST数据请求服务器
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();
                //获取服务器返回信息
                StreamReader reader = new StreamReader(myStream, code);
                StringBuilder responseData = new StringBuilder();
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    responseData.Append(line);
                }
                //释放
                myStream.Close();
                strResult = responseData.ToString();
            }
            catch (Exception exp)
            {
                strResult = "报错：" + exp.Message;
            }
            return strResult;
        }
    }
}
