using System;
using System.Xml;
using System.Xml.Linq;

namespace FSC.WUF
{
    /// <summary>
    /// A class to use HTML from the program resources
    /// </summary>
    public class Html : ResourceFileManager
    {
        /// <summary>
        /// Checks if this html is valid for this library + updates resources links
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            var xmlSettings = new XmlReaderSettings();
            xmlSettings.Async = true;
            xmlSettings.IgnoreComments = true;
            xmlSettings.DtdProcessing = DtdProcessing.Ignore;

            var xml = new XmlDocument();
            xml.LoadXml(resource);

            if (!XmlNodeCheck(xml.DocumentElement!.ChildNodes))
            {
                return false;
            }

            var guidAttr = xml!.CreateAttribute("element-guid");
            guidAttr.Value = Guid.NewGuid().ToString();
            xml.DocumentElement.Attributes?.SetNamedItem(guidAttr);

            resource = xml.OuterXml;

            return true;
        }

        private bool XmlNodeCheck(XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                var name = node.Name;
                if (name.Equals("script", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                if (name.Equals("style", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                if (name.Equals("link", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                if (node?.Attributes?.Count > 0)
                {
                    foreach (XmlAttribute attr in node.Attributes)
                    {
                        if (attr.Name.StartsWith("on", StringComparison.OrdinalIgnoreCase))
                        {
                            return false;
                        }

                        if (attr.Value.StartsWith("res://", System.StringComparison.OrdinalIgnoreCase))
                        {
                            var getName = GetSingleByNameEmbeddedResource(attr.Value.Replace("res://", "", StringComparison.OrdinalIgnoreCase));

                            if (!string.IsNullOrWhiteSpace(getName))
                            {
                                attr.Value = ReadResourceAsDataURL(getName);
                            }
                        }
                    }

                    if (name.Equals("a", StringComparison.OrdinalIgnoreCase))
                    {
                        node.Attributes.RemoveNamedItem("target");
                        var attr = node!.OwnerDocument!.CreateAttribute("target");
                        attr.Value = "_blank";
                        node.Attributes.SetNamedItem(attr);
                    }
                }

                if (name.Equals("form", StringComparison.OrdinalIgnoreCase))
                {
                    var attr = node!.OwnerDocument!.CreateAttribute("onsubmit");
                    attr.Value = "return false;";
                    node.Attributes?.SetNamedItem(attr);
                }

                var guidAttr = node!.OwnerDocument!.CreateAttribute("element-guid");
                guidAttr.Value = Guid.NewGuid().ToString();
                node.Attributes?.SetNamedItem(guidAttr);

                if (!XmlNodeCheck(node!.ChildNodes))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
