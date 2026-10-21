using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace IKVM.Tests.Util
{

    public static class DotNetSdkUtil
    {

        /// <summary>
        /// Returns <c>true</c> if the given file is an assembly.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsAssembly(string path)
        {
            try
            {
                using var st = File.OpenRead(path);
                using var pe = new PEReader(st);
                var md = pe.GetMetadataReader();
                md.GetAssemblyDefinition();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the paths to the reference assemblies of the specified TFM for the target framework.
        /// </summary>
        /// <param name="tfm"></param>
        /// <param name="targetFrameworkIdentifier"></param>
        /// <param name="targetFrameworkVersion"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string GetCoreLibName(string tfm, string targetFrameworkIdentifier, string targetFrameworkVersion)
        {
            if (targetFrameworkIdentifier == ".NETFramework")
                return "mscorlib";

            if (targetFrameworkIdentifier == ".NET")
                return "System.Runtime";

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the paths to the reference assemblies of the specified TFM for the target framework.
        /// </summary>
        /// <param name="tfm"></param>
        /// <param name="targetFrameworkIdentifier"></param>
        /// <param name="targetFrameworkVersion"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IList<string> GetPathToReferenceAssemblies(string tfm, string targetFrameworkIdentifier, string targetFrameworkVersion)
        {
            if (targetFrameworkIdentifier == ".NETFramework")
            {
                var dir = Path.Combine(Path.GetDirectoryName(typeof(DotNetSdkUtil).Assembly.Location), "netfxref", tfm);
                if (Directory.Exists(dir))
                    return [dir];

                return [];
            }

            if (targetFrameworkIdentifier == ".NET")
            {
                var dir = Path.Combine(Path.GetDirectoryName(typeof(DotNetSdkUtil).Assembly.Location), "netref", tfm);
                if (Directory.Exists(dir))
                    return [dir];

                return [];
            }

            throw new ArgumentException(nameof(targetFrameworkIdentifier));
        }

    }

}
