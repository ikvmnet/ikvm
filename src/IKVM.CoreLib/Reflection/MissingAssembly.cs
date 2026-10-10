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
using System.Collections.Generic;

namespace IKVM.Reflection
{

    /// <summary>
    /// Represents an assembly that could not be resolved, created when missing member resolution is enabled on the
    /// universe. It has a name and a placeholder manifest module; members that would need the assembly's contents
    /// throw <see cref="MissingAssemblyException"/>.
    /// </summary>
    sealed class MissingAssembly : Assembly
    {

        readonly MissingModule _module;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="universe">The universe the assembly belongs to.</param>
        /// <param name="name">The full name of the missing assembly.</param>
        internal MissingAssembly(Universe universe, string name) :
            base(universe)
        {
            _module = new MissingModule(this, -1);
            fullName = name;
        }

        /// <inheritdoc />
        /// <exception cref="MissingAssemblyException">Always thrown.</exception>
        public override Type[] GetTypes() => throw new MissingAssemblyException(this);

        /// <summary>
        /// Gets an <see cref="AssemblyName"/> parsed from the name the missing assembly was referenced by.
        /// </summary>
        /// <returns>The name of the missing assembly.</returns>
        public override AssemblyName GetName() => new AssemblyName(fullName);

        /// <inheritdoc />
        /// <exception cref="MissingAssemblyException">Always thrown.</exception>
        public override string ImageRuntimeVersion => throw new MissingAssemblyException(this);

        /// <summary>
        /// Gets the placeholder module that stands in for the missing assembly's manifest module.
        /// </summary>
        public override Module ManifestModule => _module;

        /// <inheritdoc />
        /// <exception cref="MissingAssemblyException">Always thrown.</exception>
        public override MethodInfo EntryPoint => throw new MissingAssemblyException(this);

        /// <inheritdoc />
        /// <exception cref="MissingAssemblyException">Always thrown.</exception>
        public override string Location => throw new MissingAssemblyException(this);

        /// <inheritdoc />
        /// <exception cref="MissingAssemblyException">Always thrown.</exception>
        public override AssemblyName[] GetReferencedAssemblies() => throw new MissingAssemblyException(this);

        /// <inheritdoc />
        /// <exception cref="MissingAssemblyException">Always thrown.</exception>
        public override Module[] GetModules(bool getResourceModules) => throw new MissingAssemblyException(this);

        /// <inheritdoc />
        /// <exception cref="MissingAssemblyException">Always thrown.</exception>
        public override Module[] GetLoadedModules(bool getResourceModules) => throw new MissingAssemblyException(this);

        /// <inheritdoc />
        /// <exception cref="MissingAssemblyException">Always thrown.</exception>
        public override Module GetModule(string name) => throw new MissingAssemblyException(this);

        /// <inheritdoc />
        /// <exception cref="MissingAssemblyException">Always thrown.</exception>
        public override ManifestResourceInfo GetManifestResourceInfo(string resourceName) => throw new MissingAssemblyException(this);

        /// <inheritdoc />
        /// <exception cref="MissingAssemblyException">Always thrown.</exception>
        public override System.IO.Stream GetManifestResourceStream(string resourceName) => throw new MissingAssemblyException(this);

        /// <summary>
        /// Gets whether the assembly is missing, which is always the case.
        /// </summary>
        public override bool __IsMissing => true;

        /// <summary>
        /// Finds a type by name. A missing assembly contains no types, so this always returns <see langword="null"/>.
        /// </summary>
        /// <param name="typeName">The name of the type.</param>
        /// <returns>Always <see langword="null"/>.</returns>
        internal override Type FindType(TypeName typeName) => null;

        /// <summary>
        /// Finds a type by name, ignoring case. A missing assembly contains no types, so this always returns
        /// <see langword="null"/>.
        /// </summary>
        /// <param name="lowerCaseName">The lower-case name of the type.</param>
        /// <returns>Always <see langword="null"/>.</returns>
        internal override Type FindTypeIgnoreCase(TypeName lowerCaseName) => null;

        /// <inheritdoc />
        /// <exception cref="MissingAssemblyException">Always thrown.</exception>
        internal override IList<CustomAttributeData> GetCustomAttributesData(Type attributeType) => throw new MissingAssemblyException(this);

    }

}
