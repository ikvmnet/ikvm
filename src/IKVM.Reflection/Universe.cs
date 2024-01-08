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
using System.Security;
using System.Text;

using IKVM.Reflection.Emit;
using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{

    public delegate Assembly ResolveEventHandler(object sender, ResolveEventArgs args);

    public delegate void ResolvedMissingMemberHandler(Module requestingModule, MemberInfo member);

    /// <summary>
    /// Provides a view of the types available to the reflection infrastructure.
    /// </summary>
    public sealed class Universe : IDisposable
    {

#if NETCOREAPP3_1_OR_GREATER

        public static readonly string DefaultCoreLibName = "System.Runtime";

#elif NETFRAMEWORK

        public static readonly string DefaultCoreLibName = "mscorlib";

#endif

#if NETFRAMEWORK
        internal static readonly bool MonoRuntime = System.Type.GetType("Mono.Runtime") != null;
        internal static readonly bool CoreRuntime = false;
#else
        internal static readonly bool MonoRuntime = false;
        internal static readonly bool CoreRuntime = true;
#endif

        readonly string coreLibName;
        readonly Dictionary<Type, Type> canonicalizedTypes = new Dictionary<Type, Type>();
        readonly List<AssemblyReader> assemblies = new List<AssemblyReader>();
        readonly List<AssemblyBuilder> dynamicAssemblies = new List<AssemblyBuilder>();
        readonly Dictionary<string, Assembly> assembliesByName = new Dictionary<string, Assembly>();
        readonly Dictionary<System.Type, Type> importedTypes = new Dictionary<System.Type, Type>();
        Dictionary<ScopedTypeName, Type> missingTypes;
        bool resolveMissingMembers;
        readonly bool enableFunctionPointers;
        readonly bool useNativeFusion;
        readonly bool returnPseudoCustomAttributes;
        readonly bool automaticallyProvideDefaultConstructor;
        readonly UniverseOptions options;
        Type typeof_System_Object;
        Type typeof_System_ValueType;
        Type typeof_System_Enum;
        Type typeof_System_Void;
        Type typeof_System_Boolean;
        Type typeof_System_Char;
        Type typeof_System_SByte;
        Type typeof_System_Byte;
        Type typeof_System_Int16;
        Type typeof_System_UInt16;
        Type typeof_System_Int32;
        Type typeof_System_UInt32;
        Type typeof_System_Int64;
        Type typeof_System_UInt64;
        Type typeof_System_Single;
        Type typeof_System_Double;
        Type typeof_System_String;
        Type typeof_System_IntPtr;
        Type typeof_System_UIntPtr;
        Type typeof_System_TypedReference;
        Type typeof_System_Type;
        Type typeof_System_Array;
        Type typeof_System_DateTime;
        Type typeof_System_DBNull;
        Type typeof_System_Decimal;
        Type typeof_System_AttributeUsageAttribute;
        Type typeof_System_ContextBoundObject;
        Type typeof_System_MarshalByRefObject;
        Type typeof_System_Console;
        Type typeof_System_IO_TextWriter;
        Type typeof_System_Runtime_InteropServices_DllImportAttribute;
        Type typeof_System_Runtime_InteropServices_FieldOffsetAttribute;
        Type typeof_System_Runtime_InteropServices_MarshalAsAttribute;
        Type typeof_System_Runtime_InteropServices_UnmanagedType;
        Type typeof_System_Runtime_InteropServices_VarEnum;
        Type typeof_System_Runtime_InteropServices_PreserveSigAttribute;
        Type typeof_System_Runtime_InteropServices_CallingConvention;
        Type typeof_System_Runtime_InteropServices_CharSet;
        Type typeof_System_Runtime_CompilerServices_DecimalConstantAttribute;
        Type typeof_System_Reflection_AssemblyCopyrightAttribute;
        Type typeof_System_Reflection_AssemblyTrademarkAttribute;
        Type typeof_System_Reflection_AssemblyProductAttribute;
        Type typeof_System_Reflection_AssemblyCompanyAttribute;
        Type typeof_System_Reflection_AssemblyDescriptionAttribute;
        Type typeof_System_Reflection_AssemblyTitleAttribute;
        Type typeof_System_Reflection_AssemblyInformationalVersionAttribute;
        Type typeof_System_Reflection_AssemblyFileVersionAttribute;
        Type typeof_System_Security_Permissions_CodeAccessSecurityAttribute;
        Type typeof_System_Security_Permissions_PermissionSetAttribute;
        Type typeof_System_Security_Permissions_SecurityAction;
        List<ResolveEventHandler> resolvers = new List<ResolveEventHandler>();
        Predicate<Type> missingTypeIsValueType;

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
            this.options = options;
            this.coreLibName = coreLibName ?? DefaultCoreLibName;
            enableFunctionPointers = (options & UniverseOptions.EnableFunctionPointers) != 0;
            useNativeFusion = (options & UniverseOptions.DisableFusion) == 0 && GetUseNativeFusion();
            returnPseudoCustomAttributes = (options & UniverseOptions.DisablePseudoCustomAttributeRetrieval) == 0;
            automaticallyProvideDefaultConstructor = (options & UniverseOptions.DontProvideAutomaticDefaultConstructor) == 0;
            resolveMissingMembers = (options & UniverseOptions.ResolveMissingMembers) != 0;
        }

        static bool GetUseNativeFusion()
        {
            try
            {
                return Environment.OSVersion.Platform == PlatformID.Win32NT && !MonoRuntime && !CoreRuntime && Environment.GetEnvironmentVariable("IKVM_DISABLE_FUSION") == null;
            }
            catch (SecurityException)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the name of the core library.
        /// </summary>
        public string CoreLibName => coreLibName;

        /// <summary>
        /// Gets the core library.
        /// </summary>
        internal Assembly CoreLib => Load(coreLibName);

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
        /// Resolves the primitive type with teh specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Type ResolvePrimitive(string name)
        {
            // Primitive here means that these types have a special metadata encoding, which means that
            // there can be references to them without referring to them by name explicitly.
            // We want these types to be usable even when they don't exist in mscorlib or there is no mscorlib loaded.
            return CoreLib.FindType(new TypeName("System", name)) ?? GetMissingType(null, CoreLib.ManifestModule, null, new TypeName("System", name));
        }

        internal Type System_Object => typeof_System_Object ??= ResolvePrimitive("Object");

        internal Type System_ValueType => typeof_System_ValueType ??= ResolvePrimitive("ValueType");

        internal Type System_Enum => typeof_System_Enum ??= ResolvePrimitive("Enum");

        internal Type System_Void => typeof_System_Void ??= ResolvePrimitive("Void");

        internal Type System_Boolean => typeof_System_Boolean ??= ResolvePrimitive("Boolean");

        internal Type System_Char => typeof_System_Char ??= ResolvePrimitive("Char");

        internal Type System_SByte => typeof_System_SByte ??= ResolvePrimitive("SByte");

        internal Type System_Byte => typeof_System_Byte ??= ResolvePrimitive("Byte");

        internal Type System_Int16 => typeof_System_Int16 ??= ResolvePrimitive("Int16");

        internal Type System_UInt16 => typeof_System_UInt16 ??= ResolvePrimitive("UInt16");

        internal Type System_Int32 => typeof_System_Int32 ??= ResolvePrimitive("Int32");

        internal Type System_UInt32 => typeof_System_UInt32 ??= ResolvePrimitive("UInt32");

        internal Type System_Int64 => typeof_System_Int64 ??= ResolvePrimitive("Int64");

        internal Type System_UInt64 => typeof_System_UInt64 ??= ResolvePrimitive("UInt64");

        internal Type System_Single => typeof_System_Single ??= ResolvePrimitive("Single");

        internal Type System_Double => typeof_System_Double ??= ResolvePrimitive("Double");

        internal Type System_String => typeof_System_String ??= ResolvePrimitive("String");

        internal Type System_IntPtr => typeof_System_IntPtr ??= ResolvePrimitive("IntPtr");

        internal Type System_UIntPtr => typeof_System_UIntPtr ??= ResolvePrimitive("UIntPtr");

        internal Type System_TypedReference => typeof_System_TypedReference ??= ResolvePrimitive("TypedReference");

        internal Type System_Type => typeof_System_Type ??= ResolvePrimitive("Type");

        internal Type System_Array => typeof_System_Array ??= ResolvePrimitive("Array");

        internal Type System_DateTime => typeof_System_DateTime ??= ImportCoreLibType("System", "DateTime");

        internal Type System_DBNull => typeof_System_DBNull ??= ImportCoreLibType("System", "DBNull");

        internal Type System_Decimal => typeof_System_Decimal ??= ImportCoreLibType("System", "Decimal");

        internal Type System_AttributeUsageAttribute => typeof_System_AttributeUsageAttribute ??= ImportCoreLibType("System", "AttributeUsageAttribute");

        internal Type System_ContextBoundObject => typeof_System_ContextBoundObject ??= ImportCoreLibType("System", "ContextBoundObject");

        internal Type System_MarshalByRefObject => typeof_System_MarshalByRefObject ??= ImportCoreLibType("System", "MarshalByRefObject");

        internal Type System_Console => typeof_System_Console ??= ImportCoreLibType("System", "Console");

        internal Type System_IO_TextWriter => typeof_System_IO_TextWriter ??= ImportCoreLibType("System.IO", "TextWriter");

        internal Type System_Runtime_InteropServices_DllImportAttribute => typeof_System_Runtime_InteropServices_DllImportAttribute ??= ImportCoreLibType("System.Runtime.InteropServices", "DllImportAttribute");

        internal Type System_Runtime_InteropServices_FieldOffsetAttribute => typeof_System_Runtime_InteropServices_FieldOffsetAttribute ??= ImportCoreLibType("System.Runtime.InteropServices", "FieldOffsetAttribute");

        internal Type System_Runtime_InteropServices_MarshalAsAttribute => typeof_System_Runtime_InteropServices_MarshalAsAttribute ??= ImportCoreLibType("System.Runtime.InteropServices", "MarshalAsAttribute");

        internal Type System_Runtime_InteropServices_UnmanagedType => typeof_System_Runtime_InteropServices_UnmanagedType ??= ImportCoreLibType("System.Runtime.InteropServices", "UnmanagedType");

        internal Type System_Runtime_InteropServices_VarEnum => typeof_System_Runtime_InteropServices_VarEnum ??= ImportCoreLibType("System.Runtime.InteropServices", "VarEnum");

        internal Type System_Runtime_InteropServices_PreserveSigAttribute => typeof_System_Runtime_InteropServices_PreserveSigAttribute ??= ImportCoreLibType("System.Runtime.InteropServices", "PreserveSigAttribute");

        internal Type System_Runtime_InteropServices_CallingConvention => typeof_System_Runtime_InteropServices_CallingConvention ??= ImportCoreLibType("System.Runtime.InteropServices", "CallingConvention");

        internal Type System_Runtime_InteropServices_CharSet => typeof_System_Runtime_InteropServices_CharSet ??= ImportCoreLibType("System.Runtime.InteropServices", "CharSet");

        internal Type System_Runtime_CompilerServices_DecimalConstantAttribute => typeof_System_Runtime_CompilerServices_DecimalConstantAttribute ??= ImportCoreLibType("System.Runtime.CompilerServices", "DecimalConstantAttribute");

        internal Type System_Reflection_AssemblyCopyrightAttribute => typeof_System_Reflection_AssemblyCopyrightAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyCopyrightAttribute");

        internal Type System_Reflection_AssemblyTrademarkAttribute => typeof_System_Reflection_AssemblyTrademarkAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyTrademarkAttribute");

        internal Type System_Reflection_AssemblyProductAttribute => typeof_System_Reflection_AssemblyProductAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyProductAttribute");

        internal Type System_Reflection_AssemblyCompanyAttribute => typeof_System_Reflection_AssemblyCompanyAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyCompanyAttribute");

        internal Type System_Reflection_AssemblyDescriptionAttribute => typeof_System_Reflection_AssemblyDescriptionAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyDescriptionAttribute");

        internal Type System_Reflection_AssemblyTitleAttribute => typeof_System_Reflection_AssemblyTitleAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyTitleAttribute");

        internal Type System_Reflection_AssemblyInformationalVersionAttribute => typeof_System_Reflection_AssemblyInformationalVersionAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyInformationalVersionAttribute");

        internal Type System_Reflection_AssemblyFileVersionAttribute => typeof_System_Reflection_AssemblyFileVersionAttribute ??= ImportCoreLibType("System.Reflection", "AssemblyFileVersionAttribute");

        internal Type System_Security_Permissions_CodeAccessSecurityAttribute => typeof_System_Security_Permissions_CodeAccessSecurityAttribute ??= ImportCoreLibType("System.Security.Permissions", "CodeAccessSecurityAttribute");

        internal Type System_Security_Permissions_PermissionSetAttribute => typeof_System_Security_Permissions_PermissionSetAttribute ??= ImportCoreLibType("System.Security.Permissions", "PermissionSetAttribute");

        internal Type System_Security_Permissions_SecurityAction => typeof_System_Security_Permissions_SecurityAction ??= ImportCoreLibType("System.Security.Permissions", "SecurityAction");

        /// <summary>
        /// Returns <c>true</c> if a CoreLib implementation has been loaded.
        /// </summary>
        internal bool HasCoreLib
        {
            get { return GetLoadedAssembly(coreLibName) != null; }
        }

        /// <summary>
        /// Invoked when an attempt to resolve an assembly occurs.
        /// </summary>
        public event ResolveEventHandler AssemblyResolve
        {
            add { resolvers.Add(value); }
            remove { resolvers.Remove(value); }
        }

        /// <summary>
        /// Imports the specified <see cref="System.Type"/> into the environment.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Type Import(System.Type type)
        {
            if (!importedTypes.TryGetValue(type, out var imported))
            {
                imported = ImportImpl(type);
                if (imported != null)
                    importedTypes.Add(type, imported);
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
                for (int i = 0; i < args.Length; i++)
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
        Assembly Import(System.Reflection.Assembly asm)
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
                if (MetadataOnly == false)
                    fs = null;
            }
            finally
            {
                fs?.Dispose();
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
                assemblies.Add(asm1);
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
            if (!assembliesByName.TryGetValue(refname, out var asm) && (options & UniverseOptions.DisableDefaultAssembliesLookup) == 0)
            {
                var simpleName = GetSimpleAssemblyName(refname);
                for (int i = 0; i < assemblies.Count; i++)
                {
                    if (simpleName.Equals(assemblies[i].Name, StringComparison.OrdinalIgnoreCase) && CompareAssemblyIdentity(coreLibName, refname, false, assemblies[i].FullName, false, out var result))
                    {
                        asm = assemblies[i];
                        assembliesByName.Add(refname, asm);
                        break;
                    }
                }
            }

            return asm;
        }

        Assembly GetDynamicAssembly(string refname)
        {
            var simpleName = GetSimpleAssemblyName(refname);
            foreach (var asm in dynamicAssemblies)
                if (simpleName.Equals(asm.Name, StringComparison.OrdinalIgnoreCase) && CompareAssemblyIdentity(coreLibName, refname, false, asm.FullName, false, out var result))
                    return asm;

            return null;
        }

        public Assembly Load(string refname)
        {
            return Load(refname, null, true);
        }

        internal Assembly Load(string refname, Module requestingModule, bool throwOnError)
        {
            var asm = GetLoadedAssembly(refname);
            if (asm != null)
                return asm;

            if (resolvers.Count == 0)
            {
                asm = DefaultResolver(refname, throwOnError);
            }
            else
            {
                var args = new ResolveEventArgs(refname, requestingModule == null ? null : requestingModule.Assembly);
                foreach (var evt in resolvers)
                {
                    asm = evt(this, args);
                    if (asm != null)
                        break;
                }

                if (asm == null)
                    asm = GetDynamicAssembly(refname);
            }

            if (asm != null)
            {
                var defname = asm.FullName;
                if (refname != defname)
                    assembliesByName.Add(refname, asm);

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

#if NETFRAMEWORK

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
#else
            return null;
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
            var array = new Assembly[assemblies.Count + dynamicAssemblies.Count];
            for (int i = 0; i < assemblies.Count; i++)
                array[i] = assemblies[i];
            for (int i = 0, j = assemblies.Count; j < array.Length; i++, j++)
                array[j] = dynamicAssemblies[i];

            return array;
        }

        public bool CompareAssemblyIdentity(string coreLibName,string assemblyIdentity1, bool unified1, string assemblyIdentity2, bool unified2, out AssemblyComparisonResult result)
        {
            return useNativeFusion
                ? Fusion.CompareAssemblyIdentityNative(coreLibName, assemblyIdentity1, unified1, assemblyIdentity2, unified2, out result)
                : Fusion.CompareAssemblyIdentityPure(coreLibName,assemblyIdentity1, unified1, assemblyIdentity2, unified2, out result);
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
                return missingTypeIsValueType(missingType);

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
