using System;

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

            return jar.Manifest is Manifest m ? GetModuleInfoFromManifest(m) : null;
        }

        /// <summary>
        /// Attempts to get the module name of the JAR file from the manifest.
        /// </summary>
        /// <param name="manifest"></param>
        /// <returns></returns>
        static ModuleInfo GetModuleInfoFromManifest(Manifest manifest)
        {
            if (manifest is null)
                throw new ArgumentNullException(nameof(manifest));

            return new ModuleInfo(manifest.MainAttributes.TryGetValue("Automatic-Module-Name", out var name) ? name : null, null);
        }

    }

}
