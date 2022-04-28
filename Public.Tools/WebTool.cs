using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Public.Tools
{
    public class WebTool
    {
        string m_cookie = "";

        public string m_location = "";

        /// <summary>
        /// 获取回应内容
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private string GetResponseBody(HttpWebResponse response)
        {
            string responseBody = string.Empty;

            if (response.ContentEncoding.ToLower().Contains("gzip"))
            {

                using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                {
                    Encoding encoding = Encoding.UTF8;
                    switch (response.CharacterSet.ToLower())
                    {
                        case "gb2312":
                        case "iso-8859-1":
                            encoding = Encoding.GetEncoding("gb2312");
                            break;
                    }

                    using (StreamReader reader = new StreamReader(stream, encoding))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }
            }
            else if (response.ContentEncoding.ToLower().Contains("deflate"))
            {
                using (DeflateStream stream = new DeflateStream(
                    response.GetResponseStream(), CompressionMode.Decompress))
                {
                    Encoding encoding = Encoding.UTF8;
                    switch (response.CharacterSet.ToLower())
                    {
                        case "gb2312":
                        case "iso-8859-1":
                            encoding = Encoding.GetEncoding("gb2312");
                            break;
                    }

                    using (StreamReader reader =
                        new StreamReader(stream, encoding))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }
            }
            else
            {
                using (Stream stream = response.GetResponseStream())
                {
                    Encoding encoding = Encoding.UTF8;
                    if (response.CharacterSet != null)
                    {
                        switch (response.CharacterSet.ToLower())
                        {
                            case "gb2312":
                            case "iso-8859-1":
                            case "gbk":
                                encoding = Encoding.GetEncoding("gb2312");
                                break;
                        }
                    }
                    //encoding = Encoding.GetEncoding(response.CharacterSet);

                    using (StreamReader reader =
                        new StreamReader(stream, encoding))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }
            }

            return responseBody;
        }


        /// <summary>
        /// html页面请求
        /// </summary>
        /// <param name="strUrl">请求页面地址</param>
        /// <param name="strPostData">Post数据</param>
        /// <param name="boRedirect">是否重定向</param>
        /// <param name="strPolicyPath">客户端证书全路径名</param>
        /// <returns></returns>
        public string GetHtml(string strUrl, string strPostData = null, bool boRedirect = false, string strPolicyPath = null, string strRefererHttp = null)
        {

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                //过滤服务器端证书
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);

                //过滤客户端证书
                X509Certificate Cert = null;
                if (!string.IsNullOrEmpty(strPolicyPath))
                {
                    Cert = X509Certificate.CreateFromCertFile(strPolicyPath);
                }

                request = (HttpWebRequest)WebRequest.Create(strUrl);

                if (!string.IsNullOrEmpty(strPolicyPath))
                {
                    request.ClientCertificates.Add(Cert); //添加证书
                }

                request.Method = string.IsNullOrEmpty(strPostData) ? "GET" : "POST";
                request.Accept = "*/*";
                request.KeepAlive = true;
                request.AllowAutoRedirect = boRedirect;
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
                request.Headers.Add("Accept-Language", "zh-CN");
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.SetCookies(request.RequestUri, m_cookie.Trim(','));

                if (!string.IsNullOrEmpty(strRefererHttp))
                {
                    request.Referer = strRefererHttp;
                }

                if (request.Method == "POST")
                {
                    request.ContentType = "application/x-www-form-urlencoded";

                    byte[] bytes = Encoding.UTF8.GetBytes(strPostData);
                    request.ContentLength = bytes.Length;
                    Stream os = request.GetRequestStream();
                    os.Write(bytes, 0, bytes.Length);
                    os.Close();
                }

                response = (HttpWebResponse)request.GetResponse();
                m_cookie = request.CookieContainer.GetCookieHeader(request.RequestUri).Replace(";", ",");
                if (boRedirect)
                {
                    m_location = response.Headers["Location"].ToString();
                }

                string strHtml = GetResponseBody(response);
                return strHtml;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("html请求出现问题");
                throw ex;
            }
            finally
            {
                if (request != null) request.Abort();
                if (response != null) response.Close();
            }

            return null;
        }


        //****伪造证书，总是接受
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }


    }
}
