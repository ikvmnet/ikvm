/*
  Copyright (C) 2009 Jeroen Frijters

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

namespace IKVM.Reflection.Reader
{

    /// <summary>
    /// Represents an assembly read from metadata, consisting of a manifest module and any other modules listed in its
    /// File table.
    /// </summary>
    sealed class AssemblyReader : Assembly
    {

        readonly string _location;
        readonly ModuleReader _manifestModule;

        /// <summary>
        /// The modules listed in the manifest's File table, indexed by row, loaded on demand.
        /// </summary>
        readonly Module[] _externalModules;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="location">The path the assembly was loaded from, or <see langword="null"/> if it was not loaded from a file.</param>
        /// <param name="manifestModule">The module that contains the assembly manifest.</param>
        internal AssemblyReader(string location, ModuleReader manifestModule) :
            base(manifestModule.Universe)
        {
            _location = location;
            _manifestModule = manifestModule;
            _externalModules = new Module[manifestModule.FileCount];
        }

        /// <summary>
        /// Gets the full path of the file that contains the manifest, or an empty string if the assembly was not loaded
        /// from a file.
        /// </summary>
        public override string Location => _location ?? "";

        /// <summary>
        /// Gets an <see cref="AssemblyName"/> for this assembly, built from the manifest's Assembly table row and the
        /// image's PE kind and machine.
        /// </summary>
        /// <returns>An object that contains the fully parsed display name for this assembly.</returns>
        public override AssemblyName GetName()
        {
            var definition = _manifestModule.Metadata.GetAssemblyDefinition();
            var flags = (int)definition.Flags;

            var name = new AssemblyName
            {
                Name = _manifestModule.GetString(definition.Name),
                Version = definition.Version
            };
            name.SetPublicKey(definition.PublicKey.IsNil == false ? _manifestModule.GetBlobCopy(definition.PublicKey) : Array.Empty<byte>());
            name.CultureName = definition.Culture.IsNil == false ? _manifestModule.GetString(definition.Culture) : "";
            name.HashAlgorithm = (AssemblyHashAlgorithm)(int)definition.HashAlgorithm;
            name.CodeBase = CodeBase;

            _manifestModule.GetPEKind(out var peKind, out var machine);

            switch (machine)
            {
                case ImageFileMachine.I386:
                    // FXBUG we copy the .NET bug that Preferred32Bit implies x86
                    if ((peKind & (PortableExecutableKinds.Required32Bit | PortableExecutableKinds.Preferred32Bit)) != 0)
                        name.ProcessorArchitecture = ProcessorArchitecture.X86;
                    // it's a reference assembly
                    else if ((flags & 0x70) == 0x70)
                        name.ProcessorArchitecture = ProcessorArchitecture.None;
                    else
                        name.ProcessorArchitecture = ProcessorArchitecture.MSIL;
                    break;
                case ImageFileMachine.IA64:
                    name.ProcessorArchitecture = ProcessorArchitecture.IA64;
                    break;
                case ImageFileMachine.AMD64:
                    name.ProcessorArchitecture = ProcessorArchitecture.Amd64;
                    break;
                case ImageFileMachine.ARM:
                    name.ProcessorArchitecture = ProcessorArchitecture.Arm;
                    break;
                case ImageFileMachine.ARM64:
                    name.ProcessorArchitecture = ProcessorArchitecture.Arm64;
                    break;
            }

            name.RawFlags = (AssemblyNameFlags)flags;
            return name;
        }

        /// <inheritdoc />
        public override Type[] GetTypes()
        {
            if (_externalModules.Length == 0)
                return _manifestModule.GetTypes();

            var list = new List<Type>();
            foreach (var module in GetModules(false))
                list.AddRange(module.GetTypes());

            return list.ToArray();
        }

        /// <summary>
        /// Finds a type by name in the manifest module, then in each other module that contains metadata.
        /// </summary>
        /// <param name="typeName">The name of the type to find.</param>
        /// <returns>The type, or <see langword="null"/> if no module defines it.</returns>
        internal override Type FindType(TypeName typeName)
        {
            var type = _manifestModule.FindType(typeName);
            for (var i = 0; type == null && i < _externalModules.Length; i++)
                if (_manifestModule.FileContainsMetadata(i))
                    type = GetModule(i).FindType(typeName);

            return type;
        }

        /// <summary>
        /// Finds a type by case-insensitive name in the manifest module, then in each other module that contains
        /// metadata.
        /// </summary>
        /// <param name="lowerCaseName">The lower-case name of the type to find.</param>
        /// <returns>The type, or <see langword="null"/> if no module defines it.</returns>
        internal override Type FindTypeIgnoreCase(TypeName lowerCaseName)
        {
            var type = _manifestModule.FindTypeIgnoreCase(lowerCaseName);
            for (var i = 0; type == null && i < _externalModules.Length; i++)
                if (_manifestModule.FileContainsMetadata(i))
                    type = GetModule(i).FindTypeIgnoreCase(lowerCaseName);

            return type;
        }

        /// <inheritdoc />
        public override string ImageRuntimeVersion => _manifestModule.__ImageRuntimeVersion;

        /// <inheritdoc />
        public override Module ManifestModule => _manifestModule;

        /// <summary>
        /// Gets all the loaded modules that are part of this assembly: the manifest module and any other modules that
        /// have been loaded so far.
        /// </summary>
        /// <param name="getResourceModules">Ignored; loaded resource modules are always included.</param>
        /// <returns>An array of modules.</returns>
        public override Module[] GetLoadedModules(bool getResourceModules)
        {
            List<Module> list = [_manifestModule];
            foreach (Module m in _externalModules)
            {
                if (m != null)
                {
                    list.Add(m);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// Gets all the modules that are part of this assembly, loading any that have not been loaded yet.
        /// </summary>
        /// <param name="getResourceModules"><see langword="true"/> to include resource modules; otherwise, <see langword="false"/>.</param>
        /// <returns>An array of modules.</returns>
        public override Module[] GetModules(bool getResourceModules)
        {
            if (_externalModules.Length == 0)
            {
                return new Module[] { _manifestModule };
            }
            else
            {
                List<Module> list = [_manifestModule];
                for (var i = 0; i < _manifestModule.FileCount; i++)
                {
                    if (getResourceModules || _manifestModule.FileContainsMetadata(i))
                    {
                        list.Add(GetModule(i));
                    }
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// Gets the specified module in this assembly.
        /// </summary>
        /// <param name="name">The name of the module requested, compared case-insensitively.</param>
        /// <returns>The module being requested, or <see langword="null"/> if the module is not found.</returns>
        public override Module GetModule(string name)
        {
            if (name.Equals(_manifestModule.ScopeName, StringComparison.OrdinalIgnoreCase))
                return _manifestModule;

            var index = GetModuleIndex(name);
            if (index != -1)
                return GetModule(index);

            return null;
        }

        /// <summary>
        /// Finds the File table row of a module by file name.
        /// </summary>
        /// <param name="name">The file name of the module, compared case-insensitively.</param>
        /// <returns>The zero-based index of the module in the File table, or -1 if it is not listed.</returns>
        int GetModuleIndex(string name)
        {
            for (var i = 0; i < _manifestModule.FileCount; i++)
                if (name.Equals(_manifestModule.GetFileName(i), StringComparison.OrdinalIgnoreCase))
                    return i;

            return -1;
        }

        /// <summary>
        /// Gets the module listed at a File table index, loading it on first use.
        /// </summary>
        /// <param name="index">The zero-based index of the module in the File table.</param>
        /// <returns>The module.</returns>
        Module GetModule(int index)
        {
            if (_externalModules[index] != null)
                return _externalModules[index];

            return LoadModule(index, null, _manifestModule.GetFileName(index));
        }

        /// <summary>
        /// Loads the module listed at a File table index. A file without metadata becomes a
        /// <see cref="ResourceModule"/>. A module file that cannot be found is offered to the module resolvers, then
        /// becomes a <see cref="MissingModule"/> if the universe allows missing members.
        /// </summary>
        /// <param name="index">The zero-based index of the module in the File table.</param>
        /// <param name="rawModule">The contents of the module, or <see langword="null"/> to read it from the file next to the manifest.</param>
        /// <param name="name">The file name of the module, or <see langword="null"/> if it has no location.</param>
        /// <returns>The loaded module.</returns>
        /// <exception cref="FileNotFoundException">The module file does not exist and neither a resolver nor missing member resolution supplied a module.</exception>
        Module LoadModule(int index, byte[] rawModule, string name)
        {
            var location = name == null ? null : Path.Combine(Path.GetDirectoryName(_location), name);
            if (_manifestModule.FileContainsMetadata(index) == false)
            {
                return _externalModules[index] = new ResourceModule(_manifestModule, index, location);
            }
            else
            {
                if (rawModule == null)
                {
                    try
                    {
                        rawModule = File.ReadAllBytes(location);
                    }
                    catch (FileNotFoundException)
                    {
                        if (resolvers != null)
                        {
                            var arg = new ResolveEventArgs(name, this);
                            foreach (var resolver in resolvers)
                            {
                                var module = resolver(this, arg);
                                if (module != null)
                                    return module;
                            }
                        }

                        if (Universe.MissingMemberResolution)
                            return _externalModules[index] = new MissingModule(this, index);

                        throw;
                    }
                }
                return _externalModules[index] = new ModuleReader(this, _manifestModule.Universe, new MemoryStream(rawModule), location, false);
            }
        }

        /// <inheritdoc />
        public override MethodInfo EntryPoint => _manifestModule.GetEntryPoint();

        /// <inheritdoc />
        public override ManifestResourceInfo GetManifestResourceInfo(string resourceName) => _manifestModule.GetManifestResourceInfo(resourceName);

        /// <inheritdoc />
        public override Stream GetManifestResourceStream(string resourceName) => _manifestModule.GetManifestResourceStream(resourceName);

        /// <inheritdoc />
        public override AssemblyName[] GetReferencedAssemblies() => _manifestModule.__GetReferencedAssemblies();

        /// <summary>
        /// Gets the simple name of the assembly from the manifest's Assembly table row, without building a full
        /// <see cref="AssemblyName"/>.
        /// </summary>
        internal string Name => _manifestModule.GetString(_manifestModule.Metadata.GetAssemblyDefinition().Name);

        /// <summary>
        /// Gets the custom attributes applied to the assembly, which are those whose parent is the Assembly table row
        /// (token 0x20000001).
        /// </summary>
        /// <param name="attributeType">The attribute type to filter by, or <see langword="null"/> for all attributes.</param>
        /// <returns>The matching custom attributes.</returns>
        internal override IList<CustomAttributeData> GetCustomAttributesData(Type attributeType) =>
            CustomAttributeData.GetCustomAttributesImpl(null, _manifestModule, 0x20000001, attributeType) ?? CustomAttributeData.EmptyList;

    }

}
