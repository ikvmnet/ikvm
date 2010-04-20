/*
  Copyright (C) 2009-2010 Jeroen Frijters

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
using System.Security;
using System.Text;
using System.Diagnostics;
using IKVM.Reflection.Reader;
using IKVM.Reflection.Emit;

namespace IKVM.Reflection
{
	public class ResolveEventArgs : EventArgs
	{
		private readonly string name;
		private readonly Assembly requestingAssembly;

		public ResolveEventArgs(string name)
			: this(name, null)
		{
		}

		public ResolveEventArgs(string name, Assembly requestingAssembly)
		{
			this.name = name;
			this.requestingAssembly = requestingAssembly;
		}

		public string Name
		{
			get { return name; }
		}

		public Assembly RequestingAssembly
		{
			get { return requestingAssembly; }
		}
	}

	public delegate Assembly ResolveEventHandler(object sender, ResolveEventArgs args);

	public sealed class Universe
	{
		internal readonly Dictionary<Type, Type> canonicalizedTypes = new Dictionary<Type, Type>();
		private readonly List<Assembly> assemblies = new List<Assembly>();
		private readonly Dictionary<string, Assembly> assembliesByName = new Dictionary<string, Assembly>();
		private readonly Dictionary<System.Type, Type> importedTypes = new Dictionary<System.Type, Type>();
		private Type typeof_System_Object;
		private Type typeof_System_ValueType;
		private Type typeof_System_Enum;
		private Type typeof_System_Void;
		private Type typeof_System_Boolean;
		private Type typeof_System_Char;
		private Type typeof_System_SByte;
		private Type typeof_System_Byte;
		private Type typeof_System_Int16;
		private Type typeof_System_UInt16;
		private Type typeof_System_Int32;
		private Type typeof_System_UInt32;
		private Type typeof_System_Int64;
		private Type typeof_System_UInt64;
		private Type typeof_System_Single;
		private Type typeof_System_Double;
		private Type typeof_System_String;
		private Type typeof_System_IntPtr;
		private Type typeof_System_UIntPtr;
		private Type typeof_System_TypedReference;
		private Type typeof_System_Type;
		private Type typeof_System_Array;
		private Type typeof_System_DateTime;
		private Type typeof_System_DBNull;
		private Type typeof_System_Decimal;
		private Type typeof_System_NonSerializedAttribute;
		private Type typeof_System_SerializableAttribute;
		private Type typeof_System_Reflection_AssemblyFlagsAttribute;
		private Type typeof_System_Reflection_AssemblyAlgorithmIdAttribute;
		private Type typeof_System_Reflection_AssemblyVersionAttribute;
		private Type typeof_System_Reflection_AssemblyKeyFileAttribute;
		private Type typeof_System_Reflection_AssemblyKeyNameAttribute;
		private Type typeof_System_Reflection_AssemblyCultureAttribute;
		private Type typeof_System_Runtime_InteropServices_DllImportAttribute;
		private Type typeof_System_Runtime_InteropServices_FieldOffsetAttribute;
		private Type typeof_System_Runtime_InteropServices_InAttribute;
		private Type typeof_System_Runtime_InteropServices_MarshalAsAttribute;
		private Type typeof_System_Runtime_InteropServices_UnmanagedType;
		private Type typeof_System_Runtime_InteropServices_VarEnum;
		private Type typeof_System_Runtime_InteropServices_OutAttribute;
		private Type typeof_System_Runtime_InteropServices_StructLayoutAttribute;
		private Type typeof_System_Runtime_InteropServices_OptionalAttribute;
		private Type typeof_System_Runtime_InteropServices_PreserveSigAttribute;
		private Type typeof_System_Runtime_InteropServices_ComImportAttribute;
		private Type typeof_System_Runtime_InteropServices_DefaultParameterValueAttribute;
		private Type typeof_System_Runtime_CompilerServices_TypeForwardedToAttribute;
		private Type typeof_System_Runtime_CompilerServices_SpecialNameAttribute;
		private Type typeof_System_Runtime_CompilerServices_MethodImplAttribute;
		private Type typeof_System_Security_SuppressUnmanagedCodeSecurityAttribute;
		private Type typeof_System_Reflection_AssemblyCopyrightAttribute;
		private Type typeof_System_Reflection_AssemblyTrademarkAttribute;
		private Type typeof_System_Reflection_AssemblyProductAttribute;
		private Type typeof_System_Reflection_AssemblyCompanyAttribute;
		private Type typeof_System_Reflection_AssemblyDescriptionAttribute;
		private Type typeof_System_Reflection_AssemblyTitleAttribute;
		private Type typeof_System_Reflection_AssemblyInformationalVersionAttribute;
		private Type typeof_System_Reflection_AssemblyFileVersionAttribute;
		private Type typeof_System_Security_Permissions_HostProtectionAttribute;
		private Type typeof_System_Security_Permissions_PermissionSetAttribute;
		private Type typeof_System_Security_Permissions_SecurityAction;
		private List<ResolveEventHandler> resolvers = new List<ResolveEventHandler>();

		internal Type System_Object
		{
			get { return typeof_System_Object ?? (typeof_System_Object = Import(typeof(System.Object))); }
		}

		internal Type System_ValueType
		{
			get { return typeof_System_ValueType ?? (typeof_System_ValueType = Import(typeof(System.ValueType))); }
		}

		internal Type System_Enum
		{
			get { return typeof_System_Enum ?? (typeof_System_Enum = Import(typeof(System.Enum))); }
		}

		internal Type System_Void
		{
			get { return typeof_System_Void ?? (typeof_System_Void = Import(typeof(void))); }
		}

		internal Type System_Boolean
		{
			get { return typeof_System_Boolean ?? (typeof_System_Boolean = Import(typeof(System.Boolean))); }
		}

		internal Type System_Char
		{
			get { return typeof_System_Char ?? (typeof_System_Char = Import(typeof(System.Char))); }
		}

		internal Type System_SByte
		{
			get { return typeof_System_SByte ?? (typeof_System_SByte = Import(typeof(System.SByte))); }
		}

		internal Type System_Byte
		{
			get { return typeof_System_Byte ?? (typeof_System_Byte = Import(typeof(System.Byte))); }
		}

		internal Type System_Int16
		{
			get { return typeof_System_Int16 ?? (typeof_System_Int16 = Import(typeof(System.Int16))); }
		}

		internal Type System_UInt16
		{
			get { return typeof_System_UInt16 ?? (typeof_System_UInt16 = Import(typeof(System.UInt16))); }
		}

		internal Type System_Int32
		{
			get { return typeof_System_Int32 ?? (typeof_System_Int32 = Import(typeof(System.Int32))); }
		}

		internal Type System_UInt32
		{
			get { return typeof_System_UInt32 ?? (typeof_System_UInt32 = Import(typeof(System.UInt32))); }
		}

		internal Type System_Int64
		{
			get { return typeof_System_Int64 ?? (typeof_System_Int64 = Import(typeof(System.Int64))); }
		}

		internal Type System_UInt64
		{
			get { return typeof_System_UInt64 ?? (typeof_System_UInt64 = Import(typeof(System.UInt64))); }
		}

		internal Type System_Single
		{
			get { return typeof_System_Single ?? (typeof_System_Single = Import(typeof(System.Single))); }
		}

		internal Type System_Double
		{
			get { return typeof_System_Double ?? (typeof_System_Double = Import(typeof(System.Double))); }
		}

		internal Type System_String
		{
			get { return typeof_System_String ?? (typeof_System_String = Import(typeof(System.String))); }
		}

		internal Type System_IntPtr
		{
			get { return typeof_System_IntPtr ?? (typeof_System_IntPtr = Import(typeof(System.IntPtr))); }
		}

		internal Type System_UIntPtr
		{
			get { return typeof_System_UIntPtr ?? (typeof_System_UIntPtr = Import(typeof(System.UIntPtr))); }
		}

		internal Type System_TypedReference
		{
			get { return typeof_System_TypedReference ?? (typeof_System_TypedReference = Import(typeof(System.TypedReference))); }
		}

		internal Type System_Type
		{
			get { return typeof_System_Type ?? (typeof_System_Type = Import(typeof(System.Type))); }
		}

		internal Type System_Array
		{
			get { return typeof_System_Array ?? (typeof_System_Array = Import(typeof(System.Array))); }
		}

		internal Type System_DateTime
		{
			get { return typeof_System_DateTime ?? (typeof_System_DateTime = Import(typeof(System.DateTime))); }
		}

		internal Type System_DBNull
		{
			get { return typeof_System_DBNull ?? (typeof_System_DBNull = Import(typeof(System.DBNull))); }
		}

		internal Type System_Decimal
		{
			get { return typeof_System_Decimal ?? (typeof_System_Decimal = Import(typeof(System.Decimal))); }
		}

		internal Type System_NonSerializedAttribute
		{
			get { return typeof_System_NonSerializedAttribute ?? (typeof_System_NonSerializedAttribute = Import(typeof(System.NonSerializedAttribute))); }
		}

		internal Type System_SerializableAttribute
		{
			get { return typeof_System_SerializableAttribute ?? (typeof_System_SerializableAttribute = Import(typeof(System.SerializableAttribute))); }
		}

		internal Type System_Reflection_AssemblyFlagsAttribute
		{
			get { return typeof_System_Reflection_AssemblyFlagsAttribute ?? (typeof_System_Reflection_AssemblyFlagsAttribute = Import(typeof(System.Reflection.AssemblyFlagsAttribute))); }
		}

		internal Type System_Reflection_AssemblyAlgorithmIdAttribute
		{
			get { return typeof_System_Reflection_AssemblyAlgorithmIdAttribute ?? (typeof_System_Reflection_AssemblyAlgorithmIdAttribute = Import(typeof(System.Reflection.AssemblyAlgorithmIdAttribute))); }
		}

		internal Type System_Reflection_AssemblyVersionAttribute
		{
			get { return typeof_System_Reflection_AssemblyVersionAttribute ?? (typeof_System_Reflection_AssemblyVersionAttribute = Import(typeof(System.Reflection.AssemblyVersionAttribute))); }
		}

		internal Type System_Reflection_AssemblyKeyFileAttribute
		{
			get { return typeof_System_Reflection_AssemblyKeyFileAttribute ?? (typeof_System_Reflection_AssemblyKeyFileAttribute = Import(typeof(System.Reflection.AssemblyKeyFileAttribute))); }
		}

		internal Type System_Reflection_AssemblyKeyNameAttribute
		{
			get { return typeof_System_Reflection_AssemblyKeyNameAttribute ?? (typeof_System_Reflection_AssemblyKeyNameAttribute = Import(typeof(System.Reflection.AssemblyKeyNameAttribute))); }
		}

		internal Type System_Reflection_AssemblyCultureAttribute
		{
			get { return typeof_System_Reflection_AssemblyCultureAttribute ?? (typeof_System_Reflection_AssemblyCultureAttribute = Import(typeof(System.Reflection.AssemblyCultureAttribute))); }
		}

		internal Type System_Runtime_InteropServices_DllImportAttribute
		{
			get { return typeof_System_Runtime_InteropServices_DllImportAttribute ?? (typeof_System_Runtime_InteropServices_DllImportAttribute = Import(typeof(System.Runtime.InteropServices.DllImportAttribute))); }
		}

		internal Type System_Runtime_InteropServices_FieldOffsetAttribute
		{
			get { return typeof_System_Runtime_InteropServices_FieldOffsetAttribute ?? (typeof_System_Runtime_InteropServices_FieldOffsetAttribute = Import(typeof(System.Runtime.InteropServices.FieldOffsetAttribute))); }
		}

		internal Type System_Runtime_InteropServices_InAttribute
		{
			get { return typeof_System_Runtime_InteropServices_InAttribute ?? (typeof_System_Runtime_InteropServices_InAttribute = Import(typeof(System.Runtime.InteropServices.InAttribute))); }
		}

		internal Type System_Runtime_InteropServices_MarshalAsAttribute
		{
			get { return typeof_System_Runtime_InteropServices_MarshalAsAttribute ?? (typeof_System_Runtime_InteropServices_MarshalAsAttribute = Import(typeof(System.Runtime.InteropServices.MarshalAsAttribute))); }
		}

		internal Type System_Runtime_InteropServices_UnmanagedType
		{
			get { return typeof_System_Runtime_InteropServices_UnmanagedType ?? (typeof_System_Runtime_InteropServices_UnmanagedType = Import(typeof(System.Runtime.InteropServices.UnmanagedType))); }
		}

		internal Type System_Runtime_InteropServices_VarEnum
		{
			get { return typeof_System_Runtime_InteropServices_VarEnum ?? (typeof_System_Runtime_InteropServices_VarEnum = Import(typeof(System.Runtime.InteropServices.VarEnum))); }
		}

		internal Type System_Runtime_InteropServices_OutAttribute
		{
			get { return typeof_System_Runtime_InteropServices_OutAttribute ?? (typeof_System_Runtime_InteropServices_OutAttribute = Import(typeof(System.Runtime.InteropServices.OutAttribute))); }
		}

		internal Type System_Runtime_InteropServices_StructLayoutAttribute
		{
			get { return typeof_System_Runtime_InteropServices_StructLayoutAttribute ?? (typeof_System_Runtime_InteropServices_StructLayoutAttribute = Import(typeof(System.Runtime.InteropServices.StructLayoutAttribute))); }
		}

		internal Type System_Runtime_InteropServices_OptionalAttribute
		{
			get { return typeof_System_Runtime_InteropServices_OptionalAttribute ?? (typeof_System_Runtime_InteropServices_OptionalAttribute = Import(typeof(System.Runtime.InteropServices.OptionalAttribute))); }
		}

		internal Type System_Runtime_InteropServices_PreserveSigAttribute
		{
			get { return typeof_System_Runtime_InteropServices_PreserveSigAttribute ?? (typeof_System_Runtime_InteropServices_PreserveSigAttribute = Import(typeof(System.Runtime.InteropServices.PreserveSigAttribute))); }
		}

		internal Type System_Runtime_InteropServices_ComImportAttribute
		{
			get { return typeof_System_Runtime_InteropServices_ComImportAttribute ?? (typeof_System_Runtime_InteropServices_ComImportAttribute = Import(typeof(System.Runtime.InteropServices.ComImportAttribute))); }
		}

		internal Type System_Runtime_InteropServices_DefaultParameterValueAttribute
		{
			get { return typeof_System_Runtime_InteropServices_DefaultParameterValueAttribute ?? (typeof_System_Runtime_InteropServices_DefaultParameterValueAttribute = Import(typeof(System.Runtime.InteropServices.DefaultParameterValueAttribute))); }
		}

		internal Type System_Runtime_CompilerServices_TypeForwardedToAttribute
		{
			get { return typeof_System_Runtime_CompilerServices_TypeForwardedToAttribute ?? (typeof_System_Runtime_CompilerServices_TypeForwardedToAttribute = Import(typeof(System.Runtime.CompilerServices.TypeForwardedToAttribute))); }
		}

		internal Type System_Runtime_CompilerServices_SpecialNameAttribute
		{
			get { return typeof_System_Runtime_CompilerServices_SpecialNameAttribute ?? (typeof_System_Runtime_CompilerServices_SpecialNameAttribute = Import(typeof(System.Runtime.CompilerServices.SpecialNameAttribute))); }
		}

		internal Type System_Runtime_CompilerServices_MethodImplAttribute
		{
			get { return typeof_System_Runtime_CompilerServices_MethodImplAttribute ?? (typeof_System_Runtime_CompilerServices_MethodImplAttribute = Import(typeof(System.Runtime.CompilerServices.MethodImplAttribute))); }
		}

		internal Type System_Security_SuppressUnmanagedCodeSecurityAttribute
		{
			get { return typeof_System_Security_SuppressUnmanagedCodeSecurityAttribute ?? (typeof_System_Security_SuppressUnmanagedCodeSecurityAttribute = Import(typeof(System.Security.SuppressUnmanagedCodeSecurityAttribute))); }
		}

		internal Type System_Reflection_AssemblyCopyrightAttribute
		{
			get { return typeof_System_Reflection_AssemblyCopyrightAttribute ?? (typeof_System_Reflection_AssemblyCopyrightAttribute = Import(typeof(System.Reflection.AssemblyCopyrightAttribute))); }
		}

		internal Type System_Reflection_AssemblyTrademarkAttribute
		{
			get { return typeof_System_Reflection_AssemblyTrademarkAttribute ?? (typeof_System_Reflection_AssemblyTrademarkAttribute = Import(typeof(System.Reflection.AssemblyTrademarkAttribute))); }
		}

		internal Type System_Reflection_AssemblyProductAttribute
		{
			get { return typeof_System_Reflection_AssemblyProductAttribute ?? (typeof_System_Reflection_AssemblyProductAttribute = Import(typeof(System.Reflection.AssemblyProductAttribute))); }
		}

		internal Type System_Reflection_AssemblyCompanyAttribute
		{
			get { return typeof_System_Reflection_AssemblyCompanyAttribute ?? (typeof_System_Reflection_AssemblyCompanyAttribute = Import(typeof(System.Reflection.AssemblyCompanyAttribute))); }
		}

		internal Type System_Reflection_AssemblyDescriptionAttribute
		{
			get { return typeof_System_Reflection_AssemblyDescriptionAttribute ?? (typeof_System_Reflection_AssemblyDescriptionAttribute = Import(typeof(System.Reflection.AssemblyDescriptionAttribute))); }
		}

		internal Type System_Reflection_AssemblyTitleAttribute
		{
			get { return typeof_System_Reflection_AssemblyTitleAttribute ?? (typeof_System_Reflection_AssemblyTitleAttribute = Import(typeof(System.Reflection.AssemblyTitleAttribute))); }
		}

		internal Type System_Reflection_AssemblyInformationalVersionAttribute
		{
			get { return typeof_System_Reflection_AssemblyInformationalVersionAttribute ?? (typeof_System_Reflection_AssemblyInformationalVersionAttribute = Import(typeof(System.Reflection.AssemblyInformationalVersionAttribute))); }
		}

		internal Type System_Reflection_AssemblyFileVersionAttribute
		{
			get { return typeof_System_Reflection_AssemblyFileVersionAttribute ?? (typeof_System_Reflection_AssemblyFileVersionAttribute = Import(typeof(System.Reflection.AssemblyFileVersionAttribute))); }
		}

		internal Type System_Security_Permissions_HostProtectionAttribute
		{
			get { return typeof_System_Security_Permissions_HostProtectionAttribute ?? (typeof_System_Security_Permissions_HostProtectionAttribute = Import(typeof(System.Security.Permissions.HostProtectionAttribute))); }
		}

		internal Type System_Security_Permissions_PermissionSetAttribute
		{
			get { return typeof_System_Security_Permissions_PermissionSetAttribute ?? (typeof_System_Security_Permissions_PermissionSetAttribute = Import(typeof(System.Security.Permissions.PermissionSetAttribute))); }
		}

		internal Type System_Security_Permissions_SecurityAction
		{
			get { return typeof_System_Security_Permissions_SecurityAction ?? (typeof_System_Security_Permissions_SecurityAction = Import(typeof(System.Security.Permissions.SecurityAction))); }
		}

		public event ResolveEventHandler AssemblyResolve
		{
			add { resolvers.Add(value); }
			remove { resolvers.Remove(value); }
		}

		public void LoadMscorlib(string path)
		{
			if (importedTypes.Count != 0 || assembliesByName.Count != 0)
			{
				throw new InvalidOperationException();
			}
			Assembly asm = LoadFile(path);
			if (asm.FullName != typeof(object).Assembly.FullName)
			{
				// make the current mscorlib full name an alias for the specified mscorlib,
				// this ensures that all the runtime built in type are resolved against the
				// specified mscorlib.
				assembliesByName.Add(typeof(object).Assembly.FullName, asm);
			}
		}

		public Type Import(System.Type type)
		{
			Type imported;
			if (!importedTypes.TryGetValue(type, out imported))
			{
				imported = ImportImpl(type);
				importedTypes.Add(type, imported);
			}
			return imported;
		}

		private Type ImportImpl(System.Type type)
		{
			if (type == typeof(IKVM.Reflection.Type))
			{
				throw new ArgumentException("Did you really want to import IKVM.Reflection.Type?");
			}
			if (type.HasElementType)
			{
				if (type.IsArray)
				{
					if (type.Name.EndsWith("[]"))
					{
						return Import(type.GetElementType()).MakeArrayType();
					}
					else
					{
						return Import(type.GetElementType()).MakeArrayType(type.GetArrayRank());
					}
				}
				else if (type.IsByRef)
				{
					return Import(type.GetElementType()).MakeByRefType();
				}
				else if (type.IsPointer)
				{
					return Import(type.GetElementType()).MakePointerType();
				}
				else
				{
					throw new InvalidOperationException();
				}
			}
			else if (type.IsGenericParameter)
			{
				throw new NotSupportedException();
			}
			else if (type.IsGenericType && !type.IsGenericTypeDefinition)
			{
				System.Type[] args = type.GetGenericArguments();
				Type[] importedArgs = new Type[args.Length];
				for (int i = 0; i < args.Length; i++)
				{
					importedArgs[i] = Import(args[i]);
				}
				return Import(type.GetGenericTypeDefinition()).MakeGenericType(importedArgs);
			}
			else
			{
				return Import(type.Assembly).GetType(type.FullName);
			}
		}

		private static string GetAssemblyIdentityName(AssemblyName name)
		{
			byte[] publicKeyToken = name.GetPublicKeyToken();
			if (publicKeyToken == null || publicKeyToken.Length == 0)
			{
				return name.Name;
			}
			return name.FullName;
		}

		private static string GetAssemblyIdentityName(System.Reflection.Assembly asm)
		{
			System.Reflection.AssemblyName name = asm.GetName();
			byte[] publicKeyToken = name.GetPublicKeyToken();
			if (publicKeyToken == null || publicKeyToken.Length == 0)
			{
				return name.Name;
			}
			return name.FullName;
		}

		private static bool TryParseAssemblyIdentityName(string assemblyName, out string simpleName)
		{
			// we should probably have our own parser
			AssemblyName name = new AssemblyName(assemblyName);
			byte[] key = name.GetPublicKeyToken();
			if (key == null || key.Length == 0)
			{
				simpleName = name.Name;
				return true;
			}
			simpleName = null;
			return false;
		}

		private Assembly Import(System.Reflection.Assembly asm)
		{
			Assembly imported;
			if (!assembliesByName.TryGetValue(GetAssemblyIdentityName(asm), out imported))
			{
				imported = LoadFile(asm.Location);
				// assert that the assembly was added to the cache under the right name
				Debug.Assert(assembliesByName.ContainsKey(GetAssemblyIdentityName(asm)) && imported == assembliesByName[GetAssemblyIdentityName(asm)]);
			}
			return imported;
		}

		public Assembly LoadFile(string path)
		{
			path = Path.GetFullPath(path);
			string refname = GetAssemblyIdentityName(AssemblyName.GetAssemblyName(path));
			Assembly asm;
			if (!assembliesByName.TryGetValue(refname, out asm))
			{
				asm = new ModuleReader(null, this, new MemoryStream(File.ReadAllBytes(path)), path).Assembly;
				assemblies.Add(asm);
				assembliesByName.Add(refname, asm);
				string defname = GetAssemblyIdentityName(asm.GetName());
				if (defname != refname)
				{
					assembliesByName.Add(defname, asm);
				}
			}
			return asm;
		}

		public Assembly Load(string refname)
		{
			return Load(refname, null, true);
		}

		internal Assembly Load(string refname, Assembly requestingAssembly, bool throwOnError)
		{
			Assembly asm;
			if (assembliesByName.TryGetValue(refname, out asm))
			{
				return asm;
			}
			string simpleName;
			if (TryParseAssemblyIdentityName(refname, out simpleName) && assembliesByName.TryGetValue(simpleName, out asm))
			{
				return asm;
			}
			if (resolvers.Count == 0)
			{
				asm = DefaultResolver(refname, throwOnError);
			}
			else
			{
				ResolveEventArgs args = new ResolveEventArgs(refname, requestingAssembly);
				foreach (ResolveEventHandler evt in resolvers)
				{
					asm = evt(this, args);
					if (asm != null)
					{
						break;
					}
				}
			}
			if (asm != null)
			{
				string defname = GetAssemblyIdentityName(asm.GetName());
				if (refname != defname)
				{
					assembliesByName.Add(refname, asm);
				}
				return asm;
			}
			if (throwOnError)
			{
				throw new FileNotFoundException(refname);
			}
			return null;
		}

		private Assembly DefaultResolver(string refname, bool throwOnError)
		{
			string fileName;
			if (throwOnError)
			{
				try
				{
					fileName = System.Reflection.Assembly.ReflectionOnlyLoad(refname).Location;
				}
				catch (System.BadImageFormatException x)
				{
					throw new BadImageFormatException(x.Message, x);
				}
			}
			else
			{
				try
				{
					fileName = System.Reflection.Assembly.ReflectionOnlyLoad(refname).Location;
				}
				catch (System.BadImageFormatException x)
				{
					throw new BadImageFormatException(x.Message, x);
				}
				catch (FileNotFoundException)
				{
					// we intentionally only swallow the FileNotFoundException, if the file exists but isn't a valid assembly,
					// we should throw an exception
					return null;
				}
			}
			return LoadFile(fileName);
		}

		public Type GetType(string assemblyQualifiedTypeName)
		{
			// to be more compatible with Type.GetType(), we could call Assembly.GetCallingAssembly(),
			// import that assembly and pass it as the context, but implicitly importing is considered evil
			return GetType(null, assemblyQualifiedTypeName, false);
		}

		public Type GetType(string assemblyQualifiedTypeName, bool throwOnError)
		{
			// to be more compatible with Type.GetType(), we could call Assembly.GetCallingAssembly(),
			// import that assembly and pass it as the context, but implicitly importing is considered evil
			return GetType(null, assemblyQualifiedTypeName, throwOnError);
		}

		public Type GetType(Assembly context, string assemblyQualifiedTypeName, bool throwOnError)
		{
			TypeNameParser parser = TypeNameParser.Parse(assemblyQualifiedTypeName, throwOnError);
			if (parser.Error)
			{
				return null;
			}
			return parser.GetType(this, context, throwOnError, assemblyQualifiedTypeName);
		}

		public Assembly[] GetAssemblies()
		{
			return assemblies.ToArray();
		}

		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access)
		{
			return DefineDynamicAssemblyImpl(name, access, null, null, null, null);
		}

		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir)
		{
			return DefineDynamicAssemblyImpl(name, access, dir, null, null, null);
		}

#if NET_4_0
		[Obsolete]
#endif
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions)
		{
			return DefineDynamicAssemblyImpl(name, access, dir, requiredPermissions, optionalPermissions, refusedPermissions);
		}

		private AssemblyBuilder DefineDynamicAssemblyImpl(AssemblyName name, AssemblyBuilderAccess access, string dir, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions)
		{
			AssemblyBuilder asm = new AssemblyBuilder(this, name, dir, access, requiredPermissions, optionalPermissions, refusedPermissions);
			assembliesByName.Add(GetAssemblyIdentityName(asm.GetName()), asm);
			assemblies.Add(asm);
			return asm;
 		}
	}
}
