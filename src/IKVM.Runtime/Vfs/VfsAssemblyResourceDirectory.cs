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
using System.Reflection;

namespace IKVM.Runtime.Vfs
{

    /// <summary>
    /// Represents a virtual directory for assembly resources.
    /// </summary>
    sealed class VfsAssemblyResourceDirectory : VfsDirectory
    {

        readonly Assembly assembly;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        internal VfsAssemblyResourceDirectory(VfsContext context, Assembly assembly) :
            base(context)
        {
            this.assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        /// <summary>
        /// Gets the resource entry in the directory.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override VfsEntry GetEntry(string name)
        {
            return assembly.GetManifestResourceInfo(name) != null ? new VfsAssemblyResourceFile(Context, assembly, name) : null;
        }

        /// <summary>
        /// Gets the list of resource entries within the directory.
        /// </summary>
        /// <returns></returns>
        public override string[] List()
        {
            return assembly.GetManifestResourceNames();
        }

    }

}
