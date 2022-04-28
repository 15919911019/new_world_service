using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Public.Common
{
    using System.Data;
    using System.IO;

    /// <summary>
    /// xml数据类型
    /// </summary>
    public enum XmlType
    {
        /// <summary>
        /// 文件
        /// </summary>
        File,
        /// <summary>
        /// 字符串
        /// </summary>
        XmlStr,
    }

    public partial class XMLHelper : XmlDocument, IDisposable
    {
        #region 私有变量
        /// <summary>
        /// 保存xml数据（文件路径或XML字符串）
        /// </summary>
        private string XmlData;
        /// <summary>
        /// XML类型(文件路径或XML字符串)
        /// </summary>
        private XmlType XType;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xmldata"></param>
        /// <param name="xmltype"></param>
        public XMLHelper(string xmldata, XmlType xmltype = XmlType.File)
        {
            XmlData = xmldata;
            XType = xmltype;
            switch (xmltype)
            {
                case XmlType.File:
                    this.Load(XmlData);
                    break;
                case XmlType.XmlStr:
                    this.LoadXml(xmldata);
                    break;
            }
        }
        #endregion

        #region 给定一个节点的xPath表达式并返回一个节点
        /// <summary>
        /// 给定一个节点的xPath表达式并返回一个节点
        /// </summary>
        /// <param name="xPath">节点名</param>
        /// <returns></returns>
        public XmlNode FindNode(string xPath)
        {
            XmlNode xmlNode = this.SelectSingleNode(xPath);
            return xmlNode;
        }
        #endregion

        #region 给定一个节点的表达式返回此节点下的孩子节点列表
        /// <summary>
        /// 给定一个节点的表达式返回此节点下的孩子节点列表
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public XmlNodeList GetNodeList(string xPath)
        {
            XmlNodeList nodeList = this.SelectSingleNode(xPath).ChildNodes;
            return nodeList;
        }
        #endregion

        #region 获取DataSet
        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <returns></returns>
        public DataSet GetDataSet(string xPath)
        {
            try
            {
                string str = this.SelectSingleNode(xPath).OuterXml;
                StringReader read = new StringReader(str);
                DataSet ds = new DataSet();
                ds.ReadXml(read);
                read.Close();
                return ds;
            }
            catch (XmlException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取节点值
        /// <summary>
        /// 获取节点值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xPath"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetNodeValue<T>(string xPath, object defaultValue = null)
        {
            try
            {
                XmlNode xmlNode = this.SelectSingleNode(xPath);
                if (xmlNode != null)
                {
                    object obj = Convert.ChangeType(xmlNode.InnerText, typeof(T));
                    return (T)obj;
                }
                else
                {
                    CreateNode(xPath, defaultValue);
                    return (T)defaultValue;
                }
            }
            catch (XmlException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 设置节点值
        /// <summary>
        /// 设置节点值
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetNodeValue(string xpath, object value)
        {
            try
            {
                XmlNode xmlNode = this.SelectSingleNode(xpath);
                if (xmlNode != null)
                {
                    if (value != null)
                        xmlNode.InnerText = value.ToString();
                    else
                        xmlNode.InnerText = "";
                    this.Save(XmlData);
                    return true;
                }
                else
                {
                    CreateNode(xpath, value);
                    return true;
                }
            }
            catch (XmlException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 创建节点
        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="xPath"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool CreateNode(string xPath, object defaultValue)
        {
            try
            {
                int index = xPath.LastIndexOf('/');
                string nodename = xPath.Substring(0, index);
                string path = xPath.Substring(index + 1);
                XmlNode xmlnode = this.SelectSingleNode(nodename);
                if (xmlnode != null)
                {
                    XmlElement xe1 = this.CreateElement(path);
                    if (defaultValue != null)
                        xe1.InnerText = defaultValue.ToString();
                    xmlnode.AppendChild(xe1);
                    this.Save(XmlData);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (XmlException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 创建节点属性
        /// <summary>
        /// 创建节点属性
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool CreateNodeAttribute(string xpath, string attributeName = "value", object defaultValue = null)
        {
            try
            {
                XmlElement element = (XmlElement)this.SelectSingleNode(xpath);
                string str = "";
                if (defaultValue != null)
                    str = defaultValue.ToString();
                element.SetAttribute(attributeName, str);
                this.Save(XmlData);
                return true;
            }
            catch (XmlException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取节点属性值
        /// <summary>
        /// 获取节点属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xPath"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetNodeAttributeValue<T>(string xPath, string attributeName = "value", object defaultValue = null)
        {
            try
            {
                XmlNode xmlnode = this.SelectSingleNode(xPath);
                if (xmlnode != null)
                {
                    if (xmlnode.Attributes[attributeName] != null)
                    {
                        object obj = Convert.ChangeType(xmlnode.Attributes[attributeName].Value, typeof(T));
                        return (T)obj;
                    }
                    else
                    {
                        CreateNodeAttribute(xPath, attributeName, defaultValue);
                        return (T)defaultValue;
                    }
                }
                else
                {
                    if (XType == XmlType.File)
                    {
                        CreateNode(xPath, "");
                        CreateNodeAttribute(xPath, attributeName, defaultValue);
                        return (T)defaultValue;
                    }
                    else
                    {
                        return default(T);
                    }

                }
            }
            catch (XmlException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 设置节点属性值
        /// <summary>
        /// 设置节点属性值
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="value"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public bool SetNodeAttributeValue(string xpath, object value, string attributeName = "value")
        {
            try
            {
                XmlNode xmlnode = this.SelectSingleNode(xpath);
                if (xmlnode != null)
                {
                    if (value != null)
                        xmlnode.Attributes[attributeName].Value = value.ToString();
                    else
                        xmlnode.Attributes[attributeName].Value = "";
                    this.Save(XmlData);
                    return true;
                }
                else
                {
                    CreateNode(xpath, value);
                    CreateNodeAttribute(xpath, attributeName, value);
                    return true;
                }
            }
            catch (XmlException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 保存文档
        /// <summary>
        /// 保存文档
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public bool SaveXmlFile(string FilePath)
        {
            try
            {
                if (XType == XmlType.File)
                    return true;
                else
                {
                    return this.SaveXmlFile(FilePath);
                }
            }
            catch (XmlException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 释放资源
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            GC.Collect(0);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
