/*
  Copyright (C) 2011-2012 Jeroen Frijters

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
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace IKVM.Reflection
{

    sealed class MissingModule : NonPEModule
    {

        readonly Assembly assembly;
        readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="index"></param>
        internal MissingModule(Assembly assembly, int index) :
            base(assembly.Universe)
        {
            this.assembly = assembly;
            this.index = index;
        }

        public override int MDStreamVersion
        {
            get { throw new MissingModuleException(this); }
        }

        public override Assembly Assembly
        {
            get { return assembly; }
        }

        public override string FullyQualifiedName
        {
            get { throw new MissingModuleException(this); }
        }

        public override string Name
        {
            get
            {
                if (index == -1)
                    throw new MissingModuleException(this);

                return assembly.ManifestModule.GetString(assembly.ManifestModule.FileTable.records[index].Name);
            }
        }

        public override Guid ModuleVersionId
        {
            get { throw new MissingModuleException(this); }
        }

        public override string ScopeName
        {
            get { throw new MissingModuleException(this); }
        }

        internal override Type FindType(TypeName typeName)
        {
            return null;
        }

        internal override Type FindTypeIgnoreCase(TypeName lowerCaseName)
        {
            return null;
        }

        internal override void GetTypesImpl(List<Type> list)
        {
            throw new MissingModuleException(this);
        }

        public override void __GetDataDirectoryEntry(int index, out int rva, out int length)
        {
            throw new MissingModuleException(this);
        }

        public override IList<CustomAttributeData> __GetPlaceholderAssemblyCustomAttributes(bool multiple, bool security)
        {
            throw new MissingModuleException(this);
        }

        public override long __RelativeVirtualAddressToFileOffset(int rva)
        {
            throw new MissingModuleException(this);
        }

        public override __StandAloneMethodSig __ResolveStandAloneMethodSig(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            throw new MissingModuleException(this);
        }

        public override int __Subsystem
        {
            get { throw new MissingModuleException(this); }
        }

        internal override void ExportTypes(AssemblyFileHandle handle, IKVM.Reflection.Emit.ModuleBuilder manifestModule)
        {
            throw new MissingModuleException(this);
        }

        public override void GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine)
        {
            throw new MissingModuleException(this);
        }

        public override bool __IsMissing
        {
            get { return true; }
        }

        protected override Exception InvalidOperationException()
        {
            return new MissingModuleException(this);
        }

        protected override Exception NotSupportedException()
        {
            return new MissingModuleException(this);
        }

        protected override Exception ArgumentOutOfRangeException()
        {
            return new MissingModuleException(this);
        }

        public override byte[] __ModuleHash
        {
            get
            {
                if (index == -1)
                    throw new MissingModuleException(this);
                if (assembly.ManifestModule.FileTable.records[index].HashValue.IsNil)
                    return null;

                var br = assembly.ManifestModule.GetBlobReader(assembly.ManifestModule.FileTable.records[index].HashValue);
                return br.ReadBytes(br.Length);
            }
        }

    }

}
