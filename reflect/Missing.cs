/*
  Copyright (C) 2011 Jeroen Frijters

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

namespace IKVM.Reflection
{
	sealed class MissingAssembly : Assembly
	{
		private readonly Dictionary<string, Type> types = new Dictionary<string, Type>();
		private readonly MissingModule module;
		private readonly string name;

		internal MissingAssembly(Universe universe, string name)
			: base(universe)
		{
			module = new MissingModule(this);
			this.name = name;
		}

		internal override Type ResolveType(string ns, string name)
		{
			string fullName = ns == null ? name : ns + "." + name;
			Type type;
			if (!types.TryGetValue(fullName, out type))
			{
				type = new MissingType(module, null, ns, name);
				types.Add(fullName, type);
			}
			return type;
		}

		public override Type[] GetTypes()
		{
			throw new NotImplementedException();
		}

		public override string FullName
		{
			get { return name; }
		}

		public override AssemblyName GetName()
		{
			return new AssemblyName(name);
		}

		public override string ImageRuntimeVersion
		{
			get { throw new NotImplementedException(); }
		}

		public override Module ManifestModule
		{
			get { return module; }
		}

		public override MethodInfo EntryPoint
		{
			get { throw new NotImplementedException(); }
		}

		public override string Location
		{
			get { throw new NotImplementedException(); }
		}

		public override AssemblyName[] GetReferencedAssemblies()
		{
			throw new NotImplementedException();
		}

		public override Module[] GetModules(bool getResourceModules)
		{
			throw new NotImplementedException();
		}

		public override Module[] GetLoadedModules(bool getResourceModules)
		{
			throw new NotImplementedException();
		}

		public override Module GetModule(string name)
		{
			throw new NotImplementedException();
		}

		public override string[] GetManifestResourceNames()
		{
			throw new NotImplementedException();
		}

		public override ManifestResourceInfo GetManifestResourceInfo(string resourceName)
		{
			throw new NotImplementedException();
		}

		public override System.IO.Stream GetManifestResourceStream(string resourceName)
		{
			throw new NotImplementedException();
		}

		internal override Type GetTypeImpl(string typeName)
		{
			throw new NotImplementedException();
		}

		internal override IList<CustomAttributeData> GetCustomAttributesData(Type attributeType)
		{
			throw new NotImplementedException();
		}
	}

	sealed class MissingModule : Module
	{
		private readonly MissingAssembly assembly;

		internal MissingModule(MissingAssembly assembly)
			: base(assembly.universe)
		{
			this.assembly = assembly;
		}

		public override int MDStreamVersion
		{
			get { throw new NotImplementedException(); }
		}

		public override Assembly Assembly
		{
			get { return assembly; }
		}

		public override string FullyQualifiedName
		{
			get { throw new NotImplementedException(); }
		}

		public override string Name
		{
			get { throw new NotImplementedException(); }
		}

		public override Guid ModuleVersionId
		{
			get { throw new NotImplementedException(); }
		}

		public override Type ResolveType(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			throw new NotImplementedException();
		}

		public override MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			throw new NotImplementedException();
		}

		public override FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			throw new NotImplementedException();
		}

		public override MemberInfo ResolveMember(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			throw new NotImplementedException();
		}

		public override string ResolveString(int metadataToken)
		{
			throw new NotImplementedException();
		}

		public override Type[] __ResolveOptionalParameterTypes(int metadataToken)
		{
			throw new NotImplementedException();
		}

		public override string ScopeName
		{
			get { throw new NotImplementedException(); }
		}

		internal override Type GetTypeImpl(string typeName)
		{
			throw new NotImplementedException();
		}

		internal override void GetTypesImpl(System.Collections.Generic.List<Type> list)
		{
			throw new NotImplementedException();
		}

		public override AssemblyName[] __GetReferencedAssemblies()
		{
			throw new NotImplementedException();
		}

		internal override Type GetModuleType()
		{
			throw new NotImplementedException();
		}

		internal override IKVM.Reflection.Reader.ByteReader GetBlob(int blobIndex)
		{
			throw new NotImplementedException();
		}
	}

	sealed class MissingType : Type
	{
		private readonly MissingModule module;
		private readonly Type declaringType;
		private readonly string ns;
		private readonly string name;
		private Dictionary<string, Type> types;

		internal MissingType(MissingModule module, Type declaringType, string ns, string name)
		{
			this.module = module;
			this.declaringType = declaringType;
			this.ns = ns;
			this.name = name;
		}

		internal override Type ResolveNestedType(string ns, string name)
		{
			if (types == null)
			{
				types = new Dictionary<string, Type>();
			}
			string fullName = ns == null ? name : ns + "." + name;
			Type type;
			if (!types.TryGetValue(fullName, out type))
			{
				type = new MissingType(module, this, ns, name);
				types.Add(fullName, type);
			}
			return type;
		}

		public override Type DeclaringType
		{
			get { return declaringType; }
		}

		public override string __Name
		{
			get { return name; }
		}

		public override string __Namespace
		{
			get { return ns; }
		}

		public override string Name
		{
			get { return TypeNameParser.Escape(name); }
		}

		public override string FullName
		{
			get { return GetFullName(); }
		}

		public override Module Module
		{
			get { return module; }
		}

		public override Type BaseType
		{
			get { throw new NotImplementedException(); }
		}

		public override TypeAttributes Attributes
		{
			get { throw new NotImplementedException(); }
		}
	}
}
