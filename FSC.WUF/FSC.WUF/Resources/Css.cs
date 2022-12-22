using System;
using System.Text.RegularExpressions;

namespace FSC.WUF
{
    /// <summary>
    /// A class to use CSS from the program resources
    /// </summary>
    public class Css : ResourceFileManager
    {
        /// <summary>
        /// Checks if this css is valid for this library + updates resources links
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            var findResources = Regex.Matches(resource, """("|\(|,\s?)(res://.*?)("|\)|\s,)""");

            foreach (Match foundResource in findResources)
            {
                var resLink = foundResource.Groups[2].Value;
                var getName = GetSingleByNameEmbeddedResource(resLink.Replace("res://", "", StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrWhiteSpace(getName))
                {
                    continue;
                }

                var dataUri = ReadResourceAsDataURL(getName);
                resource = resource.Replace(resLink, dataUri!);
            }

            return true;
        }
    }
}
