using System;
using System.Xml.Linq;

namespace Simbad.Utils.Extenders
{
    public static class LinqXmlExtender
    {
        public static XElement FindElementWithAttributeValue(this XElement xElement, XName attributeName, string value)
        {
            return FindElementWithAttributeValue(xElement, attributeName, value, StringComparison.OrdinalIgnoreCase);
        }

        public static XElement FindElementWithAttributeValue(
            this XElement xElement,
            XName attributeName,
            string value,
            StringComparison stringComparison)
        {
            var childs = xElement.Elements();

            foreach (var child in childs)
            {
                var childAttrValue = child.AttributeValue(attributeName, null);

                if (childAttrValue != null && childAttrValue.Equals(value, stringComparison))
                {
                    return child;
                }
            }

            return null;
        }

        public static string AttributeValue(this XElement xElement, XName attributeName, string defaultValue)
        {
            var attr = xElement.Attribute(attributeName);

            return attr == null ? defaultValue : attr.Value;
        }
    }
}