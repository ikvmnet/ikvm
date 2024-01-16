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
using IKVM.Reflection.Metadata;
using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{

    public sealed class ManifestResourceInfo
    {

        readonly ModuleReader module;
        readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="index"></param>
        internal ManifestResourceInfo(ModuleReader module, int index)
        {
            this.module = module;
            this.index = index;
        }

        public ResourceAttributes __ResourceAttributes
        {
            get { return (ResourceAttributes)module.ManifestResourceTable.records[index].Flags; }
        }

        public int __Offset
        {
            get { return module.ManifestResourceTable.records[index].Offset; }
        }

        public ResourceLocation ResourceLocation
        {
            get
            {
                var implementation = module.ManifestResourceTable.records[index].Implementation;
                if ((implementation >> 24) == AssemblyRefTable.Index)
                {
                    var asm = ReferencedAssembly;
                    if (asm == null || asm.__IsMissing)
                        return ResourceLocation.ContainedInAnotherAssembly;

                    return asm.GetManifestResourceInfo(module.GetString(module.ManifestResourceTable.records[index].Name)).ResourceLocation | ResourceLocation.ContainedInAnotherAssembly;
                }
                else if ((implementation >> 24) == FileTable.Index)
                {
                    if ((implementation & 0xFFFFFF) == 0)
                        return ResourceLocation.ContainedInManifestFile | ResourceLocation.Embedded;

                    return 0;
                }
                else
                {
                    throw new BadImageFormatException();
                }
            }
        }

        public Assembly ReferencedAssembly
        {
            get
            {
                var implementation = module.ManifestResourceTable.records[index].Implementation;
                if ((implementation >> 24) == AssemblyRefTable.Index)
                    return module.ResolveAssemblyRef((implementation & 0xFFFFFF) - 1);

                return null;
            }
        }

        public string FileName
        {
            get
            {
                var implementation = module.ManifestResourceTable.records[index].Implementation;
                if ((implementation >> 24) == FileTable.Index)
                {
                    if ((implementation & 0xFFFFFF) == 0)
                        return null;
                    else
                        return module.GetString(module.FileTable.records[(implementation & 0xFFFFFF) - 1].Name);
                }

                return null;
            }
        }

    }

}
