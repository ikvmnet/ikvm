using System;

using IKVM.ByteCode.Decoding;
using IKVM.CoreLib.Modules;

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
            var m = ModuleDescriptor.Read(c);
            return new ModuleInfo(m.Name, m.Version);
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

            return new ModuleInfo(jar.Manifest.MainAttributes.TryGetValue("Automatic-Module-Name", out var name) ? name : null, default);
        }

    }

}
