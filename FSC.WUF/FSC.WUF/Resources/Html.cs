using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media.Media3D;
using System.Xml;

namespace FSC.WUF
{
    /// <summary>
    /// A class to use HTML from the program resources
    /// </summary>
    public class Html : ResourceFileManager
    {
        /// <summary>
        /// A new, but not recommend way to use html. Use the other constructor instead. This constructor is only for cases, if there is no other way to use html.
        /// </summary>
        /// <param name="html">A string that has to match all HTML/XML rules</param>
        public Html(string html)
        {
            resource = html;
        }

        /// <summary>
        /// A class to use HTML from the program resources
        /// </summary>
        public Html()
        {
            // Empty
        }

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

        /// <summary>
        /// Bind a class to html. Place in html @Binding->PropertyName; at the place where the information should be.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance of your class with the binding (If fail, make it public))</param>
        public void ForEachBinding<T>(T instance) where T : class
        {
            var xmlSettings = new XmlReaderSettings();
            xmlSettings.Async = true;
            xmlSettings.IgnoreComments = true;
            xmlSettings.DtdProcessing = DtdProcessing.Ignore;

            var xml = new XmlDocument();
            xml.LoadXml(resource);
            var namespaceManager = new XmlNamespaceManager(xml.NameTable);
            namespaceManager.AddNamespace("x", "http://www.w3.org/1999/xhtml");

            var foreachNodes = xml.SelectNodes("//x:foreach[@from and @as]", namespaceManager);

            if (foreachNodes is null)
            {
                return;
            }

            foreach (XmlNode foreachNode in foreachNodes)
            {
                string? asValue = foreachNode.Attributes?["as"]?.Value;
                string? fromValue = foreachNode.Attributes?["from"]?.Value;

                MemberInfo[] members = instance.GetType().GetMembers();

                dynamic? realFromValue = null;

                foreach (var member in members)
                {
                    switch (member.MemberType)
                    {
                        case MemberTypes.Field:
                            realFromValue = ((FieldInfo)member).GetValue(instance);
                            break;

                        case MemberTypes.Property:
                            realFromValue = ((PropertyInfo)member).GetValue(instance);
                            break;

                        default:
                            continue;
                    }
                }

                if (string.IsNullOrWhiteSpace(asValue) || string.IsNullOrWhiteSpace(fromValue) || realFromValue is null)
                {
                    continue;
                }

                List<XmlNode> newNodes = new List<XmlNode>();
                foreach (dynamic realValue in realFromValue)
                {
                    XmlNode foreachNodeClone = foreachNode.CloneNode(true);
                    List<XmlNode> nodes = FindAllNodesAndSubNodes(foreachNodeClone);
                    foreach (var node in nodes)
                    {
                        if (node.Attributes is not null)
                        {
                            foreach (XmlAttribute attribut in node.Attributes)
                            {
                                attribut.Value = MapItemToForEachBinding(realValue, realValue, asValue, attribut.Value);
                            }
                        }

                        if (node.HasChildNodes && node.ChildNodes[0].NodeType == XmlNodeType.Element)
                        {
                            continue;
                        }

                        node.InnerText = MapItemToForEachBinding(realValue, realValue, asValue, node.InnerText);
                    }
                    
                    foreach (XmlNode node in foreachNodeClone.ChildNodes)
                    {
                        newNodes.Add(node.CloneNode(true));
                    }
                }

                var parent = foreachNode.ParentNode;

                if (parent is null)
                {
                    continue;
                }

                newNodes.Reverse();

                foreach (XmlNode node in newNodes)
                {
                    parent.InsertAfter(node, foreachNode);
                }

                parent.RemoveChild(foreachNode);
            }

            resource = xml.OuterXml;
        }

        private List<XmlNode> FindAllNodesAndSubNodes(XmlNode node)
        {
            var nodes = new List<XmlNode>();

            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.HasChildNodes)
                {
                    nodes.AddRange(FindAllNodesAndSubNodes(childNode));
                }

                if (childNode.NodeType == XmlNodeType.Element)
                //if (childNode.Name.Equals("foreach", StringComparison.OrdinalIgnoreCase))
                {
                    nodes.Add(childNode);
                }
            }

            return nodes;
        }

        private string MapItemToForEachBinding<T>(dynamic? item, T instance, string asValue, string content)
        {
            string result = Regex.Replace(content, $@"@Binding->{asValue}\.(.*?);", (match) =>
            {
                string? value = match.Groups[1].Value;
                
                if (value is null)
                {
                    return match.Value;
                }

                string[] values = value.Split('.');

                foreach (var val in values)
                {
                    if (item is null)
                    {
                        return string.Empty;
                    }

                    if (!val.EndsWith("]"))
                    {
                        MemberInfo[] members = item.GetType().GetMembers();
                        MemberInfo? member = members.FirstOrDefault(x => x.Name.Equals(val, StringComparison.OrdinalIgnoreCase));

                        if (member is null)
                        {
                            return match.Value;
                        }

                        object? temp = null;
                        Type? type = null;

                        switch (member.MemberType)
                        {
                            case MemberTypes.Field:
                                FieldInfo fieldInfo = ((FieldInfo)member);
                                temp = fieldInfo.GetValue(instance);
                                type = temp.GetType();
                                item = Convert.ChangeType(temp, type);
                                break;

                            case MemberTypes.Property:
                                PropertyInfo propertyInfo = ((PropertyInfo)member);
                                temp = propertyInfo.GetValue(instance);
                                type = temp.GetType();
                                item = Convert.ChangeType(temp, type);
                                break;

                            default:
                                return match.Value;
                        }
                    }
                    else
                    {
                        string val2 = val.Split('[')[0];
                        string index = val.Split('[')[1].Split(']')[0];

                        MemberInfo[] members = item.GetType().GetMembers();
                        MemberInfo? member = members.FirstOrDefault(x => x.Name.Equals(val2, StringComparison.OrdinalIgnoreCase));

                        if (member is null)
                        {
                            return match.Value;
                        }

                        object? temp = null;
                        Type? type = null;

                        switch (member.MemberType)
                        {
                            case MemberTypes.Field:
                                FieldInfo fieldInfo = ((FieldInfo)member);
                                temp = fieldInfo.GetValue(instance);
                                type = temp.GetType();
                                item = Convert.ChangeType(temp, type);
                                break;

                            case MemberTypes.Property:
                                PropertyInfo propertyInfo = ((PropertyInfo)member);
                                temp = propertyInfo.GetValue(instance);
                                type = temp.GetType();
                                item = Convert.ChangeType(temp, type);
                                break;

                            default:
                                return match.Value;
                        }

                        if (item is null)
                        {
                            return string.Empty;
                        }

                        if (int.TryParse(index, out int indexInt))
                        {
                            item = item[indexInt];
                        }
                        else if (index.StartsWith(@"""") && index.EndsWith(@""""))
                        {
                            item = item[index.Replace(@"""", string.Empty)];
                        }
                        else
                        {
                            return match.Value;
                        }
                    }
                }

                return item?.ToString() ?? content;
            });

            return result;
        }

        /// <summary>
        /// Returns the html as string. Be careful. Using this with the new string parameter constructor and the validation can slow down your application
        /// </summary>
        /// <returns>Returns html code as string</returns>
        public override string ToString()
        {
            return resource;
        }
    }
}
