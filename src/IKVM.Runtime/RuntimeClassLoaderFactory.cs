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

using IKVM.CoreLib.Diagnostics;

#if NETCOREAPP
using System.Runtime.Loader;
#endif

#if IMPORTER || EXPORTER
using IKVM.Reflection;

using Type = IKVM.Reflection.Type;
using ProtectionDomain = System.Object;
#else
using System.Reflection;
#endif

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Runtime
{
    /// <summary>
    /// Manages instances of <see cref="RuntimeClassLoader"/>.
    /// </summary>
    class RuntimeClassLoaderFactory
    {

        readonly RuntimeContext context;
        readonly object wrapperLock = new object();
        internal readonly Dictionary<Type, RuntimeJavaType> globalTypeToTypeWrapper = new Dictionary<Type, RuntimeJavaType>();
        internal readonly Dictionary<Type, string> remappedTypes = new Dictionary<Type, string>();
        readonly List<RuntimeGenericClassLoader> genericClassLoaders = new List<RuntimeGenericClassLoader>();

#if IMPORTER || EXPORTER
        internal RuntimeClassLoader bootstrapClassLoader;
#else
        internal RuntimeAssemblyClassLoader bootstrapClassLoader;
#endif

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        public RuntimeClassLoaderFactory(RuntimeContext context)
        {
            this.context = context;

            globalTypeToTypeWrapper[context.PrimitiveJavaTypeFactory.BOOLEAN.TypeAsTBD] = context.PrimitiveJavaTypeFactory.BOOLEAN;
            globalTypeToTypeWrapper[context.PrimitiveJavaTypeFactory.BYTE.TypeAsTBD] = context.PrimitiveJavaTypeFactory.BYTE;
            globalTypeToTypeWrapper[context.PrimitiveJavaTypeFactory.CHAR.TypeAsTBD] = context.PrimitiveJavaTypeFactory.CHAR;
            globalTypeToTypeWrapper[context.PrimitiveJavaTypeFactory.DOUBLE.TypeAsTBD] = context.PrimitiveJavaTypeFactory.DOUBLE;
            globalTypeToTypeWrapper[context.PrimitiveJavaTypeFactory.FLOAT.TypeAsTBD] = context.PrimitiveJavaTypeFactory.FLOAT;
            globalTypeToTypeWrapper[context.PrimitiveJavaTypeFactory.INT.TypeAsTBD] = context.PrimitiveJavaTypeFactory.INT;
            globalTypeToTypeWrapper[context.PrimitiveJavaTypeFactory.LONG.TypeAsTBD] = context.PrimitiveJavaTypeFactory.LONG;
            globalTypeToTypeWrapper[context.PrimitiveJavaTypeFactory.SHORT.TypeAsTBD] = context.PrimitiveJavaTypeFactory.SHORT;
            globalTypeToTypeWrapper[context.PrimitiveJavaTypeFactory.VOID.TypeAsTBD] = context.PrimitiveJavaTypeFactory.VOID;
            LoadRemappedTypes();
        }

#if IMPORTER || EXPORTER

        // HACK this is used by the ahead-of-time compiler to overrule the bootstrap classloader
        // when we're compiling the core class libraries and by ikvmstub with the -bootstrap option
        internal void SetBootstrapClassLoader(RuntimeClassLoader bootstrapClassLoader)
        {
            Debug.Assert(this.bootstrapClassLoader == null);

            this.bootstrapClassLoader = bootstrapClassLoader;
        }

#endif

        internal void LoadRemappedTypes()
        {
            // if we're compiling the base assembly, we won't be able to resolve one
            var baseAssembly = context.Resolver.ResolveBaseAssembly();
            if (baseAssembly != null && remappedTypes.Count == 0)
            {
                var remapped = context.AttributeHelper.GetRemappedClasses(baseAssembly);
                if (remapped.Length > 0)
                {
                    foreach (var r in remapped)
                        remappedTypes.Add(r.RemappedType, r.Name);
                }
                else
                {
#if IMPORTER
                    throw new FatalCompilerErrorException(Diagnostic.CoreClassesMissing.Event([]));
#else
                    throw new InternalException("Failed to find core classes in core library.");
#endif
                }
            }
        }

        internal bool IsRemappedType(Type type)
        {
            return remappedTypes.ContainsKey(type);
        }

#if IMPORTER || EXPORTER
        internal RuntimeClassLoader GetBootstrapClassLoader()
#else
        internal RuntimeAssemblyClassLoader GetBootstrapClassLoader()
#endif
        {
            lock (wrapperLock)
                return bootstrapClassLoader ??= new BootstrapClassLoader(context);
        }

#if !IMPORTER && !EXPORTER

        internal RuntimeClassLoader GetClassLoaderWrapper(java.lang.ClassLoader javaClassLoader)
        {
            if (javaClassLoader == null)
                return GetBootstrapClassLoader();

            lock (wrapperLock)
            {
#if FIRST_PASS
                RuntimeClassLoader wrapper = null;
#else
                RuntimeClassLoader wrapper = javaClassLoader.wrapper;
#endif

                if (wrapper == null)
                {
                    var opt = CodeGenOptions.None;
                    if (JVM.EmitSymbols)
                        opt |= CodeGenOptions.DisableOptimizations;
#if NETFRAMEWORK
                    if (!AppDomain.CurrentDomain.IsFullyTrusted)
                        opt |= CodeGenOptions.NoAutomagicSerialization;
#endif
                    wrapper = new RuntimeClassLoader(context, opt, javaClassLoader);
                    SetWrapperForClassLoader(javaClassLoader, wrapper);
                }
                return wrapper;
            }
        }
#endif

        /// <summary>
        /// Returns the <see cref="RuntimeJavaType"/> associated with the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal RuntimeJavaType GetJavaTypeFromType(Type type)
        {
#if IMPORTER
            if (type.__ContainsMissingType)
            {
                return new RuntimeUnloadableJavaType(context, type);
            }
#endif
            //Tracer.Info(Tracer.Runtime, "GetWrapperFromType: {0}", type.AssemblyQualifiedName);
#if !IMPORTER
            RuntimeJavaType.AssertFinished(type);
#endif
            Debug.Assert(!type.IsPointer);
            Debug.Assert(!type.IsByRef);
            RuntimeJavaType wrapper;
            lock (globalTypeToTypeWrapper)
            {
                globalTypeToTypeWrapper.TryGetValue(type, out wrapper);
            }

            if (wrapper != null)
            {
                return wrapper;
            }

#if EXPORTER
            if (type.__IsMissing || type.__ContainsMissingType)
            {
                wrapper = new RuntimeUnloadableJavaType(context, "Missing/" + type.Assembly.FullName);
                globalTypeToTypeWrapper.Add(type, wrapper);
                return wrapper;
            }
#endif

            if (remappedTypes.TryGetValue(type, out var remapped))
            {
                wrapper = LoadClassCritical(remapped);
            }
            else if (ReflectUtil.IsVector(type))
            {
                // it might be an array of a dynamically compiled Java type
                int rank = 1;
                Type elem = type.GetElementType();
                while (ReflectUtil.IsVector(elem))
                {
                    rank++;
                    elem = elem.GetElementType();
                }

                wrapper = GetJavaTypeFromType(elem).MakeArrayType(rank);
            }
            else
            {
                var asm = type.Assembly;

#if !IMPORTER && !EXPORTER
                if (RuntimeAnonymousJavaType.IsAnonymous(context, type))
                {
                    var typeToTypeWrapper = globalTypeToTypeWrapper;
                    var tw = new RuntimeAnonymousJavaType(context, type);
                    lock (typeToTypeWrapper)
                        if (!typeToTypeWrapper.TryGetValue(type, out wrapper))
                            typeToTypeWrapper.Add(type, wrapper = tw);

                    return wrapper;
                }

                if (ReflectUtil.IsReflectionOnly(type))
                {
                    // historically we've always returned null for types that don't have a corresponding TypeWrapper (or java.lang.Class)
                    return null;
                }
#endif
                // if the wrapper doesn't already exist, that must mean that the type
                // is a .NET type (or a pre-compiled Java class), which means that it
                // was "loaded" by an assembly classloader
                wrapper = context.AssemblyClassLoaderFactory.FromAssembly(asm).GetJavaTypeFromAssemblyType(type);
            }

            lock (globalTypeToTypeWrapper)
            {
                try
                {
                    // critical code in the finally block to avoid Thread.Abort interrupting the thread
                }
                finally
                {
                    globalTypeToTypeWrapper[type] = wrapper;
                }
            }

            return wrapper;
        }

        internal RuntimeClassLoader GetGenericClassLoader(RuntimeJavaType wrapper)
        {
            Type type = wrapper.TypeAsTBD;
            Debug.Assert(type.IsGenericType);
            Debug.Assert(!type.ContainsGenericParameters);

            List<RuntimeClassLoader> list = new List<RuntimeClassLoader>();
            list.Add(context.AssemblyClassLoaderFactory.FromAssembly(type.Assembly));
            foreach (Type arg in type.GetGenericArguments())
            {
                RuntimeClassLoader loader = GetJavaTypeFromType(arg).ClassLoader;
                if (!list.Contains(loader) && loader != bootstrapClassLoader)
                {
                    list.Add(loader);
                }
            }
            RuntimeClassLoader[] key = list.ToArray();
            RuntimeClassLoader matchingLoader = GetGenericClassLoaderByKey(key);
            matchingLoader.RegisterInitiatingLoader(wrapper);
            return matchingLoader;
        }

        internal RuntimeJavaType LoadClassCritical(string name)
        {
#if IMPORTER
            var wrapper = GetBootstrapClassLoader().TryLoadClassByName(name);
            if (wrapper == null)
                throw new FatalCompilerErrorException(Diagnostic.CriticalClassNotFound.Event([name]));

            return wrapper;
#else
            try
            {
                return GetBootstrapClassLoader().LoadClassByName(name);
            }
            catch (Exception e)
            {
                throw new InternalException("Loading of critical class failed.", e);
            }
#endif
        }

        RuntimeClassLoader GetGenericClassLoaderByKey(RuntimeClassLoader[] key)
        {
            lock (wrapperLock)
            {
                foreach (RuntimeGenericClassLoader loader in genericClassLoaders)
                    if (loader.Matches(key))
                        return loader;

#if IMPORTER || EXPORTER || FIRST_PASS
                var newLoader = new RuntimeGenericClassLoader(context, key, null);
#else
                var javaClassLoader = new ikvm.runtime.GenericClassLoader();
                var newLoader = new RuntimeGenericClassLoader(context, key, javaClassLoader);
                SetWrapperForClassLoader(javaClassLoader, newLoader);
#endif
                genericClassLoaders.Add(newLoader);
                return newLoader;
            }
        }

#if !IMPORTER && !EXPORTER

        public void SetWrapperForClassLoader(java.lang.ClassLoader javaClassLoader, RuntimeClassLoader wrapper)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            javaClassLoader.wrapper = wrapper;
#endif
        }

#endif

#if !IMPORTER && !EXPORTER

        internal RuntimeClassLoader GetGenericClassLoaderByName(string name)
        {
            Debug.Assert(name.StartsWith("[[") && name.EndsWith("]]"));

            var stack = new Stack<List<RuntimeClassLoader>>();
            List<RuntimeClassLoader> list = null;

            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == '[')
                {
                    if (name[i + 1] == '[')
                    {
                        stack.Push(list);
                        list = new List<RuntimeClassLoader>();
                        if (name[i + 2] == '[')
                        {
                            i++;
                        }
                    }
                    else
                    {
                        int start = i + 1;
                        i = name.IndexOf(']', i);
                        list.Add(context.ClassLoaderFactory.GetAssemblyClassLoaderByName(name.Substring(start, i - start)));
                    }
                }
                else if (name[i] == ']')
                {
                    var loader = GetGenericClassLoaderByKey(list.ToArray());
                    list = stack.Pop();
                    if (list == null)
                        return loader;

                    list.Add(loader);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            throw new InvalidOperationException();
        }

        internal RuntimeClassLoader GetAssemblyClassLoaderByName(string name)
        {
            if (name.StartsWith("[["))
                return GetGenericClassLoaderByName(name);

#if NETFRAMEWORK
            return context.AssemblyClassLoaderFactory.FromAssembly(Assembly.Load(name));
#else
            return context.AssemblyClassLoaderFactory.FromAssembly(AssemblyLoadContext.GetLoadContext(typeof(RuntimeClassLoader).Assembly).LoadFromAssemblyName(new AssemblyName(name)));
#endif
        }

#endif

        internal int GetGenericClassLoaderId(RuntimeClassLoader wrapper)
        {
            lock (wrapperLock)
                return genericClassLoaders.IndexOf(wrapper as RuntimeGenericClassLoader);
        }

        internal RuntimeClassLoader GetGenericClassLoaderById(int id)
        {
            lock (wrapperLock)
                return genericClassLoaders[id];
        }

        internal void SetWrapperForType(Type type, RuntimeJavaType wrapper)
        {
#if !IMPORTER
            RuntimeJavaType.AssertFinished(type);
#endif

            lock (globalTypeToTypeWrapper)
            {
                try
                {
                    // critical code in the finally block to avoid Thread.Abort interrupting the thread
                }
                finally
                {
                    globalTypeToTypeWrapper.Add(type, wrapper);
                }
            }
        }

    }

}
