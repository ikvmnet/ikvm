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
        public static string GetModuleName(this JarFile jar)
        {
            return jar.Manifest is Manifest m ? GetModuleNameFromManifest(m) : null;
        }

        /// <summary>
        /// Attempts to get the module name of the JAR file from the manifest.
        /// </summary>
        /// <param name="manifest"></param>
        /// <returns></returns>
        static string GetModuleNameFromManifest(Manifest manifest)
        {
            return manifest.MainAttributes.TryGetValue("Automatic-Module-Name", out var name) ? name : null;
        }

    }

}
