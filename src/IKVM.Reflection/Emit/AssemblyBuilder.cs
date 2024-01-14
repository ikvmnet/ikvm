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

using IKVM.Reflection.Metadata;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Emit
{

    public sealed class AssemblyBuilder : Assembly
    {

        readonly string name;
        ushort majorVersion;
        ushort minorVersion;
        ushort buildVersion;
        ushort revisionVersion;
        string culture;
        AssemblyNameFlags flags;
        AssemblyHashAlgorithm hashAlgorithm;
        StrongNameKeyPair keyPair;
        byte[] publicKey;
        internal readonly string dir;
        PEFileKinds fileKind = PEFileKinds.Dll;
        MethodInfo entryPoint;
        VersionInfo versionInfo;
        byte[] win32icon;
        byte[] win32manifest;
        byte[] win32resources;
        string imageRuntimeVersion;
        internal int mdStreamVersion = 0x20000;
        Module pseudoManifestModule;
        readonly List<ResourceFile> resourceFiles = new List<ResourceFile>();
        readonly List<ModuleBuilder> modules = new List<ModuleBuilder>();
        readonly List<Module> addedModules = new List<Module>();
        readonly List<CustomAttributeBuilder> customAttributes = new List<CustomAttributeBuilder>();
        readonly List<CustomAttributeBuilder> declarativeSecurity = new List<CustomAttributeBuilder>();
        readonly List<TypeForwarder> typeForwarders = new List<TypeForwarder>();

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
            internal ResourceWriter Writer;

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
            this.name = name.Name;
            SetVersionHelper(name.Version);
            if (!string.IsNullOrEmpty(name.CultureName))
            {
                this.culture = name.CultureName;
            }

            this.flags = name.RawFlags;
            this.hashAlgorithm = name.HashAlgorithm;
            if (this.hashAlgorithm == AssemblyHashAlgorithm.None)
                this.hashAlgorithm = AssemblyHashAlgorithm.SHA1;

            this.keyPair = name.KeyPair;
            if (this.keyPair != null)
            {
                this.publicKey = this.keyPair.PublicKey;
            }
            else
            {
                var publicKey = name.GetPublicKey();
                if (publicKey != null && publicKey.Length != 0)
                    this.publicKey = (byte[])publicKey.Clone();
            }

            this.dir = dir ?? ".";
            if (customAttributes != null)
                this.customAttributes.AddRange(customAttributes);

            if (universe.HasCoreLib && !universe.CoreLib.__IsMissing && universe.CoreLib.ImageRuntimeVersion != null)
                this.imageRuntimeVersion = universe.CoreLib.ImageRuntimeVersion;
            else
                this.imageRuntimeVersion = TypeUtil.GetAssembly(typeof(object)).ImageRuntimeVersion;

            universe.RegisterDynamicAssembly(this);
        }

        void SetVersionHelper(Version version)
        {
            if (version == null)
            {
                majorVersion = 0;
                minorVersion = 0;
                buildVersion = 0;
                revisionVersion = 0;
            }
            else
            {
                majorVersion = (ushort)version.Major;
                minorVersion = (ushort)version.Minor;
                buildVersion = version.Build == -1 ? (ushort)0 : (ushort)version.Build;
                revisionVersion = version.Revision == -1 ? (ushort)0 : (ushort)version.Revision;
            }
        }

        void Rename(AssemblyName oldName)
        {
            this.fullName = null;
            universe.RenameAssembly(this, oldName);
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
            this.culture = cultureName;
            Rename(oldName);
        }

        public void __SetAssemblyKeyPair(StrongNameKeyPair keyPair)
        {
            AssemblyName oldName = GetName();
            this.keyPair = keyPair;
            if (keyPair != null)
            {
                this.publicKey = keyPair.PublicKey;
            }
            Rename(oldName);
        }

        // this is used in combination with delay signing
        public void __SetAssemblyPublicKey(byte[] publicKey)
        {
            AssemblyName oldName = GetName();
            this.publicKey = publicKey == null ? null : (byte[])publicKey.Clone();
            Rename(oldName);
        }

        public void __SetAssemblyAlgorithmId(AssemblyHashAlgorithm hashAlgorithm)
        {
            this.hashAlgorithm = hashAlgorithm;
        }

        [Obsolete("Use __AssemblyFlags property instead.")]
        public void __SetAssemblyFlags(AssemblyNameFlags flags)
        {
            this.__AssemblyFlags = flags;
        }

        protected override AssemblyNameFlags GetAssemblyFlags()
        {
            return flags;
        }

        public new AssemblyNameFlags __AssemblyFlags
        {
            get { return flags; }
            set
            {
                AssemblyName oldName = GetName();
                this.flags = value;
                Rename(oldName);
            }
        }

        internal string Name
        {
            get { return name; }
        }

        public override AssemblyName GetName()
        {
            AssemblyName n = new AssemblyName();
            n.Name = name;
            n.Version = new Version(majorVersion, minorVersion, buildVersion, revisionVersion);
            n.CultureName = culture ?? "";
            n.HashAlgorithm = hashAlgorithm;
            n.RawFlags = flags;
            n.SetPublicKey(publicKey != null ? (byte[])publicKey.Clone() : Array.Empty<byte>());
            n.KeyPair = keyPair;
            return n;
        }

        public override string Location
        {
            get { throw new NotSupportedException(); }
        }

        public ModuleBuilder DefineDynamicModule(string name, string fileName)
        {
            return DefineDynamicModule(name, fileName, false);
        }

        public ModuleBuilder DefineDynamicModule(string name, string fileName, bool emitSymbolInfo)
        {
            ModuleBuilder module = new ModuleBuilder(this, name, fileName, emitSymbolInfo);
            modules.Add(module);
            return module;
        }

        public ModuleBuilder GetDynamicModule(string name)
        {
            foreach (ModuleBuilder module in modules)
            {
                if (module.Name == name)
                {
                    return module;
                }
            }
            return null;
        }

        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
        {
            SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
        }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            customAttributes.Add(customBuilder);
        }

        public void __AddDeclarativeSecurity(CustomAttributeBuilder customBuilder)
        {
            declarativeSecurity.Add(customBuilder);
        }

        public void __AddTypeForwarder(Type type)
        {
            __AddTypeForwarder(type, true);
        }

        public void __AddTypeForwarder(Type type, bool includeNested)
        {
            typeForwarders.Add(new TypeForwarder(type, includeNested));
        }

        public void SetEntryPoint(MethodInfo entryMethod)
        {
            SetEntryPoint(entryMethod, PEFileKinds.ConsoleApplication);
        }

        public void SetEntryPoint(MethodInfo entryMethod, PEFileKinds fileKind)
        {
            this.entryPoint = entryMethod;
            this.fileKind = fileKind;
        }

        public void __Save(Stream stream, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
        {
            if (!stream.CanRead || !stream.CanWrite || !stream.CanSeek || stream.Position != 0)
                throw new ArgumentException("Stream must support read/write/seek and current position must be zero.", "stream");
            if (modules.Count != 1)
                throw new NotSupportedException("Saving to a stream is only supported for single module assemblies.");

            SaveImpl(modules[0].fileName, stream, portableExecutableKind, imageFileMachine);
        }

        public void Save(string assemblyFileName)
        {
            Save(assemblyFileName, PortableExecutableKinds.ILOnly, ImageFileMachine.I386);
        }

        public void Save(string assemblyFileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
        {
            SaveImpl(assemblyFileName, null, portableExecutableKind, imageFileMachine);
        }

        /// <summary>
        /// Implements the logic to save all of the modules within the assembly.
        /// </summary>
        /// <param name="assemblyFileName"></param>
        /// <param name="streamOrNull"></param>
        /// <param name="portableExecutableKind"></param>
        /// <param name="imageFileMachine"></param>
        void SaveImpl(string assemblyFileName, Stream streamOrNull, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
        {
            ModuleBuilder manifestModule = null;

            // finalize and populate all modules
            foreach (var moduleBuilder in modules)
            {
                moduleBuilder.SetIsSaved();
                moduleBuilder.PopulatePropertyAndEventTables();

                // is this the default manifest module?
                if (manifestModule == null && string.Compare(moduleBuilder.fileName, assemblyFileName, StringComparison.OrdinalIgnoreCase) == 0)
                    manifestModule = moduleBuilder;
            }

            // generate new manifest module
            manifestModule ??= DefineDynamicModule("RefEmit_OnDiskManifestModule", assemblyFileName, false);

            // assembly record goes on manifest module
            var assemblyRecord = new AssemblyTable.Record();
            assemblyRecord.HashAlgId = (int)hashAlgorithm;
            assemblyRecord.Name = manifestModule.GetOrAddString(name);
            assemblyRecord.MajorVersion = majorVersion;
            assemblyRecord.MinorVersion = minorVersion;
            assemblyRecord.BuildNumber = buildVersion;
            assemblyRecord.RevisionNumber = revisionVersion;

            if (publicKey != null)
            {
                assemblyRecord.PublicKey = manifestModule.GetOrAddBlob(publicKey);
                assemblyRecord.Flags = (int)(flags | AssemblyNameFlags.PublicKey);
            }
            else
            {
                assemblyRecord.Flags = (int)(flags & ~AssemblyNameFlags.PublicKey);
            }

            if (culture != null)
                assemblyRecord.Culture = manifestModule.GetOrAddString(culture);

            manifestModule.AssemblyTable.AddRecord(assemblyRecord);

            // final copy of manifest module native resources
            var nativeResources = manifestModule.nativeResources != null ? new ModuleResourceSectionBuilder(manifestModule.nativeResources) : new ModuleResourceSectionBuilder();

            // version info specified on assembly: insert into manifest module
            if (versionInfo != null)
            {
                versionInfo.SetName(GetName());
                versionInfo.SetFileName(assemblyFileName);
                foreach (var cab in customAttributes)
                {
                    // .NET doesn't support copying blob custom attributes into the version info
                    if (cab.HasBlob == false || universe.DecodeVersionInfoAttributeBlobs)
                        versionInfo.SetAttribute(this, cab);
                }

                var versionInfoData = new ByteBuffer(512);
                versionInfo.Write(versionInfoData);
                nativeResources.AddVersionInfo(versionInfoData);
            }

            // win32 icon specified on assembly: insert into manifest module
            if (win32icon != null)
                nativeResources.AddIcon(win32icon);

            // win32 manifest specified on assembly: insert into manifest module
            if (win32manifest != null)
                nativeResources.AddManifest(win32manifest, fileKind == PEFileKinds.Dll ? (ushort)2 : (ushort)1);

            if (win32resources != null)
                nativeResources.ImportWin32ResourceFile(win32resources);

            // we intentionally don't filter out the version info (pseudo) custom attributes (to be compatible with .NET)
            foreach (var cab in customAttributes)
                manifestModule.SetCustomAttribute(0x20000001, cab);

            manifestModule.AddDeclarativeSecurity(0x20000001, declarativeSecurity);

            foreach (var fwd in typeForwarders)
                manifestModule.AddTypeForwarder(fwd.Type, fwd.IncludeNested);

            // add resource files for assembly to manifest module
            foreach (var resfile in resourceFiles)
            {
                if (resfile.Writer != null)
                {
                    resfile.Writer.Generate();
                    resfile.Writer.Close();
                }

                var fileToken = AddFile(manifestModule, resfile.FileName, 1 /*ContainsNoMetaData*/);
                var rec = new ManifestResourceTable.Record();
                rec.Offset = 0;
                rec.Flags = (int)resfile.Attributes;
                rec.Name = manifestModule.GetOrAddString(resfile.Name);
                rec.Implementation = MetadataTokens.GetToken(fileToken);
                manifestModule.ManifestResource.AddRecord(rec);
            }

            // write each non-manifest module
            foreach (var moduleBuilder in modules)
            {
                moduleBuilder.FillAssemblyRefTable();

                if (moduleBuilder != manifestModule)
                {
                    var fileToken = default(AssemblyFileHandle);
                    if (entryPoint != null && entryPoint.Module == moduleBuilder)
                    {
                        throw new NotSupportedException("Multi-module assemblies cannot have an entry point in a module other than the manifest module.");
                    }
                    else
                    {
                        ModuleWriter.WriteModule(null, null, moduleBuilder, fileKind, portableExecutableKind, imageFileMachine, moduleBuilder.nativeResources, default);
                        fileToken = AddFile(manifestModule, moduleBuilder.fileName, 0 /*ContainsMetaData*/);
                    }

                    moduleBuilder.ExportTypes(fileToken, manifestModule);
                }
            }

            // import existing modules
            foreach (var module in addedModules)
            {
                var fileToken = AddFile(manifestModule, module.FullyQualifiedName, 0 /*ContainsMetaData*/);
                module.ExportTypes(fileToken, manifestModule);
            }

            // finally, write the manifest module
            ModuleWriter.WriteModule(keyPair, publicKey, manifestModule, fileKind, portableExecutableKind, imageFileMachine, nativeResources, entryPoint, streamOrNull);
        }

        AssemblyFileHandle AddFile(ModuleBuilder manifestModule, string fileName, int flags)
        {
            var fullPath = fileName;
            if (dir != null)
                fullPath = Path.Combine(dir, fileName);

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
            resourceFiles.Add(resfile);
        }

        public IResourceWriter DefineResource(string name, string description, string fileName)
        {
            return DefineResource(name, description, fileName, ResourceAttributes.Public);
        }

        public IResourceWriter DefineResource(string name, string description, string fileName, ResourceAttributes attribute)
        {
            // FXBUG we ignore the description, because there is no such thing

            var fullPath = fileName;
            if (dir != null)
                fullPath = Path.Combine(dir, fileName);

            var rw = new ResourceWriter(fullPath);
            var resfile = new ResourceFile();
            resfile.Name = name;
            resfile.FileName = fileName;
            resfile.Attributes = attribute;
            resfile.Writer = rw;
            resourceFiles.Add(resfile);
            return rw;
        }

        public void DefineVersionInfoResource()
        {
            if (versionInfo != null || win32resources != null)
                throw new ArgumentException("Native resource has already been defined.");

            versionInfo = new VersionInfo();
        }

        public void DefineVersionInfoResource(string product, string productVersion, string company, string copyright, string trademark)
        {
            if (versionInfo != null || win32resources != null)
                throw new ArgumentException("Native resource has already been defined.");

            versionInfo = new VersionInfo();
            versionInfo.product = product;
            versionInfo.informationalVersion = productVersion;
            versionInfo.company = company;
            versionInfo.copyright = copyright;
            versionInfo.trademark = trademark;
        }

        public void __DefineIconResource(byte[] iconFile)
        {
            if (win32icon != null || win32resources != null)
                throw new ArgumentException("Native resource has already been defined.");

            win32icon = (byte[])iconFile.Clone();
        }

        public void __DefineManifestResource(byte[] manifest)
        {
            if (win32manifest != null || win32resources != null)
                throw new ArgumentException("Native resource has already been defined.");

            win32manifest = (byte[])manifest.Clone();
        }

        public void __DefineUnmanagedResource(byte[] resource)
        {
            if (versionInfo != null || win32icon != null || win32manifest != null || win32resources != null)
                throw new ArgumentException("Native resource has already been defined.");

            // The standard .NET DefineUnmanagedResource(byte[]) is useless, because it embeds "resource" (as-is) as the .rsrc section,
            // but it doesn't set the PE file Resource Directory entry to point to it. That's why we have a renamed version, which behaves
            // like DefineUnmanagedResource(string).
            win32resources = (byte[])resource.Clone();
        }

        public void DefineUnmanagedResource(string resourceFileName)
        {
            // This method reads the specified resource file (Win32 .res file) and converts it into the appropriate format and embeds it in the .rsrc section,
            // also setting the Resource Directory entry.
            __DefineUnmanagedResource(File.ReadAllBytes(resourceFileName));
        }

        public override Type[] GetTypes()
        {
            var list = new List<Type>();

            foreach (var module in modules)
                module.GetTypesImpl(list);

            foreach (var module in addedModules)
                module.GetTypesImpl(list);

            return list.ToArray();
        }

        internal override Type FindType(TypeName typeName)
        {
            foreach (var mb in modules)
            {
                var type = mb.FindType(typeName);
                if (type != null)
                    return type;
            }

            foreach (Module module in addedModules)
            {
                var type = module.FindType(typeName);
                if (type != null)
                    return type;
            }

            return null;
        }

        internal override Type FindTypeIgnoreCase(TypeName lowerCaseName)
        {
            foreach (var mb in modules)
            {
                var type = mb.FindTypeIgnoreCase(lowerCaseName);
                if (type != null)
                    return type;
            }

            foreach (Module module in addedModules)
            {
                var type = module.FindTypeIgnoreCase(lowerCaseName);
                if (type != null)
                    return type;
            }

            return null;
        }

        public override string ImageRuntimeVersion
        {
            get { return imageRuntimeVersion; }
        }

        public void __SetImageRuntimeVersion(string imageRuntimeVersion, int mdStreamVersion)
        {
            this.imageRuntimeVersion = imageRuntimeVersion;
            this.mdStreamVersion = mdStreamVersion;
        }

        public override Module ManifestModule => pseudoManifestModule ??= new ManifestModule(this);

        public override MethodInfo EntryPoint => entryPoint;

        public override AssemblyName[] GetReferencedAssemblies() => Array.Empty<AssemblyName>();

        public override Module[] GetLoadedModules(bool getResourceModules)
        {
            return GetModules(getResourceModules);
        }

        public override Module[] GetModules(bool getResourceModules)
        {
            var list = new List<Module>();

            foreach (var module in modules)
                if (getResourceModules || !module.IsResource())
                    list.Add(module);

            foreach (var module in addedModules)
                if (getResourceModules || !module.IsResource())
                    list.Add(module);

            return list.ToArray();
        }

        public override Module GetModule(string name)
        {
            foreach (var module in modules)
                if (module.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return module;

            foreach (var module in addedModules)
                if (module.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return module;

            return null;
        }

        public Module __AddModule(RawModule module)
        {
            Module mod = module.ToModule(this);
            addedModules.Add(mod);
            return mod;
        }

        public override ManifestResourceInfo GetManifestResourceInfo(string resourceName)
        {
            throw new NotSupportedException();
        }

        public override string[] GetManifestResourceNames()
        {
            throw new NotSupportedException();
        }

        public override Stream GetManifestResourceStream(string resourceName)
        {
            throw new NotSupportedException();
        }

        public override bool IsDynamic
        {
            get { return true; }
        }

        public static AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access)
        {
            return new Universe().DefineDynamicAssembly(name, access);
        }

        public static AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, IEnumerable<CustomAttributeBuilder> assemblyAttributes)
        {
            return new Universe().DefineDynamicAssembly(name, access, assemblyAttributes);
        }

        internal override IList<CustomAttributeData> GetCustomAttributesData(Type attributeType)
        {
            var list = new List<CustomAttributeData>();
            foreach (var cab in customAttributes)
                if (attributeType == null || attributeType.IsAssignableFrom(cab.Constructor.DeclaringType))
                    list.Add(cab.ToData(this));

            return list;
        }

        internal bool IsWindowsRuntime
        {
            get { return (flags & (AssemblyNameFlags)0x200) != 0; }
        }

    }

}
