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

    /// <summary>
    /// Stands in for a module that could not be loaded, either the manifest module of a missing assembly or a module
    /// file of an assembly that was not found, when <see cref="Universe.MissingMemberResolution"/> is enabled. Most
    /// members throw <see cref="MissingModuleException"/>.
    /// </summary>
    sealed class MissingModule : NonPEModule
    {

        readonly Assembly _assembly;

        /// <summary>
        /// The index of the module in the manifest's File table, or -1 for the manifest module of a missing assembly.
        /// </summary>
        readonly int _index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly">The assembly the module belongs to.</param>
        /// <param name="index">The index of the module in the manifest's File table, or -1 for the manifest module of a missing assembly.</param>
        internal MissingModule(Assembly assembly, int index) :
            base(assembly.Universe)
        {
            _assembly = assembly;
            _index = index;
        }

        /// <summary>
        /// Not available for a missing module.
        /// </summary>
        /// <exception cref="MissingModuleException">Always thrown.</exception>
        public override int MDStreamVersion => throw new MissingModuleException(this);

        /// <inheritdoc />
        public override Assembly Assembly => _assembly;

        /// <summary>
        /// Not available for a missing module.
        /// </summary>
        /// <exception cref="MissingModuleException">Always thrown.</exception>
        public override string FullyQualifiedName => throw new MissingModuleException(this);

        /// <summary>
        /// Gets the file name of the module, as listed in the manifest's File table.
        /// </summary>
        /// <exception cref="MissingModuleException">The module is the manifest module of a missing assembly, whose name is unknown.</exception>
        public override string Name
        {
            get
            {
                if (_index == -1)
                    throw new MissingModuleException(this);

                return ((Reader.ModuleReader)_assembly.ManifestModule).GetFileName(_index);
            }
        }

        /// <summary>
        /// Not available for a missing module.
        /// </summary>
        /// <exception cref="MissingModuleException">Always thrown.</exception>
        public override Guid ModuleVersionId => throw new MissingModuleException(this);

        /// <summary>
        /// Not available for a missing module.
        /// </summary>
        /// <exception cref="MissingModuleException">Always thrown.</exception>
        public override string ScopeName => throw new MissingModuleException(this);

        /// <summary>
        /// Finds no type, because the contents of a missing module are unknown.
        /// </summary>
        /// <param name="typeName">The name of the type to find.</param>
        /// <returns>Always <see langword="null"/>.</returns>
        internal override Type FindType(TypeName typeName) => null;

        /// <summary>
        /// Finds no type, because the contents of a missing module are unknown.
        /// </summary>
        /// <param name="lowerCaseName">The lower-case name of the type to find.</param>
        /// <returns>Always <see langword="null"/>.</returns>
        internal override Type FindTypeIgnoreCase(TypeName lowerCaseName) => null;

        /// <summary>
        /// Not available for a missing module.
        /// </summary>
        /// <param name="list">The list the types would be added to.</param>
        /// <exception cref="MissingModuleException">Always thrown.</exception>
        internal override void GetTypesImpl(List<Type> list) => throw new MissingModuleException(this);

        /// <summary>
        /// Gets a value indicating whether the module is missing, which is always <see langword="true"/>.
        /// </summary>
        public override bool __IsMissing => true;

        /// <summary>
        /// Creates the exception thrown where the base class would throw <see cref="System.InvalidOperationException"/>.
        /// </summary>
        /// <returns>A <see cref="MissingModuleException"/> for this module.</returns>
        protected override Exception InvalidOperationException() => new MissingModuleException(this);

        /// <summary>
        /// Creates the exception thrown where the base class would throw <see cref="System.NotSupportedException"/>.
        /// </summary>
        /// <returns>A <see cref="MissingModuleException"/> for this module.</returns>
        protected override Exception NotSupportedException() => new MissingModuleException(this);

        /// <summary>
        /// Creates the exception thrown where the base class would throw
        /// <see cref="System.ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <returns>A <see cref="MissingModuleException"/> for this module.</returns>
        protected override Exception ArgumentOutOfRangeException() => new MissingModuleException(this);

    }

}
