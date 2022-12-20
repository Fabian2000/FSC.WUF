using System;
using System.Xml;

namespace FSC.WUF
{
    public class Html : ResourceFileManager
    {
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
                }

                if (!XmlNodeCheck(node!.ChildNodes))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
