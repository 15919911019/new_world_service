using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Public.Common.Freedom.Xml
{
    public class XmlConvertDynamic : DynamicObject, IEnumerable
    {
        readonly List<XElement> _elements;

        public XmlConvertDynamic(string strXml)
        {
            var doc = XDocument.Parse(strXml);
            _elements = new List<XElement> { doc.Root };
        }

        protected XmlConvertDynamic(XElement element)
        {
            _elements = new List<XElement> { element };
        }

        protected XmlConvertDynamic(IEnumerable<XElement> elements)
        {
            _elements = new List<XElement>(elements);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            if (binder.Name == "Value")
                result = _elements[0].Value;
            else if (binder.Name == "Count")
                result = _elements.Count;
            else
            {
                var attr = _elements[0].Attribute(XName.Get(binder.Name));
                if (attr != null)
                    result = attr;
                else
                {
                    var items = _elements.Descendants(XName.Get(binder.Name));
                    if (items == null || items.Count() == 0)
                        return false;
                    result = new XmlConvertDynamic(items);
                }
            }
            return true;
        }


        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            int ndx = (int)indexes[0];
            result = new XmlConvertDynamic(_elements[ndx]);
            return true;
        }


        public IEnumerator GetEnumerator()
        {
            foreach (var element in _elements)
                yield return new XmlConvertDynamic(element);
        }


        public override string ToString()
        {
            if (_elements.Count == 1 && !_elements[0].HasElements)
            {
                return _elements[0].Value;
            }
            return string.Join("\n", _elements);
        }
    }

    public class DynamicHelper
    {
        public static string ToXml(dynamic dynamicObject)
        {
            DynamicXElement xmlNode = dynamicObject;
            return xmlNode.XContent.ToString();
        }

        public static dynamic ToObject(string xml, dynamic dynamicResult)
        {
            var doc = XDocument.Parse(xml);
            XElement element = doc.Root;
            dynamicResult = new DynamicXElement(element);
            return dynamicResult;
        }

        public static dynamic ToObject(string xml)
        {
            var doc = XDocument.Parse(xml);
            XElement element = doc.Root;
            //XElement element = XElement.Parse(xml);
            dynamic dynamicResult = new DynamicXElement(element);
            return dynamicResult;
        }
    }

   
 public class DynamicXElement : DynamicObject
    {
        public DynamicXElement(XElement node)
        {
            this.XContent = node;
        }

        public DynamicXElement()
        {
        }

        public DynamicXElement(String name)
        {
            this.XContent = new XElement(name);
        }

        public XElement XContent
        {
            get;
            private set;
        }

        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            XElement setNode = this.XContent.Element(binder.Name);
            if (setNode != null)
                setNode.SetValue(value);
            else
            {
                //creates an XElement without a value.
                if (value.GetType() == typeof(DynamicXElement))
                    this.XContent.Add(new XElement(binder.Name));
                else
                    this.XContent.Add(new XElement(binder.Name, value));
            }
            return true;
        }

        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            XElement getNode = this.XContent.Element(binder.Name);
            if (getNode != null)
            {
                result = new DynamicXElement(getNode);
            }
            else
            {
                result = new DynamicXElement(binder.Name);
            }
            return true;
        }

        public override bool TryConvert(
    ConvertBinder binder, out object result)
        {
            if (binder.Type == typeof(String))
            {
                result = this.XContent.Value;
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }
    }
}
