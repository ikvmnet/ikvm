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
using System.Reflection;

namespace IKVM.Runtime.Vfs
{

    /// <summary>
    /// Represents an assembly resource file within the virtual file system.
    /// </summary>
    internal sealed class VfsAssemblyResourceFile : VfsFile
    {

        readonly Assembly asm;
        readonly string name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        /// <param name="name"></param>
        internal VfsAssemblyResourceFile(VfsContext context, Assembly assembly, string name) :
            base(context)
        {
            this.asm = assembly ?? throw new ArgumentNullException(nameof(assembly));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Opens the assembly resource for reading.
        /// </summary>
        /// <returns></returns>
        protected override Stream OpenRead()
        {
            return asm.GetManifestResourceStream(name);
        }

        /// <summary>
        /// Gets the size of the assembly resource.
        /// </summary>
        public override long Size
        {
            get
            {
                using var stream = OpenRead();
                return stream.Length;
            }
        }

    }

}
