using System.IO;
using System.Reflection;

namespace FSC.WUF
{
    /// <summary>
    /// A class to use {files} from the program resources
    /// </summary>
    public abstract partial class ResourceFileManager
    {
        /// <summary>
        /// Loads a resource file
        /// </summary>
        /// <param name="resourceName">For example: index.html or to be more exact: namespace.index.html ... To load resources from html and css directly, use res://namespace.index.html. It will call this method in background</param>
        /// <exception cref="IOException"></exception>
        public virtual void Load(string resourceName)
        {
            _assembly = Assembly.GetCallingAssembly();

            var getName = GetSingleByNameEmbeddedResource(resourceName);

            if (string.IsNullOrWhiteSpace(getName))
            {
                throw new IOException();
            }

            resource = ReadFromResource(getName);
        }

        public abstract bool IsValid();
    }
}
