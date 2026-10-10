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
    /// Provides a view of the types available to the reflection infrastructure.
    /// </summary>
    sealed class Universe : IDisposable
    {

        public static readonly string NetCoreLibName = "System.Runtime";
        public static readonly string NetFxCoreLibName = "mscorlib";

#if NETCOREAPP3_1_OR_GREATER

        public static readonly string DefaultCoreLibName = NetCoreLibName;

#elif NETFRAMEWORK

        public static readonly string DefaultCoreLibName = NetFxCoreLibName;

#endif

#if NETFRAMEWORK
        internal static readonly bool MonoRuntime = System.Type.GetType("Mono.Runtime") != null;
        static readonly bool s_coreRuntime = false;
#else
        internal static readonly bool MonoRuntime = false;
        static readonly bool s_coreRuntime = true;
#endif

        readonly string _coreLibName;
        readonly Dictionary<Type, Type> _canonicalizedTypes = [];
        readonly List<AssemblyReader> _assemblies = [];
        readonly List<AssemblyBuilder> _dynamicAssemblies = [];
        readonly Dictionary<string, Assembly> _assembliesByName = [];
        readonly Dictionary<System.Type, Type> _importedTypes = [];
        Dictionary<ScopedTypeName, Type> _missingTypes;
        readonly bool _resolveMissingMembers;
        readonly bool _enableFunctionPointers;
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
        /// <param name="coreLibName"></param>
        public Universe(string coreLibName = null) :
            this(UniverseOptions.None, coreLibName)
        {
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="coreLibName"></param>
        public Universe(UniverseOptions options, string coreLibName = null)
        {
            _options = options;
            _coreLibName = coreLibName ?? DefaultCoreLibName;
            _enableFunctionPointers = (options & UniverseOptions.EnableFunctionPointers) != 0;
            _useNativeFusion = GetUseNativeFusion();
            _resolveMissingMembers = (options & UniverseOptions.ResolveMissingMembers) != 0;
        }

        /// <summary>
        /// Sets a factory to create a <see cref="ISymbolWriter"/> for a module name.
        /// </summary>
        /// <param name="factory"></param>
        public void SetSymbolWriterFactory(Func<ModuleBuilder, ISymbolWriter> factory) => _symbolWriterFactory = factory;

        /// <summary>
        /// Creates a new <see cref="ISymbolWriter"/> for the specified assembly builder and new module name.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        internal ISymbolWriter CreateSymbolWriter(ModuleBuilder module) => _symbolWriterFactory?.Invoke(module);

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
        /// Gets the core library.
        /// </summary>
        public Assembly CoreLib => Load(_coreLibName);

        /// <summary>
        /// Attempts to import the type from the core library.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Type ImportCoreLibType(string ns, string name)
        {
            if (CoreLib.__IsMissing)
                return CoreLib.ResolveType(null, new TypeName(ns, name));

            // We use FindType instead of ResolveType here, because on some versions of mscorlib some of
            // the special types we use/support are missing and the type properties are defined to
            // return null in that case.
            // Note that we don't have to unescape type.Name here, because none of the names contain special characters.
            return CoreLib.FindType(new TypeName(ns, name));
        }

        /// <summary>
        /// Imports a type from the System.Runtime.InteropServices namespace. The .NET Framework core library defines all
        /// of them, but .NET reference assemblies define some in the System.Runtime.InteropServices assembly instead.
        /// Returns <c>null</c> if the type cannot be found.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Type ImportInteropType(string name)
        {
            var typeName = new TypeName("System.Runtime.InteropServices", name);
            if (ImportCoreLibType(typeName.Namespace, typeName.Name) is { } type)
                return type;

            return Load("System.Runtime.InteropServices", null, false) is { __IsMissing: false } assembly ? assembly.FindType(typeName) : null;
        }

        /// <summary>
        /// Resolves the primitive type with teh specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Type ResolvePrimitive(string name) =>
            // Primitive here means that these types have a special metadata encoding, which means that
            // there can be references to them without referring to them by name explicitly.
            // We want these types to be usable even when they don't exist in mscorlib or there is no mscorlib loaded.
            CoreLib.FindType(new TypeName("System", name)) ?? GetMissingType(null, CoreLib.ManifestModule, null, new TypeName("System", name));

        internal Type System_Object => _typeof_System_Object ??= ResolvePrimitive("Object");

        internal Type System_ValueType => _typeof_System_ValueType ??= ResolvePrimitive("ValueType");

        internal Type System_MulticastDelegate => _typeof_System_MulticastDelegate ??= ImportCoreLibType("System", "MulticastDelegate");

        internal Type System_Void => _typeof_System_Void ??= ResolvePrimitive("Void");

        internal Type System_Boolean => _typeof_System_Boolean ??= ResolvePrimitive("Boolean");

        internal Type System_Char => _typeof_System_Char ??= ResolvePrimitive("Char");

        internal Type System_SByte => _typeof_System_SByte ??= ResolvePrimitive("SByte");

        internal Type System_Byte => _typeof_System_Byte ??= ResolvePrimitive("Byte");

        internal Type System_Int16 => _typeof_System_Int16 ??= ResolvePrimitive("Int16");

        internal Type System_UInt16 => _typeof_System_UInt16 ??= ResolvePrimitive("UInt16");

        internal Type System_Int32 => _typeof_System_Int32 ??= ResolvePrimitive("Int32");

        internal Type System_UInt32 => _typeof_System_UInt32 ??= ResolvePrimitive("UInt32");

        internal Type System_Int64 => _typeof_System_Int64 ??= ResolvePrimitive("Int64");

        internal Type System_UInt64 => _typeof_System_UInt64 ??= ResolvePrimitive("UInt64");

        internal Type System_Single => _typeof_System_Single ??= ResolvePrimitive("Single");

        internal Type System_Double => _typeof_System_Double ??= ResolvePrimitive("Double");

        internal Type System_String => _typeof_System_String ??= ResolvePrimitive("String");

        internal Type System_IntPtr => _typeof_System_IntPtr ??= ResolvePrimitive("IntPtr");

        internal Type System_UIntPtr => _typeof_System_UIntPtr ??= ResolvePrimitive("UIntPtr");

        internal Type System_TypedReference => _typeof_System_TypedReference ??= ResolvePrimitive("TypedReference");

        internal Type System_Type => _typeof_System_Type ??= ResolvePrimitive("Type");

        internal Type System_Array => _typeof_System_Array ??= ResolvePrimitive("Array");

        internal Type System_DateTime => _typeof_System_DateTime ??= ImportCoreLibType("System", "DateTime");

        internal Type System_DBNull => _typeof_System_DBNull ??= ImportCoreLibType("System", "DBNull");

        internal Type System_Decimal => _typeof_System_Decimal ??= ImportCoreLibType("System", "Decimal");

        internal Type System_AttributeUsageAttribute =>
            _typeof_System_AttributeUsageAttribute ??= ImportCoreLibType("System", "AttributeUsageAttribute");

        internal Type System_Runtime_InteropServices_DllImportAttribute =>
            _typeof_System_Runtime_InteropServices_DllImportAttribute ??= ImportInteropType("DllImportAttribute");

        internal Type System_Runtime_InteropServices_FieldOffsetAttribute =>
            _typeof_System_Runtime_InteropServices_FieldOffsetAttribute ??= ImportInteropType("FieldOffsetAttribute");

        internal Type System_Runtime_InteropServices_MarshalAsAttribute =>
            _typeof_System_Runtime_InteropServices_MarshalAsAttribute ??= ImportInteropType("MarshalAsAttribute");

        internal Type System_Runtime_InteropServices_UnmanagedType =>
            _typeof_System_Runtime_InteropServices_UnmanagedType ??= ImportInteropType("UnmanagedType");

        internal Type System_Runtime_InteropServices_VarEnum => _typeof_System_Runtime_InteropServices_VarEnum ??= ImportInteropType("VarEnum");

        internal Type System_Runtime_InteropServices_PreserveSigAttribute =>
            _typeof_System_Runtime_InteropServices_PreserveSigAttribute ??= ImportInteropType("PreserveSigAttribute");

        internal Type System_Runtime_InteropServices_CallingConvention =>
            _typeof_System_Runtime_InteropServices_CallingConvention ??= ImportInteropType("CallingConvention");

        internal Type System_Runtime_InteropServices_CharSet => _typeof_System_Runtime_InteropServices_CharSet ??= ImportInteropType("CharSet");

        internal Type System_Runtime_CompilerServices_DecimalConstantAttribute =>
            _typeof_System_Runtime_CompilerServices_DecimalConstantAttribute ??= ImportCoreLibType("System.Runtime.CompilerServices", "DecimalConstantAttribute");

        internal Type System_Runtime_CompilerServices_DateTimeConstantAttribute =>
            _typeof_System_Runtime_CompilerServices_DateTimeConstantAttribute ??= ImportCoreLibType("System.Runtime.CompilerServices", "DateTimeConstantAttribute");

        internal Type System_Reflection_AssemblyCopyrightAttribute =>
            _typeof_System_Reflection_AssemblyCopyrightAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyCopyrightAttribute");

        internal Type System_Reflection_AssemblyTrademarkAttribute =>
            _typeof_System_Reflection_AssemblyTrademarkAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyTrademarkAttribute");

        internal Type System_Reflection_AssemblyProductAttribute =>
            _typeof_System_Reflection_AssemblyProductAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyProductAttribute");

        internal Type System_Reflection_AssemblyCompanyAttribute =>
            _typeof_System_Reflection_AssemblyCompanyAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyCompanyAttribute");

        internal Type System_Reflection_AssemblyDescriptionAttribute =>
            _typeof_System_Reflection_AssemblyDescriptionAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyDescriptionAttribute");

        internal Type System_Reflection_AssemblyTitleAttribute =>
            _typeof_System_Reflection_AssemblyTitleAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyTitleAttribute");

        internal Type System_Reflection_AssemblyInformationalVersionAttribute =>
            _typeof_System_Reflection_AssemblyInformationalVersionAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyInformationalVersionAttribute");

        internal Type System_Reflection_AssemblyFileVersionAttribute =>
            _typeof_System_Reflection_AssemblyFileVersionAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyFileVersionAttribute");

        internal Type System_Security_Permissions_CodeAccessSecurityAttribute =>
            _typeof_System_Security_Permissions_CodeAccessSecurityAttribute ??= ImportCoreLibType("System.Security.Permissions", "CodeAccessSecurityAttribute");

        internal Type System_Security_Permissions_SecurityAction =>
            _typeof_System_Security_Permissions_SecurityAction ??= ImportCoreLibType("System.Security.Permissions", "SecurityAction");

        /// <summary>
        /// Returns <c>true</c> if a CoreLib implementation has been loaded.
        /// </summary>
        internal bool HasCoreLib => GetLoadedAssembly(_coreLibName) != null;

        /// <summary>
        /// Invoked when an attempt to resolve an assembly occurs.
        /// </summary>
        public event ResolveEventHandler AssemblyResolve
        { add => _resolvers.Add(value); remove => _resolvers.Remove(value);
        }

        /// <summary>
        /// Imports the specified <see cref="System.Type"/> into the environment.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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
        /// Imports the specified type into the environment.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NotImplementedException"></exception>
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
                    // type has no rank
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

            // type represents a generic parameter of a generic method definition
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
        /// Imports the specified <see cref="System.Reflection.Assembly"/> into the environment.
        /// </summary>
        /// <param name="asm"></param>
        /// <returns></returns>
        Assembly Import(System.Reflection.Assembly asm) => Load(asm.FullName);

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

        public RawModule OpenRawModule(Stream stream, string location) => OpenRawModule(stream, location, false);

        RawModule OpenRawModule(Stream stream, string location, bool mapped)
        {
            if (!stream.CanRead || !stream.CanSeek || stream.Position != 0)
                throw new ArgumentException("Stream must support read/seek and current position must be zero.", "stream");

            return new RawModule(new ModuleReader(null, this, stream, location, mapped));
        }

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

        static string GetSimpleAssemblyName(string refname)
        {
            if (Fusion.ParseAssemblySimpleName(refname, out var pos, out var name) != ParseAssemblyResult.OK)
                throw new ArgumentException();

            return name;
        }

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

        Assembly GetDynamicAssembly(string refname)
        {
            var simpleName = GetSimpleAssemblyName(refname);
            foreach (var asm in _dynamicAssemblies)
                if (simpleName.Equals(asm.Name, StringComparison.OrdinalIgnoreCase) && CompareAssemblyIdentity(_coreLibName, refname, false, asm.FullName, false, out var result))
                    return asm;

            return null;
        }

        public Assembly Load(string refname) => Load(refname, null, true);

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

        Assembly DefaultResolver(string refname, bool throwOnError)
        {
            var asm = GetDynamicAssembly(refname);
            if (asm != null)
                return asm;

            return null;
        }

        public Type GetType(string assemblyQualifiedTypeName, bool throwOnError) =>
            // to be more compatible with Type.GetType(), we could call Assembly.GetCallingAssembly(),
            // import that assembly and pass it as the context, but implicitly importing is considered evil
            GetType(null, assemblyQualifiedTypeName, throwOnError, false);

        // note that context is slightly different from the calling assembly (System.Type.GetType),
        // because context is passed to the AssemblyResolve event as the RequestingAssembly
        public Type GetType(Assembly context, string assemblyQualifiedTypeName, bool throwOnError, bool ignoreCase)
        {
            TypeNameParser parser = TypeNameParser.Parse(assemblyQualifiedTypeName, throwOnError);
            if (parser.Error)
            {
                return null;
            }
            return parser.GetType(this, context?.ManifestModule, throwOnError, assemblyQualifiedTypeName, false, ignoreCase);
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
            return parser.GetType(this, context?.ManifestModule, false, assemblyQualifiedTypeName, true, false);
        }

        /// <summary>
        /// Gets the builtin type with the specified name. The <paramref name="ns"/> parameter must be "System".
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="name"></param>
        /// <returns></returns>
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
        /// <returns></returns>
        public Assembly[] GetAssemblies()
        {
            var array = new Assembly[_assemblies.Count + _dynamicAssemblies.Count];
            for (var i = 0; i < _assemblies.Count; i++)
                array[i] = _assemblies[i];
            for (int i = 0, j = _assemblies.Count; j < array.Length; i++, j++)
                array[j] = _dynamicAssemblies[i];

            return array;
        }

        public bool CompareAssemblyIdentity(string coreLibName, string assemblyIdentity1, bool unified1, string assemblyIdentity2, bool unified2, out AssemblyComparisonResult result)
        {
            return _useNativeFusion
                ? Fusion.CompareAssemblyIdentityNative(coreLibName, assemblyIdentity1, unified1, assemblyIdentity2, unified2, out result)
                : Fusion.CompareAssemblyIdentityPure(coreLibName, assemblyIdentity1, unified1, assemblyIdentity2, unified2, out result);
        }

        public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir) =>
            new AssemblyBuilder(this, name, dir, null);

        internal void RegisterDynamicAssembly(AssemblyBuilder asm) => _dynamicAssemblies.Add(asm);

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

        internal bool MissingMemberResolution => _resolveMissingMembers;

        internal bool EnableFunctionPointers => _enableFunctionPointers;

        struct ScopedTypeName : IEquatable<ScopedTypeName>
        {
            readonly object _scope;
            readonly TypeName _name;

            internal ScopedTypeName(object scope, TypeName name)
            {
                _scope = scope;
                _name = name;
            }

            public override bool Equals(object obj)
            {
                ScopedTypeName? other = obj as ScopedTypeName?;
                return other != null && ((IEquatable<ScopedTypeName>)other.Value).Equals(this);
            }

            public override int GetHashCode() => _scope.GetHashCode() * 7 + _name.GetHashCode();

            bool IEquatable<ScopedTypeName>.Equals(ScopedTypeName other) => other._scope == _scope && other._name == _name;
        }

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

        internal Type CanonicalizeType(Type type)
        {
            if (!_canonicalizedTypes.TryGetValue(type, out var canon))
            {
                canon = type;
                _canonicalizedTypes.Add(canon, canon);
            }

            return canon;
        }

        public event ResolvedMissingMemberHandler ResolvedMissingMember;

        /// <summary>
        /// Returns <c>true</c> if the module builders should produce deterministic images.
        /// </summary>
        internal bool Deterministic => (_options & UniverseOptions.DeterministicOutput) != 0;

        /// <summary>
        /// Gets the hash algorithm to use for PDB checksums.
        /// </summary>
        internal HashAlgorithmName PdbChecksumAlgorithm => _pdbChecksumAlgorithm;

    }

}
