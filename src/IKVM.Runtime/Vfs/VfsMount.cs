/*
  Copyright (C) 2007-2011 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;

using IKVM.Runtime.Extensions;

namespace IKVM.Runtime.Vfs
{

    /// <summary>
    /// Implements a virtual file system available to Java libraries.
    /// </summary>
    internal partial class VfsMount
    {

        readonly string rootPath;
        readonly VfsDirectory root;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="root"></param>
        public VfsMount(string rootPath, VfsDirectory root)
        {
            if (rootPath is null)
                throw new ArgumentNullException(nameof(rootPath));
            if (root is null)
                throw new ArgumentNullException(nameof(root));

            this.rootPath = PathExtensions.EnsureEndingDirectorySeparator(rootPath);
            this.root = root;
        }

        /// <summary>
        /// Gets the path of this virtual file system.
        /// </summary>
        public string RootPath => rootPath;

        /// <summary>
        /// Gets the root directory of this virtual file system.
        /// </summary>
        public VfsDirectory Root => root;

        /// <summary>
        /// Returns <c>true</c> if the given path is within the virtual file system mount.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsPath(string path) => path.StartsWith(rootPath, StringComparison.Ordinal);

        /// <summary>
        /// Gets the entry for he given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public VfsEntry GetPath(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            var p = path.Split(PathExtensions.DirectorySeparatorChars, StringSplitOptions.RemoveEmptyEntries);
            var c = (VfsEntry)Root;
            for (int i = 0; i < p.Length; i++)
            {
                // can only recurse into directory
                if (c is VfsDirectory directory)
                {
                    // obtain next entry, but if null, simply return early
                    if ((c = directory.GetEntry(p[i])) == null)
                        return null;
                }
                else
                {
                    return null;
                }
            }

            // we're left with the last element
            return c;
        }

    }

}
