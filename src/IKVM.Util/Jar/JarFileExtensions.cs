using System;
using System.Linq;

using IKVM.ByteCode;
using IKVM.ByteCode.Decoding;
using IKVM.Util.Modules;

using static IKVM.Util.Jar.JarFileUtil;

namespace IKVM.Util.Jar
{

    /// <summary>
    /// Various extension methods for the <see cref="JarFile"/> class.
    /// </summary>
    public static class JarFileExtensions
    {

        /// <summary>
        /// Attempts to get the module name of the JAR file pointed to by the path.
        /// </summary>
        /// <param name="jar"></param>
        /// <returns></returns>
        public static ModuleInfo GetModuleInfo(this JarFile jar)
        {
            if (jar is null)
                throw new ArgumentNullException(nameof(jar));

            return GetModuleInfoFromClass(jar) ?? GetModuleInfoFromManifest(jar);
        }

        /// <summary>
        /// Attempts to get the module name of the JAR file from the module-info.class file.
        /// </summary>
        /// <param name="jar"></param>
        /// <returns></returns>
        static ModuleInfo GetModuleInfoFromClass(JarFile jar)
        {
            if (jar is null)
                throw new ArgumentNullException(nameof(jar));

            var e = jar.GetEntry("module-info.class");
            if (e == null)
                return null;

            using var s = e.Open();
            using var c = ClassFile.Read(s);
            if ((c.AccessFlags & AccessFlag.Module) != 0)
            {
                var a = c.Attributes.FirstOrDefault(i => i.IsNotNil && i.Name.IsNotNil && c.Constants.Get(i.Name).Value == AttributeName.Module);
                if (a.IsNotNil)
                {
                    var m = (ModuleAttribute)a;
                    var name_ = c.Constants.Get(m.Name).Name;
                    var version_ = c.Constants.Get(m.Version).Value;
                    return new ModuleInfo(name_, version_ != null && ModuleVersion.TryParse(version_.AsSpan(), out var version) ? version : null);
                }
            }

            return null;
        }

        /// <summary>
        /// Attempts to get the module name of the JAR file from the manifest.
        /// </summary>
        /// <param name="jar"></param>
        /// <returns></returns>
        static ModuleInfo GetModuleInfoFromManifest(JarFile jar)
        {
            if (jar.Manifest is null)
                return null;

            return new ModuleInfo(jar.Manifest.MainAttributes.TryGetValue("Automatic-Module-Name", out var name) ? name : null, null);
        }

    }

}
