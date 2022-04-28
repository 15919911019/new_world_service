using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Public.Common.Freedom.Xml
{
    public class XmlReaderHelper
    {
        /// <summary>
        /// 获取XML字符串里面唯一节点值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static T GetValue<T>(string text, string elementName)
        {
            string regexValue = string.Format("<{0}>(?<BUHUO>[\\s|\\S]*?)</{0}>", elementName);
            string result = GetRegValue(text, regexValue);
            if (string.IsNullOrEmpty(result) || "null" == result)
                return default(T);
            else
            {
                return (T)Convert.ChangeType(result, typeof(T));
            }
        }

        /// <summary>
        /// 获取指定正则值
        /// </summary>
        /// <param name="remoteStr"></param>
        /// <param name="regexValue"></param>
        /// <returns></returns>
        public static string GetRegValue(string remoteStr, string regexValue)
        {
            Regex regex = new Regex(regexValue, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Match match = regex.Match(remoteStr);
            string result = "";
            if (match.Success)
                result = match.Groups["BUHUO"].Value;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="remoteStr"></param>
        /// <param name="regexValue"></param>
        /// <returns></returns>
        private static IList<string> GetRegValues(string remoteStr, string regexValue)
        {
            Regex regex = new Regex(regexValue, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection matchCollections = regex.Matches(remoteStr);
            IList<string> strs = new List<string>();
            foreach (Match match in matchCollections)
            {
                strs.Add(match.Value);
            }
            return strs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlText"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static List<T> DeserializeList<T>(string xmlText, string elementName)
            where T : class, new()
        {
            List<T> results = new List<T>();
            string format = "<{0}>(?<BUHUO>[\\s|\\S]*?)</{0}>";
            var elements = GetRegValues(xmlText,
                string.Format("<{0}>(?<BUHUO>[\\s|\\S]*?)</{0}>", elementName));
            PropertyInfo[] pInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (string element in elements)
            {
                T result = new T();
                Array.ForEach<PropertyInfo>(pInfos,
                    pInfo =>
                    {
                        Type targetType = pInfo.PropertyType;
                        string eleName = pInfo.Name;
                        #region Set  PropertyName
                        var elementAttrs = pInfo.GetCustomAttributes(typeof(XmlElementAttribute), false);
                        if (elementAttrs.Length > 0)
                        {
                            eleName = ((XmlElementAttribute)elementAttrs[0]).ElementName;
                        }
                        #endregion
                        if (string.IsNullOrEmpty(eleName) == false)
                        {
                            try
                            {
                                string eleValue = GetRegValue(element, string.Format(format, eleName));
                                if (string.IsNullOrEmpty(eleValue) == false &&
                                    !"null".Equals(eleValue))
                                    pInfo.SetValue(result, Convert.ChangeType(eleValue, targetType), null);
                            }
                            catch (Exception)
                            {

                            }
                        }
                    });
                results.Add(result);
            }
            return results;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlText"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string xmlText, string elementName)
            where T : class, new()
        {
            string format = "<{0}>(?<BUHUO>[\\s|\\S]*?)</{0}>";
            var elements = GetRegValues(xmlText,
                string.Format("<{0}>(?<BUHUO>[\\s|\\S]*?)</{0}>", elementName));
            PropertyInfo[] pInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (string element in elements)
            {
                T result = new T();
                Array.ForEach<PropertyInfo>(pInfos,
                    pInfo =>
                    {
                        Type targetType = pInfo.PropertyType;
                        string eleName = pInfo.Name;
                        #region Set  PropertyName
                        var elementAttrs = pInfo.GetCustomAttributes(typeof(XmlElementAttribute), false);
                        if (elementAttrs.Length > 0)
                        {
                            eleName = ((XmlElementAttribute)elementAttrs[0]).ElementName;
                        }
                        #endregion
                        if (string.IsNullOrEmpty(eleName) == false)
                        {
                            try
                            {
                                string eleValue = GetRegValue(element, string.Format(format, eleName));
                                if (string.IsNullOrEmpty(eleValue) == false &&
                                    !"null".Equals(eleValue))
                                    pInfo.SetValue(result, Convert.ChangeType(eleValue, targetType), null);
                            }
                            catch (Exception)
                            {

                            }
                        }
                    });
                return result;
            }
            return default(T);
        }
    }
}
