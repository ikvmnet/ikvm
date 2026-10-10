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

        readonly Assembly _assembly;
        readonly int _index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="index"></param>
        internal MissingModule(Assembly assembly, int index) :
            base(assembly.Universe)
        {
            _assembly = assembly;
            _index = index;
        }

        public override int MDStreamVersion => throw new MissingModuleException(this);

        public override Assembly Assembly => _assembly;

        public override string FullyQualifiedName => throw new MissingModuleException(this);

        public override string Name
        {
            get
            {
                if (_index == -1)
                    throw new MissingModuleException(this);

                return ((Reader.ModuleReader)_assembly.ManifestModule).GetFileName(_index);
            }
        }

        public override Guid ModuleVersionId => throw new MissingModuleException(this);

        public override string ScopeName => throw new MissingModuleException(this);

        internal override Type FindType(TypeName typeName) => null;

        internal override Type FindTypeIgnoreCase(TypeName lowerCaseName) => null;

        internal override void GetTypesImpl(List<Type> list) => throw new MissingModuleException(this);

        internal override void ExportTypes(AssemblyFileHandle handle, IKVM.Reflection.Emit.ModuleBuilder manifestModule) =>
            throw new MissingModuleException(this);

        public override bool __IsMissing => true;

        protected override Exception InvalidOperationException() => new MissingModuleException(this);

        protected override Exception NotSupportedException() => new MissingModuleException(this);

        protected override Exception ArgumentOutOfRangeException() => new MissingModuleException(this);

    }

}
