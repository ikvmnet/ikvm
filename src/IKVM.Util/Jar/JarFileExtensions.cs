using System;

using IKVM.ByteCode;
using IKVM.ByteCode.Reading;
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
            var e = jar.GetEntry("module-info.class");
            if (e == null)
                return null;

            using var s = e.Open();
            var c = ClassReader.Read(s);
            if ((c.AccessFlags & AccessFlag.ACC_MODULE) != 0 && c.Attributes.Module != null)
                return new ModuleInfo(c.Attributes.Module.Name.Name.Value, c.Attributes.Module.Version != null && ModuleVersion.TryParse(c.Attributes.Module.Version.Value.AsSpan(), out var version) ? version : null);

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
