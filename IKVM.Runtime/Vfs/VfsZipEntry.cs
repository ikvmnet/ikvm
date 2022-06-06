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
using System.IO.Compression;

namespace IKVM.Runtime.Vfs
{

    /// <summary>
    /// Describes a file within the virtual file system which is backed by an entry in a zip file.
    /// </summary>
    sealed class VfsZipEntry : VfsFile
    {

        readonly ZipArchiveEntry entry;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entry"></param>
        public VfsZipEntry(VfsContext context, ZipArchiveEntry entry) :
            base(context)
        {
            this.entry = entry ?? throw new ArgumentNullException(nameof(entry));
        }

        /// <summary>
        /// Gets the size of the entry.
        /// </summary>
        public override long Size => entry.Length;

        /// <summary>
        /// Opens the entry for reading.
        /// </summary>
        /// <returns></returns>
        protected override Stream OpenRead()
        {
            // copy the contents to a memory stream to allow seeking and length
            using var s = entry.Open();
            var m = new MemoryStream((int)entry.Length);
            s.CopyTo(m);
            m.Position = 0;
            return m;
        }

    }

}
