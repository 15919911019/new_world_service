using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Public.Common
{
    public partial class LinqXmlFile:XDocument
    {
        private XDocument doc;
        public LinqXmlFile(string filePath)
        {
            doc = Load(filePath);
        }

        #region 获取名族名称及代码
        /// <summary>
        /// 获取名族名称及代码
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns>0名族名称1名族代码</returns>
        public string[] GetNationCode(string keyword)
        {
            XAttribute[] xattr = GetNodeValue(keyword, "NationItem", "NationName", "NationCode");
            return GetList(xattr);
        }
        #endregion

        #region 获取出生地代码
        /// <summary>
        /// 获取出生地代码
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public string[] GetCsdCode(string keyWord)
        {
            XAttribute[] xattr = GetNodeValue(keyWord, "placeItem", "placeName", "placeCode");
            return GetList(xattr);
        }
        #endregion

        #region 获取国家代码
        /// <summary>
        /// 获取国家代码
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public string[] GetCountryCode(string keyWord)
        {
            XAttribute[] xattr = GetNodeValue(keyWord, "countryItem", "countryName", "countryCode");
            return GetList(xattr);
        }
        #endregion

        #region 获取指定项数据
        /// <summary>
        /// 获取指定项数据
        /// </summary>
        /// <param name="selectNodeName"></param>
        /// <param name="keyWord"></param>
        /// <param name="itemName"></param>
        /// <param name="attribute1"></param>
        /// <param name="attribute2"></param>
        /// <returns></returns>
        public string[] GetItemByKeyWord(string selectNodeName, string keyWord, string itemName = "Item", string attribute1 = "Name", string attribute2 = "Code")
        {
            XAttribute[] xattr = GetNodeValue(selectNodeName, keyWord, itemName, attribute1, attribute2);
            return GetList(xattr);
        }
        #endregion

        #region
        /// <summary>
        /// 获取户口所在地
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public string[] GetHKSZDCode(string keyWord)
        {
            XAttribute[] xattr = GetNodeValue(keyWord, "HKSZDItem", "HKSZDName", "HKSZDCode");
            return GetList(xattr);
        }
        #endregion

        #region
        private string[] GetList(XAttribute[] xattr)
        {
            if (xattr != null)
            {
                string[] Resutl = new string[xattr.Length];
                for (int i = 0; i < xattr.Length; i++)
                {
                    Resutl[i] = xattr[i].Value.ToString();
                }
                return Resutl;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="ItemName"></param>
        /// <param name="Attribute1">属性名称1</param>
        /// <param name="Attribute2">属性名称2</param>
        /// <returns></returns>
        private XAttribute[] GetNodeValue(string keyWord, string ItemName, string Attribute1, string Attribute2)
        {
            var str = from p in doc.Descendants(ItemName)
                      where p.Attribute(Attribute1).Value == keyWord
                      select p.Attributes();
            if (str.ToArray().Length > 0)
            {
                return str.ToArray()[0].ToArray();
            }
            else
            {
                str = from p in doc.Descendants(ItemName)
                      where p.Attribute(Attribute2).Value == keyWord
                      select p.Attributes();
                if (str.ToArray().Length > 0)
                    return str.ToArray()[0].ToArray();
                else
                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectNodeName">指定选择查询的节点</param>
        /// <param name="keyWord"></param>
        /// <param name="ItemName"></param>
        /// <param name="Attribute1">属性名称1</param>
        /// <param name="Attribute2">属性名称2</param>
        /// <returns></returns>
        private XAttribute[] GetNodeValue(string selectNodeName, string keyWord, string ItemName, string Attribute1, string Attribute2)
        {
            var str = from p in doc.Descendants(selectNodeName).Elements(ItemName)
                      where p.Attribute(Attribute1).Value == keyWord
                      select p.Attributes();
            if (str.ToArray().Length > 0)
            {
                return str.ToArray()[0].ToArray();
            }
            else
            {
                str = from p in doc.Descendants(ItemName)
                      where p.Attribute(Attribute2).Value == keyWord
                      select p.Attributes();
                if (str.ToArray().Length > 0)
                    return str.ToArray()[0].ToArray();
                else
                    return null;
            }
        }
        #endregion
    }
}
