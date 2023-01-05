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

    }

}
