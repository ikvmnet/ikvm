using System;
using System.Runtime.InteropServices;

namespace IKVM.Runtime
{

    /// <summary>
    /// Maintains various information about the current runtime environment.
    /// </summary>
    public static class RuntimeUtil
    {

        /// <summary>
        /// Returns <c>true</c> if the current runtime is Mono.
        /// </summary>
        public static bool IsMono { get; } = Type.GetType("Mono.Runtime") != null;

        /// <summary>
        /// Returns <c>true</c> if the current platform is Windows.
        /// </summary>
        public static bool IsWindows { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        /// <summary>
        /// Returns <c>true</c> if the current platform is Linux.
        /// </summary>
        public static bool IsLinux { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        /// <summary>
        /// Returns <c>true</c> if the current platform is Mac OS X.
        /// </summary>
        public static bool IsOSX { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        /// <summary>
        /// Returns the set of supported .NET RID values for the current runtime.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetSupportedRuntimeIdentifiers()
        {
            var arch = RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.X86 => "x86",
                Architecture.X64 => "x64",
                Architecture.Arm => "arm",
                Architecture.Arm64 => "arm64",
                _ => null,
            };

            if (IsWindows)
            {
                var v = Environment.OSVersion.Version;

                // Windows 10
                if (v.Major > 10 || (v.Major == 10 && v.Minor >= 0))
                    yield return $"win10-{arch}";

                // Windows 8.1
                if (v.Major > 6 || (v.Major == 6 && v.Minor >= 3))
                    yield return $"win81-{arch}";

                // Windows 7
                if (v.Major > 6 || (v.Major == 6 && v.Minor >= 1))
                    yield return $"win7-{arch}";

                // fallback
                yield return $"win-{arch}";
            }

            if (IsLinux)
            {
                yield return $"linux-{arch}";
            }

            if (IsOSX)
            {
                yield return $"osx-{arch}";
            }
        }

    }

}
