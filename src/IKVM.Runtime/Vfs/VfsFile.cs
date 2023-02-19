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
using System.IO;

namespace IKVM.Runtime.Vfs
{

    /// <summary>
    /// Represents a file within the virtual file system.
    /// </summary>
    internal abstract class VfsFile : VfsEntry
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public VfsFile(VfsContext context) :
            base(context)
        {

        }

        /// <summary>
        /// Gets the size of the file.
        /// </summary>
        public abstract long Size { get; }

        /// <summary>
        /// Returns <c>true</c> if the given entry can be opened.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="access"></param>
        /// <returns></returns>
        public virtual bool CanOpen(FileMode mode, FileAccess access)
        {
            return mode == FileMode.Open && access == FileAccess.Read;
        }

        /// <summary>
        /// Attempts to open the the entry.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="access"></param>
        /// <returns></returns>
        public virtual Stream Open(FileMode mode, FileAccess access)
        {
            return CanOpen(mode, access) ? OpenRead() : throw new UnauthorizedAccessException("Access to the file was denied. The IKVM VFS currently only support read operations.");
        }

        /// <summary>
        /// Opens the file for reading.
        /// </summary>
        /// <returns></returns>
        protected virtual Stream OpenRead()
        {
            throw new NotSupportedException();
        }

    }

}
