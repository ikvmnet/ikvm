/*
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

    /// <summary>
    /// The in-memory manifest module of an <see cref="AssemblyBuilder"/>. It holds no types and stands in as the
    /// assembly's manifest module, mirroring the <c>RefEmit_InMemoryManifestModule</c> module of System.Reflection.Emit.
    /// </summary>
    sealed class ManifestModule : NonPEModule
    {

        readonly AssemblyBuilder _assembly;
        readonly Guid _guid = Guid.NewGuid();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly">The assembly builder whose manifest module this is.</param>
        internal ManifestModule(AssemblyBuilder assembly) :
            base(assembly.Universe)
        {
            _assembly = assembly;
        }

        /// <summary>
        /// Gets the metadata stream version of the owning assembly builder.
        /// </summary>
        public override int MDStreamVersion => _assembly.MdStreamVersion;

        /// <inheritdoc />
        public override Assembly Assembly => _assembly;

        /// <summary>
        /// Finds a type by name. The manifest module contains no types, so this always returns <see langword="null"/>.
        /// </summary>
        /// <param name="typeName">The name of the type to find.</param>
        /// <returns><see langword="null"/>.</returns>
        internal override Type FindType(TypeName typeName) => null;

        /// <summary>
        /// Finds a type by name, ignoring case. The manifest module contains no types, so this always returns
        /// <see langword="null"/>.
        /// </summary>
        /// <param name="lowerCaseName">The lower-cased name of the type to find.</param>
        /// <returns><see langword="null"/>.</returns>
        internal override Type FindTypeIgnoreCase(TypeName lowerCaseName) => null;

        /// <summary>
        /// Adds the types defined in the module to a list. The manifest module contains no types, so this adds nothing.
        /// </summary>
        /// <param name="list">The list to add the types to.</param>
        internal override void GetTypesImpl(List<Type> list)
        {
        }

        /// <summary>
        /// Gets a path for the module, formed from the assembly builder's directory and the name
        /// <c>RefEmit_InMemoryManifestModule</c>.
        /// </summary>
        public override string FullyQualifiedName => Path.Combine(_assembly.Dir, "RefEmit_InMemoryManifestModule");

        /// <summary>
        /// Gets the name of the module, which is always <c>&lt;In Memory Module&gt;</c>.
        /// </summary>
        public override string Name => "<In Memory Module>";

        /// <summary>
        /// Gets a version identifier for the module, generated randomly when the instance is created.
        /// </summary>
        public override Guid ModuleVersionId => _guid;

        /// <summary>
        /// Gets the scope name of the module, which is always <c>RefEmit_InMemoryManifestModule</c>.
        /// </summary>
        public override string ScopeName => "RefEmit_InMemoryManifestModule";

        /// <summary>
        /// Creates the exception thrown by operations this module does not support. Unlike the base class, the
        /// manifest module reports these as an invalid operation.
        /// </summary>
        /// <returns>A new <see cref="InvalidOperationException"/>.</returns>
        protected override Exception NotSupportedException() => new InvalidOperationException();

    }

}
