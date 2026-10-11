/*
  Copyright (C) 2009-2012 Jeroen Frijters

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
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{

    internal sealed class ManifestResourceInfo
    {

        readonly ModuleReader module;
        readonly EntityHandle implementation;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="implementation">The Implementation of the ManifestResource row, nil for a resource embedded in the module.</param>
        internal ManifestResourceInfo(ModuleReader module, EntityHandle implementation)
        {
            this.module = module;
            this.implementation = implementation;
        }

        public Assembly ReferencedAssembly => implementation.Kind == HandleKind.AssemblyReference && implementation.IsNil == false ? module.ResolveAssemblyRef(MetadataTokens.GetRowNumber(implementation) - 1) : null;

        public string FileName => implementation.Kind == HandleKind.AssemblyFile && implementation.IsNil == false ? module.GetFileName(MetadataTokens.GetRowNumber(implementation) - 1) : null;

    }

}
