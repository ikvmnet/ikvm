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
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;

using IKVM.Reflection.Emit;
using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{

    /// <summary>
    /// Provides a view of the types available to the reflection infrastructure. A universe owns a set of loaded and
    /// dynamic assemblies, resolves assembly references among them, and identifies the core library that supplies the
    /// built-in types.
    /// </summary>
    sealed class Universe : IDisposable
    {

        /// <summary>
        /// The name of the core library of .NET (Core) reference assemblies.
        /// </summary>
        public static readonly string NetCoreLibName = "System.Runtime";

        /// <summary>
        /// The name of the core library of the .NET Framework.
        /// </summary>
        public static readonly string NetFxCoreLibName = "mscorlib";

#if NETCOREAPP3_1_OR_GREATER

        /// <summary>
        /// The name of the core library used when none is specified: the one that matches the runtime IKVM.Reflection is
        /// built for.
        /// </summary>
        public static readonly string DefaultCoreLibName = NetCoreLibName;

#elif NETFRAMEWORK

        /// <summary>
        /// The name of the core library used when none is specified: the one that matches the runtime IKVM.Reflection is
        /// built for.
        /// </summary>
        public static readonly string DefaultCoreLibName = NetFxCoreLibName;

#endif

#if NETFRAMEWORK
        /// <summary>
        /// Whether the current process is running on Mono.
        /// </summary>
        internal static readonly bool MonoRuntime = System.Type.GetType("Mono.Runtime") != null;

        /// <summary>
        /// Whether the current process is running on .NET (Core) rather than the .NET Framework.
        /// </summary>
        static readonly bool s_coreRuntime = false;
#else
        /// <summary>
        /// Whether the current process is running on Mono.
        /// </summary>
        internal static readonly bool MonoRuntime = false;

        /// <summary>
        /// Whether the current process is running on .NET (Core) rather than the .NET Framework.
        /// </summary>
        static readonly bool s_coreRuntime = true;
#endif

        readonly string _coreLibName;

        /// <summary>
        /// The canonical instance of each type that is created on demand but must be unique, keyed by itself.
        /// </summary>
        readonly Dictionary<Type, Type> _canonicalizedTypes = [];

        readonly List<AssemblyReader> _assemblies = [];
        readonly List<AssemblyBuilder> _dynamicAssemblies = [];

        /// <summary>
        /// The assemblies found so far, by every name they have been requested or resolved under.
        /// </summary>
        readonly Dictionary<string, Assembly> _assembliesByName = [];

        /// <summary>
        /// The types imported from the running runtime by <see cref="Import(System.Type)"/>.
        /// </summary>
        readonly Dictionary<System.Type, Type> _importedTypes = [];

        /// <summary>
        /// The missing types created so far, keyed by their declaring type or module and name, so each is created once.
        /// </summary>
        Dictionary<ScopedTypeName, Type> _missingTypes;

        readonly bool _resolveMissingMembers;
        readonly bool _enableFunctionPointers;

        /// <summary>
        /// Whether assembly identities are compared with the native fusion API instead of the managed implementation.
        /// </summary>
        readonly bool _useNativeFusion;

        readonly HashAlgorithmName _pdbChecksumAlgorithm = HashAlgorithmName.SHA256;
        readonly UniverseOptions _options;
        Func<ModuleBuilder, ISymbolWriter> _symbolWriterFactory;
        Type _typeof_System_Object;
        Type _typeof_System_ValueType;
        Type _typeof_System_MulticastDelegate;
        Type _typeof_System_Void;
        Type _typeof_System_Boolean;
        Type _typeof_System_Char;
        Type _typeof_System_SByte;
        Type _typeof_System_Byte;
        Type _typeof_System_Int16;
        Type _typeof_System_UInt16;
        Type _typeof_System_Int32;
        Type _typeof_System_UInt32;
        Type _typeof_System_Int64;
        Type _typeof_System_UInt64;
        Type _typeof_System_Single;
        Type _typeof_System_Double;
        Type _typeof_System_String;
        Type _typeof_System_IntPtr;
        Type _typeof_System_UIntPtr;
        Type _typeof_System_TypedReference;
        Type _typeof_System_Type;
        Type _typeof_System_Array;
        Type _typeof_System_DateTime;
        Type _typeof_System_DBNull;
        Type _typeof_System_Decimal;
        Type _typeof_System_AttributeUsageAttribute;
        Type _typeof_System_Runtime_InteropServices_DllImportAttribute;
        Type _typeof_System_Runtime_InteropServices_FieldOffsetAttribute;
        Type _typeof_System_Runtime_InteropServices_MarshalAsAttribute;
        Type _typeof_System_Runtime_InteropServices_UnmanagedType;
        Type _typeof_System_Runtime_InteropServices_VarEnum;
        Type _typeof_System_Runtime_InteropServices_PreserveSigAttribute;
        Type _typeof_System_Runtime_InteropServices_CallingConvention;
        Type _typeof_System_Runtime_InteropServices_CharSet;
        Type _typeof_System_Runtime_CompilerServices_DecimalConstantAttribute;
        Type _typeof_System_Runtime_CompilerServices_DateTimeConstantAttribute;
        Type _typeof_System_Reflection_AssemblyCopyrightAttribute;
        Type _typeof_System_Reflection_AssemblyTrademarkAttribute;
        Type _typeof_System_Reflection_AssemblyProductAttribute;
        Type _typeof_System_Reflection_AssemblyCompanyAttribute;
        Type _typeof_System_Reflection_AssemblyDescriptionAttribute;
        Type _typeof_System_Reflection_AssemblyTitleAttribute;
        Type _typeof_System_Reflection_AssemblyInformationalVersionAttribute;
        Type _typeof_System_Reflection_AssemblyFileVersionAttribute;
        Type _typeof_System_Security_Permissions_CodeAccessSecurityAttribute;
        Type _typeof_System_Security_Permissions_SecurityAction;
        readonly List<ResolveEventHandler> _resolvers = [];

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="coreLibName">The simple name of the core library, or <see langword="null"/> for <see cref="DefaultCoreLibName"/>.</param>
        public Universe(string coreLibName = null) :
            this(UniverseOptions.None, coreLibName)
        {
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options">The options that control the behavior of the universe.</param>
        /// <param name="coreLibName">The simple name of the core library, or <see langword="null"/> for <see cref="DefaultCoreLibName"/>.</param>
        public Universe(UniverseOptions options, string coreLibName = null)
        {
            _options = options;
            _coreLibName = coreLibName ?? DefaultCoreLibName;
            _enableFunctionPointers = (options & UniverseOptions.EnableFunctionPointers) != 0;
            _useNativeFusion = GetUseNativeFusion();
            _resolveMissingMembers = (options & UniverseOptions.ResolveMissingMembers) != 0;
        }

        /// <summary>
        /// Sets a factory to create a <see cref="ISymbolWriter"/> for a module builder.
        /// </summary>
        /// <param name="factory">The factory, or <see langword="null"/> to emit no symbols.</param>
        public void SetSymbolWriterFactory(Func<ModuleBuilder, ISymbolWriter> factory) => _symbolWriterFactory = factory;

        /// <summary>
        /// Creates a new <see cref="ISymbolWriter"/> for the specified module builder.
        /// </summary>
        /// <param name="module">The module builder that will emit symbols.</param>
        /// <returns>The symbol writer, or <see langword="null"/> if no factory has been set.</returns>
        internal ISymbolWriter CreateSymbolWriter(ModuleBuilder module) => _symbolWriterFactory?.Invoke(module);

        /// <summary>
        /// Determines whether the native fusion API can be used to compare assembly identities: only on Windows, on the
        /// .NET Framework (not Mono or .NET Core), and when the <c>IKVM_DISABLE_FUSION</c> environment variable is not
        /// set.
        /// </summary>
        /// <returns><see langword="true"/> if the native fusion API should be used; otherwise, <see langword="false"/>.</returns>
        static bool GetUseNativeFusion()
        {
            try
            {
                return Environment.OSVersion.Platform == PlatformID.Win32NT && !MonoRuntime && !s_coreRuntime && Environment.GetEnvironmentVariable("IKVM_DISABLE_FUSION") == null;
            }
            catch (SecurityException)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the name of the core library.
        /// </summary>
        public string CoreLibName => _coreLibName;

        /// <summary>
        /// Gets the core library, loading it if necessary.
        /// </summary>
        /// <exception cref="FileNotFoundException">The core library cannot be loaded.</exception>
        public Assembly CoreLib => Load(_coreLibName);

        /// <summary>
        /// Attempts to import the type from the core library. If the core library is itself missing, a missing type is
        /// returned instead.
        /// </summary>
        /// <param name="ns">The namespace of the type.</param>
        /// <param name="name">The name of the type.</param>
        /// <returns>The type, or <see langword="null"/> if the core library does not define it.</returns>
        Type ImportCoreLibType(string ns, string name)
        {
            if (CoreLib.__IsMissing)
                return CoreLib.ResolveType(null, new TypeName(ns, name));

            // we use FindType instead of ResolveType here, because on some versions of mscorlib some of
            // the special types we use/support are missing and the type properties are defined to
            // return null in that case.
            // note that we don't have to unescape type.Name here, because none of the names contain special characters.
            return CoreLib.FindType(new TypeName(ns, name));
        }

        /// <summary>
        /// Imports a type from the System.Runtime.InteropServices namespace. The .NET Framework core library defines all
        /// of them, but .NET reference assemblies define some in the System.Runtime.InteropServices assembly instead.
        /// </summary>
        /// <param name="name">The name of the type, without its namespace.</param>
        /// <returns>The type, or <see langword="null"/> if it cannot be found.</returns>
        Type ImportInteropType(string name)
        {
            var typeName = new TypeName("System.Runtime.InteropServices", name);
            if (ImportCoreLibType(typeName.Namespace, typeName.Name) is { } type)
                return type;

            return Load("System.Runtime.InteropServices", null, false) is { __IsMissing: false } assembly ? assembly.FindType(typeName) : null;
        }

        /// <summary>
        /// Resolves the primitive type with the specified name from the core library, falling back to a missing type
        /// if the core library does not define it.
        /// </summary>
        /// <param name="name">The name of the type in the <c>System</c> namespace.</param>
        /// <returns>The type, or a missing type if the core library does not define it.</returns>
        Type ResolvePrimitive(string name) =>
            // primitive here means that these types have a special metadata encoding, which means that
            // there can be references to them without referring to them by name explicitly.
            // We want these types to be usable even when they don't exist in mscorlib or there is no mscorlib loaded.
            CoreLib.FindType(new TypeName("System", name)) ?? GetMissingType(null, CoreLib.ManifestModule, null, new TypeName("System", name));

        /// <summary>
        /// Gets the <c>System.Object</c> type of the core library.
        /// </summary>
        internal Type System_Object => _typeof_System_Object ??= ResolvePrimitive("Object");

        /// <summary>
        /// Gets the <c>System.ValueType</c> type of the core library.
        /// </summary>
        internal Type System_ValueType => _typeof_System_ValueType ??= ResolvePrimitive("ValueType");

        /// <summary>
        /// Gets the <c>System.MulticastDelegate</c> type of the core library, or <see langword="null"/> if it is not defined.
        /// </summary>
        internal Type System_MulticastDelegate => _typeof_System_MulticastDelegate ??= ImportCoreLibType("System", "MulticastDelegate");

        /// <summary>
        /// Gets the <c>System.Void</c> type of the core library.
        /// </summary>
        internal Type System_Void => _typeof_System_Void ??= ResolvePrimitive("Void");

        /// <summary>
        /// Gets the <c>System.Boolean</c> type of the core library.
        /// </summary>
        internal Type System_Boolean => _typeof_System_Boolean ??= ResolvePrimitive("Boolean");

        /// <summary>
        /// Gets the <c>System.Char</c> type of the core library.
        /// </summary>
        internal Type System_Char => _typeof_System_Char ??= ResolvePrimitive("Char");

        /// <summary>
        /// Gets the <c>System.SByte</c> type of the core library.
        /// </summary>
        internal Type System_SByte => _typeof_System_SByte ??= ResolvePrimitive("SByte");

        /// <summary>
        /// Gets the <c>System.Byte</c> type of the core library.
        /// </summary>
        internal Type System_Byte => _typeof_System_Byte ??= ResolvePrimitive("Byte");

        /// <summary>
        /// Gets the <c>System.Int16</c> type of the core library.
        /// </summary>
        internal Type System_Int16 => _typeof_System_Int16 ??= ResolvePrimitive("Int16");

        /// <summary>
        /// Gets the <c>System.UInt16</c> type of the core library.
        /// </summary>
        internal Type System_UInt16 => _typeof_System_UInt16 ??= ResolvePrimitive("UInt16");

        /// <summary>
        /// Gets the <c>System.Int32</c> type of the core library.
        /// </summary>
        internal Type System_Int32 => _typeof_System_Int32 ??= ResolvePrimitive("Int32");

        /// <summary>
        /// Gets the <c>System.UInt32</c> type of the core library.
        /// </summary>
        internal Type System_UInt32 => _typeof_System_UInt32 ??= ResolvePrimitive("UInt32");

        /// <summary>
        /// Gets the <c>System.Int64</c> type of the core library.
        /// </summary>
        internal Type System_Int64 => _typeof_System_Int64 ??= ResolvePrimitive("Int64");

        /// <summary>
        /// Gets the <c>System.UInt64</c> type of the core library.
        /// </summary>
        internal Type System_UInt64 => _typeof_System_UInt64 ??= ResolvePrimitive("UInt64");

        /// <summary>
        /// Gets the <c>System.Single</c> type of the core library.
        /// </summary>
        internal Type System_Single => _typeof_System_Single ??= ResolvePrimitive("Single");

        /// <summary>
        /// Gets the <c>System.Double</c> type of the core library.
        /// </summary>
        internal Type System_Double => _typeof_System_Double ??= ResolvePrimitive("Double");

        /// <summary>
        /// Gets the <c>System.String</c> type of the core library.
        /// </summary>
        internal Type System_String => _typeof_System_String ??= ResolvePrimitive("String");

        /// <summary>
        /// Gets the <c>System.IntPtr</c> type of the core library.
        /// </summary>
        internal Type System_IntPtr => _typeof_System_IntPtr ??= ResolvePrimitive("IntPtr");

        /// <summary>
        /// Gets the <c>System.UIntPtr</c> type of the core library.
        /// </summary>
        internal Type System_UIntPtr => _typeof_System_UIntPtr ??= ResolvePrimitive("UIntPtr");

        /// <summary>
        /// Gets the <c>System.TypedReference</c> type of the core library.
        /// </summary>
        internal Type System_TypedReference => _typeof_System_TypedReference ??= ResolvePrimitive("TypedReference");

        /// <summary>
        /// Gets the <c>System.Type</c> type of the core library.
        /// </summary>
        internal Type System_Type => _typeof_System_Type ??= ResolvePrimitive("Type");

        /// <summary>
        /// Gets the <c>System.Array</c> type of the core library.
        /// </summary>
        internal Type System_Array => _typeof_System_Array ??= ResolvePrimitive("Array");

        /// <summary>
        /// Gets the <c>System.DateTime</c> type of the core library, or <see langword="null"/> if it is not defined.
        /// </summary>
        internal Type System_DateTime => _typeof_System_DateTime ??= ImportCoreLibType("System", "DateTime");

        /// <summary>
        /// Gets the <c>System.DBNull</c> type of the core library, or <see langword="null"/> if it is not defined.
        /// </summary>
        internal Type System_DBNull => _typeof_System_DBNull ??= ImportCoreLibType("System", "DBNull");

        /// <summary>
        /// Gets the <c>System.Decimal</c> type of the core library, or <see langword="null"/> if it is not defined.
        /// </summary>
        internal Type System_Decimal => _typeof_System_Decimal ??= ImportCoreLibType("System", "Decimal");

        /// <summary>
        /// Gets the <c>System.AttributeUsageAttribute</c> type of the core library, or <see langword="null"/> if it is
        /// not defined.
        /// </summary>
        internal Type System_AttributeUsageAttribute =>
            _typeof_System_AttributeUsageAttribute ??= ImportCoreLibType("System", "AttributeUsageAttribute");

        /// <summary>
        /// Gets the <c>System.Runtime.InteropServices.DllImportAttribute</c> type, or <see langword="null"/> if it cannot
        /// be found.
        /// </summary>
        internal Type System_Runtime_InteropServices_DllImportAttribute =>
            _typeof_System_Runtime_InteropServices_DllImportAttribute ??= ImportInteropType("DllImportAttribute");

        /// <summary>
        /// Gets the <c>System.Runtime.InteropServices.FieldOffsetAttribute</c> type, or <see langword="null"/> if it
        /// cannot be found.
        /// </summary>
        internal Type System_Runtime_InteropServices_FieldOffsetAttribute =>
            _typeof_System_Runtime_InteropServices_FieldOffsetAttribute ??= ImportInteropType("FieldOffsetAttribute");

        /// <summary>
        /// Gets the <c>System.Runtime.InteropServices.MarshalAsAttribute</c> type, or <see langword="null"/> if it cannot
        /// be found.
        /// </summary>
        internal Type System_Runtime_InteropServices_MarshalAsAttribute =>
            _typeof_System_Runtime_InteropServices_MarshalAsAttribute ??= ImportInteropType("MarshalAsAttribute");

        /// <summary>
        /// Gets the <c>System.Runtime.InteropServices.UnmanagedType</c> type, or <see langword="null"/> if it cannot be
        /// found.
        /// </summary>
        internal Type System_Runtime_InteropServices_UnmanagedType =>
            _typeof_System_Runtime_InteropServices_UnmanagedType ??= ImportInteropType("UnmanagedType");

        /// <summary>
        /// Gets the <c>System.Runtime.InteropServices.VarEnum</c> type, or <see langword="null"/> if it cannot be found.
        /// </summary>
        internal Type System_Runtime_InteropServices_VarEnum => _typeof_System_Runtime_InteropServices_VarEnum ??= ImportInteropType("VarEnum");

        /// <summary>
        /// Gets the <c>System.Runtime.InteropServices.PreserveSigAttribute</c> type, or <see langword="null"/> if it
        /// cannot be found.
        /// </summary>
        internal Type System_Runtime_InteropServices_PreserveSigAttribute =>
            _typeof_System_Runtime_InteropServices_PreserveSigAttribute ??= ImportInteropType("PreserveSigAttribute");

        /// <summary>
        /// Gets the <c>System.Runtime.InteropServices.CallingConvention</c> type, or <see langword="null"/> if it cannot
        /// be found.
        /// </summary>
        internal Type System_Runtime_InteropServices_CallingConvention =>
            _typeof_System_Runtime_InteropServices_CallingConvention ??= ImportInteropType("CallingConvention");

        /// <summary>
        /// Gets the <c>System.Runtime.InteropServices.CharSet</c> type, or <see langword="null"/> if it cannot be found.
        /// </summary>
        internal Type System_Runtime_InteropServices_CharSet => _typeof_System_Runtime_InteropServices_CharSet ??= ImportInteropType("CharSet");

        /// <summary>
        /// Gets the <c>System.Runtime.CompilerServices.DecimalConstantAttribute</c> type of the core library, or
        /// <see langword="null"/> if it is not defined.
        /// </summary>
        internal Type System_Runtime_CompilerServices_DecimalConstantAttribute =>
            _typeof_System_Runtime_CompilerServices_DecimalConstantAttribute ??= ImportCoreLibType("System.Runtime.CompilerServices", "DecimalConstantAttribute");

        /// <summary>
        /// Gets the <c>System.Runtime.CompilerServices.DateTimeConstantAttribute</c> type of the core library, or
        /// <see langword="null"/> if it is not defined.
        /// </summary>
        internal Type System_Runtime_CompilerServices_DateTimeConstantAttribute =>
            _typeof_System_Runtime_CompilerServices_DateTimeConstantAttribute ??= ImportCoreLibType("System.Runtime.CompilerServices", "DateTimeConstantAttribute");

        /// <summary>
        /// Gets the <c>System.Reflection.AssemblyCopyrightAttribute</c> type of the core library, or
        /// <see langword="null"/> if it is not defined.
        /// </summary>
        internal Type System_Reflection_AssemblyCopyrightAttribute =>
            _typeof_System_Reflection_AssemblyCopyrightAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyCopyrightAttribute");

        /// <summary>
        /// Gets the <c>System.Reflection.AssemblyTrademarkAttribute</c> type of the core library, or
        /// <see langword="null"/> if it is not defined.
        /// </summary>
        internal Type System_Reflection_AssemblyTrademarkAttribute =>
            _typeof_System_Reflection_AssemblyTrademarkAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyTrademarkAttribute");

        /// <summary>
        /// Gets the <c>System.Reflection.AssemblyProductAttribute</c> type of the core library, or <see langword="null"/>
        /// if it is not defined.
        /// </summary>
        internal Type System_Reflection_AssemblyProductAttribute =>
            _typeof_System_Reflection_AssemblyProductAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyProductAttribute");

        /// <summary>
        /// Gets the <c>System.Reflection.AssemblyCompanyAttribute</c> type of the core library, or <see langword="null"/>
        /// if it is not defined.
        /// </summary>
        internal Type System_Reflection_AssemblyCompanyAttribute =>
            _typeof_System_Reflection_AssemblyCompanyAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyCompanyAttribute");

        /// <summary>
        /// Gets the <c>System.Reflection.AssemblyDescriptionAttribute</c> type of the core library, or
        /// <see langword="null"/> if it is not defined.
        /// </summary>
        internal Type System_Reflection_AssemblyDescriptionAttribute =>
            _typeof_System_Reflection_AssemblyDescriptionAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyDescriptionAttribute");

        /// <summary>
        /// Gets the <c>System.Reflection.AssemblyTitleAttribute</c> type of the core library, or <see langword="null"/>
        /// if it is not defined.
        /// </summary>
        internal Type System_Reflection_AssemblyTitleAttribute =>
            _typeof_System_Reflection_AssemblyTitleAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyTitleAttribute");

        /// <summary>
        /// Gets the <c>System.Reflection.AssemblyInformationalVersionAttribute</c> type of the core library, or
        /// <see langword="null"/> if it is not defined.
        /// </summary>
        internal Type System_Reflection_AssemblyInformationalVersionAttribute =>
            _typeof_System_Reflection_AssemblyInformationalVersionAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyInformationalVersionAttribute");

        /// <summary>
        /// Gets the <c>System.Reflection.AssemblyFileVersionAttribute</c> type of the core library, or
        /// <see langword="null"/> if it is not defined.
        /// </summary>
        internal Type System_Reflection_AssemblyFileVersionAttribute =>
            _typeof_System_Reflection_AssemblyFileVersionAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyFileVersionAttribute");

        /// <summary>
        /// Gets the <c>System.Security.Permissions.CodeAccessSecurityAttribute</c> type of the core library, or
        /// <see langword="null"/> if it is not defined.
        /// </summary>
        internal Type System_Security_Permissions_CodeAccessSecurityAttribute =>
            _typeof_System_Security_Permissions_CodeAccessSecurityAttribute ??= ImportCoreLibType("System.Security.Permissions", "CodeAccessSecurityAttribute");

        /// <summary>
        /// Gets the <c>System.Security.Permissions.SecurityAction</c> type of the core library, or <see langword="null"/>
        /// if it is not defined.
        /// </summary>
        internal Type System_Security_Permissions_SecurityAction =>
            _typeof_System_Security_Permissions_SecurityAction ??= ImportCoreLibType("System.Security.Permissions", "SecurityAction");

        /// <summary>
        /// Gets a value indicating whether the core library is already known to the universe, without attempting to
        /// resolve it.
        /// </summary>
        internal bool HasCoreLib => GetLoadedAssembly(_coreLibName) != null;

        /// <summary>
        /// Invoked when an assembly reference cannot be satisfied by the assemblies already loaded. Handlers are called
        /// in the order they were added until one returns an assembly.
        /// </summary>
        public event ResolveEventHandler AssemblyResolve
        {
            add => _resolvers.Add(value);
            remove => _resolvers.Remove(value);
        }

        /// <summary>
        /// Imports the specified <see cref="System.Type"/> of the running runtime into the universe. Successful imports
        /// are cached.
        /// </summary>
        /// <param name="type">The runtime type to import.</param>
        /// <returns>The corresponding type in this universe, or <see langword="null"/> if it cannot be resolved.</returns>
        /// <exception cref="ArgumentException"><paramref name="type"/> is an IKVM.Reflection type.</exception>
        public Type Import(System.Type type)
        {
            if (!_importedTypes.TryGetValue(type, out var imported))
            {
                imported = ImportImpl(type);
                if (imported != null)
                    _importedTypes.Add(type, imported);
            }

            return imported;
        }

        /// <summary>
        /// Imports the specified runtime type into the universe, recursing into element types, generic type arguments
        /// and the declaring types of generic type parameters.
        /// </summary>
        /// <param name="type">The runtime type to import.</param>
        /// <returns>The corresponding type in this universe, or <see langword="null"/> if it cannot be resolved.</returns>
        /// <exception cref="ArgumentException"><paramref name="type"/> is an IKVM.Reflection type.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="type"/> has an element type but is not an array, by-reference or pointer type.</exception>
        /// <exception cref="NotImplementedException"><paramref name="type"/> is a generic method parameter.</exception>
        Type ImportImpl(System.Type type)
        {
            // caller should not attempt to import a custom reflection type
            if (TypeUtil.GetAssembly(type) == TypeUtil.GetAssembly(typeof(IKVM.Reflection.Type)))
                throw new ArgumentException("Did you really want to import " + type.FullName + "?");

            // array, pointer, or reference type
            if (type.HasElementType)
            {
                if (type.IsArray)
                {
                    // a single-dimensional, zero-based array has no explicit rank
                    if (type.Name.EndsWith("[]"))
                        return Import(type.GetElementType()).MakeArrayType();
                    else
                        return Import(type.GetElementType()).MakeArrayType(type.GetArrayRank());
                }

                // reimport the underlying element type and convert to byref
                if (type.IsByRef)
                    return Import(type.GetElementType()).MakeByRefType();

                // reimport the underlying element type and convert to pointer
                if (type.IsPointer)
                    return Import(type.GetElementType()).MakePointerType();

                throw new InvalidOperationException();
            }

            // type is a generic parameter; only generic type parameters are supported, by looking them up on the
            // imported declaring type
            if (type.IsGenericParameter)
            {
                if (TypeUtil.GetDeclaringMethod(type) != null)
                    throw new NotImplementedException();

                return Import(type.DeclaringType).GetGenericArguments()[type.GenericParameterPosition];
            }

            if (TypeUtil.IsGenericType(type) && !TypeUtil.IsGenericTypeDefinition(type))
            {
                System.Type[] args = TypeUtil.GetGenericArguments(type);
                Type[] importedArgs = new Type[args.Length];
                for (var i = 0; i < args.Length; i++)
                    importedArgs[i] = Import(args[i]);

                return Import(type.GetGenericTypeDefinition()).MakeGenericType(importedArgs);
            }

            if (TypeUtil.GetAssembly(type) == TypeUtil.GetAssembly(typeof(object)))
                // make sure mscorlib types always end up in our mscorlib
                return ResolveType(CoreLib, type.FullName);

            // FXBUG we parse the FullName here, because type.Namespace and type.Name are both broken on the CLR
            return ResolveType(Import(TypeUtil.GetAssembly(type)), type.FullName);
        }

        /// <summary>
        /// Imports the specified <see cref="System.Reflection.Assembly"/> into the universe by loading the assembly with
        /// the same full name.
        /// </summary>
        /// <param name="asm">The runtime assembly to import.</param>
        /// <returns>The corresponding assembly in this universe.</returns>
        /// <exception cref="FileNotFoundException">The assembly cannot be loaded.</exception>
        Assembly Import(System.Reflection.Assembly asm) => Load(asm.FullName);

        /// <summary>
        /// Opens a module file without loading it into the universe.
        /// </summary>
        /// <param name="path">The path of the module file.</param>
        /// <returns>The opened module, which owns the file until it is disposed or loaded.</returns>
        public RawModule OpenRawModule(string path)
        {
            path = Path.GetFullPath(path);

            FileStream fs = null;
            RawModule module;

            try
            {
                fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                module = OpenRawModule(fs, path);
                fs = null;
            }
            finally
            {
                fs?.Dispose();
            }

            return module;
        }

        /// <summary>
        /// Opens a module from a stream without loading it into the universe.
        /// </summary>
        /// <param name="stream">The stream that contains the module. It must be readable and seekable and be positioned at its start.</param>
        /// <param name="location">The location reported for the module.</param>
        /// <returns>The opened module.</returns>
        /// <exception cref="ArgumentException"><paramref name="stream"/> is not readable or seekable, or is not positioned at zero.</exception>
        public RawModule OpenRawModule(Stream stream, string location) => OpenRawModule(stream, location, false);

        /// <summary>
        /// Opens a module from a stream without loading it into the universe.
        /// </summary>
        /// <param name="stream">The stream that contains the module. It must be readable and seekable and be positioned at its start.</param>
        /// <param name="location">The location reported for the module.</param>
        /// <param name="mapped"><see langword="true"/> if the stream contains the image as mapped into memory rather than as laid out on disk.</param>
        /// <returns>The opened module.</returns>
        /// <exception cref="ArgumentException"><paramref name="stream"/> is not readable or seekable, or is not positioned at zero.</exception>
        RawModule OpenRawModule(Stream stream, string location, bool mapped)
        {
            if (!stream.CanRead || !stream.CanSeek || stream.Position != 0)
                throw new ArgumentException("Stream must support read/seek and current position must be zero.", "stream");

            return new RawModule(new ModuleReader(null, this, stream, location, mapped));
        }

        /// <summary>
        /// Loads the assembly whose manifest is in the module, unless an assembly with the same identity is already
        /// loaded.
        /// </summary>
        /// <param name="module">The module that contains the assembly manifest.</param>
        /// <returns>The loaded assembly, or the already loaded assembly with the same identity.</returns>
        public Assembly LoadAssembly(RawModule module)
        {
            var refname = module.GetAssemblyName().FullName;
            var asm = GetLoadedAssembly(refname);
            if (asm == null)
            {
                var asm1 = module.ToAssembly();
                _assemblies.Add(asm1);
                asm = asm1;
            }

            return asm;
        }

        /// <summary>
        /// Loads the contents of an assembly file on the specified path.
        /// </summary>
        /// <param name="path">The fully qualified path of the file to load.</param>
        /// <returns>The loaded assembly.</returns>
        /// <exception cref="FileNotFoundException">The file cannot be read.</exception>
        public Assembly LoadFile(string path)
        {
            try
            {
                using (var module = OpenRawModule(path))
                    return LoadAssembly(module);
            }
            catch (IOException e)
            {
                throw new FileNotFoundException(e.Message, e);
            }
            catch (UnauthorizedAccessException e)
            {
                throw new FileNotFoundException(e.Message, e);
            }
        }

        /// <summary>
        /// Gets the simple name from an assembly display name.
        /// </summary>
        /// <param name="refname">The assembly display name.</param>
        /// <returns>The simple name of the assembly.</returns>
        /// <exception cref="ArgumentException"><paramref name="refname"/> is not a valid assembly name.</exception>
        static string GetSimpleAssemblyName(string refname)
        {
            if (Fusion.ParseAssemblySimpleName(refname, out var pos, out var name) != ParseAssemblyResult.OK)
                throw new ArgumentException();

            return name;
        }

        /// <summary>
        /// Finds an already known assembly that matches the assembly name: first in the cache of names previously
        /// resolved, then among the assemblies loaded from files, caching a match by name.
        /// </summary>
        /// <param name="refname">The assembly display name to match.</param>
        /// <returns>The matching assembly, or <see langword="null"/> if none is loaded.</returns>
        Assembly GetLoadedAssembly(string refname)
        {
            if (!_assembliesByName.TryGetValue(refname, out var asm))
            {
                var simpleName = GetSimpleAssemblyName(refname);
                for (var i = 0; i < _assemblies.Count; i++)
                {
                    if (simpleName.Equals(_assemblies[i].Name, StringComparison.OrdinalIgnoreCase) && CompareAssemblyIdentity(_coreLibName, refname, false, _assemblies[i].FullName, false, out var result))
                    {
                        asm = _assemblies[i];
                        _assembliesByName.Add(refname, asm);
                        break;
                    }
                }
            }

            return asm;
        }

        /// <summary>
        /// Finds a dynamic assembly defined in this universe that matches the assembly name.
        /// </summary>
        /// <param name="refname">The assembly display name to match.</param>
        /// <returns>The matching dynamic assembly, or <see langword="null"/> if there is none.</returns>
        Assembly GetDynamicAssembly(string refname)
        {
            var simpleName = GetSimpleAssemblyName(refname);
            foreach (var asm in _dynamicAssemblies)
                if (simpleName.Equals(asm.Name, StringComparison.OrdinalIgnoreCase) && CompareAssemblyIdentity(_coreLibName, refname, false, asm.FullName, false, out var result))
                    return asm;

            return null;
        }

        /// <summary>
        /// Loads an assembly given the long form of its name.
        /// </summary>
        /// <param name="refname">The long form of the assembly name.</param>
        /// <returns>The loaded assembly.</returns>
        /// <exception cref="FileNotFoundException">The assembly cannot be found.</exception>
        public Assembly Load(string refname) => Load(refname, null, true);

        /// <summary>
        /// Loads an assembly given the long form of its name. Assemblies already loaded from files are tried first, then
        /// the <see cref="AssemblyResolve"/> handlers, and then the dynamic assemblies of the universe.
        /// </summary>
        /// <param name="refname">The long form of the assembly name.</param>
        /// <param name="requestingModule">The module that references the assembly, passed to the resolve handlers, or <see langword="null"/>.</param>
        /// <param name="throwOnError"><see langword="true"/> to throw if the assembly cannot be found; <see langword="false"/> to return <see langword="null"/>.</param>
        /// <returns>The assembly, or <see langword="null"/> if it cannot be found and <paramref name="throwOnError"/> is <see langword="false"/>.</returns>
        /// <exception cref="FileNotFoundException">The assembly cannot be found and <paramref name="throwOnError"/> is <see langword="true"/>.</exception>
        internal Assembly Load(string refname, Module requestingModule, bool throwOnError)
        {
            var asm = GetLoadedAssembly(refname);
            if (asm != null)
                return asm;

            if (_resolvers.Count == 0)
            {
                asm = DefaultResolver(refname, throwOnError);
            }
            else
            {
                var args = new ResolveEventArgs(refname, requestingModule?.Assembly);
                foreach (var evt in _resolvers)
                {
                    asm = evt(this, args);
                    if (asm != null)
                        break;
                }

                asm ??= GetDynamicAssembly(refname);
            }

            if (asm != null)
            {
                // cache assembly by both the lookup name and the resolved name
                var defname = asm.FullName;
                _assembliesByName[refname] = asm;
                _assembliesByName[defname] = asm;
                return asm;
            }

            if (throwOnError)
                throw new FileNotFoundException(refname);

            return null;
        }

        /// <summary>
        /// Resolves an assembly when no <see cref="AssemblyResolve"/> handlers are registered, by looking among the
        /// dynamic assemblies of the universe.
        /// </summary>
        /// <param name="refname">The assembly display name to resolve.</param>
        /// <param name="throwOnError">Unused.</param>
        /// <returns>The matching dynamic assembly, or <see langword="null"/> if there is none.</returns>
        Assembly DefaultResolver(string refname, bool throwOnError)
        {
            var asm = GetDynamicAssembly(refname);
            if (asm != null)
                return asm;

            return null;
        }

        /// <summary>
        /// Gets the type with the specified assembly-qualified name. A name without an assembly is looked up in the core
        /// library.
        /// </summary>
        /// <param name="assemblyQualifiedTypeName">The assembly-qualified name of the type.</param>
        /// <param name="throwOnError"><see langword="true"/> to throw if the type cannot be found; <see langword="false"/> to return <see langword="null"/>.</param>
        /// <returns>The type, or <see langword="null"/> if it cannot be found and <paramref name="throwOnError"/> is <see langword="false"/>.</returns>
        public Type GetType(string assemblyQualifiedTypeName, bool throwOnError) =>
            // to be more compatible with Type.GetType(), we could call Assembly.GetCallingAssembly(),
            // import that assembly and pass it as the context, but implicitly importing is considered evil
            GetType(null, assemblyQualifiedTypeName, throwOnError, false);

        /// <summary>
        /// Gets the type with the specified assembly-qualified name, resolving a name without an assembly in
        /// <paramref name="context"/>. Note that the context is slightly different from the calling assembly of
        /// <c>System.Type.GetType</c>, because it is also passed to the <see cref="AssemblyResolve"/> event as the
        /// requesting assembly.
        /// </summary>
        /// <param name="context">The assembly the name is resolved relative to, or <see langword="null"/> to use the core library.</param>
        /// <param name="assemblyQualifiedTypeName">The assembly-qualified name of the type.</param>
        /// <param name="throwOnError"><see langword="true"/> to throw if the type cannot be found; <see langword="false"/> to return <see langword="null"/>.</param>
        /// <param name="ignoreCase"><see langword="true"/> to perform a case-insensitive search for the type name.</param>
        /// <returns>The type, or <see langword="null"/> if it cannot be found and <paramref name="throwOnError"/> is <see langword="false"/>.</returns>
        public Type GetType(Assembly context, string assemblyQualifiedTypeName, bool throwOnError, bool ignoreCase)
        {
            TypeNameParser parser = TypeNameParser.Parse(assemblyQualifiedTypeName, throwOnError);
            if (parser.Error)
            {
                return null;
            }
            return parser.GetType(this, context?.ManifestModule, throwOnError, assemblyQualifiedTypeName, false, ignoreCase);
        }

        /// <summary>
        /// Resolves the type with the specified assembly-qualified name. This is similar to
        /// <see cref="GetType(Assembly, string, bool, bool)"/>, but assumes that the type must exist: if
        /// <see cref="UniverseOptions.ResolveMissingMembers"/> is enabled, a missing type is created when it cannot be
        /// found.
        /// </summary>
        /// <param name="context">The assembly the name is resolved relative to, or <see langword="null"/> to use the core library.</param>
        /// <param name="assemblyQualifiedTypeName">The assembly-qualified name of the type.</param>
        /// <returns>The type, or <see langword="null"/> if the name is malformed or the type cannot be found.</returns>
        public Type ResolveType(Assembly context, string assemblyQualifiedTypeName)
        {
            TypeNameParser parser = TypeNameParser.Parse(assemblyQualifiedTypeName, false);
            if (parser.Error)
            {
                return null;
            }
            return parser.GetType(this, context?.ManifestModule, false, assemblyQualifiedTypeName, true, false);
        }

        /// <summary>
        /// Gets the builtin type with the specified name. The <paramref name="ns"/> parameter must be "System".
        /// </summary>
        /// <param name="ns">The namespace of the type.</param>
        /// <param name="name">The name of the type.</param>
        /// <returns>The built-in type, or <see langword="null"/> if <paramref name="ns"/> and <paramref name="name"/> do not name one of the types with a special signature encoding.</returns>
        public Type GetBuiltInType(string ns, string name)
        {
            if (ns != "System")
                return null;

            return name switch
            {
                "Boolean" => System_Boolean,
                "Char" => System_Char,
                "Object" => System_Object,
                "String" => System_String,
                "Single" => System_Single,
                "Double" => System_Double,
                "SByte" => System_SByte,
                "Int16" => System_Int16,
                "Int32" => System_Int32,
                "Int64" => System_Int64,
                "IntPtr" => System_IntPtr,
                "UIntPtr" => System_UIntPtr,
                "TypedReference" => System_TypedReference,
                "Byte" => System_Byte,
                "UInt16" => System_UInt16,
                "UInt32" => System_UInt32,
                "UInt64" => System_UInt64,
                "Void" => System_Void,
                _ => null,
            };
        }

        /// <summary>
        /// Gets the set of all loaded assemblies.
        /// </summary>
        /// <returns>The assemblies loaded from files, followed by the dynamic assemblies.</returns>
        public Assembly[] GetAssemblies()
        {
            var array = new Assembly[_assemblies.Count + _dynamicAssemblies.Count];
            for (var i = 0; i < _assemblies.Count; i++)
                array[i] = _assemblies[i];
            for (int i = 0, j = _assemblies.Count; j < array.Length; i++, j++)
                array[j] = _dynamicAssemblies[i];

            return array;
        }

        /// <summary>
        /// Compares two assembly identities to determine whether they are equivalent, using the native fusion API
        /// where available and the managed implementation otherwise.
        /// </summary>
        /// <param name="coreLibName">The simple name of the core library, which is matched regardless of version.</param>
        /// <param name="assemblyIdentity1">The textual identity of the first assembly.</param>
        /// <param name="unified1"><see langword="true"/> if the first assembly is unified by the runtime.</param>
        /// <param name="assemblyIdentity2">The textual identity of the second assembly.</param>
        /// <param name="unified2"><see langword="true"/> if the second assembly is unified by the runtime.</param>
        /// <param name="result">Set to a value that describes how the identities compare.</param>
        /// <returns><see langword="true"/> if the identities are equivalent; otherwise, <see langword="false"/>.</returns>
        public bool CompareAssemblyIdentity(string coreLibName, string assemblyIdentity1, bool unified1, string assemblyIdentity2, bool unified2, out AssemblyComparisonResult result)
        {
            return _useNativeFusion
                ? Fusion.CompareAssemblyIdentityNative(coreLibName, assemblyIdentity1, unified1, assemblyIdentity2, unified2, out result)
                : Fusion.CompareAssemblyIdentityPure(coreLibName, assemblyIdentity1, unified1, assemblyIdentity2, unified2, out result);
        }

        /// <summary>
        /// Defines a dynamic assembly that has the specified name and access rights, saved to the specified directory.
        /// </summary>
        /// <param name="name">The unique identity of the assembly.</param>
        /// <param name="access">The access rights of the assembly. Ignored; every dynamic assembly can be saved.</param>
        /// <param name="dir">The directory the assembly is saved to, or <see langword="null"/> for the current directory.</param>
        /// <returns>An object that represents the new assembly.</returns>
        public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir) =>
            new AssemblyBuilder(this, name, dir, null);

        /// <summary>
        /// Adds a newly created dynamic assembly to the universe.
        /// </summary>
        /// <param name="asm">The dynamic assembly.</param>
        internal void RegisterDynamicAssembly(AssemblyBuilder asm) => _dynamicAssemblies.Add(asm);

        /// <summary>
        /// Forgets every name an assembly has been cached under, after its name has changed.
        /// </summary>
        /// <param name="assembly">The assembly whose name changed.</param>
        /// <param name="oldName">The previous name of the assembly. Unused; the cache is searched by assembly.</param>
        internal void RenameAssembly(Assembly assembly, AssemblyName oldName)
        {
            List<string> remove = [];
            foreach (KeyValuePair<string, Assembly> kv in _assembliesByName)
            {
                if (kv.Value == assembly)
                {
                    remove.Add(kv.Key);
                }
            }
            foreach (var key in remove)
            {
                _assembliesByName.Remove(key);
            }
        }

        /// <summary>
        /// Disposes the modules of every loaded and dynamic assembly.
        /// </summary>
        public void Dispose()
        {
            foreach (Assembly asm in _assemblies)
            {
                foreach (Module mod in asm.GetLoadedModules())
                {
                    mod.Dispose();
                }
            }
            foreach (AssemblyBuilder asm in _dynamicAssemblies)
            {
                foreach (Module mod in asm.GetLoadedModules())
                {
                    mod.Dispose();
                }
            }
        }

        /// <summary>
        /// Creates a placeholder for an assembly that cannot be found, and registers it under its full name unless
        /// another assembly is already registered under that name.
        /// </summary>
        /// <param name="assemblyName">The display name of the missing assembly.</param>
        /// <returns>The missing assembly.</returns>
        public Assembly CreateMissingAssembly(string assemblyName)
        {
            Assembly asm = new MissingAssembly(this, assemblyName);
            var name = asm.FullName;
            if (!_assembliesByName.ContainsKey(name))
            {
                _assembliesByName.Add(name, asm);
            }
            return asm;
        }

        /// <summary>
        /// Gets a value indicating whether references to members that cannot be found produce missing members instead
        /// of exceptions.
        /// </summary>
        internal bool MissingMemberResolution => _resolveMissingMembers;

        /// <summary>
        /// Gets a value indicating whether function pointers in signatures are represented as first class types instead
        /// of being replaced by <c>System.IntPtr</c>.
        /// </summary>
        internal bool EnableFunctionPointers => _enableFunctionPointers;

        /// <summary>
        /// The key of a missing type: its name together with the declaring type or module that scopes it.
        /// </summary>
        struct ScopedTypeName : IEquatable<ScopedTypeName>
        {

            readonly object _scope;
            readonly TypeName _name;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="scope">The declaring type, or the module for a top-level type.</param>
            /// <param name="name">The name of the type.</param>
            internal ScopedTypeName(object scope, TypeName name)
            {
                _scope = scope;
                _name = name;
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                ScopedTypeName? other = obj as ScopedTypeName?;
                return other != null && ((IEquatable<ScopedTypeName>)other.Value).Equals(this);
            }

            /// <inheritdoc />
            public override int GetHashCode() => _scope.GetHashCode() * 7 + _name.GetHashCode();

            /// <summary>
            /// Determines whether two keys have the same scope, compared by reference, and the same name.
            /// </summary>
            /// <param name="other">The key to compare with.</param>
            /// <returns><see langword="true"/> if the keys are equal; otherwise, <see langword="false"/>.</returns>
            bool IEquatable<ScopedTypeName>.Equals(ScopedTypeName other) => other._scope == _scope && other._name == _name;

        }

        /// <summary>
        /// Gets the missing type with the specified name, creating it on first request, and raises
        /// <see cref="ResolvedMissingMember"/> unless the type belongs to a missing assembly.
        /// </summary>
        /// <param name="requester">The module that references the type, reported to <see cref="ResolvedMissingMember"/>.</param>
        /// <param name="module">The module the type is expected in.</param>
        /// <param name="declaringType">The declaring type of a nested type, or <see langword="null"/> for a top-level type.</param>
        /// <param name="typeName">The name of the type.</param>
        /// <returns>The missing type.</returns>
        Type GetMissingType(Module requester, Module module, Type declaringType, TypeName typeName)
        {
            _missingTypes ??= [];
            ScopedTypeName stn = new ScopedTypeName(declaringType ?? (object)module, typeName);
            if (!_missingTypes.TryGetValue(stn, out var type))
            {
                type = new MissingType(module, declaringType, typeName.Namespace, typeName.Name);
                _missingTypes.Add(stn, type);
            }
            if (ResolvedMissingMember != null && !module.Assembly.__IsMissing)
            {
                ResolvedMissingMember(requester, type);
            }
            return type;
        }

        /// <summary>
        /// Gets a missing type for a type that cannot be found, if missing member resolution is enabled or the module
        /// belongs to a missing assembly.
        /// </summary>
        /// <param name="requester">The module that references the type.</param>
        /// <param name="module">The module the type is expected in.</param>
        /// <param name="declaringType">The declaring type of a nested type, or <see langword="null"/> for a top-level type.</param>
        /// <param name="typeName">The name of the type.</param>
        /// <returns>The missing type.</returns>
        /// <exception cref="TypeLoadException">Missing member resolution is disabled and the module's assembly is not missing.</exception>
        internal Type GetMissingTypeOrThrow(Module requester, Module module, Type declaringType, TypeName typeName)
        {
            if (_resolveMissingMembers || module.Assembly.__IsMissing)
            {
                return GetMissingType(requester, module, declaringType, typeName);
            }
            var fullName = TypeNameParser.Escape(typeName.ToString());
            if (declaringType != null)
            {
                fullName = declaringType.FullName + "+" + fullName;
            }
            throw new TypeLoadException(string.Format("Type '{0}' not found in assembly '{1}'", fullName, module.Assembly.FullName));
        }

        /// <summary>
        /// Creates a missing method or constructor for a method that cannot be found, if missing member resolution is
        /// enabled.
        /// </summary>
        /// <param name="requester">The module that references the method.</param>
        /// <param name="declaringType">The type the method is expected on.</param>
        /// <param name="name">The name of the method; <c>.ctor</c> produces a constructor.</param>
        /// <param name="signature">The signature of the method.</param>
        /// <returns>The missing method or constructor.</returns>
        /// <exception cref="MissingMethodException">Missing member resolution is disabled.</exception>
        internal MethodBase GetMissingMethodOrThrow(Module requester, Type declaringType, string name, MethodSignature signature)
        {
            if (_resolveMissingMembers)
            {
                var method = (MethodBase)new MissingMethod(declaringType, name, signature);
                if (name == ".ctor")
                    method = new ConstructorInfoImpl((MethodInfo)method);

                if (ResolvedMissingMember != null)
                    ResolvedMissingMember(requester, method);

                return method;
            }

            throw new MissingMethodException(declaringType.ToString(), name);
        }

        /// <summary>
        /// Creates a missing field for a field that cannot be found, if missing member resolution is enabled.
        /// </summary>
        /// <param name="requester">The module that references the field.</param>
        /// <param name="declaringType">The type the field is expected on.</param>
        /// <param name="name">The name of the field.</param>
        /// <param name="signature">The signature of the field.</param>
        /// <returns>The missing field.</returns>
        /// <exception cref="MissingFieldException">Missing member resolution is disabled.</exception>
        internal FieldInfo GetMissingFieldOrThrow(Module requester, Type declaringType, string name, FieldSignature signature)
        {
            if (_resolveMissingMembers)
            {
                var field = (FieldInfo)new MissingField(declaringType, name, signature);
                if (ResolvedMissingMember != null)
                    ResolvedMissingMember(requester, field);

                return field;
            }

            throw new MissingFieldException(declaringType.ToString(), name);
        }

        /// <summary>
        /// Creates a missing property for a property that cannot be found, if missing member resolution is enabled or
        /// the declaring type is itself missing.
        /// </summary>
        /// <param name="requester">The module that references the property.</param>
        /// <param name="declaringType">The type the property is expected on.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="propertySignature">The signature of the property.</param>
        /// <returns>The missing property.</returns>
        /// <exception cref="System.MissingMemberException">Missing member resolution is disabled and the declaring type is not missing.</exception>
        internal PropertyInfo GetMissingPropertyOrThrow(Module requester, Type declaringType, string name, PropertySignature propertySignature)
        {
            // HACK we need to check __IsMissing here, because Type doesn't have a FindProperty API
            // since properties are never resolved, except by custom attributes
            if (_resolveMissingMembers || declaringType.__IsMissing)
            {
                var property = (PropertyInfo)new MissingProperty(declaringType, name, propertySignature);
                if (ResolvedMissingMember != null && !declaringType.__IsMissing)
                    ResolvedMissingMember(requester, property);

                return property;
            }

            throw new System.MissingMemberException(declaringType.ToString(), name);
        }

        /// <summary>
        /// Returns the canonical instance of a type, registering <paramref name="type"/> as the canonical instance if no
        /// equal type has been registered yet.
        /// </summary>
        /// <param name="type">The type to canonicalize.</param>
        /// <returns>The registered type that is equal to <paramref name="type"/>.</returns>
        internal Type CanonicalizeType(Type type)
        {
            if (!_canonicalizedTypes.TryGetValue(type, out var canon))
            {
                canon = type;
                _canonicalizedTypes.Add(canon, canon);
            }

            return canon;
        }

        /// <summary>
        /// Raised when a reference to a type or member that cannot be found is resolved to a missing type or member.
        /// </summary>
        public event ResolvedMissingMemberHandler ResolvedMissingMember;

        /// <summary>
        /// Gets a value indicating whether the module builders should produce deterministic images.
        /// </summary>
        internal bool Deterministic => (_options & UniverseOptions.DeterministicOutput) != 0;

        /// <summary>
        /// Gets the hash algorithm to use for PDB checksums.
        /// </summary>
        internal HashAlgorithmName PdbChecksumAlgorithm => _pdbChecksumAlgorithm;

    }

}
