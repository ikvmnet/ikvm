﻿/*
  Copyright (C) 2008-2013 Jeroen Frijters

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
using System.IO;

namespace IKVM.Reflection.Emit
{

    sealed class ManifestModule : NonPEModule
    {

        readonly AssemblyBuilder assembly;
        readonly Guid guid = Guid.NewGuid();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        internal ManifestModule(AssemblyBuilder assembly) :
            base(assembly.universe)
        {
            this.assembly = assembly;
        }

        public override int MDStreamVersion => assembly.mdStreamVersion;

        public override Assembly Assembly => assembly;

        internal override Type FindType(TypeName typeName) => null;

        internal override Type FindTypeIgnoreCase(TypeName lowerCaseName) => null;

        internal override void GetTypesImpl(List<Type> list)
        {

        }

        public override string FullyQualifiedName => Path.Combine(assembly.dir, "RefEmit_InMemoryManifestModule");

        public override string Name => "<In Memory Module>";

        public override Guid ModuleVersionId => guid;

        public override string ScopeName => "RefEmit_InMemoryManifestModule";

        protected override Exception NotSupportedException() => new InvalidOperationException();

    }

}
