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
using System.Configuration.Assemblies;
using System.IO;
using IKVM.Reflection.Metadata;

namespace IKVM.Reflection.Reader
{

	sealed class AssemblyReader : Assembly
	{

		private const int ContainsNoMetaData = 0x0001;
		private readonly string location;
		private readonly ModuleReader manifestModule;
		private readonly Module[] externalModules;

		internal AssemblyReader(string location, ModuleReader manifestModule)
			: base(manifestModule.universe)
		{
			this.location = location;
			this.manifestModule = manifestModule;
			externalModules = new Module[manifestModule.File.records.Length];
		}

		public override string Location
		{
			get { return location ?? ""; }
		}

		public override AssemblyName GetName()
		{
			return GetNameImpl(ref manifestModule.AssemblyTable.records[0]);
		}

		private AssemblyName GetNameImpl(ref AssemblyTable.Record rec)
		{
			AssemblyName name = new AssemblyName();
			name.Name = manifestModule.GetString(rec.Name);
			name.Version = new Version(rec.MajorVersion, rec.MinorVersion, rec.BuildNumber, rec.RevisionNumber);
			if (rec.PublicKey != 0)
			{
				name.SetPublicKey(manifestModule.GetBlobCopy(rec.PublicKey));
			}
			else
			{
				name.SetPublicKey(Empty<byte>.Array);
			}
			if (rec.Culture != 0)
			{
				name.CultureName = manifestModule.GetString(rec.Culture);
			}
			else
			{
				name.CultureName = "";
			}
			name.HashAlgorithm = (AssemblyHashAlgorithm)rec.HashAlgId;
			name.CodeBase = this.CodeBase;
			PortableExecutableKinds peKind;
			ImageFileMachine machine;
			manifestModule.GetPEKind(out peKind, out machine);
			switch (machine)
			{
				case ImageFileMachine.I386:
					// FXBUG we copy the .NET bug that Preferred32Bit implies x86
					if ((peKind & (PortableExecutableKinds.Required32Bit | PortableExecutableKinds.Preferred32Bit)) != 0)
					{
						name.ProcessorArchitecture = ProcessorArchitecture.X86;
					}
					else if ((rec.Flags & 0x70) == 0x70)
					{
						// it's a reference assembly
						name.ProcessorArchitecture = ProcessorArchitecture.None;
					}
					else
					{
						name.ProcessorArchitecture = ProcessorArchitecture.MSIL;
					}
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
			name.RawFlags = (AssemblyNameFlags)rec.Flags;
			return name;
		}

		public override Type[] GetTypes()
		{
			if (externalModules.Length == 0)
			{
				return manifestModule.GetTypes();
			}

			List<Type> list = new List<Type>();
			foreach (Module module in GetModules(false))
			{
				list.AddRange(module.GetTypes());
			}
			return list.ToArray();
		}

		internal override Type FindType(TypeName typeName)
		{
			Type type = manifestModule.FindType(typeName);
			for (int i = 0; type == null && i < externalModules.Length; i++)
			{
				if ((manifestModule.File.records[i].Flags & ContainsNoMetaData) == 0)
				{
					type = GetModule(i).FindType(typeName);
				}
			}
			return type;
		}

		internal override Type FindTypeIgnoreCase(TypeName lowerCaseName)
		{
			Type type = manifestModule.FindTypeIgnoreCase(lowerCaseName);
			for (int i = 0; type == null && i < externalModules.Length; i++)
			{
				if ((manifestModule.File.records[i].Flags & ContainsNoMetaData) == 0)
				{
					type = GetModule(i).FindTypeIgnoreCase(lowerCaseName);
				}
			}
			return type;
		}

		public override string ImageRuntimeVersion
		{
			get { return manifestModule.__ImageRuntimeVersion; }
		}

		public override Module ManifestModule
		{
			get { return manifestModule; }
		}

		public override Module[] GetLoadedModules(bool getResourceModules)
		{
			List<Module> list = new List<Module>();
			list.Add(manifestModule);
			foreach (Module m in externalModules)
			{
				if (m != null)
				{
					list.Add(m);
				}
			}
			return list.ToArray();
		}

		public override Module[] GetModules(bool getResourceModules)
		{
			if (externalModules.Length == 0)
			{
				return new Module[] { manifestModule };
			}
			else
			{
				List<Module> list = new List<Module>();
				list.Add(manifestModule);
				for (int i = 0; i < manifestModule.File.records.Length; i++)
				{
					if (getResourceModules || (manifestModule.File.records[i].Flags & ContainsNoMetaData) == 0)
					{
						list.Add(GetModule(i));
					}
				}
				return list.ToArray();
			}
		}

		public override Module GetModule(string name)
		{
			if (name.Equals(manifestModule.ScopeName, StringComparison.OrdinalIgnoreCase))
			{
				return manifestModule;
			}
			int index = GetModuleIndex(name);
			if (index != -1)
			{
				return GetModule(index);
			}
			return null;
		}

		private int GetModuleIndex(string name)
		{
			for (int i = 0; i < manifestModule.File.records.Length; i++)
			{
				if (name.Equals(manifestModule.GetString(manifestModule.File.records[i].Name), StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		private Module GetModule(int index)
		{
			if (externalModules[index] != null)
			{
				return externalModules[index];
			}
			return LoadModule(index, null, manifestModule.GetString(manifestModule.File.records[index].Name));
		}

		private Module LoadModule(int index, byte[] rawModule, string name)
		{
			string location = name == null ? null : Path.Combine(Path.GetDirectoryName(this.location), name);
			if ((manifestModule.File.records[index].Flags & ContainsNoMetaData) != 0)
			{
				return externalModules[index] = new ResourceModule(manifestModule, index, location);
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
							ResolveEventArgs arg = new ResolveEventArgs(name, this);
							foreach (ModuleResolveEventHandler resolver in resolvers)
							{
								Module module = resolver(this, arg);
								if (module != null)
								{
									return module;
								}
							}
						}
						if (universe.MissingMemberResolution)
						{
							return externalModules[index] = new MissingModule(this, index);
						}
						throw;
					}
				}
				return externalModules[index] = new ModuleReader(this, manifestModule.universe, new MemoryStream(rawModule), location, false);
			}
		}

		public override Module LoadModule(string moduleName, byte[] rawModule)
		{
			int index = GetModuleIndex(moduleName);
			if (index == -1)
			{
				throw new ArgumentException();
			}
			if (externalModules[index] != null)
			{
				return externalModules[index];
			}
			return LoadModule(index, rawModule, null);
		}

		public override MethodInfo EntryPoint
		{
			get { return manifestModule.GetEntryPoint(); }
		}

		public override string[] GetManifestResourceNames()
		{
			return manifestModule.GetManifestResourceNames();
		}

		public override ManifestResourceInfo GetManifestResourceInfo(string resourceName)
		{
			return manifestModule.GetManifestResourceInfo(resourceName);
		}

		public override Stream GetManifestResourceStream(string resourceName)
		{
			return manifestModule.GetManifestResourceStream(resourceName);
		}

		public override AssemblyName[] GetReferencedAssemblies()
		{
			return manifestModule.__GetReferencedAssemblies();
		}

		protected override AssemblyNameFlags GetAssemblyFlags()
		{
			return (AssemblyNameFlags)manifestModule.AssemblyTable.records[0].Flags;
		}

		internal string Name
		{
			get { return manifestModule.GetString(manifestModule.AssemblyTable.records[0].Name); }
		}

		internal override IList<CustomAttributeData> GetCustomAttributesData(Type attributeType)
		{
			return CustomAttributeData.GetCustomAttributesImpl(null, manifestModule, 0x20000001, attributeType) ?? CustomAttributeData.EmptyList;
		}
	}
}
