﻿using System;

using IKVM.Runtime;
using IKVM.Runtime.Vfs;

namespace IKVM.Java.Externs.sun.nio.fs
{

    /// <summary>
    /// Implements the native methods for 'DotNetWindowsUriSupport'.
    /// </summary>
    static class DotNetWindowsUriSupport
    {

        /// <summary>
        /// Implements the native method 'isVfsDirectory'.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool isVfsDirectory(string path)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return JVM.Vfs.GetEntry(path) is VfsDirectory;
#endif
        }

    }

}
