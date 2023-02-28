using IKVM.Runtime.Vfs;

namespace IKVM.Java.Externs.sun.nio.fs
{

    /// <summary>
    /// Implements the native methods for 'DotNetUnixUriUtils'.
    /// </summary>
    static class DotNetUnixUriUtils
    {

        /// <summary>
        /// Implements the native method 'isVfsDirectory'.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool isVfsDirectory(string path)
        {
            return VfsTable.Default.GetEntry(path) is VfsDirectory;
        }

    }

}
