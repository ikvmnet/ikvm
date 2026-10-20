/*
  Copyright (C) 2002-2015 Jeroen Frijters

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
using System.Diagnostics;
using System.Threading;
using System.Text;

using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Linking;
using IKVM.CoreLib.Runtime;
using IKVM.Runtime.Accessors.Java.Lang;

#if NETCOREAPP
using System.Runtime.Loader;
#endif

#if IMPORTER || EXPORTER
using IKVM.Reflection;

using Type = IKVM.Reflection.Type;
using ProtectionDomain = System.Object;

#else
using System.Reflection;

using ProtectionDomain = java.security.ProtectionDomain;
#endif

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Runtime
{

    /// <summary>
    /// Runtime support for a class loader.
    /// </summary>
    class RuntimeClassLoader
    {

        readonly RuntimeContext context;

#if !IMPORTER && !FIRST_PASS && !EXPORTER

        ClassLoaderAccessor classLoaderAccessor;
        ClassLoaderAccessor ClassLoaderAccessor => JVM.Internal.BaseAccessors.Get(ref classLoaderAccessor);

        protected java.lang.ClassLoader javaClassLoader;

#endif

#if !EXPORTER
        RuntimeJavaTypeFactory factory;
#endif // !EXPORTER
        readonly Dictionary<string, RuntimeJavaType> types = new Dictionary<string, RuntimeJavaType>();
        readonly Dictionary<string, Thread> defineClassInProgress = new Dictionary<string, Thread>();
        List<IntPtr> nativeLibraries;
        readonly CodeGenOptions codegenoptions;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="codegenoptions"></param>
        /// <param name="javaClassLoader"></param>
        internal RuntimeClassLoader(RuntimeContext context, CodeGenOptions codegenoptions, object javaClassLoader)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.codegenoptions = codegenoptions;
#if !IMPORTER && !FIRST_PASS && !EXPORTER
            this.javaClassLoader = (java.lang.ClassLoader)javaClassLoader;
#endif
        }

        /// <summary>
        /// Gets the <see cref="IDiagnosticHandler"/> events originated by this class loader should be sent to.
        /// </summary>
        public virtual IDiagnosticHandler Diagnostics => Context.Diagnostics;

#if IMPORTER || EXPORTER

        internal void SetRemappedType(Type type, RuntimeJavaType tw)
        {
            lock (types)
                types.Add(tw.Name, tw);

            lock (Context.ClassLoaderFactory.globalTypeToTypeWrapper)
                context.ClassLoaderFactory.globalTypeToTypeWrapper.Add(type, tw);

            context.ClassLoaderFactory.remappedTypes.Add(type, tw.Name);
        }

#endif

        /// <summary>
        /// Gets a reference to the <see cref="RuntimeContext"/> that this <see cref="RuntimeClassLoader"/> is hosted within.
        /// </summary>
        public RuntimeContext Context => context;

        // return the TypeWrapper if it is already loaded, this exists for DynamicTypeWrapper.SetupGhosts
        // and implements ClassLoader.findLoadedClass()
        internal RuntimeJavaType FindLoadedClass(string name)
        {
            if (name.Length > 1 && name[0] == '[')
                return FindOrLoadArrayClass(name, LoadMode.Find);

            RuntimeJavaType tw;
            lock (types)
                types.TryGetValue(name, out tw);

            return tw ?? FindLoadedClassLazy(name);
        }

        protected virtual RuntimeJavaType FindLoadedClassLazy(string name)
        {
            return null;
        }

        internal RuntimeJavaType RegisterInitiatingLoader(RuntimeJavaType tw)
        {
            Debug.Assert(tw != null);
            Debug.Assert(!tw.IsUnloadable);
            Debug.Assert(!tw.IsPrimitive);

            try
            {
                // critical code in the finally block to avoid Thread.Abort interrupting the thread
            }
            finally
            {
                tw = RegisterInitiatingLoaderCritical(tw);
            }

            return tw;
        }

        RuntimeJavaType RegisterInitiatingLoaderCritical(RuntimeJavaType tw)
        {
            lock (types)
            {
                types.TryGetValue(tw.Name, out var existing);
                if (existing != tw)
                {
                    if (existing != null)
                    {
                        // another thread beat us to it, discard the new TypeWrapper and
                        // return the previous one
                        return existing;
                    }
                    // NOTE if types.ContainsKey(tw.Name) is true (i.e. the value is null),
                    // we currently have a DefineClass in progress on another thread and we've
                    // beaten that thread to the punch by loading the class from a parent class
                    // loader instead. This is ok as DefineClass will throw a LinkageError when
                    // it is done.
                    types[tw.Name] = tw;
                }
            }

            return tw;
        }

        internal bool EmitSymbols => (codegenoptions & CodeGenOptions.EmitSymbols) != 0;

        internal bool EmitStackTraceInfo => (codegenoptions & CodeGenOptions.NoStackTraceInfo) == 0;

        internal bool StrictFinalFieldSemantics => (codegenoptions & CodeGenOptions.StrictFinalFieldSemantics) != 0;

        internal bool NoJNI => (codegenoptions & CodeGenOptions.NoJNI) != 0;

        internal bool RemoveAsserts => (codegenoptions & CodeGenOptions.RemoveAsserts) != 0;

        internal bool NoAutomagicSerialization => (codegenoptions & CodeGenOptions.NoAutomagicSerialization) != 0;

        internal bool DisableDynamicBinding => (codegenoptions & CodeGenOptions.DisableDynamicBinding) != 0;

        internal bool EmitNoRefEmitHelpers => (codegenoptions & CodeGenOptions.NoRefEmitHelpers) != 0;

        internal bool RemoveUnusedFields => (codegenoptions & CodeGenOptions.RemoveUnusedFields) != 0;

        internal bool EnableOptimizations => (codegenoptions & CodeGenOptions.DisableOptimizations) == 0;

#if !IMPORTER && !EXPORTER
#if FIRST_PASS == false

        /// <summary>
        /// Returns <c>true</c> if the class loader is considered trusted.
        /// </summary>
        /// <remarks>
        /// Implementation of Hotspot's java_lang_ClassLoader::is_trusted_loader(). 
        /// </remarks>
        internal bool IsTrusted
        {
            get
            {
                var scl = ClassLoaderAccessor.GetScl();

                // are we within the parent hierarchy of the system class loader?
                for (var cl = scl; cl != null; cl = ClassLoaderAccessor.GetParent(cl))
                    if (javaClassLoader == cl)
                        return true;

                return false;
            }
        }

#endif

        internal bool RelaxedClassNameValidation
        {
            get
            {
#if FIRST_PASS
                return true;
#else
                return JVM.RelaxedVerification && (javaClassLoader == null || IsTrusted);
#endif
            }
        }

#endif

        protected virtual void CheckProhibitedPackage(string className)
        {
            if (className.StartsWith("java.", StringComparison.Ordinal))
                throw new JavaSecurityException("Prohibited package name: " + className.Substring(0, className.LastIndexOf('.')));
        }

#if !EXPORTER

        internal RuntimeJavaType DefineClass(ClassFile f, ProtectionDomain protectionDomain)
        {
#if !IMPORTER
            var dotnetAssembly = f.IKVMAssemblyAttribute;
            if (dotnetAssembly != null)
            {
                // It's a stub class generated by ikvmstub (or generated by the runtime when getResource was
                // called on a statically compiled class).
                RuntimeClassLoader loader;
                try
                {
                    loader = Context.ClassLoaderFactory.GetAssemblyClassLoaderByName(dotnetAssembly);
                }
                catch (Exception x)
                {
                    // TODO don't catch all exceptions here
                    throw new NoClassDefFoundError($"{f.Name} ({x.Message})");
                }

                var tw = loader.TryLoadClassByName(f.Name);
                if (tw == null)
                    throw new NoClassDefFoundError($"{f.Name} (type not found in {dotnetAssembly})");

                return RegisterInitiatingLoader(tw);
            }
#endif
            CheckProhibitedPackage(f.Name);

            // check if the class already exists if we're an AssemblyClassLoader
            if (FindLoadedClassLazy(f.Name) != null)
                throw new LinkageError("duplicate class definition: " + f.Name);

            RuntimeJavaType def;
            try
            {
                // critical code in the finally block to avoid Thread.Abort interrupting the thread
            }
            finally
            {
                def = DefineClassCritical(f, protectionDomain);
            }

            return def;
        }

        RuntimeJavaType DefineClassCritical(ClassFile classFile, ProtectionDomain protectionDomain)
        {
            lock (types)
            {
                if (types.ContainsKey(classFile.Name))
                    throw new LinkageError($"duplicate class definition: {classFile.Name}");

                // mark the type as "loading in progress", so that we can detect circular dependencies.
                types.Add(classFile.Name, null);
                defineClassInProgress.Add(classFile.Name, Thread.CurrentThread);
            }
            try
            {
                return GetTypeWrapperFactory().DefineClassImpl(types, null, classFile, this, protectionDomain);
            }
            finally
            {
                lock (types)
                {
                    if (types[classFile.Name] == null)
                    {
                        // if loading the class fails, we remove the indicator that we're busy loading the class,
                        // because otherwise we get a ClassCircularityError if we try to load the class again.
                        types.Remove(classFile.Name);
                    }
                    defineClassInProgress.Remove(classFile.Name);
                    Monitor.PulseAll(types);
                }
            }
        }

        internal RuntimeJavaTypeFactory GetTypeWrapperFactory()
        {
            if (factory == null)
            {
                lock (this)
                {
                    try
                    {
                        // critical code in the finally block to avoid Thread.Abort interrupting the thread
                    }
                    finally
                    {
                        factory ??= context.DynamicClassLoaderFactory.GetOrCreate(this);
                    }
                }
            }

            return factory;
        }

#endif // !EXPORTER

        internal RuntimeJavaType LoadClassByName(string name)
        {
            return LoadClass(name, LoadMode.LoadOrThrow);
        }

        internal RuntimeJavaType TryLoadClassByName(string name)
        {
            return LoadClass(name, LoadMode.LoadOrNull);
        }

        internal RuntimeJavaType LoadClass(string name, LoadMode mode)
        {
            Profiler.Enter("LoadClass");

            try
            {
                var javaType = LoadRegisteredOrPendingClass(name);
                if (javaType != null)
                    return javaType;

                if (name.Length > 1 && name[0] == '[')
                    javaType = FindOrLoadArrayClass(name, mode);
                else
                    javaType = LoadClassImpl(name, mode);

                if (javaType != null)
                    return RegisterInitiatingLoader(javaType);

                if (!(name.Length > 1 && name[0] == '[') && ((mode & LoadMode.WarnClassNotFound) != 0) || WarningLevelHigh)
                    Diagnostics.ClassNotFound(name);

                return (mode & LoadMode.MaskReturn) switch
                {
                    LoadMode.ReturnNull => null,
                    LoadMode.ReturnUnloadable => new RuntimeUnloadableJavaType(context, name),
                    LoadMode.ThrowClassNotFound => throw new ClassNotFoundException(name),
                    _ => throw new InvalidOperationException(),
                };
            }
            finally
            {
                Profiler.Leave("LoadClass");
            }
        }

        RuntimeJavaType LoadRegisteredOrPendingClass(string name)
        {
            RuntimeJavaType javaType;

            lock (types)
            {
                if (types.TryGetValue(name, out javaType) && javaType == null)
                {
                    if (defineClassInProgress.TryGetValue(name, out var defineThread))
                    {
                        if (Thread.CurrentThread == defineThread)
                            throw new ClassCircularityError(name);

                        // the requested class is currently being defined by another thread, so we have to wait on that
                        while (defineClassInProgress.ContainsKey(name))
                            Monitor.Wait(types);

                        // the defineClass may have failed, so we need to use TryGetValue
                        types.TryGetValue(name, out javaType);
                    }
                }
            }

            return javaType;
        }

        /// <summary>
        /// Returns the <see cref="RuntimeJavaType"/> for the given array class name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        RuntimeJavaType FindOrLoadArrayClass(string name, LoadMode mode)
        {
            // calculate number of dimensions of type name
            int pos = 1;
            while (name[pos] == '[')
                if (++pos == name.Length)
                    return null; // malformed class name

            // type signature is an Object
            if (name[pos] == 'L')
            {
                // must end with ';', and not contain improper array characters
                if (name.EndsWith(";") == false || name.Length <= pos + 2 || name[pos + 1] == '[')
                    return null; // malformed class name

                var elemClass = name.Substring(pos + 1, name.Length - pos - 2);

                // it's important that we're registered as the initiating loader for the element type here
                var type = LoadClass(elemClass, mode | LoadMode.DontReturnUnloadable);
                if (type != null)
                    type = CreateArrayType(name, type, pos);

                return type;
            }

            if (name.Length != pos + 1)
                return null; // malformed class name

            // array of primitive type
            return name[pos] switch
            {
                'B' => CreateArrayType(name, context.PrimitiveJavaTypeFactory.BYTE, pos),
                'C' => CreateArrayType(name, context.PrimitiveJavaTypeFactory.CHAR, pos),
                'D' => CreateArrayType(name, context.PrimitiveJavaTypeFactory.DOUBLE, pos),
                'F' => CreateArrayType(name, context.PrimitiveJavaTypeFactory.FLOAT, pos),
                'I' => CreateArrayType(name, context.PrimitiveJavaTypeFactory.INT, pos),
                'J' => CreateArrayType(name, context.PrimitiveJavaTypeFactory.LONG, pos),
                'S' => CreateArrayType(name, context.PrimitiveJavaTypeFactory.SHORT, pos),
                'Z' => CreateArrayType(name, context.PrimitiveJavaTypeFactory.BOOLEAN, pos),
                _ => null,
            };
        }

        internal RuntimeJavaType FindOrLoadGenericClass(string name, LoadMode mode)
        {
            // we don't want to expose any failures to load any of the component types
            mode = (mode & LoadMode.MaskReturn) | LoadMode.ReturnNull;

            // we need to handle delegate methods here (for generic delegates)
            // (note that other types with manufactured inner classes such as Attribute and Enum can't be generic)
            if (name.EndsWith(RuntimeManagedJavaType.DelegateInterfaceSuffix))
            {
                var outer = FindOrLoadGenericClass(name.Substring(0, name.Length - RuntimeManagedJavaType.DelegateInterfaceSuffix.Length), mode);
                if (outer != null && outer.IsFakeTypeContainer)
                    foreach (var javaType in outer.InnerClasses)
                        if (javaType.Name == name)
                            return javaType;
            }

            // generic class name grammar:
            //
            // mangled(open_generic_type_name) "_$$$_" M(parameter_class_name) ( "_$$_" M(parameter_class_name) )* "_$$$$_"
            //
            // mangled() is the normal name mangling algorithm
            // M() is a replacement of "__" with "$$005F$$005F" followed by a replace of "." with "__"
            //
            var pos = name.IndexOf("_$$$_");
            if (pos <= 0 || !name.EndsWith("_$$$$_"))
                return null;

            var def = LoadClass(name.Substring(0, pos), mode);
            if (def == null || !def.TypeAsTBD.IsGenericTypeDefinition)
                return null;

            var type = def.TypeAsTBD;
            var typeParamNames = new List<string>();
            pos += 5;
            int start = pos;
            int nest = 0;
            for (; ; )
            {
                pos = name.IndexOf("_$$", pos);
                if (pos == -1)
                    return null;

                if (name.IndexOf("_$$_", pos, 4) == pos)
                {
                    if (nest == 0)
                    {
                        typeParamNames.Add(name.Substring(start, pos - start));
                        start = pos + 4;
                    }

                    pos += 4;
                }
                else if (name.IndexOf("_$$$_", pos, 5) == pos)
                {
                    nest++;
                    pos += 5;
                }
                else if (name.IndexOf("_$$$$_", pos, 6) == pos)
                {
                    if (nest == 0)
                    {
                        if (pos + 6 != name.Length)
                        {
                            return null;
                        }
                        typeParamNames.Add(name.Substring(start, pos - start));
                        break;
                    }
                    nest--;
                    pos += 6;
                }
                else
                {
                    pos += 3;
                }
            }

            var typeArguments = new Type[typeParamNames.Count];
            for (int i = 0; i < typeArguments.Length; i++)
            {
                var s = typeParamNames[i];
                // only do the unmangling for non-generic types (because we don't want to convert
                // the double underscores in two adjacent _$$$_ or _$$$$_ markers)
                if (s.IndexOf("_$$$_") == -1)
                {
                    s = s.Replace("__", ".");
                    s = s.Replace("$$005F$$005F", "__");
                }

                int dims = 0;
                while (s.Length > dims && s[dims] == 'A')
                    dims++;

                if (s.Length == dims)
                    return null;

                RuntimeJavaType tw;
                switch (s[dims])
                {
                    case 'L':
                        tw = LoadClass(s.Substring(dims + 1), mode);
                        if (tw == null)
                        {
                            return null;
                        }
                        tw.Finish();
                        break;
                    case 'Z':
                        tw = context.PrimitiveJavaTypeFactory.BOOLEAN;
                        break;
                    case 'B':
                        tw = context.PrimitiveJavaTypeFactory.BYTE;
                        break;
                    case 'S':
                        tw = context.PrimitiveJavaTypeFactory.SHORT;
                        break;
                    case 'C':
                        tw = context.PrimitiveJavaTypeFactory.CHAR;
                        break;
                    case 'I':
                        tw = context.PrimitiveJavaTypeFactory.INT;
                        break;
                    case 'F':
                        tw = context.PrimitiveJavaTypeFactory.FLOAT;
                        break;
                    case 'J':
                        tw = context.PrimitiveJavaTypeFactory.LONG;
                        break;
                    case 'D':
                        tw = context.PrimitiveJavaTypeFactory.DOUBLE;
                        break;
                    default:
                        return null;
                }

                if (dims > 0)
                    tw = tw.MakeArrayType(dims);

                typeArguments[i] = tw.TypeAsSignatureType;
            }

            try
            {
                type = type.MakeGenericType(typeArguments);
            }
            catch (ArgumentException)
            {
                // one of the typeArguments failed to meet the constraints
                return null;
            }

            var wrapper = context.ClassLoaderFactory.GetJavaTypeFromType(type);
            if (wrapper != null && wrapper.Name != name)
            {
                // the name specified was not in canonical form
                return null;
            }

            return wrapper;
        }

        protected virtual RuntimeJavaType LoadClassImpl(string name, LoadMode mode)
        {
            var javaType = FindOrLoadGenericClass(name, mode);
            if (javaType != null)
                return javaType;

#if !FIRST_PASS && !IMPORTER && !EXPORTER

            if ((mode & LoadMode.Load) == 0)
                return null;

            Profiler.Enter("ClassLoader.loadClass");

            try
            {
                // invoke 'loadClass' on the associated Java class loader instance
                // this can cause a call to defineClass
                var c = (java.lang.Class)ClassLoaderAccessor.InvokeLoadClassInternal(GetJavaClassLoader(), name);
                if (c == null)
                    return null;

                // map resulting reflective instance back into Java type
                var type = RuntimeJavaType.FromClass(c);
                if (type.Name != name)
                    return null;

                return type;
            }
            catch (java.lang.ClassNotFoundException x)
            {
                if ((mode & LoadMode.MaskReturn) == LoadMode.ThrowClassNotFound)
                    throw new ClassLoadingException(ikvm.runtime.Util.mapException(x), name);

                return null;
            }
            catch (global::java.lang.ThreadDeath)
            {
                throw;
            }
            catch (Exception x)
            {
                if ((mode & LoadMode.SuppressExceptions) == 0)
                    throw new ClassLoadingException(ikvm.runtime.Util.mapException(x), name);

                if (Diagnostics.IsEnabled(Diagnostic.GenericClassLoadingError))
                {
                    var cl = GetJavaClassLoader();
                    if (cl != null)
                    {
                        var sb = new ValueStringBuilder();
                        var sep = "";
                        while (cl != null)
                        {
                            sb.Append(sep);
                            sb.Append(cl.ToString());
                            sep = " -> ";
                            cl = cl.getParent();
                        }

                        Diagnostics.GenericClassLoadingError($"ClassLoader chain: {sb.ToString()}");
                    }

                    var m = ikvm.runtime.Util.mapException(x);
                    Diagnostics.GenericClassLoadingError(m.ToString() + Environment.NewLine + m.StackTrace);
                }

                return null;
            }
            finally
            {
                Profiler.Leave("ClassLoader.loadClass");
            }
#else
            return null;
#endif
        }

        /// <summary>
        /// Creates a new <see cref="RuntimeJavaType"/> representing an array type with the specified number of dimensions.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="elementJavaType"></param>
        /// <param name="dimensions"></param>
        /// <returns></returns>
        static RuntimeJavaType CreateArrayType(string name, RuntimeJavaType elementJavaType, int dimensions)
        {
            Debug.Assert(new string('[', dimensions) + elementJavaType.SignatureName == name);
            Debug.Assert(!elementJavaType.IsUnloadable && !elementJavaType.IsVerifierType && !elementJavaType.IsArray);
            Debug.Assert(dimensions >= 1);

            return elementJavaType.ClassLoader.RegisterInitiatingLoader(new RuntimeArrayJavaType(elementJavaType.Context, elementJavaType, name));
        }

#if !IMPORTER && !EXPORTER

        /// <summary>
        /// Gets the real underlying Java class loader instance associated with this runtime class loader.
        /// </summary>
        /// <returns></returns>
        internal virtual java.lang.ClassLoader GetJavaClassLoader()
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return javaClassLoader;
#endif
        }
#endif

        /// <summary>
        /// Takes a Java type signature and returns an array of types representing the types of the signature arguments.
        /// </summary>
        /// <remarks>
        /// This exposes potentially unfinished types
        /// </remarks>.
        /// <param name="signature"></param>
        /// <returns></returns>
        internal Type[] ArgTypeListFromSig(string signature)
        {
            if (signature[1] == ')')
                return Type.EmptyTypes;

            var javaTypes = ArgJavaTypeListFromSig(signature, LoadMode.LoadOrThrow);
            var types = new Type[javaTypes.Length];
            for (int i = 0; i < javaTypes.Length; i++)
                types[i] = javaTypes[i].TypeAsSignatureType;

            return types;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// This will ignore anything following the sig marker (so that it can be used to decode method signatures).
        /// </remarks>
        /// <param name="index"></param>
        /// <param name="signature"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        RuntimeJavaType SigDecoderWrapper(ref int index, string signature, LoadMode mode)
        {
            switch (signature[index++])
            {
                case 'B':
                    return context.PrimitiveJavaTypeFactory.BYTE;
                case 'C':
                    return context.PrimitiveJavaTypeFactory.CHAR;
                case 'D':
                    return context.PrimitiveJavaTypeFactory.DOUBLE;
                case 'F':
                    return context.PrimitiveJavaTypeFactory.FLOAT;
                case 'I':
                    return context.PrimitiveJavaTypeFactory.INT;
                case 'J':
                    return context.PrimitiveJavaTypeFactory.LONG;
                case 'L':
                    {
                        int pos = index;
                        index = signature.IndexOf(';', index) + 1;
                        return LoadClass(signature.Substring(pos, index - pos - 1), mode);
                    }
                case 'S':
                    return context.PrimitiveJavaTypeFactory.SHORT;
                case 'Z':
                    return context.PrimitiveJavaTypeFactory.BOOLEAN;
                case 'V':
                    return context.PrimitiveJavaTypeFactory.VOID;
                case '[':
                    {
                        // TODO this can be optimized
                        // should be able to navigate over the original sig and pass spans

                        var array = "[";
                        while (signature[index] == '[')
                        {
                            index++;
                            array += "[";
                        }

                        switch (signature[index])
                        {
                            case 'L':
                                {
                                    var pos = index;
                                    index = signature.IndexOf(';', index) + 1;
                                    return LoadClass(array + signature.Substring(pos, index - pos), mode);
                                }
                            case 'B':
                            case 'C':
                            case 'D':
                            case 'F':
                            case 'I':
                            case 'J':
                            case 'S':
                            case 'Z':
                                return LoadClass(array + signature[index++], mode);
                            default:
                                throw new InvalidOperationException(signature.Substring(index));
                        }
                    }
                default:
                    throw new InvalidOperationException(signature.Substring(index));
            }
        }

        internal RuntimeJavaType FieldTypeWrapperFromSig(string sig, LoadMode mode)
        {
            int index = 0;
            return SigDecoderWrapper(ref index, sig, mode);
        }

        internal RuntimeJavaType RetTypeWrapperFromSig(string sig, LoadMode mode)
        {
            int index = sig.IndexOf(')') + 1;
            return SigDecoderWrapper(ref index, sig, mode);
        }

        internal RuntimeJavaType[] ArgJavaTypeListFromSig(string sig, LoadMode mode)
        {
            if (sig[1] == ')')
                return [];

            var list = new List<RuntimeJavaType>();
            for (int i = 1; sig[i] != ')';)
                list.Add(SigDecoderWrapper(ref i, sig, mode));

            return list.ToArray();
        }

#if !IMPORTER && !FIRST_PASS && !EXPORTER

        internal static object DoPrivileged(java.security.PrivilegedAction action)
        {
            return java.security.AccessController.doPrivileged(action, ikvm.@internal.CallerID.create(typeof(java.lang.ClassLoader).TypeHandle));
        }

#endif

        internal void RegisterNativeLibrary(IntPtr p)
        {
            lock (this)
            {
                try
                {
                    // critical code in the finally block to avoid Thread.Abort interrupting the thread
                }
                finally
                {
                    nativeLibraries ??= new List<IntPtr>();
                    nativeLibraries.Add(p);
                }
            }
        }

        internal void UnregisterNativeLibrary(IntPtr p)
        {
            lock (this)
            {
                try
                {
                    // critical code in the finally block to avoid Thread.Abort interrupting the thread
                }
                finally
                {
                    nativeLibraries.Remove(p);
                }
            }
        }

        internal nint[] GetNativeLibraries()
        {
            lock (this)
                return nativeLibraries == null ? Array.Empty<nint>() : nativeLibraries.ToArray();
        }

#if !IMPORTER && !FIRST_PASS && !EXPORTER

        public override string ToString()
        {
            var javaClassLoader = GetJavaClassLoader();
            if (javaClassLoader == null)
                return "null";
            else
                return string.Format("{0}@{1:X}", Context.ClassLoaderFactory.GetJavaTypeFromType(javaClassLoader.GetType()).Name, javaClassLoader.GetHashCode());
        }

#endif

        internal virtual bool InternalsVisibleToImpl(RuntimeJavaType wrapper, RuntimeJavaType friend)
        {
            Debug.Assert(wrapper.ClassLoader == this);
            return this == friend.ClassLoader;
        }

#if !IMPORTER && !EXPORTER

        // this method is used by IKVM.Runtime.JNI
        internal static RuntimeClassLoader FromCallerID(ikvm.@internal.CallerID callerID)
        {
#if FIRST_PASS
            return null;
#else
            return JVM.Context.ClassLoaderFactory.GetClassLoaderWrapper(callerID.getCallerClassLoader());
#endif
        }

#endif

        internal void CheckPackageAccess(RuntimeJavaType tw, ProtectionDomain pd)
        {
#if !IMPORTER && !FIRST_PASS && !EXPORTER
            if (javaClassLoader != null)
                ClassLoaderAccessor.InvokeCheckPackageAccess(javaClassLoader, tw.ClassObject, pd);
#endif
        }

#if !EXPORTER

        internal ClassFileParseOptions ClassFileParseOptions
        {
            get
            {
#if IMPORTER
                var cfp = ClassFileParseOptions.LocalVariableTable | ClassFileParseOptions.StaticImport;
                if (EmitStackTraceInfo)
                    cfp |= ClassFileParseOptions.LineNumberTable;
                if (context.ClassLoaderFactory.bootstrapClassLoader is ImportClassLoader)
                    cfp |= ClassFileParseOptions.TrustedAnnotations;
                if (RemoveAsserts)
                    cfp |= ClassFileParseOptions.RemoveAssertions;
                return cfp;
#else
                var cfp = ClassFileParseOptions.LineNumberTable;
                if (EmitSymbols)
                    cfp |= ClassFileParseOptions.LocalVariableTable;
                if (RelaxedClassNameValidation)
                    cfp |= ClassFileParseOptions.RelaxedClassNameValidation;
                if (this == Context.ClassLoaderFactory.bootstrapClassLoader)
                    cfp |= ClassFileParseOptions.TrustedAnnotations;

                return cfp;
#endif
            }
        }

#endif

        internal virtual bool WarningLevelHigh
        {
            get { return false; }
        }

        internal virtual bool NoParameterReflection
        {
            get { return false; }
        }

    }

}
