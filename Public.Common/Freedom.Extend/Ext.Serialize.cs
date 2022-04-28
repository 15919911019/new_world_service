using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Public.Common
{
    public static partial class Ext
    {
        #region 序列化 对象到字符串
        public static string Serialize<T>(this T entity)
        {
            if (entity == null)
                return string.Empty;
            try
            {
                StringBuilder buffer = new StringBuilder();
                XmlSerializer serializer = new XmlSerializer(entity.GetType());
                using (TextWriter writer = new StringWriter(buffer))
                {
                    serializer.Serialize(writer, entity);
                }
                return buffer.ToString();
            }
            catch (System.Exception)
            {
                return string.Empty;
            }
        }
        #endregion

        #region XML字符串序列化对象
        /// <summary>
        /// 字符串序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(this string xmlStr)
        {
            XmlDocument xdoc = new XmlDocument();
            try
            {
                xdoc.LoadXml(xmlStr);
                XmlNodeReader reader = new XmlNodeReader(xdoc.DocumentElement);
                XmlSerializer ser = new XmlSerializer(typeof(T));
                object obj = ser.Deserialize(reader);
                return (T)obj;
            }
            catch
            {
                return default(T);
            }
        }
        #endregion

        #region 反序列化 字符串到对象
        /// <summary>
        /// 反序列化 字符串到对象
        /// </summary>
        /// <param name="obj">泛型对象</param>
        /// <param name="str">要转换为对象的字符串</param>
        /// <returns>反序列化出来的对象</returns>
        public static T Desrialize<T>(this string xmlString) where T : new()
        {
            try
            {
                T cloneObject = default(T);
                StringBuilder buffer = new StringBuilder();
                buffer.Append(xmlString);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (TextReader reader = new StringReader(buffer.ToString()))
                {
                    Object obj = serializer.Deserialize(reader);
                    cloneObject = (T)obj;
                }
                return cloneObject;
            }
            catch (System.Exception)
            {
                return default(T);
            }
        }
        #endregion
    }
}
