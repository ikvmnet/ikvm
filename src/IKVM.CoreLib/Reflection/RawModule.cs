/*
  Copyright (C) 2009-2013 Jeroen Frijters

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

using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{

    /// <summary>
    /// A module that has been read from a file or stream but not yet loaded into its <see cref="Universe"/>. It allows
    /// inspecting the assembly identity and references before deciding whether to load it with
    /// <see cref="Universe.LoadAssembly(RawModule)"/>.
    /// </summary>
    sealed class RawModule : IDisposable
    {

        readonly ModuleReader _module;
        readonly bool _isManifestModule;
        bool _imported;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module">The module that has been read.</param>
        internal RawModule(ModuleReader module)
        {
            _module = module;
            _isManifestModule = module.Assembly != null;
        }

        /// <summary>
        /// Gets the fully qualified path of the module.
        /// </summary>
        public string Location => _module.FullyQualifiedName;

        /// <summary>
        /// Gets whether the module contains an assembly manifest.
        /// </summary>
        public bool IsManifestModule => _isManifestModule;

        /// <summary>
        /// Throws if the module does not contain an assembly manifest.
        /// </summary>
        /// <exception cref="BadImageFormatException">The module does not contain a manifest.</exception>
        void CheckManifestModule()
        {
            if (!IsManifestModule)
                throw new BadImageFormatException("Module does not contain a manifest");
        }

        /// <summary>
        /// Gets the name of the assembly whose manifest the module contains.
        /// </summary>
        /// <returns>The assembly name.</returns>
        /// <exception cref="BadImageFormatException">The module does not contain a manifest.</exception>
        public AssemblyName GetAssemblyName()
        {
            CheckManifestModule();
            return _module.Assembly.GetName();
        }

        /// <summary>
        /// Gets the names of the assemblies the module references.
        /// </summary>
        /// <returns>The referenced assembly names.</returns>
        public AssemblyName[] GetReferencedAssemblies() => _module.__GetReferencedAssemblies();

        /// <summary>
        /// Releases the underlying module, unless it has been loaded as an assembly with <see cref="ToAssembly"/>.
        /// </summary>
        public void Dispose()
        {
            if (!_imported)
                _module.Dispose();
        }

        /// <summary>
        /// Hands the module over to be loaded as an assembly. After this call, disposing the raw module no longer
        /// releases the underlying module.
        /// </summary>
        /// <returns>The assembly the module is the manifest module of.</returns>
        /// <exception cref="InvalidOperationException">The module has already been loaded.</exception>
        internal AssemblyReader ToAssembly()
        {
            if (_imported)
                throw new InvalidOperationException();

            _imported = true;
            return (AssemblyReader)_module.Assembly;
        }

    }

}
