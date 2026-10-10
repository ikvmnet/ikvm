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

    internal sealed class AssemblyBuilder : Assembly
    {

        readonly string _name;
        ushort _majorVersion;
        ushort _minorVersion;
        ushort _buildVersion;
        ushort _revisionVersion;
        string _culture;
        AssemblyNameFlags _flags;
        AssemblyHashAlgorithm _hashAlgorithm;
        StrongNameKeyPair _keyPair;
        byte[] _publicKey;
        internal readonly string Dir;
        PEFileKinds _fileKind = PEFileKinds.Dll;
        MethodInfo _entryPoint;
        VersionInfo _versionInfo;
        byte[] _win32icon;
        byte[] _win32manifest;
        string _imageRuntimeVersion;
        internal int MdStreamVersion = 0x20000;
        Module _pseudoManifestModule;
        readonly List<ResourceFile> _resourceFiles = new List<ResourceFile>();
        readonly List<ModuleBuilder> _modules = new List<ModuleBuilder>();
        readonly List<Module> _addedModules = new List<Module>();
        readonly List<CustomAttributeBuilder> _customAttributes = new List<CustomAttributeBuilder>();
        readonly List<TypeForwarder> _typeForwarders = new List<TypeForwarder>();

        readonly struct TypeForwarder
        {

            internal readonly Type Type;
            internal readonly bool IncludeNested;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="type"></param>
            /// <param name="includeNested"></param>
            internal TypeForwarder(Type type, bool includeNested)
            {
                this.Type = type;
                this.IncludeNested = includeNested;
            }

        }

        struct ResourceFile
        {

            internal string Name;
            internal string FileName;
            internal ResourceAttributes Attributes;

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="universe"></param>
        /// <param name="name"></param>
        /// <param name="dir"></param>
        /// <param name="customAttributes"></param>
        internal AssemblyBuilder(Universe universe, AssemblyName name, string dir, IEnumerable<CustomAttributeBuilder> customAttributes) :
            base(universe)
        {
            this._name = name.Name;
            SetVersionHelper(name.Version);
            if (!string.IsNullOrEmpty(name.CultureName))
            {
                this._culture = name.CultureName;
            }

            this._flags = name.RawFlags;
            this._hashAlgorithm = name.HashAlgorithm;
            if (this._hashAlgorithm == AssemblyHashAlgorithm.None)
                this._hashAlgorithm = AssemblyHashAlgorithm.SHA1;

            this._keyPair = name.KeyPair;
            if (this._keyPair != null)
            {
                this._publicKey = this._keyPair.PublicKey;
            }
            else
            {
                var publicKey = name.GetPublicKey();
                if (publicKey != null && publicKey.Length != 0)
                    this._publicKey = (byte[])publicKey.Clone();
            }

            this.Dir = dir ?? ".";
            if (customAttributes != null)
                this._customAttributes.AddRange(customAttributes);

            if (universe.HasCoreLib && !universe.CoreLib.__IsMissing && universe.CoreLib.ImageRuntimeVersion != null)
                this._imageRuntimeVersion = universe.CoreLib.ImageRuntimeVersion;
            else
                this._imageRuntimeVersion = TypeUtil.GetAssembly(typeof(object)).ImageRuntimeVersion;

            universe.RegisterDynamicAssembly(this);
        }

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

        void Rename(AssemblyName oldName)
        {
            this.fullName = null;
            Universe.RenameAssembly(this, oldName);
        }

        public void __SetAssemblyVersion(Version version)
        {
            AssemblyName oldName = GetName();
            SetVersionHelper(version);
            Rename(oldName);
        }

        public void __SetAssemblyCulture(string cultureName)
        {
            AssemblyName oldName = GetName();
            this._culture = cultureName;
            Rename(oldName);
        }

        internal string Name
        {
            get { return _name; }
        }

        public override AssemblyName GetName()
        {
            AssemblyName n = new AssemblyName();
            n.Name = _name;
            n.Version = new Version(_majorVersion, _minorVersion, _buildVersion, _revisionVersion);
            n.CultureName = _culture ?? "";
            n.HashAlgorithm = _hashAlgorithm;
            n.RawFlags = _flags;
            n.SetPublicKey(_publicKey != null ? (byte[])_publicKey.Clone() : Array.Empty<byte>());
            n.KeyPair = _keyPair;
            return n;
        }

        public override string Location
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Defines a persistable dynamic module, specifying the module name, the name of the file to which the module will be saved, and whether symbol information should be emitted using the default symbol writer.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        /// <param name="emitSymbolInfo"></param>
        /// <returns></returns>
        public ModuleBuilder DefineDynamicModule(string name, string fileName, bool emitSymbolInfo)
        {
            var module = new ModuleBuilder(this, name, fileName);
            module.SetSymWriter(emitSymbolInfo ? Universe.CreateSymbolWriter(module) : null);
            _modules.Add(module);
            return module;
        }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            _customAttributes.Add(customBuilder);
        }

        public void __AddTypeForwarder(Type type)
        {
            __AddTypeForwarder(type, true);
        }

        public void __AddTypeForwarder(Type type, bool includeNested)
        {
            _typeForwarders.Add(new TypeForwarder(type, includeNested));
        }

        public void SetEntryPoint(MethodInfo entryMethod, PEFileKinds fileKind)
        {
            this._entryPoint = entryMethod;
            this._fileKind = fileKind;
        }

        /// <summary>
        /// Saves this dynamic assembly to disk.
        /// </summary>
        /// <param name="assemblyFileName"></param>
        public void Save(string assemblyFileName)
        {
            Save(assemblyFileName, PortableExecutableKinds.ILOnly, ImageFileMachine.I386);
        }

        /// <summary>
        /// Saves this dynamic assembly to disk, specifying the nature of code in the assembly's executables and the target platform.
        /// </summary>
        /// <param name="assemblyFileName"></param>
        /// <param name="portableExecutableKind"></param>
        /// <param name="imageFileMachine"></param>
        public void Save(string assemblyFileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
        {
            SaveImpl(assemblyFileName, null, null, portableExecutableKind, imageFileMachine);
        }

        /// <summary>
        /// Implements the logic to save all of the modules within the assembly.
        /// </summary>
        /// <param name="assemblyFileName"></param>
        /// <param name="peStream"></param>
        /// <param name="pdbStream"></param>
        /// <param name="portableExecutableKind"></param>
        /// <param name="imageFileMachine"></param>
        void SaveImpl(string assemblyFileName, Stream peStream, Stream pdbStream, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
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
            var nativeResources = manifestModule.NativeResources != null ? new ModuleResourceSectionBuilder(manifestModule.NativeResources) : new ModuleResourceSectionBuilder();

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

            // write each non-manifest module
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
                        ModuleWriter.WriteModule(null, null, module, _fileKind, portableExecutableKind, imageFileMachine, module.NativeResources, default);
                        fileToken = AddFile(manifestModule, module.FileName, 0 /*ContainsMetaData*/);
                    }

                    module.ExportTypes(fileToken, manifestModule);
                }
            }

            // import existing modules
            foreach (var module in _addedModules)
            {
                var fileToken = AddFile(manifestModule, module.FullyQualifiedName, 0 /*ContainsMetaData*/);
                module.ExportTypes(fileToken, manifestModule);
            }

            // finally, write the manifest module
            ModuleWriter.WriteModule(_keyPair, _publicKey, manifestModule, _fileKind, portableExecutableKind, imageFileMachine, nativeResources, _entryPoint, null, peStream, null, pdbStream);
        }

        AssemblyFileHandle AddFile(ModuleBuilder manifestModule, string fileName, int flags)
        {
            var fullPath = fileName;
            if (Dir != null)
                fullPath = Path.Combine(Dir, fileName);

            using var sha1 = SHA1.Create();
            using var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            var hash = sha1.ComputeHash(fs);

            return MetadataTokens.AssemblyFileHandle(manifestModule.__AddModule(flags, Path.GetFileName(fileName), hash));
        }

        public void AddResourceFile(string name, string fileName)
        {
            AddResourceFile(name, fileName, ResourceAttributes.Public);
        }

        public void AddResourceFile(string name, string fileName, ResourceAttributes attribs)
        {
            var resfile = new ResourceFile();
            resfile.Name = name;
            resfile.FileName = fileName;
            resfile.Attributes = attribs;
            _resourceFiles.Add(resfile);
        }

        public void DefineVersionInfoResource()
        {
            if (_versionInfo != null)
                throw new ArgumentException("Native resource has already been defined.");

            _versionInfo = new VersionInfo();
        }

        public void __DefineIconResource(byte[] iconFile)
        {
            if (_win32icon != null)
                throw new ArgumentException("Native resource has already been defined.");

            _win32icon = (byte[])iconFile.Clone();
        }

        public void __DefineManifestResource(byte[] manifest)
        {
            if (_win32manifest != null)
                throw new ArgumentException("Native resource has already been defined.");

            _win32manifest = (byte[])manifest.Clone();
        }

        public override Type[] GetTypes()
        {
            var list = new List<Type>();

            foreach (var module in _modules)
                module.GetTypesImpl(list);

            foreach (var module in _addedModules)
                module.GetTypesImpl(list);

            return list.ToArray();
        }

        internal override Type FindType(TypeName typeName)
        {
            foreach (var mb in _modules)
            {
                var type = mb.FindType(typeName);
                if (type != null)
                    return type;
            }

            foreach (Module module in _addedModules)
            {
                var type = module.FindType(typeName);
                if (type != null)
                    return type;
            }

            return null;
        }

        internal override Type FindTypeIgnoreCase(TypeName lowerCaseName)
        {
            foreach (var mb in _modules)
            {
                var type = mb.FindTypeIgnoreCase(lowerCaseName);
                if (type != null)
                    return type;
            }

            foreach (Module module in _addedModules)
            {
                var type = module.FindTypeIgnoreCase(lowerCaseName);
                if (type != null)
                    return type;
            }

            return null;
        }

        public override string ImageRuntimeVersion
        {
            get { return _imageRuntimeVersion; }
        }

        public override Module ManifestModule => _pseudoManifestModule ??= new ManifestModule(this);

        public override MethodInfo EntryPoint => _entryPoint;

        public override AssemblyName[] GetReferencedAssemblies() => Array.Empty<AssemblyName>();

        public override Module[] GetLoadedModules(bool getResourceModules)
        {
            return GetModules(getResourceModules);
        }

        public override Module[] GetModules(bool getResourceModules)
        {
            var list = new List<Module>();

            foreach (var module in _modules)
                if (getResourceModules || !module.IsResource())
                    list.Add(module);

            foreach (var module in _addedModules)
                if (getResourceModules || !module.IsResource())
                    list.Add(module);

            return list.ToArray();
        }

        public override Module GetModule(string name)
        {
            foreach (var module in _modules)
                if (module.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return module;

            foreach (var module in _addedModules)
                if (module.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return module;

            return null;
        }

        public override ManifestResourceInfo GetManifestResourceInfo(string resourceName)
        {
            throw new NotSupportedException();
        }

        public override Stream GetManifestResourceStream(string resourceName)
        {
            throw new NotSupportedException();
        }

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
