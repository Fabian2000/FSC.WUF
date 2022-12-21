using System;
using System.Text.RegularExpressions;

namespace FSC.WUF
{
    public class Css : ResourceFileManager
    {
        public override bool IsValid()
        {
            var findResources = Regex.Matches(resource, """("|\(|,\s?)(res://.*?)("|\)|\s,)""");

            foreach (Match foundResource in findResources)
            {
                var resLink = foundResource.Groups[1].Value;
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
