/*
  Copyright (C) 2008, 2009 Jeroen Frijters

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
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;
using IKVM.Reflection.Emit.Writer;
using IKVM.Reflection.Emit.Impl;
using System.Security;

namespace IKVM.Reflection.Emit
{
	public sealed class AssemblyBuilder :
#if NET_4_0
		Assembly
#else
		IkvmAssembly
#endif
	{
		private readonly AssemblyName name;
		private readonly string dir;
		private readonly PermissionSet requiredPermissions;
		private readonly PermissionSet optionalPermissions;
		private readonly PermissionSet refusedPermissions;
		private PEFileKinds fileKind = PEFileKinds.Dll;
		private MethodBuilder entryPoint;
		private bool setVersionInfo;
		private string imageRuntimeVersion = typeof(object).Assembly.ImageRuntimeVersion;
		private List<ResourceFile> resourceFiles = new List<ResourceFile>();
		private List<ModuleBuilder> modules = new List<ModuleBuilder>();
		private List<CustomAttributeBuilder> customAttributes = new List<CustomAttributeBuilder>();

		private struct ResourceFile
		{
			internal string Name;
			internal string FileName;
			internal ResourceAttributes Attributes;
		}

		private AssemblyBuilder(AssemblyName name, string dir, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions)
		{
			this.name = name;
			this.dir = dir;
			this.requiredPermissions = requiredPermissions;
			this.optionalPermissions = optionalPermissions;
			this.refusedPermissions = refusedPermissions;
		}

		public static AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access)
		{
			return DefineDynamicAssembly(name, access, null);
		}

		public static AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir)
		{
			return new AssemblyBuilder(name, dir, null, null, null);
		}

#if NET_4_0
		[Obsolete]
#endif
		public static AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions)
		{
			return new AssemblyBuilder(name, dir, requiredPermissions, optionalPermissions, refusedPermissions);
		}

		public override AssemblyName GetName()
		{
			AssemblyName n = new AssemblyName();
			n.Name = name.Name;
			n.Version = name.Version ?? new Version(0, 0, 0, 0);
			n.CultureInfo = name.CultureInfo ?? System.Globalization.CultureInfo.InvariantCulture;
			if (name.KeyPair != null)
			{
				n.SetPublicKey(name.KeyPair.PublicKey);
			}
			else
			{
				n.SetPublicKey(new byte[0]);
			}
			return n;
		}

		public override string FullName
		{
			get { return GetName().FullName; }
		}

		public ModuleBuilder DefineDynamicModule(string name, string fileName, bool emitSymbolInfo)
		{
			ModuleBuilder module = new ModuleBuilder(this, name, fileName, emitSymbolInfo);
			modules.Add(module);
			return module;
		}

		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder.Constructor.DeclaringType == typeof(AssemblyVersionAttribute))
			{
				throw new NotImplementedException();
			}
			else
			{
				customAttributes.Add(customBuilder);
			}
		}

		public void SetEntryPoint(MethodBuilder mb)
		{
			SetEntryPoint(mb, PEFileKinds.ConsoleApplication);
		}

		public void SetEntryPoint(MethodBuilder mb, PEFileKinds kind)
		{
			this.entryPoint = mb;
			this.fileKind = kind;
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

			TableHeap.AssemblyTable.Record assemblyRecord = new TableHeap.AssemblyTable.Record();
			assemblyRecord.HashAlgId = 0x8004;	// SHA1
			assemblyRecord.Name = manifestModule.Strings.Add(name.Name);
			if (name.Version != null)
			{
				assemblyRecord.MajorVersion = (short)name.Version.Major;
				assemblyRecord.MinorVersion = (short)name.Version.Minor;
				assemblyRecord.BuildNumber = (short)name.Version.Build;
				assemblyRecord.RevisionNumber = (short)name.Version.Revision;
			}
			if (name.KeyPair != null)
			{
				assemblyRecord.PublicKey = manifestModule.Blobs.Add(ByteBuffer.Wrap(name.KeyPair.PublicKey));
				assemblyRecord.Flags |= 0x0001;	// PublicKey
			}
			if (name.CultureInfo != null)
			{
				assemblyRecord.Culture = manifestModule.Strings.Add(name.CultureInfo.Name);
			}
			int token = 0x20000000 + manifestModule.Tables.Assembly.AddRecord(assemblyRecord);

#pragma warning disable 618
			// this values are obsolete, but we already know that so we disable the warning
			System.Security.Permissions.SecurityAction requestMinimum = System.Security.Permissions.SecurityAction.RequestMinimum;
			System.Security.Permissions.SecurityAction requestOptional = System.Security.Permissions.SecurityAction.RequestOptional;
			System.Security.Permissions.SecurityAction requestRefuse = System.Security.Permissions.SecurityAction.RequestRefuse;
#pragma warning restore 618
			if (requiredPermissions != null)
			{
				manifestModule.AddDeclaritiveSecurity(token, requestMinimum, requiredPermissions);
			}
			if (optionalPermissions != null)
			{
				manifestModule.AddDeclaritiveSecurity(token, requestOptional, optionalPermissions);
			}
			if (refusedPermissions != null)
			{
				manifestModule.AddDeclaritiveSecurity(token, requestRefuse, refusedPermissions);
			}

			ByteBuffer versionInfoData = null;
			if (setVersionInfo)
			{
				VersionInfo versionInfo = new VersionInfo();
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

			foreach (ResourceFile resfile in resourceFiles)
			{
				int fileToken = AddFile(manifestModule, resfile.FileName, 1 /*ContainsNoMetaData*/);
				TableHeap.ManifestResourceTable.Record rec = new TableHeap.ManifestResourceTable.Record();
				rec.Offset = 0;
				rec.Flags = (int)resfile.Attributes;
				rec.Name = manifestModule.Strings.Add(resfile.Name);
				rec.Implementation = fileToken;
				manifestModule.Tables.ManifestResource.AddRecord(rec);
			}

			int entryPointToken = 0;

			foreach (ModuleBuilder moduleBuilder in modules)
			{
				if (moduleBuilder != manifestModule)
				{
					int fileToken;
					if (entryPoint != null && entryPoint.ModuleBuilder == moduleBuilder)
					{
						ModuleWriter.WriteModule(dir, null, moduleBuilder, fileKind, portableExecutableKind, imageFileMachine, null, entryPoint.MetadataToken);
						entryPointToken = fileToken = AddFile(manifestModule, moduleBuilder.fileName, 0 /*ContainsMetaData*/);
					}
					else
					{
						ModuleWriter.WriteModule(dir, null, moduleBuilder, fileKind, portableExecutableKind, imageFileMachine, null, 0);
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
			ModuleWriter.WriteModule(dir, name.KeyPair, manifestModule, fileKind, portableExecutableKind, imageFileMachine, versionInfoData, entryPointToken);
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
			TableHeap.FileTable.Record file = new TableHeap.FileTable.Record();
			file.Flags = flags;
			file.Name = manifestModule.Strings.Add(fileName);
			file.HashValue = manifestModule.Blobs.Add(ByteBuffer.Wrap(hash.Hash));
			return 0x26000000 + manifestModule.Tables.File.AddRecord(file);
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
			setVersionInfo = true;
		}

		public override Type GetType(string typeName)
		{
			foreach (ModuleBuilder mb in modules)
			{
				Type type = mb.GetType(typeName);
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
	}
}
