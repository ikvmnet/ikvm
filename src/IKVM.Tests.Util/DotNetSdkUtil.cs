using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

using Microsoft.Build.Utilities;

using Semver;

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
            if (targetFrameworkIdentifier == ".NETCore")
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
        /// <exception cref="InvalidOperationException"></exception>
        public static IList<string> GetPathToReferenceAssemblies(string tfm, string targetFrameworkIdentifier, string targetFrameworkVersion)
        {
            if (targetFrameworkIdentifier == ".NETFramework")
                return ToolLocationHelper.GetPathToReferenceAssemblies(targetFrameworkIdentifier, targetFrameworkVersion, "");
            if (targetFrameworkIdentifier == ".NETCore")
                return GetCorePathToReferenceAssemblies(tfm, targetFrameworkVersion);

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Discovers the reference assembly paths for .NET Core TFM and framework version.
        /// </summary>
        /// <param name="tfm"></param>
        /// <param name="targetFrameworkVersion"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        static IList<string> GetCorePathToReferenceAssemblies(string tfm, string targetFrameworkVersion)
        {
            // parse requested version
            if (SemVersion.TryParse(targetFrameworkVersion, SemVersionStyles.Any, out var targetVer) == false)
                throw new InvalidOperationException(targetFrameworkVersion);

            // back up to pack directory and get list of ref packs
            var sdkBase = DotNetSdkResolver.ResolvePath(null) ?? throw new InvalidOperationException();
            var packDir = Path.Combine(sdkBase, "..", "..", "packs", "Microsoft.NETCore.App.Ref");
            var sdkVers = Directory.EnumerateDirectories(packDir).Select(Path.GetFileName);

            // identify maximum matching version
            var thisVer = default(SemVersion);
            foreach (var ver in sdkVers)
                if (SemVersion.TryParse(ver, SemVersionStyles.Any, out var v))
                    if (v.IsPrerelease == false && v.Major == targetVer.Major && v.Minor == targetVer.Minor && v.ComparePrecedenceTo(thisVer) == 1)
                        thisVer = v;

            // no higher version found
            if (thisVer == null)
                return null;

            // check for TFM refs directory
            var refsDir = Path.Combine(packDir, thisVer.ToString(), "ref", tfm);
            if (Directory.Exists(refsDir) == false)
                throw new InvalidOperationException();

            // find all ref assemblies
            return new[] { Path.GetFullPath(refsDir) };
        }

    }

}
