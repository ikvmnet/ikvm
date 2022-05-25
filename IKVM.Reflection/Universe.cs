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
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

using IKVM.Reflection.Emit;
using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{

    public sealed class ResolveEventArgs : EventArgs
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

    public enum AssemblyComparisonResult
    {
        Unknown = 0,
        EquivalentFullMatch = 1,
        EquivalentWeakNamed = 2,
        EquivalentFXUnified = 3,
        EquivalentUnified = 4,
        NonEquivalentVersion = 5,
        NonEquivalent = 6,
        EquivalentPartialMatch = 7,
        EquivalentPartialWeakNamed = 8,
        EquivalentPartialUnified = 9,
        EquivalentPartialFXUnified = 10,
        NonEquivalentPartialVersion = 11,
    }

    public delegate Assembly ResolveEventHandler(object sender, ResolveEventArgs args);

    public delegate void ResolvedMissingMemberHandler(Module requestingModule, MemberInfo member);

    /*
	 * UniverseOptions:
	 *
	 *   None
	 *		Default behavior, most compatible with System.Reflection[.Emit]
	 *
	 *   EnableFunctionPointers
	 *		Normally function pointers in signatures are replaced by System.IntPtr
	 *		(for compatibility with System.Reflection), when this option is enabled
	 *		they are represented as first class types (Type.__IsFunctionPointer will
	 *		return true for them).
	 *
	 *   DisableFusion
	 *      Don't use native Fusion API to resolve assembly names.
	 *
	 *   DisablePseudoCustomAttributeRetrieval
	 *      Set this option to disable the generaton of pseudo-custom attributes
	 *      when querying custom attributes.
	 *
	 *   DontProvideAutomaticDefaultConstructor
	 *      Normally TypeBuilder, like System.Reflection.Emit, will provide a default
	 *      constructor for types that meet the requirements. By enabling this
	 *      option this behavior is disabled.
	 *
	 *   MetadataOnly
	 *      By default, when a module is read in, the stream is kept open to satisfy
	 *      subsequent lazy loading. In MetadataOnly mode only the metadata is read in
	 *      and after that the stream is closed immediately. Subsequent lazy loading
	 *      attempts will fail with an InvalidOperationException.
	 *      APIs that are not available is MetadataOnly mode are:
	 *      - Module.ResolveString()
	 *      - Module.GetSignerCertificate()
	 *      - Module.GetManifestResourceStream()
	 *      - Module.__ReadDataFromRVA()
	 *      - MethodBase.GetMethodBody()
	 *      - FieldInfo.__GetDataFromRVA()
	 *
	 *   DeterministicOutput
	 *      The generated output file will depend only on the input. In other words,
	 *      the PE file header time stamp will be set to zero and the module version
	 *      id will be based on a SHA1 of the contents, instead of a random guid.
	 *      This option can not be used in combination with PDB file generation.
	 */

    [Flags]
    public enum UniverseOptions
    {
        None = 0,
        EnableFunctionPointers = 1,
        DisableFusion = 2,
        DisablePseudoCustomAttributeRetrieval = 4,
        DontProvideAutomaticDefaultConstructor = 8,
        MetadataOnly = 16,
        ResolveMissingMembers = 32,
        DisableWindowsRuntimeProjection = 64,
        DecodeVersionInfoAttributeBlobs = 128,
        DeterministicOutput = 256,
        DisableDefaultAssembliesLookup = 512,
    }

    public sealed class Universe : IDisposable
    {

#if NETCOREAPP3_1

        public static readonly string CoreLibName = "netstandard";

        public static string ReferenceAssembliesDirectory
        {
            get
            {
                return BuildRefDirFrom(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory());
            }
        }

        private static string BuildRefDirFrom(string runtimeDir)
        {
            // transform a thing like
            // C:\Program Files\dotnet\shared\Microsoft.NETCore.App\3.1.7
            // to
            // C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\3.1.0\ref\netcoreapp3.1

            var parts = runtimeDir.Split(Path.DirectorySeparatorChar);
            var n = string.IsNullOrEmpty(parts[parts.Length - 1]) ? parts.Length - 2 : parts.Length - 1;
            var versionDir = parts[n--];
            var frameworkDir = parts[n--];
            var newParts = new string[n + 5];
            Array.Copy(parts, newParts, n);
            var suffixParts = new string[] { "packs", frameworkDir + ".Ref", "3.1.0", "ref", "netcoreapp3.1" };
            Array.Copy(suffixParts, 0, newParts, n, suffixParts.Length);
            var dir = Path.Combine(newParts);
            if (!Directory.Exists(dir))
            {
                throw new FileNotFoundException("Reference assemblies directory: " + dir);
            }
            return dir;
        }

#elif NETFRAMEWORK || MONO

		public static readonly string CoreLibName = "mscorlib";

#endif

        internal static readonly bool MonoRuntime = System.Type.GetType("Mono.Runtime") != null;
#if NET461
        internal static readonly bool CoreRuntime = false;
#else
        internal static readonly bool CoreRuntime = true;
#endif
        private readonly Dictionary<Type, Type> canonicalizedTypes = new Dictionary<Type, Type>();
        private readonly List<AssemblyReader> assemblies = new List<AssemblyReader>();
        private readonly List<AssemblyBuilder> dynamicAssemblies = new List<AssemblyBuilder>();
        private readonly Dictionary<string, Assembly> assembliesByName = new Dictionary<string, Assembly>();
        private readonly Dictionary<System.Type, Type> importedTypes = new Dictionary<System.Type, Type>();
        private Dictionary<ScopedTypeName, Type> missingTypes;
        private bool resolveMissingMembers;
        private readonly bool enableFunctionPointers;
        private readonly bool useNativeFusion;
        private readonly bool returnPseudoCustomAttributes;
        private readonly bool automaticallyProvideDefaultConstructor;
        private readonly UniverseOptions options;
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
        private Type typeof_System_AttributeUsageAttribute;
        private Type typeof_System_ContextBoundObject;
        private Type typeof_System_MarshalByRefObject;
        private Type typeof_System_Console;
        private Type typeof_System_IO_TextWriter;
        private Type typeof_System_Runtime_InteropServices_DllImportAttribute;
        private Type typeof_System_Runtime_InteropServices_FieldOffsetAttribute;
        private Type typeof_System_Runtime_InteropServices_MarshalAsAttribute;
        private Type typeof_System_Runtime_InteropServices_UnmanagedType;
        private Type typeof_System_Runtime_InteropServices_VarEnum;
        private Type typeof_System_Runtime_InteropServices_PreserveSigAttribute;
        private Type typeof_System_Runtime_InteropServices_CallingConvention;
        private Type typeof_System_Runtime_InteropServices_CharSet;
        private Type typeof_System_Runtime_CompilerServices_DecimalConstantAttribute;
        private Type typeof_System_Reflection_AssemblyCopyrightAttribute;
        private Type typeof_System_Reflection_AssemblyTrademarkAttribute;
        private Type typeof_System_Reflection_AssemblyProductAttribute;
        private Type typeof_System_Reflection_AssemblyCompanyAttribute;
        private Type typeof_System_Reflection_AssemblyDescriptionAttribute;
        private Type typeof_System_Reflection_AssemblyTitleAttribute;
        private Type typeof_System_Reflection_AssemblyInformationalVersionAttribute;
        private Type typeof_System_Reflection_AssemblyFileVersionAttribute;
        private Type typeof_System_Security_Permissions_CodeAccessSecurityAttribute;
        private Type typeof_System_Security_Permissions_PermissionSetAttribute;
        private Type typeof_System_Security_Permissions_SecurityAction;
        private List<ResolveEventHandler> resolvers = new List<ResolveEventHandler>();
        private Predicate<Type> missingTypeIsValueType;

        public Universe()
            : this(UniverseOptions.None)
        {
        }

        public Universe(UniverseOptions options)
        {
            this.options = options;
            enableFunctionPointers = (options & UniverseOptions.EnableFunctionPointers) != 0;
            useNativeFusion = (options & UniverseOptions.DisableFusion) == 0 && GetUseNativeFusion();
            returnPseudoCustomAttributes = (options & UniverseOptions.DisablePseudoCustomAttributeRetrieval) == 0;
            automaticallyProvideDefaultConstructor = (options & UniverseOptions.DontProvideAutomaticDefaultConstructor) == 0;
            resolveMissingMembers = (options & UniverseOptions.ResolveMissingMembers) != 0;
        }

        private static bool GetUseNativeFusion()
        {
            try
            {
                return Environment.OSVersion.Platform == PlatformID.Win32NT && !MonoRuntime && !CoreRuntime && Environment.GetEnvironmentVariable("IKVM_DISABLE_FUSION") == null;
            }
            catch (System.Security.SecurityException)
            {
                return false;
            }
        }

        internal Assembly Mscorlib
        {
            get { return Load(CoreLibName); }
        }

        private Type ImportMscorlibType(string ns, string name)
        {
            if (Mscorlib.__IsMissing)
            {
                return Mscorlib.ResolveType(null, new TypeName(ns, name));
            }
            // We use FindType instead of ResolveType here, because on some versions of mscorlib some of
            // the special types we use/support are missing and the type properties are defined to
            // return null in that case.
            // Note that we don't have to unescape type.Name here, because none of the names contain special characters.
            return Mscorlib.FindType(new TypeName(ns, name));
        }

        private Type ResolvePrimitive(string name)
        {
            // Primitive here means that these types have a special metadata encoding, which means that
            // there can be references to them without referring to them by name explicitly.
            // We want these types to be usable even when they don't exist in mscorlib or there is no mscorlib loaded.
            return Mscorlib.FindType(new TypeName("System", name)) ?? GetMissingType(null, Mscorlib.ManifestModule, null, new TypeName("System", name));
        }

        internal Type System_Object
        {
            get { return typeof_System_Object ?? (typeof_System_Object = ResolvePrimitive("Object")); }
        }

        internal Type System_ValueType
        {
            // System.ValueType is not a primitive, but generic type parameters can have a ValueType constraint
            // (we also don't want to return null here)
            get { return typeof_System_ValueType ?? (typeof_System_ValueType = ResolvePrimitive("ValueType")); }
        }

        internal Type System_Enum
        {
            // System.Enum is not a primitive, but we don't want to return null
            get { return typeof_System_Enum ?? (typeof_System_Enum = ResolvePrimitive("Enum")); }
        }

        internal Type System_Void
        {
            get { return typeof_System_Void ?? (typeof_System_Void = ResolvePrimitive("Void")); }
        }

        internal Type System_Boolean
        {
            get { return typeof_System_Boolean ?? (typeof_System_Boolean = ResolvePrimitive("Boolean")); }
        }

        internal Type System_Char
        {
            get { return typeof_System_Char ?? (typeof_System_Char = ResolvePrimitive("Char")); }
        }

        internal Type System_SByte
        {
            get { return typeof_System_SByte ?? (typeof_System_SByte = ResolvePrimitive("SByte")); }
        }

        internal Type System_Byte
        {
            get { return typeof_System_Byte ?? (typeof_System_Byte = ResolvePrimitive("Byte")); }
        }

        internal Type System_Int16
        {
            get { return typeof_System_Int16 ?? (typeof_System_Int16 = ResolvePrimitive("Int16")); }
        }

        internal Type System_UInt16
        {
            get { return typeof_System_UInt16 ?? (typeof_System_UInt16 = ResolvePrimitive("UInt16")); }
        }

        internal Type System_Int32
        {
            get { return typeof_System_Int32 ?? (typeof_System_Int32 = ResolvePrimitive("Int32")); }
        }

        internal Type System_UInt32
        {
            get { return typeof_System_UInt32 ?? (typeof_System_UInt32 = ResolvePrimitive("UInt32")); }
        }

        internal Type System_Int64
        {
            get { return typeof_System_Int64 ?? (typeof_System_Int64 = ResolvePrimitive("Int64")); }
        }

        internal Type System_UInt64
        {
            get { return typeof_System_UInt64 ?? (typeof_System_UInt64 = ResolvePrimitive("UInt64")); }
        }

        internal Type System_Single
        {
            get { return typeof_System_Single ?? (typeof_System_Single = ResolvePrimitive("Single")); }
        }

        internal Type System_Double
        {
            get { return typeof_System_Double ?? (typeof_System_Double = ResolvePrimitive("Double")); }
        }

        internal Type System_String
        {
            get { return typeof_System_String ?? (typeof_System_String = ResolvePrimitive("String")); }
        }

        internal Type System_IntPtr
        {
            get { return typeof_System_IntPtr ?? (typeof_System_IntPtr = ResolvePrimitive("IntPtr")); }
        }

        internal Type System_UIntPtr
        {
            get { return typeof_System_UIntPtr ?? (typeof_System_UIntPtr = ResolvePrimitive("UIntPtr")); }
        }

        internal Type System_TypedReference
        {
            get { return typeof_System_TypedReference ?? (typeof_System_TypedReference = ResolvePrimitive("TypedReference")); }
        }

        internal Type System_Type
        {
            // System.Type is not a primitive, but it does have a special encoding in custom attributes
            get { return typeof_System_Type ?? (typeof_System_Type = ResolvePrimitive("Type")); }
        }

        internal Type System_Array
        {
            // System.Array is not a primitive, but it used as a base type for array types (that are primitives)
            get { return typeof_System_Array ?? (typeof_System_Array = ResolvePrimitive("Array")); }
        }

        internal Type System_DateTime
        {
            get { return typeof_System_DateTime ?? (typeof_System_DateTime = ImportMscorlibType("System", "DateTime")); }
        }

        internal Type System_DBNull
        {
            get { return typeof_System_DBNull ?? (typeof_System_DBNull = ImportMscorlibType("System", "DBNull")); }
        }

        internal Type System_Decimal
        {
            get { return typeof_System_Decimal ?? (typeof_System_Decimal = ImportMscorlibType("System", "Decimal")); }
        }

        internal Type System_AttributeUsageAttribute
        {
            get { return typeof_System_AttributeUsageAttribute ?? (typeof_System_AttributeUsageAttribute = ImportMscorlibType("System", "AttributeUsageAttribute")); }
        }

        internal Type System_ContextBoundObject
        {
            get { return typeof_System_ContextBoundObject ?? (typeof_System_ContextBoundObject = ImportMscorlibType("System", "ContextBoundObject")); }
        }

        internal Type System_MarshalByRefObject
        {
            get { return typeof_System_MarshalByRefObject ?? (typeof_System_MarshalByRefObject = ImportMscorlibType("System", "MarshalByRefObject")); }
        }

        internal Type System_Console
        {
            get { return typeof_System_Console ?? (typeof_System_Console = ImportMscorlibType("System", "Console")); }
        }

        internal Type System_IO_TextWriter
        {
            get { return typeof_System_IO_TextWriter ?? (typeof_System_IO_TextWriter = ImportMscorlibType("System.IO", "TextWriter")); }
        }

        internal Type System_Runtime_InteropServices_DllImportAttribute
        {
            get { return typeof_System_Runtime_InteropServices_DllImportAttribute ?? (typeof_System_Runtime_InteropServices_DllImportAttribute = ImportMscorlibType("System.Runtime.InteropServices", "DllImportAttribute")); }
        }

        internal Type System_Runtime_InteropServices_FieldOffsetAttribute
        {
            get { return typeof_System_Runtime_InteropServices_FieldOffsetAttribute ?? (typeof_System_Runtime_InteropServices_FieldOffsetAttribute = ImportMscorlibType("System.Runtime.InteropServices", "FieldOffsetAttribute")); }
        }

        internal Type System_Runtime_InteropServices_MarshalAsAttribute
        {
            get { return typeof_System_Runtime_InteropServices_MarshalAsAttribute ?? (typeof_System_Runtime_InteropServices_MarshalAsAttribute = ImportMscorlibType("System.Runtime.InteropServices", "MarshalAsAttribute")); }
        }

        internal Type System_Runtime_InteropServices_UnmanagedType
        {
            get { return typeof_System_Runtime_InteropServices_UnmanagedType ?? (typeof_System_Runtime_InteropServices_UnmanagedType = ImportMscorlibType("System.Runtime.InteropServices", "UnmanagedType")); }
        }

        internal Type System_Runtime_InteropServices_VarEnum
        {
            get { return typeof_System_Runtime_InteropServices_VarEnum ?? (typeof_System_Runtime_InteropServices_VarEnum = ImportMscorlibType("System.Runtime.InteropServices", "VarEnum")); }
        }

        internal Type System_Runtime_InteropServices_PreserveSigAttribute
        {
            get { return typeof_System_Runtime_InteropServices_PreserveSigAttribute ?? (typeof_System_Runtime_InteropServices_PreserveSigAttribute = ImportMscorlibType("System.Runtime.InteropServices", "PreserveSigAttribute")); }
        }

        internal Type System_Runtime_InteropServices_CallingConvention
        {
            get { return typeof_System_Runtime_InteropServices_CallingConvention ?? (typeof_System_Runtime_InteropServices_CallingConvention = ImportMscorlibType("System.Runtime.InteropServices", "CallingConvention")); }
        }

        internal Type System_Runtime_InteropServices_CharSet
        {
            get { return typeof_System_Runtime_InteropServices_CharSet ?? (typeof_System_Runtime_InteropServices_CharSet = ImportMscorlibType("System.Runtime.InteropServices", "CharSet")); }
        }

        internal Type System_Runtime_CompilerServices_DecimalConstantAttribute
        {
            get { return typeof_System_Runtime_CompilerServices_DecimalConstantAttribute ?? (typeof_System_Runtime_CompilerServices_DecimalConstantAttribute = ImportMscorlibType("System.Runtime.CompilerServices", "DecimalConstantAttribute")); }
        }

        internal Type System_Reflection_AssemblyCopyrightAttribute
        {
            get { return typeof_System_Reflection_AssemblyCopyrightAttribute ?? (typeof_System_Reflection_AssemblyCopyrightAttribute = ImportMscorlibType("System.Reflection", "AssemblyCopyrightAttribute")); }
        }

        internal Type System_Reflection_AssemblyTrademarkAttribute
        {
            get { return typeof_System_Reflection_AssemblyTrademarkAttribute ?? (typeof_System_Reflection_AssemblyTrademarkAttribute = ImportMscorlibType("System.Reflection", "AssemblyTrademarkAttribute")); }
        }

        internal Type System_Reflection_AssemblyProductAttribute
        {
            get { return typeof_System_Reflection_AssemblyProductAttribute ?? (typeof_System_Reflection_AssemblyProductAttribute = ImportMscorlibType("System.Reflection", "AssemblyProductAttribute")); }
        }

        internal Type System_Reflection_AssemblyCompanyAttribute
        {
            get { return typeof_System_Reflection_AssemblyCompanyAttribute ?? (typeof_System_Reflection_AssemblyCompanyAttribute = ImportMscorlibType("System.Reflection", "AssemblyCompanyAttribute")); }
        }

        internal Type System_Reflection_AssemblyDescriptionAttribute
        {
            get { return typeof_System_Reflection_AssemblyDescriptionAttribute ?? (typeof_System_Reflection_AssemblyDescriptionAttribute = ImportMscorlibType("System.Reflection", "AssemblyDescriptionAttribute")); }
        }

        internal Type System_Reflection_AssemblyTitleAttribute
        {
            get { return typeof_System_Reflection_AssemblyTitleAttribute ?? (typeof_System_Reflection_AssemblyTitleAttribute = ImportMscorlibType("System.Reflection", "AssemblyTitleAttribute")); }
        }

        internal Type System_Reflection_AssemblyInformationalVersionAttribute
        {
            get { return typeof_System_Reflection_AssemblyInformationalVersionAttribute ?? (typeof_System_Reflection_AssemblyInformationalVersionAttribute = ImportMscorlibType("System.Reflection", "AssemblyInformationalVersionAttribute")); }
        }

        internal Type System_Reflection_AssemblyFileVersionAttribute
        {
            get { return typeof_System_Reflection_AssemblyFileVersionAttribute ?? (typeof_System_Reflection_AssemblyFileVersionAttribute = ImportMscorlibType("System.Reflection", "AssemblyFileVersionAttribute")); }
        }

        internal Type System_Security_Permissions_CodeAccessSecurityAttribute
        {
            get { return typeof_System_Security_Permissions_CodeAccessSecurityAttribute ?? (typeof_System_Security_Permissions_CodeAccessSecurityAttribute = ImportMscorlibType("System.Security.Permissions", "CodeAccessSecurityAttribute")); }
        }

        internal Type System_Security_Permissions_PermissionSetAttribute
        {
            get { return typeof_System_Security_Permissions_PermissionSetAttribute ?? (typeof_System_Security_Permissions_PermissionSetAttribute = ImportMscorlibType("System.Security.Permissions", "PermissionSetAttribute")); }
        }

        internal Type System_Security_Permissions_SecurityAction
        {
            get { return typeof_System_Security_Permissions_SecurityAction ?? (typeof_System_Security_Permissions_SecurityAction = ImportMscorlibType("System.Security.Permissions", "SecurityAction")); }
        }

        internal bool HasMscorlib
        {
            get { return GetLoadedAssembly(CoreLibName) != null; }
        }

        public event ResolveEventHandler AssemblyResolve
        {
            add { resolvers.Add(value); }
            remove { resolvers.Remove(value); }
        }

        public Type Import(System.Type type)
        {
            Type imported;
            if (!importedTypes.TryGetValue(type, out imported))
            {
                imported = ImportImpl(type);
                if (imported != null)
                {
                    importedTypes.Add(type, imported);
                }
            }
            return imported;
        }

        private Type ImportImpl(System.Type type)
        {
            if (TypeUtil.GetAssembly(type) == TypeUtil.GetAssembly(typeof(IKVM.Reflection.Type)))
            {
                throw new ArgumentException("Did you really want to import " + type.FullName + "?");
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
                if (TypeUtil.GetDeclaringMethod(type) != null)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    return Import(type.DeclaringType).GetGenericArguments()[type.GenericParameterPosition];
                }
            }
            else if (TypeUtil.IsGenericType(type) && !TypeUtil.IsGenericTypeDefinition(type))
            {
                System.Type[] args = TypeUtil.GetGenericArguments(type);
                Type[] importedArgs = new Type[args.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    importedArgs[i] = Import(args[i]);
                }
                return Import(type.GetGenericTypeDefinition()).MakeGenericType(importedArgs);
            }
            else if (TypeUtil.GetAssembly(type) == TypeUtil.GetAssembly(typeof(object)))
            {
                // make sure mscorlib types always end up in our mscorlib
                return ResolveType(Mscorlib, type.FullName);
            }
            else
            {
                // FXBUG we parse the FullName here, because type.Namespace and type.Name are both broken on the CLR
                return ResolveType(Import(TypeUtil.GetAssembly(type)), type.FullName);
            }
        }

        private Assembly Import(System.Reflection.Assembly asm)
        {
            return Load(asm.FullName);
        }

        public RawModule OpenRawModule(string path)
        {
            path = Path.GetFullPath(path);
            FileStream fs = null;
            RawModule module;
            try
            {
                fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                module = OpenRawModule(fs, path);
                if (!MetadataOnly)
                {
                    fs = null;
                }
            }
            finally
            {
                if (fs != null)
                {
                    fs.Dispose();
                }
            }
            return module;
        }

        public RawModule OpenRawModule(Stream stream, string location)
        {
            return OpenRawModule(stream, location, false);
        }

        public RawModule OpenMappedRawModule(Stream stream, string location)
        {
            return OpenRawModule(stream, location, true);
        }

        private RawModule OpenRawModule(Stream stream, string location, bool mapped)
        {
            if (!stream.CanRead || !stream.CanSeek || stream.Position != 0)
            {
                throw new ArgumentException("Stream must support read/seek and current position must be zero.", "stream");
            }
            return new RawModule(new ModuleReader(null, this, stream, location, mapped));
        }

        public Assembly LoadAssembly(RawModule module)
        {
            string refname = module.GetAssemblyName().FullName;
            Assembly asm = GetLoadedAssembly(refname);
            if (asm == null)
            {
                AssemblyReader asm1 = module.ToAssembly();
                assemblies.Add(asm1);
                asm = asm1;
            }
            return asm;
        }

        public Assembly LoadFile(string path)
        {
            try
            {
                using (RawModule module = OpenRawModule(path))
                {
                    return LoadAssembly(module);
                }
            }
            catch (IOException x)
            {
                throw new FileNotFoundException(x.Message, x);
            }
            catch (UnauthorizedAccessException x)
            {
                throw new FileNotFoundException(x.Message, x);
            }
        }

        private static string GetSimpleAssemblyName(string refname)
        {
            int pos;
            string name;
            if (Fusion.ParseAssemblySimpleName(refname, out pos, out name) != ParseAssemblyResult.OK)
            {
                throw new ArgumentException();
            }
            return name;
        }

        private Assembly GetLoadedAssembly(string refname)
        {
            Assembly asm;
            if (!assembliesByName.TryGetValue(refname, out asm) && (options & UniverseOptions.DisableDefaultAssembliesLookup) == 0)
            {
                string simpleName = GetSimpleAssemblyName(refname);
                for (int i = 0; i < assemblies.Count; i++)
                {
                    AssemblyComparisonResult result;
                    if (simpleName.Equals(assemblies[i].Name, StringComparison.OrdinalIgnoreCase)
                        && CompareAssemblyIdentity(refname, false, assemblies[i].FullName, false, out result))
                    {
                        asm = assemblies[i];
                        assembliesByName.Add(refname, asm);
                        break;
                    }
                }
            }
            return asm;
        }

        private Assembly GetDynamicAssembly(string refname)
        {
            string simpleName = GetSimpleAssemblyName(refname);
            foreach (AssemblyBuilder asm in dynamicAssemblies)
            {
                AssemblyComparisonResult result;
                if (simpleName.Equals(asm.Name, StringComparison.OrdinalIgnoreCase)
                    && CompareAssemblyIdentity(refname, false, asm.FullName, false, out result))
                {
                    return asm;
                }
            }
            return null;
        }

        public Assembly Load(string refname)
        {
            return Load(refname, null, true);
        }

        internal Assembly Load(string refname, Module requestingModule, bool throwOnError)
        {
            Assembly asm = GetLoadedAssembly(refname);
            if (asm != null)
            {
                return asm;
            }
            if (resolvers.Count == 0)
            {
                asm = DefaultResolver(refname, throwOnError);
            }
            else
            {
                ResolveEventArgs args = new ResolveEventArgs(refname, requestingModule == null ? null : requestingModule.Assembly);
                foreach (ResolveEventHandler evt in resolvers)
                {
                    asm = evt(this, args);
                    if (asm != null)
                    {
                        break;
                    }
                }
                if (asm == null)
                {
                    asm = GetDynamicAssembly(refname);
                }
            }
            if (asm != null)
            {
                string defname = asm.FullName;
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
            Assembly asm = GetDynamicAssembly(refname);
            if (asm != null)
            {
                return asm;
            }

#if NETCOREAPP3_1
            string filepath = Path.Combine(ReferenceAssembliesDirectory, GetSimpleAssemblyName(refname) + ".dll");
            if (File.Exists(filepath))
            {
                using (RawModule module = OpenRawModule(filepath))
                {
                    AssemblyComparisonResult result;
                    if (module.IsManifestModule && CompareAssemblyIdentity(refname, false, module.GetAssemblyName().FullName, false, out result))
                    {
                        return LoadAssembly(module);
                    }
                }
            }
            return null;
#else
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
#endif
        }

        public Type GetType(string assemblyQualifiedTypeName)
        {
            // to be more compatible with Type.GetType(), we could call Assembly.GetCallingAssembly(),
            // import that assembly and pass it as the context, but implicitly importing is considered evil
            return GetType(null, assemblyQualifiedTypeName, false, false);
        }

        public Type GetType(string assemblyQualifiedTypeName, bool throwOnError)
        {
            // to be more compatible with Type.GetType(), we could call Assembly.GetCallingAssembly(),
            // import that assembly and pass it as the context, but implicitly importing is considered evil
            return GetType(null, assemblyQualifiedTypeName, throwOnError, false);
        }

        public Type GetType(string assemblyQualifiedTypeName, bool throwOnError, bool ignoreCase)
        {
            // to be more compatible with Type.GetType(), we could call Assembly.GetCallingAssembly(),
            // import that assembly and pass it as the context, but implicitly importing is considered evil
            return GetType(null, assemblyQualifiedTypeName, throwOnError, ignoreCase);
        }

        // note that context is slightly different from the calling assembly (System.Type.GetType),
        // because context is passed to the AssemblyResolve event as the RequestingAssembly
        public Type GetType(Assembly context, string assemblyQualifiedTypeName, bool throwOnError)
        {
            return GetType(context, assemblyQualifiedTypeName, throwOnError, false);
        }

        // note that context is slightly different from the calling assembly (System.Type.GetType),
        // because context is passed to the AssemblyResolve event as the RequestingAssembly
        public Type GetType(Assembly context, string assemblyQualifiedTypeName, bool throwOnError, bool ignoreCase)
        {
            TypeNameParser parser = TypeNameParser.Parse(assemblyQualifiedTypeName, throwOnError);
            if (parser.Error)
            {
                return null;
            }
            return parser.GetType(this, context == null ? null : context.ManifestModule, throwOnError, assemblyQualifiedTypeName, false, ignoreCase);
        }

        // this is similar to GetType(Assembly context, string assemblyQualifiedTypeName, bool throwOnError),
        // but instead it assumes that the type must exist (i.e. if EnableMissingMemberResolution is enabled
        // it will create a missing type)
        public Type ResolveType(Assembly context, string assemblyQualifiedTypeName)
        {
            TypeNameParser parser = TypeNameParser.Parse(assemblyQualifiedTypeName, false);
            if (parser.Error)
            {
                return null;
            }
            return parser.GetType(this, context == null ? null : context.ManifestModule, false, assemblyQualifiedTypeName, true, false);
        }

        public Type GetBuiltInType(string ns, string name)
        {
            if (ns != "System")
            {
                return null;
            }
            switch (name)
            {
                case "Boolean":
                    return System_Boolean;
                case "Char":
                    return System_Char;
                case "Object":
                    return System_Object;
                case "String":
                    return System_String;
                case "Single":
                    return System_Single;
                case "Double":
                    return System_Double;
                case "SByte":
                    return System_SByte;
                case "Int16":
                    return System_Int16;
                case "Int32":
                    return System_Int32;
                case "Int64":
                    return System_Int64;
                case "IntPtr":
                    return System_IntPtr;
                case "UIntPtr":
                    return System_UIntPtr;
                case "TypedReference":
                    return System_TypedReference;
                case "Byte":
                    return System_Byte;
                case "UInt16":
                    return System_UInt16;
                case "UInt32":
                    return System_UInt32;
                case "UInt64":
                    return System_UInt64;
                case "Void":
                    return System_Void;
                default:
                    return null;
            }
        }

        public Assembly[] GetAssemblies()
        {
            Assembly[] array = new Assembly[assemblies.Count + dynamicAssemblies.Count];
            for (int i = 0; i < assemblies.Count; i++)
            {
                array[i] = assemblies[i];
            }
            for (int i = 0, j = assemblies.Count; j < array.Length; i++, j++)
            {
                array[j] = dynamicAssemblies[i];
            }
            return array;
        }

        public bool CompareAssemblyIdentity(string assemblyIdentity1, bool unified1, string assemblyIdentity2, bool unified2, out AssemblyComparisonResult result)
        {
            return useNativeFusion
                ? Fusion.CompareAssemblyIdentityNative(assemblyIdentity1, unified1, assemblyIdentity2, unified2, out result)
                : Fusion.CompareAssemblyIdentityPure(assemblyIdentity1, unified1, assemblyIdentity2, unified2, out result);
        }

        public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access)
        {
            return new AssemblyBuilder(this, name, null, null);
        }

        public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, IEnumerable<CustomAttributeBuilder> assemblyAttributes)
        {
            return new AssemblyBuilder(this, name, null, assemblyAttributes);
        }

        public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir)
        {
            return new AssemblyBuilder(this, name, dir, null);
        }

        [Obsolete]
        public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions)
        {
            AssemblyBuilder ab = new AssemblyBuilder(this, name, dir, null);
            AddLegacyPermissionSet(ab, requiredPermissions, System.Security.Permissions.SecurityAction.RequestMinimum);
            AddLegacyPermissionSet(ab, optionalPermissions, System.Security.Permissions.SecurityAction.RequestOptional);
            AddLegacyPermissionSet(ab, refusedPermissions, System.Security.Permissions.SecurityAction.RequestRefuse);
            return ab;
        }

        private static void AddLegacyPermissionSet(AssemblyBuilder ab, PermissionSet permissionSet, System.Security.Permissions.SecurityAction action)
        {
            if (permissionSet != null)
            {
                ab.__AddDeclarativeSecurity(CustomAttributeBuilder.__FromBlob(CustomAttributeBuilder.LegacyPermissionSet, (int)action, Encoding.Unicode.GetBytes(permissionSet.ToXml().ToString())));
            }
        }

        internal void RegisterDynamicAssembly(AssemblyBuilder asm)
        {
            dynamicAssemblies.Add(asm);
        }

        internal void RenameAssembly(Assembly assembly, AssemblyName oldName)
        {
            List<string> remove = new List<string>();
            foreach (KeyValuePair<string, Assembly> kv in assembliesByName)
            {
                if (kv.Value == assembly)
                {
                    remove.Add(kv.Key);
                }
            }
            foreach (string key in remove)
            {
                assembliesByName.Remove(key);
            }
        }

        public void Dispose()
        {
            foreach (Assembly asm in assemblies)
            {
                foreach (Module mod in asm.GetLoadedModules())
                {
                    mod.Dispose();
                }
            }
            foreach (AssemblyBuilder asm in dynamicAssemblies)
            {
                foreach (Module mod in asm.GetLoadedModules())
                {
                    mod.Dispose();
                }
            }
        }

        public Assembly CreateMissingAssembly(string assemblyName)
        {
            Assembly asm = new MissingAssembly(this, assemblyName);
            string name = asm.FullName;
            if (!assembliesByName.ContainsKey(name))
            {
                assembliesByName.Add(name, asm);
            }
            return asm;
        }

        [Obsolete("Please set UniverseOptions.ResolveMissingMembers instead.")]
        public void EnableMissingMemberResolution()
        {
            resolveMissingMembers = true;
        }

        internal bool MissingMemberResolution
        {
            get { return resolveMissingMembers; }
        }

        internal bool EnableFunctionPointers
        {
            get { return enableFunctionPointers; }
        }

        private struct ScopedTypeName : IEquatable<ScopedTypeName>
        {
            private readonly object scope;
            private readonly TypeName name;

            internal ScopedTypeName(object scope, TypeName name)
            {
                this.scope = scope;
                this.name = name;
            }

            public override bool Equals(object obj)
            {
                ScopedTypeName? other = obj as ScopedTypeName?;
                return other != null && ((IEquatable<ScopedTypeName>)other.Value).Equals(this);
            }

            public override int GetHashCode()
            {
                return scope.GetHashCode() * 7 + name.GetHashCode();
            }

            bool IEquatable<ScopedTypeName>.Equals(ScopedTypeName other)
            {
                return other.scope == scope && other.name == name;
            }
        }

        private Type GetMissingType(Module requester, Module module, Type declaringType, TypeName typeName)
        {
            if (missingTypes == null)
            {
                missingTypes = new Dictionary<ScopedTypeName, Type>();
            }
            ScopedTypeName stn = new ScopedTypeName(declaringType ?? (object)module, typeName);
            Type type;
            if (!missingTypes.TryGetValue(stn, out type))
            {
                type = new MissingType(module, declaringType, typeName.Namespace, typeName.Name);
                missingTypes.Add(stn, type);
            }
            if (ResolvedMissingMember != null && !module.Assembly.__IsMissing)
            {
                ResolvedMissingMember(requester, type);
            }
            return type;
        }

        internal Type GetMissingTypeOrThrow(Module requester, Module module, Type declaringType, TypeName typeName)
        {
            if (resolveMissingMembers || module.Assembly.__IsMissing)
            {
                return GetMissingType(requester, module, declaringType, typeName);
            }
            string fullName = TypeNameParser.Escape(typeName.ToString());
            if (declaringType != null)
            {
                fullName = declaringType.FullName + "+" + fullName;
            }
            throw new TypeLoadException(String.Format("Type '{0}' not found in assembly '{1}'", fullName, module.Assembly.FullName));
        }

        internal MethodBase GetMissingMethodOrThrow(Module requester, Type declaringType, string name, MethodSignature signature)
        {
            if (resolveMissingMembers)
            {
                MethodBase method = new MissingMethod(declaringType, name, signature);
                if (name == ".ctor")
                {
                    method = new ConstructorInfoImpl((MethodInfo)method);
                }
                if (ResolvedMissingMember != null)
                {
                    ResolvedMissingMember(requester, method);
                }
                return method;
            }

            throw new MissingMethodException(declaringType.ToString(), name);
        }

        internal FieldInfo GetMissingFieldOrThrow(Module requester, Type declaringType, string name, FieldSignature signature)
        {
            if (resolveMissingMembers)
            {
                FieldInfo field = new MissingField(declaringType, name, signature);
                if (ResolvedMissingMember != null)
                {
                    ResolvedMissingMember(requester, field);
                }
                return field;
            }

            throw new MissingFieldException(declaringType.ToString(), name);
        }

        internal PropertyInfo GetMissingPropertyOrThrow(Module requester, Type declaringType, string name, PropertySignature propertySignature)
        {
            // HACK we need to check __IsMissing here, because Type doesn't have a FindProperty API
            // since properties are never resolved, except by custom attributes
            if (resolveMissingMembers || declaringType.__IsMissing)
            {
                PropertyInfo property = new MissingProperty(declaringType, name, propertySignature);
                if (ResolvedMissingMember != null && !declaringType.__IsMissing)
                {
                    ResolvedMissingMember(requester, property);
                }
                return property;
            }

            throw new System.MissingMemberException(declaringType.ToString(), name);
        }

        internal Type CanonicalizeType(Type type)
        {
            Type canon;
            if (!canonicalizedTypes.TryGetValue(type, out canon))
            {
                canon = type;
                canonicalizedTypes.Add(canon, canon);
            }
            return canon;
        }

        public Type MakeFunctionPointer(__StandAloneMethodSig sig)
        {
            return FunctionPointerType.Make(this, sig);
        }

        public __StandAloneMethodSig MakeStandAloneMethodSig(System.Runtime.InteropServices.CallingConvention callingConvention, Type returnType, CustomModifiers returnTypeCustomModifiers, Type[] parameterTypes, CustomModifiers[] parameterTypeCustomModifiers)
        {
            return new __StandAloneMethodSig(true, callingConvention, 0, returnType ?? this.System_Void, Util.Copy(parameterTypes), Type.EmptyTypes,
                PackedCustomModifiers.CreateFromExternal(returnTypeCustomModifiers, parameterTypeCustomModifiers, Util.NullSafeLength(parameterTypes)));
        }

        public __StandAloneMethodSig MakeStandAloneMethodSig(CallingConventions callingConvention, Type returnType, CustomModifiers returnTypeCustomModifiers, Type[] parameterTypes, Type[] optionalParameterTypes, CustomModifiers[] parameterTypeCustomModifiers)
        {
            return new __StandAloneMethodSig(false, 0, callingConvention, returnType ?? this.System_Void, Util.Copy(parameterTypes), Util.Copy(optionalParameterTypes),
                PackedCustomModifiers.CreateFromExternal(returnTypeCustomModifiers, parameterTypeCustomModifiers, Util.NullSafeLength(parameterTypes) + Util.NullSafeLength(optionalParameterTypes)));
        }

        public event ResolvedMissingMemberHandler ResolvedMissingMember;

        public event Predicate<Type> MissingTypeIsValueType
        {
            add
            {
                if (missingTypeIsValueType != null)
                {
                    throw new InvalidOperationException("Only a single MissingTypeIsValueType handler can be registered.");
                }
                missingTypeIsValueType = value;
            }
            remove
            {
                if (value.Equals(missingTypeIsValueType))
                {
                    missingTypeIsValueType = null;
                }
            }
        }

        public static Universe FromAssembly(Assembly assembly)
        {
            return assembly.universe;
        }

        internal bool ResolveMissingTypeIsValueType(MissingType missingType)
        {
            if (missingTypeIsValueType != null)
            {
                return missingTypeIsValueType(missingType);
            }
            throw new MissingMemberException(missingType);
        }

        internal bool ReturnPseudoCustomAttributes
        {
            get { return returnPseudoCustomAttributes; }
        }

        internal bool AutomaticallyProvideDefaultConstructor
        {
            get { return automaticallyProvideDefaultConstructor; }
        }

        internal bool MetadataOnly
        {
            get { return (options & UniverseOptions.MetadataOnly) != 0; }
        }

        internal bool WindowsRuntimeProjection
        {
            get { return (options & UniverseOptions.DisableWindowsRuntimeProjection) == 0; }
        }

        internal bool DecodeVersionInfoAttributeBlobs
        {
            get { return (options & UniverseOptions.DecodeVersionInfoAttributeBlobs) != 0; }
        }

        internal bool Deterministic
        {
            get { return (options & UniverseOptions.DeterministicOutput) != 0; }
        }
    }
}
