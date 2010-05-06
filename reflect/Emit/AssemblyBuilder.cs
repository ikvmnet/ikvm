/*
  Copyright (C) 2008-2010 Jeroen Frijters

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
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security;
using IKVM.Reflection.Metadata;
using IKVM.Reflection.Impl;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Emit
{
	public sealed class AssemblyBuilder : Assembly
	{
		private readonly AssemblyName name;
		internal readonly string dir;
		private readonly PermissionSet requiredPermissions;
		private readonly PermissionSet optionalPermissions;
		private readonly PermissionSet refusedPermissions;
		private PEFileKinds fileKind = PEFileKinds.Dll;
		private MethodInfo entryPoint;
		private VersionInfo versionInfo;
		private byte[] unmanagedResources;
		private string imageRuntimeVersion;
		internal int mdStreamVersion = 0x20000;
		private Module pseudoManifestModule;
		private readonly List<ResourceFile> resourceFiles = new List<ResourceFile>();
		private readonly List<ModuleBuilder> modules = new List<ModuleBuilder>();
		private readonly List<CustomAttributeBuilder> customAttributes = new List<CustomAttributeBuilder>();
		private readonly List<CustomAttributeBuilder> declarativeSecurity = new List<CustomAttributeBuilder>();
		private readonly List<Type> typeForwarders = new List<Type>();

		private struct ResourceFile
		{
			internal string Name;
			internal string FileName;
			internal ResourceAttributes Attributes;
		}

		internal AssemblyBuilder(Universe universe, AssemblyName name, string dir, AssemblyBuilderAccess access, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions)
			: base(universe)
		{
			this.name = name;
			this.dir = dir ?? ".";
			this.requiredPermissions = requiredPermissions;
			this.optionalPermissions = optionalPermissions;
			this.refusedPermissions = refusedPermissions;
			this.imageRuntimeVersion = universe.System_Object.Assembly.ImageRuntimeVersion;
		}

		public override AssemblyName GetName()
		{
			AssemblyName n = new AssemblyName();
			n.Name = name.Name;
			n.Version = name.Version ?? new Version(0, 0, 0, 0);
			n.CultureInfo = name.CultureInfo ?? System.Globalization.CultureInfo.InvariantCulture;
			n.SetPublicKey(GetPublicKey(name) ?? Empty<byte>.Array);
			return n;
		}

		public override string FullName
		{
			get { return GetName().FullName; }
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
			typeForwarders.Add(type);
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

		public void Save(string assemblyFileName)
		{
			Save(assemblyFileName, PortableExecutableKinds.ILOnly, ImageFileMachine.I386);
		}

		public void Save(string assemblyFileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
		{
			ModuleBuilder manifestModule = null;

			foreach (ModuleBuilder moduleBuilder in modules)
			{
				if (string.Compare(moduleBuilder.fileName, assemblyFileName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					manifestModule = moduleBuilder;
					break;
				}
			}

			if (manifestModule == null)
			{
				manifestModule = DefineDynamicModule("RefEmit_OnDiskManifestModule", assemblyFileName, false);
			}

			AssemblyTable.Record assemblyRecord = new AssemblyTable.Record();
			assemblyRecord.HashAlgId = 0x8004;	// SHA1
			assemblyRecord.Name = manifestModule.Strings.Add(name.Name);
			if (name.Version != null)
			{
				assemblyRecord.MajorVersion = (ushort)name.Version.Major;
				assemblyRecord.MinorVersion = (ushort)name.Version.Minor;
				assemblyRecord.BuildNumber = (ushort)(name.Version.Build == -1 ? 0 : name.Version.Build);
				assemblyRecord.RevisionNumber = (ushort)(name.Version.Revision == -1 ? 0 : name.Version.Revision);
			}
			byte[] publicKey = GetPublicKey(name);
			if (publicKey != null)
			{
				assemblyRecord.PublicKey = manifestModule.Blobs.Add(ByteBuffer.Wrap(publicKey));
				assemblyRecord.Flags |= 0x0001;	// PublicKey
			}
			if (name.CultureInfo != null)
			{
				assemblyRecord.Culture = manifestModule.Strings.Add(name.CultureInfo.Name);
			}
			int token = 0x20000000 + manifestModule.AssemblyTable.AddRecord(assemblyRecord);

#pragma warning disable 618
			// this values are obsolete, but we already know that so we disable the warning
			System.Security.Permissions.SecurityAction requestMinimum = System.Security.Permissions.SecurityAction.RequestMinimum;
			System.Security.Permissions.SecurityAction requestOptional = System.Security.Permissions.SecurityAction.RequestOptional;
			System.Security.Permissions.SecurityAction requestRefuse = System.Security.Permissions.SecurityAction.RequestRefuse;
#pragma warning restore 618
			if (requiredPermissions != null)
			{
				manifestModule.AddDeclarativeSecurity(token, requestMinimum, requiredPermissions);
			}
			if (optionalPermissions != null)
			{
				manifestModule.AddDeclarativeSecurity(token, requestOptional, optionalPermissions);
			}
			if (refusedPermissions != null)
			{
				manifestModule.AddDeclarativeSecurity(token, requestRefuse, refusedPermissions);
			}

			ByteBuffer versionInfoData = null;
			if (versionInfo != null)
			{
				versionInfo.SetName(name);
				versionInfo.SetFileName(assemblyFileName);
				foreach (CustomAttributeBuilder cab in customAttributes)
				{
					versionInfo.SetAttribute(cab);
				}
				versionInfoData = new ByteBuffer(512);
				versionInfo.Write(versionInfoData);
			}

			foreach (CustomAttributeBuilder cab in customAttributes)
			{
				manifestModule.SetAssemblyCustomAttribute(cab);
			}

			manifestModule.AddDeclarativeSecurity(0x20000001, declarativeSecurity);

			foreach (Type type in typeForwarders)
			{
				manifestModule.AddTypeForwarder(type);
			}

			foreach (ResourceFile resfile in resourceFiles)
			{
				int fileToken = AddFile(manifestModule, resfile.FileName, 1 /*ContainsNoMetaData*/);
				ManifestResourceTable.Record rec = new ManifestResourceTable.Record();
				rec.Offset = 0;
				rec.Flags = (int)resfile.Attributes;
				rec.Name = manifestModule.Strings.Add(resfile.Name);
				rec.Implementation = fileToken;
				manifestModule.ManifestResource.AddRecord(rec);
			}

			int entryPointToken = 0;

			foreach (ModuleBuilder moduleBuilder in modules)
			{
				if (moduleBuilder != manifestModule)
				{
					int fileToken;
					if (entryPoint != null && entryPoint.Module == moduleBuilder)
					{
						ModuleWriter.WriteModule(null, null, moduleBuilder, fileKind, portableExecutableKind, imageFileMachine, null, moduleBuilder.unmanagedResources, entryPoint.MetadataToken);
						entryPointToken = fileToken = AddFile(manifestModule, moduleBuilder.fileName, 0 /*ContainsMetaData*/);
					}
					else
					{
						ModuleWriter.WriteModule(null, null, moduleBuilder, fileKind, portableExecutableKind, imageFileMachine, null, moduleBuilder.unmanagedResources, 0);
						fileToken = AddFile(manifestModule, moduleBuilder.fileName, 0 /*ContainsMetaData*/);
					}
					moduleBuilder.ExportTypes(fileToken, manifestModule);
				}
			}

			if (entryPointToken == 0 && entryPoint != null)
			{
				entryPointToken = entryPoint.MetadataToken;
			}

			// finally, write the manifest module
			ModuleWriter.WriteModule(name.KeyPair, publicKey, manifestModule, fileKind, portableExecutableKind, imageFileMachine, versionInfoData, unmanagedResources ?? manifestModule.unmanagedResources, entryPointToken);
		}

		private static byte[] GetPublicKey(AssemblyName name)
		{
			StrongNameKeyPair keyPair = name.KeyPair;
			if (keyPair != null)
			{
				return keyPair.PublicKey;
			}
			byte[] key = name.GetPublicKey();
			if (key == null || key.Length == 0)
			{
				return null;
			}
			return key;
		}

		private int AddFile(ModuleBuilder manifestModule, string fileName, int flags)
		{
			SHA1Managed hash = new SHA1Managed();
			string fullPath = fileName;
			if (dir != null)
			{
				fullPath = Path.Combine(dir, fileName);
			}
			using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
			{
				using (CryptoStream cs = new CryptoStream(Stream.Null, hash, CryptoStreamMode.Write))
				{
					byte[] buf = new byte[8192];
					ModuleWriter.HashChunk(fs, cs, buf, (int)fs.Length);
				}
			}
			FileTable.Record file = new FileTable.Record();
			file.Flags = flags;
			file.Name = manifestModule.Strings.Add(fileName);
			file.HashValue = manifestModule.Blobs.Add(ByteBuffer.Wrap(hash.Hash));
			return 0x26000000 + manifestModule.File.AddRecord(file);
		}

		public void AddResourceFile(string name, string fileName)
		{
			AddResourceFile(name, fileName, ResourceAttributes.Public);
		}

		public void AddResourceFile(string name, string fileName, ResourceAttributes attribs)
		{
			ResourceFile resfile = new ResourceFile();
			resfile.Name = name;
			resfile.FileName = fileName;
			resfile.Attributes = attribs;
			resourceFiles.Add(resfile);
		}

		public void DefineVersionInfoResource()
		{
			versionInfo = new VersionInfo();
		}

		public void DefineVersionInfoResource(string product, string productVersion, string company, string copyright, string trademark)
		{
			versionInfo = new VersionInfo();
			versionInfo.product = product;
			versionInfo.informationalVersion = productVersion;
			versionInfo.company = company;
			versionInfo.copyright = copyright;
			versionInfo.trademark = trademark;
		}

		public void __DefineUnmanagedResource(byte[] resource)
		{
			// The standard .NET DefineUnmanagedResource(byte[]) is useless, because it embeds "resource" (as-is) as the .rsrc section,
			// but it doesn't set the PE file Resource Directory entry to point to it. That's why we have a renamed version, which behaves
			// like DefineUnmanagedResource(string).
			this.unmanagedResources = (byte[])resource.Clone();
		}

		public void DefineUnmanagedResource(string resourceFileName)
		{
			// This method reads the specified resource file (Win32 .res file) and converts it into the appropriate format and embeds it in the .rsrc section,
			// also setting the Resource Directory entry.
			this.unmanagedResources = File.ReadAllBytes(resourceFileName);
		}

		public override Type[] GetTypes()
		{
			List<Type> list = new List<Type>();
			foreach (ModuleBuilder module in modules)
			{
				module.GetTypesImpl(list);
			}
			return list.ToArray();
		}

		internal override Type GetTypeImpl(string typeName)
		{
			foreach (ModuleBuilder mb in modules)
			{
				Type type = mb.GetTypeImpl(typeName);
				if (type != null)
				{
					return type;
				}
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

		public override Module ManifestModule
		{
			get
			{
				if (pseudoManifestModule == null)
				{
					pseudoManifestModule = new ManifestModule(this);
				}
				return pseudoManifestModule;
			}
		}

		public override MethodInfo EntryPoint
		{
			get { return entryPoint; }
		}

		public override AssemblyName[] GetReferencedAssemblies()
		{
			return Empty<AssemblyName>.Array;
		}

		public override Module[] GetLoadedModules(bool getResourceModules)
		{
			return GetModules(getResourceModules);
		}

		public override Module[] GetModules(bool getResourceModules)
		{
			List<Module> list = new List<Module>();
			foreach (ModuleBuilder module in modules)
			{
				if (getResourceModules || !module.IsResource())
				{
					list.Add(module);
				}
			}
			return list.ToArray();
		}

		public override Module GetModule(string name)
		{
			foreach (ModuleBuilder module in modules)
			{
				if (module.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
				{
					return module;
				}
			}
			return null;
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

		internal override IList<CustomAttributeData> GetCustomAttributesData()
		{
			List<CustomAttributeData> list = new List<CustomAttributeData>();
			foreach (CustomAttributeBuilder cab in customAttributes)
			{
				list.Add(cab.ToData(this));
			}
			return list;
		}
	}

	sealed class ManifestModule : Module
	{
		private readonly AssemblyBuilder assembly;
		private readonly Guid guid = Guid.NewGuid();

		internal ManifestModule(AssemblyBuilder assembly)
			: base(assembly.universe)
		{
			this.assembly = assembly;
		}

		public override int MDStreamVersion
		{
			get { return assembly.mdStreamVersion; }
		}

		public override Assembly Assembly
		{
			get { return assembly; }
		}

		internal override Type GetTypeImpl(string typeName)
		{
			return null;
		}

		internal override void  GetTypesImpl(List<Type> list)
		{
		}

		public override string FullyQualifiedName
		{
			get { return Path.Combine(assembly.dir, "RefEmit_InMemoryManifestModule"); }
		}

		public override string Name
		{
			get { return "<In Memory Module>"; }
		}

		public override Guid ModuleVersionId
		{
			get { return guid; }
		}

		public override Type ResolveType(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			throw new ArgumentException();
		}

		public override MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			throw new ArgumentException();
		}

		public override FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			throw new ArgumentException();
		}

		public override MemberInfo ResolveMember(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			throw new ArgumentException();
		}

		public override string ResolveString(int metadataToken)
		{
			throw new ArgumentException();
		}

		public override Type[] __ResolveOptionalParameterTypes(int metadataToken)
		{
			throw new ArgumentException();
		}

		public override string ScopeName
		{
			get { return "RefEmit_InMemoryManifestModule"; }
		}

		internal override Type GetModuleType()
		{
			throw new InvalidOperationException();
		}

		internal override IKVM.Reflection.Reader.ByteReader GetBlob(int blobIndex)
		{
			throw new InvalidOperationException();
		}
	}
}
