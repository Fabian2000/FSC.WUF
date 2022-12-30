using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace FSC.WUF
{
    /// <summary>
    /// A class to use HTML from the program resources
    /// </summary>
    public class Html : ResourceFileManager
    {
        /// <summary>
        /// Bind a class to html. Place in html @Binding->PropertyName; at the place where the information should be.
        /// </summary>
        /// <typeparam name="T">The class that is used to pass its properties to html</typeparam>
        /// <param name="class">An instance of the class</param>
        public void Bind<T>(T @class) where T : class
        {
            Bind(new T[] { @class });
        }

        /// <summary>
        /// Bind a class to html. Place in html @Binding->PropertyName; at the place where the information should be. The array makes it possible to automatically duplicate one html element until the array ended
        /// </summary>
        /// <typeparam name="T">The class that is used to pass its properties to html</typeparam>
        /// <param name="classes">An instance of the class</param>
        public void Bind<T>(T[] classes) where T : class
        {
            Bind(new List<T>(classes));
        }

        /// <summary>
        /// Bind a class to html. Place in html @Binding->PropertyName; at the place where the information should be. The list makes it possible to automatically duplicate one html element until the list ended
        /// </summary>
        /// <typeparam name="T">The class that is used to pass its properties to html</typeparam>
        /// <param name="classes">An instance of the class</param>
        public void Bind<T>(List<T> classes) where T : class
        {
            var htmlBuilder = new StringBuilder("<wuf-binding>");
            var properties = typeof(T).GetProperties();
            
            foreach (var @class in classes)
            {
                var temp = resource;
                foreach (var property in properties)
                {
                    var propForValue = @class.GetType().GetProperty(property.Name);
                    var propVal = propForValue?.GetValue(@class)?.ToString() ?? string.Empty;

                    temp = temp.Replace
                    (
                        $"@Binding->{property.Name};",
                        propVal?.ToString() ?? string.Empty
                    );
                }

                htmlBuilder.Append(temp);
            }

            htmlBuilder.AppendLine("</wuf-binding>");

            resource = htmlBuilder.ToString();
        }

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
            resource = resource.Replace("<wuf-binding>", string.Empty);
            resource = resource.Replace("</wuf-binding>", string.Empty);

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
