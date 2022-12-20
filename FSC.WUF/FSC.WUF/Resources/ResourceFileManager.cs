using System.IO;
using System.Reflection;

namespace FSC.WUF
{
    public abstract partial class ResourceFileManager
    {
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
