using MimeTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FSC.WUF
{
    public abstract partial class ResourceFileManager
    {
        internal string resource = string.Empty;
        internal Assembly? _assembly;
        internal string dataURL = string.Empty;

        internal string[] GetAllEmbeddedResources()
        {
            return _assembly!.GetManifestResourceNames();
        }

        internal string GetSingleByNameEmbeddedResource(string resourceName)
        {
            var resources = GetAllEmbeddedResources();

            var matchingResources = resources?.Where
            (
                resName => resName.EndsWith(resourceName, System.StringComparison.OrdinalIgnoreCase)
            );

            var firstResource = matchingResources?.FirstOrDefault();

            return firstResource ?? string.Empty;
        }

        internal string ReadFromResource(string resourceName)
        {
            if (resourceName == string.Empty)
            {
                return string.Empty;
            }

            using (var stream = _assembly!.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream!))
            {
                return reader.ReadToEnd();
            }
        }

        internal byte[] ReadByteFromResource(string resourceName)
        {
            if (resourceName == string.Empty)
            {
                return Array.Empty<byte>();
            }

            using (var stream = _assembly!.GetManifestResourceStream(resourceName))
            using (var writer = new MemoryStream())
            {
                stream!.CopyTo(writer);

                return writer.ToArray();
            }
        }

        internal string ReadResourceAsDataURL(string resourceName)
        {
            var bytes = ReadByteFromResource(resourceName);
            var base64 = Convert.ToBase64String(bytes);
            var dataUrl = $"data:{MimeTypeMap.GetMimeType(Path.GetExtension(resourceName))};base64,{base64}";

            return dataUrl;
        }

        internal void BuildDataUrl(string extension)
        {
            var bytes = Encoding.UTF8.GetBytes(resource);
            var base64 = Convert.ToBase64String(bytes);
            dataURL = $"data:{MimeTypeMap.GetMimeType(extension)};base64,{base64}";
        }
    }
}
