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
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Resources;
using System.Security.Cryptography;

using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Defines and represents a dynamic assembly.
    /// </summary>
    sealed class AssemblyBuilder : Assembly
    {

        readonly string _name;
        ushort _majorVersion;
        ushort _minorVersion;
        ushort _buildVersion;
        ushort _revisionVersion;
        string _culture;
        readonly AssemblyNameFlags _flags;
        readonly AssemblyHashAlgorithm _hashAlgorithm;
        readonly StrongNameKeyPair _keyPair;
        readonly byte[] _publicKey;

        /// <summary>
        /// The directory that the assembly's modules are written to and its resource files and added modules are read
        /// from.
        /// </summary>
        internal readonly string Dir;

        PEFileKinds _fileKind = PEFileKinds.Dll;
        MethodInfo _entryPoint;

        /// <summary>
        /// The Win32 version resource to generate, or <see langword="null"/> if
        /// <see cref="DefineVersionInfoResource"/> has not been called.
        /// </summary>
        VersionInfo _versionInfo;

        byte[] _win32icon;
        byte[] _win32manifest;
        readonly string _imageRuntimeVersion;

        /// <summary>
        /// The metadata stream version reported by the modules of this assembly.
        /// </summary>
        internal int MdStreamVersion = 0x20000;

        /// <summary>
        /// The placeholder module returned by <see cref="ManifestModule"/>, created on first access.
        /// </summary>
        Module _pseudoManifestModule;

        readonly List<ResourceFile> _resourceFiles = [];
        readonly List<ModuleBuilder> _modules = [];

        readonly List<CustomAttributeBuilder> _customAttributes = [];
        readonly List<TypeForwarder> _typeForwarders = [];

        /// <summary>
        /// Records a type forwarded to another assembly, to be written to the <c>ExportedType</c> table.
        /// </summary>
        readonly struct TypeForwarder
        {

            /// <summary>
            /// The forwarded type.
            /// </summary>
            internal readonly Type Type;

            /// <summary>
            /// Whether the nested types of <see cref="Type"/> are forwarded as well.
            /// </summary>
            internal readonly bool IncludeNested;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="type">The forwarded type.</param>
            /// <param name="includeNested"><see langword="true"/> to forward the nested types of <paramref name="type"/> as well.</param>
            internal TypeForwarder(Type type, bool includeNested)
            {
                Type = type;
                IncludeNested = includeNested;
            }

        }

        /// <summary>
        /// Records a file linked into the assembly as a manifest resource.
        /// </summary>
        struct ResourceFile
        {

            /// <summary>
            /// The logical name of the resource.
            /// </summary>
            internal string Name;

            /// <summary>
            /// The name of the file that contains the resource, relative to <see cref="Dir"/>.
            /// </summary>
            internal string FileName;

            /// <summary>
            /// The visibility of the resource.
            /// </summary>
            internal ResourceAttributes Attributes;

        }

        /// <summary>
        /// Initializes a new instance and registers it with the universe.
        /// </summary>
        /// <param name="universe">The universe that owns the assembly.</param>
        /// <param name="name">The name of the assembly, including its version, culture, flags, hash algorithm and key.</param>
        /// <param name="dir">The directory the assembly is saved to, or <see langword="null"/> for the current directory.</param>
        /// <param name="customAttributes">Custom attributes to apply to the assembly, or <see langword="null"/>.</param>
        internal AssemblyBuilder(Universe universe, AssemblyName name, string dir, IEnumerable<CustomAttributeBuilder> customAttributes) :
            base(universe)
        {
            _name = name.Name;
            SetVersionHelper(name.Version);
            if (!string.IsNullOrEmpty(name.CultureName))
            {
                _culture = name.CultureName;
            }

            _flags = name.RawFlags;
            _hashAlgorithm = name.HashAlgorithm;
            if (_hashAlgorithm == AssemblyHashAlgorithm.None)
                _hashAlgorithm = AssemblyHashAlgorithm.SHA1;

            _keyPair = name.KeyPair;
            if (_keyPair != null)
            {
                _publicKey = _keyPair.PublicKey;
            }
            else
            {
                var publicKey = name.GetPublicKey();
                if (publicKey != null && publicKey.Length != 0)
                    _publicKey = (byte[])publicKey.Clone();
            }

            Dir = dir ?? ".";
            if (customAttributes != null)
                _customAttributes.AddRange(customAttributes);

            if (universe.HasCoreLib && !universe.CoreLib.__IsMissing && universe.CoreLib.ImageRuntimeVersion != null)
                _imageRuntimeVersion = universe.CoreLib.ImageRuntimeVersion;
            else
                _imageRuntimeVersion = TypeUtil.GetAssembly(typeof(object)).ImageRuntimeVersion;

            universe.RegisterDynamicAssembly(this);
        }

        /// <summary>
        /// Stores the components of the version, treating undefined components as zero.
        /// </summary>
        /// <param name="version">The version, or <see langword="null"/> for 0.0.0.0.</param>
        void SetVersionHelper(Version version)
        {
            if (version == null)
            {
                _majorVersion = 0;
                _minorVersion = 0;
                _buildVersion = 0;
                _revisionVersion = 0;
            }
            else
            {
                _majorVersion = (ushort)version.Major;
                _minorVersion = (ushort)version.Minor;
                _buildVersion = version.Build == -1 ? (ushort)0 : (ushort)version.Build;
                _revisionVersion = version.Revision == -1 ? (ushort)0 : (ushort)version.Revision;
            }
        }

        /// <summary>
        /// Clears the cached full name and re-registers the assembly with the universe under its new name.
        /// </summary>
        void Rename()
        {
            fullName = null;
            Universe.RenameAssembly(this);
        }

        /// <summary>
        /// Changes the version of the assembly after it has been defined.
        /// </summary>
        /// <param name="version">The new version, or <see langword="null"/> for 0.0.0.0.</param>
        public void __SetAssemblyVersion(Version version)
        {
            SetVersionHelper(version);
            Rename();
        }

        /// <summary>
        /// Changes the culture of the assembly after it has been defined.
        /// </summary>
        /// <param name="cultureName">The name of the new culture, or <see langword="null"/> for the invariant culture.</param>
        public void __SetAssemblyCulture(string cultureName)
        {
            _culture = cultureName;
            Rename();
        }

        /// <summary>
        /// Gets the simple name of the assembly.
        /// </summary>
        internal string Name => _name;

        /// <inheritdoc />
        public override AssemblyName GetName()
        {
            AssemblyName n = new AssemblyName
            {
                Name = _name,
                Version = new Version(_majorVersion, _minorVersion, _buildVersion, _revisionVersion),
                CultureName = _culture ?? "",
                HashAlgorithm = _hashAlgorithm,
                RawFlags = _flags
            };
            n.SetPublicKey(_publicKey != null ? (byte[])_publicKey.Clone() : Array.Empty<byte>());
            n.KeyPair = _keyPair;
            return n;
        }

        /// <summary>
        /// Gets the location of the assembly. Not supported for dynamic assemblies.
        /// </summary>
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        public override string Location => throw new NotSupportedException();

        /// <summary>
        /// Defines a persistable dynamic module, specifying the module name, the name of the file to which the module
        /// will be saved, and whether symbol information should be emitted using the default symbol writer.
        /// </summary>
        /// <param name="name">The name of the dynamic module.</param>
        /// <param name="fileName">The name of the file to which the dynamic module should be saved.</param>
        /// <param name="emitSymbolInfo"><see langword="true"/> if symbolic information is to be emitted; otherwise, <see langword="false"/>.</param>
        /// <returns>A <see cref="ModuleBuilder"/> object representing the defined dynamic module.</returns>
        public ModuleBuilder DefineDynamicModule(string name, string fileName, bool emitSymbolInfo)
        {
            var module = new ModuleBuilder(this, name, fileName);
            module.SetSymWriter(emitSymbolInfo ? Universe.CreateSymbolWriter(module) : null);
            _modules.Add(module);
            return module;
        }

        /// <summary>
        /// Sets a custom attribute on this assembly using a custom attribute builder.
        /// </summary>
        /// <param name="customBuilder">An instance of a helper class to define the custom attribute.</param>
        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) => _customAttributes.Add(customBuilder);

        /// <summary>
        /// Adds a type forwarder for the type and all of its nested types.
        /// </summary>
        /// <param name="type">The type that has been moved to another assembly.</param>
        public void __AddTypeForwarder(Type type) => __AddTypeForwarder(type, true);

        /// <summary>
        /// Adds a type forwarder for the type, written to the <c>ExportedType</c> table of the manifest module.
        /// </summary>
        /// <param name="type">The type that has been moved to another assembly.</param>
        /// <param name="includeNested"><see langword="true"/> to forward the nested types of <paramref name="type"/> as well.</param>
        public void __AddTypeForwarder(Type type, bool includeNested) => _typeForwarders.Add(new TypeForwarder(type, includeNested));

        /// <summary>
        /// Sets the entry point for this assembly and defines the type of the portable executable (PE file) being built.
        /// </summary>
        /// <param name="entryMethod">A reference to the method that represents the entry point for this dynamic assembly.</param>
        /// <param name="fileKind">The type of the assembly executable being built.</param>
        public void SetEntryPoint(MethodInfo entryMethod, PEFileKinds fileKind)
        {
            _entryPoint = entryMethod;
            _fileKind = fileKind;
        }

        /// <summary>
        /// Saves this dynamic assembly to disk.
        /// </summary>
        /// <param name="assemblyFileName">The file name of the assembly.</param>
        public void Save(string assemblyFileName) => Save(assemblyFileName, PortableExecutableKinds.ILOnly, ImageFileMachine.I386);

        /// <summary>
        /// Saves this dynamic assembly to disk, specifying the nature of code in the assembly's executables and the
        /// target platform.
        /// </summary>
        /// <param name="assemblyFileName">The file name of the assembly.</param>
        /// <param name="portableExecutableKind">A bitwise combination of the <see cref="PortableExecutableKinds"/> values that specifies the nature of the code.</param>
        /// <param name="imageFileMachine">One of the <see cref="ImageFileMachine"/> values that specifies the target platform.</param>
        public void Save(string assemblyFileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine) =>
            SaveImpl(assemblyFileName, portableExecutableKind, imageFileMachine);

        /// <summary>
        /// Implements the logic to save all of the modules within the assembly. The module whose file name matches
        /// <paramref name="assemblyFileName"/> becomes the manifest module; if there is none, an empty manifest module
        /// is defined. Every other module is written to its own file and linked from the manifest.
        /// </summary>
        /// <param name="assemblyFileName">The file name of the manifest module.</param>
        /// <param name="portableExecutableKind">The nature of the code in the executables.</param>
        /// <param name="imageFileMachine">The target platform.</param>
        /// <exception cref="NotSupportedException">The entry point is in a module other than the manifest module.</exception>
        void SaveImpl(string assemblyFileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
        {
            ModuleBuilder manifestModule = null;

            // finalize and populate all modules
            foreach (var moduleBuilder in _modules)
            {
                moduleBuilder.SetIsSaved();
                moduleBuilder.PopulatePropertyTables();

                // is this the default manifest module?
                if (manifestModule == null && string.Compare(moduleBuilder.FileName, assemblyFileName, StringComparison.OrdinalIgnoreCase) == 0)
                    manifestModule = moduleBuilder;
            }

            // generate new manifest module
            manifestModule ??= DefineDynamicModule("RefEmit_OnDiskManifestModule", assemblyFileName, false);

            // the assembly row goes on the manifest module
            manifestModule.Metadata.AddAssembly(
                manifestModule.GetOrAddString(_name),
                new Version(_majorVersion, _minorVersion, _buildVersion, _revisionVersion),
                _culture != null ? manifestModule.GetOrAddString(_culture) : default,
                _publicKey != null ? manifestModule.GetOrAddBlob(_publicKey) : default,
                (System.Reflection.AssemblyFlags)(_publicKey != null ? _flags | AssemblyNameFlags.PublicKey : _flags & ~AssemblyNameFlags.PublicKey),
                (System.Reflection.AssemblyHashAlgorithm)_hashAlgorithm);

            // final copy of manifest module native resources
            var nativeResources = new ModuleResourceSectionBuilder();

            // version info specified on assembly: insert into manifest module
            if (_versionInfo != null)
            {
                _versionInfo.SetName(GetName());
                _versionInfo.SetFileName(assemblyFileName);
                foreach (var cab in _customAttributes)
                {
                    // .NET doesn't support copying blob custom attributes into the version info
                    if (cab.HasBlob == false)
                        _versionInfo.SetAttribute(this, cab);
                }

                var versionInfoData = new ByteBuffer(512);
                _versionInfo.Write(versionInfoData);
                nativeResources.AddVersionInfo(versionInfoData);
            }

            // win32 icon specified on assembly: insert into manifest module
            if (_win32icon != null)
                nativeResources.AddIcon(_win32icon);

            // win32 manifest specified on assembly: insert into manifest module
            if (_win32manifest != null)
                nativeResources.AddManifest(_win32manifest, _fileKind == PEFileKinds.Dll ? (ushort)2 : (ushort)1);

            // we intentionally don't filter out the version info (pseudo) custom attributes (to be compatible with .NET)
            foreach (var cab in _customAttributes)
                manifestModule.SetCustomAttribute(0x20000001, cab);

            foreach (var fwd in _typeForwarders)
                manifestModule.AddTypeForwarder(fwd.Type, fwd.IncludeNested);

            // add resource files for assembly to manifest module
            foreach (var resfile in _resourceFiles)
            {
                var fileToken = AddFile(manifestModule, resfile.FileName, 1 /*ContainsNoMetaData*/);
                manifestModule.AddManifestResource(resfile.Attributes, resfile.Name, MetadataTokens.GetToken(fileToken), 0);
            }

            // fill in the assembly references of every module, and write each non-manifest module
            foreach (var module in _modules)
            {
                module.FillAssemblyRefTable();

                if (module != manifestModule)
                {
                    var fileToken = default(AssemblyFileHandle);
                    if (_entryPoint != null && _entryPoint.Module == module)
                    {
                        throw new NotSupportedException("Multi-module assemblies cannot have an entry point in a module other than the manifest module.");
                    }
                    else
                    {
                        ModuleWriter.WriteModule(null, null, module, _fileKind, portableExecutableKind, imageFileMachine, null, default);
                        fileToken = AddFile(manifestModule, module.FileName, 0 /*ContainsMetaData*/);
                    }

                    module.ExportTypes(fileToken, manifestModule);
                }
            }

            // finally, write the manifest module
            ModuleWriter.WriteModule(_keyPair, _publicKey, manifestModule, _fileKind, portableExecutableKind, imageFileMachine, nativeResources, _entryPoint, null, null, null, null);
        }

        /// <summary>
        /// Adds a <c>File</c> row for a file that belongs to the assembly, hashing the file's contents with SHA-1.
        /// </summary>
        /// <param name="manifestModule">The manifest module to add the row to.</param>
        /// <param name="fileName">The name of the file, relative to <see cref="Dir"/>.</param>
        /// <param name="flags">The <c>File</c> row flags: 0 if the file contains metadata, 1 if it does not.</param>
        /// <returns>A handle to the added <c>File</c> row.</returns>
        AssemblyFileHandle AddFile(ModuleBuilder manifestModule, string fileName, int flags)
        {
            var fullPath = Path.Combine(Dir, fileName);

            using var sha1 = SHA1.Create();
            using var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            var hash = sha1.ComputeHash(fs);

            return MetadataTokens.AssemblyFileHandle(manifestModule.__AddModule(flags, Path.GetFileName(fileName), hash));
        }

        /// <summary>
        /// Adds an existing resource file to this assembly.
        /// </summary>
        /// <param name="name">The logical name of the resource.</param>
        /// <param name="fileName">The physical file name (.resources file) to which the logical name is mapped. This should not include a path; the file must be in the same directory as the assembly to which it is added.</param>
        public void AddResourceFile(string name, string fileName) => AddResourceFile(name, fileName, ResourceAttributes.Public);

        /// <summary>
        /// Adds an existing resource file to this assembly.
        /// </summary>
        /// <param name="name">The logical name of the resource.</param>
        /// <param name="fileName">The physical file name (.resources file) to which the logical name is mapped. This should not include a path; the file must be in the same directory as the assembly to which it is added.</param>
        /// <param name="attribs">The resource attributes.</param>
        public void AddResourceFile(string name, string fileName, ResourceAttributes attribs)
        {
            var resfile = new ResourceFile
            {
                Name = name,
                FileName = fileName,
                Attributes = attribs
            };
            _resourceFiles.Add(resfile);
        }

        /// <summary>
        /// Defines an unmanaged version information resource using the information specified in the assembly's
        /// <see cref="AssemblyName"/> object and the assembly's custom attributes.
        /// </summary>
        /// <exception cref="ArgumentException">An unmanaged version information resource was previously defined.</exception>
        public void DefineVersionInfoResource()
        {
            if (_versionInfo != null)
                throw new ArgumentException("Native resource has already been defined.");

            _versionInfo = new VersionInfo();
        }

        /// <summary>
        /// Defines an unmanaged icon resource for the assembly.
        /// </summary>
        /// <param name="iconFile">The contents of the icon file; the array is copied.</param>
        /// <exception cref="ArgumentException">An icon resource was previously defined.</exception>
        public void __DefineIconResource(byte[] iconFile)
        {
            if (_win32icon != null)
                throw new ArgumentException("Native resource has already been defined.");

            _win32icon = (byte[])iconFile.Clone();
        }

        /// <summary>
        /// Defines an unmanaged Win32 application manifest resource for the assembly.
        /// </summary>
        /// <param name="manifest">The contents of the manifest; the array is copied.</param>
        /// <exception cref="ArgumentException">A manifest resource was previously defined.</exception>
        public void __DefineManifestResource(byte[] manifest)
        {
            if (_win32manifest != null)
                throw new ArgumentException("Native resource has already been defined.");

            _win32manifest = (byte[])manifest.Clone();
        }

        /// <inheritdoc />
        public override Type[] GetTypes()
        {
            var list = new List<Type>();

            foreach (var module in _modules)
                module.GetTypesImpl(list);

            return list.ToArray();
        }

        /// <inheritdoc />
        internal override Type FindType(TypeName typeName)
        {
            foreach (var mb in _modules)
            {
                var type = mb.FindType(typeName);
                if (type != null)
                    return type;
            }

            return null;
        }

        /// <inheritdoc />
        internal override Type FindTypeIgnoreCase(TypeName lowerCaseName)
        {
            foreach (var mb in _modules)
            {
                var type = mb.FindTypeIgnoreCase(lowerCaseName);
                if (type != null)
                    return type;
            }

            return null;
        }

        /// <summary>
        /// Gets the version of the common language runtime that will be saved in the file containing the manifest. This
        /// is taken from the core library of the universe when it is available, and from the running runtime otherwise.
        /// </summary>
        public override string ImageRuntimeVersion => _imageRuntimeVersion;

        /// <summary>
        /// Gets a module that represents the manifest of this assembly. It is a placeholder that stands for the assembly
        /// before it is saved, not one of the modules defined with <see cref="DefineDynamicModule"/>.
        /// </summary>
        public override Module ManifestModule => _pseudoManifestModule ??= new ManifestModule(this);

        /// <inheritdoc />
        public override MethodInfo EntryPoint => _entryPoint;

        /// <summary>
        /// Gets the names of the assemblies referenced by this assembly. Dynamic assemblies report none.
        /// </summary>
        /// <returns>An empty array.</returns>
        public override AssemblyName[] GetReferencedAssemblies() => Array.Empty<AssemblyName>();

        /// <inheritdoc />
        public override Module[] GetLoadedModules(bool getResourceModules) => GetModules(getResourceModules);

        /// <inheritdoc />
        public override Module[] GetModules(bool getResourceModules)
        {
            var list = new List<Module>();

            foreach (var module in _modules)
                if (getResourceModules || !module.IsResource())
                    list.Add(module);

            return list.ToArray();
        }

        /// <summary>
        /// Gets the specified module in this assembly, comparing names case-insensitively.
        /// </summary>
        /// <param name="name">The name of the module being requested.</param>
        /// <returns>The module being requested, or <see langword="null"/> if the module is not found.</returns>
        public override Module GetModule(string name)
        {
            foreach (var module in _modules)
                if (module.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return module;

            return null;
        }

        /// <summary>
        /// Returns information about how the given resource has been persisted. Not supported for dynamic assemblies.
        /// </summary>
        /// <param name="resourceName">The case-sensitive name of the resource.</param>
        /// <returns>Does not return.</returns>
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        public override ManifestResourceInfo GetManifestResourceInfo(string resourceName) => throw new NotSupportedException();

        /// <summary>
        /// Loads the specified manifest resource from this assembly. Not supported for dynamic assemblies.
        /// </summary>
        /// <param name="resourceName">The case-sensitive name of the manifest resource being requested.</param>
        /// <returns>Does not return.</returns>
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        public override Stream GetManifestResourceStream(string resourceName) => throw new NotSupportedException();

        /// <summary>
        /// Gets the custom attributes applied to the assembly so far, built from their <see cref="CustomAttributeBuilder"/>s.
        /// </summary>
        /// <param name="attributeType">The attribute type to filter by, or <see langword="null"/> for all.</param>
        /// <returns>The custom attributes whose type is assignable to <paramref name="attributeType"/>.</returns>
        internal override IList<CustomAttributeData> GetCustomAttributesData(Type attributeType)
        {
            var list = new List<CustomAttributeData>();
            foreach (var cab in _customAttributes)
                if (attributeType == null || attributeType.IsAssignableFrom(cab.Constructor.DeclaringType))
                    list.Add(cab.ToData(this));

            return list;
        }

    }

}
