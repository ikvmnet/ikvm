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
    static class RuntimeClassLoaderFactory
    {

        static readonly object wrapperLock = new object();
        internal static readonly Dictionary<Type, RuntimeJavaType> globalTypeToTypeWrapper = new Dictionary<Type, RuntimeJavaType>();
        internal static readonly Dictionary<Type, string> remappedTypes = new Dictionary<Type, string>();
        static List<RuntimeGenericClassLoader> genericClassLoaders;

#if IMPORTER || EXPORTER
        internal static RuntimeClassLoader bootstrapClassLoader;
#else
        internal static RuntimeAssemblyClassLoader bootstrapClassLoader;
#endif

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static RuntimeClassLoaderFactory()
        {
            globalTypeToTypeWrapper[RuntimePrimitiveJavaType.BOOLEAN.TypeAsTBD] = RuntimePrimitiveJavaType.BOOLEAN;
            globalTypeToTypeWrapper[RuntimePrimitiveJavaType.BYTE.TypeAsTBD] = RuntimePrimitiveJavaType.BYTE;
            globalTypeToTypeWrapper[RuntimePrimitiveJavaType.CHAR.TypeAsTBD] = RuntimePrimitiveJavaType.CHAR;
            globalTypeToTypeWrapper[RuntimePrimitiveJavaType.DOUBLE.TypeAsTBD] = RuntimePrimitiveJavaType.DOUBLE;
            globalTypeToTypeWrapper[RuntimePrimitiveJavaType.FLOAT.TypeAsTBD] = RuntimePrimitiveJavaType.FLOAT;
            globalTypeToTypeWrapper[RuntimePrimitiveJavaType.INT.TypeAsTBD] = RuntimePrimitiveJavaType.INT;
            globalTypeToTypeWrapper[RuntimePrimitiveJavaType.LONG.TypeAsTBD] = RuntimePrimitiveJavaType.LONG;
            globalTypeToTypeWrapper[RuntimePrimitiveJavaType.SHORT.TypeAsTBD] = RuntimePrimitiveJavaType.SHORT;
            globalTypeToTypeWrapper[RuntimePrimitiveJavaType.VOID.TypeAsTBD] = RuntimePrimitiveJavaType.VOID;
            LoadRemappedTypes();
        }

#if IMPORTER || EXPORTER

        // HACK this is used by the ahead-of-time compiler to overrule the bootstrap classloader
        // when we're compiling the core class libraries and by ikvmstub with the -bootstrap option
        internal static void SetBootstrapClassLoader(RuntimeClassLoader bootstrapClassLoader)
        {
            Debug.Assert(RuntimeClassLoaderFactory.bootstrapClassLoader == null);

            RuntimeClassLoaderFactory.bootstrapClassLoader = bootstrapClassLoader;
        }

#endif

        internal static void LoadRemappedTypes()
        {
            // if we're compiling the core, coreAssembly will be null
            var coreAssembly = JVM.BaseAssembly;
            if (coreAssembly != null && remappedTypes.Count == 0)
            {
                var remapped = AttributeHelper.GetRemappedClasses(coreAssembly);
                if (remapped.Length > 0)
                {
                    foreach (var r in remapped)
                        remappedTypes.Add(r.RemappedType, r.Name);
                }
                else
                {
#if IMPORTER
                    throw new FatalCompilerErrorException(Message.CoreClassesMissing);
#else
                    throw new InternalException("Failed to find core classes in core library.");
#endif
                }
            }
        }

        internal static bool IsRemappedType(Type type)
        {
            return remappedTypes.ContainsKey(type);
        }


#if IMPORTER || EXPORTER
        internal static RuntimeClassLoader GetBootstrapClassLoader()
#else
        internal static RuntimeAssemblyClassLoader GetBootstrapClassLoader()
#endif
        {
            lock (wrapperLock)
            {
                if (bootstrapClassLoader == null)
                    bootstrapClassLoader = new BootstrapClassLoader();

                return bootstrapClassLoader;
            }
        }

#if !IMPORTER && !EXPORTER

        internal static RuntimeClassLoader GetClassLoaderWrapper(java.lang.ClassLoader javaClassLoader)
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
                        opt |= CodeGenOptions.Debug;
#if NETFRAMEWORK
                    if (!AppDomain.CurrentDomain.IsFullyTrusted)
                        opt |= CodeGenOptions.NoAutomagicSerialization;
#endif
                    wrapper = new RuntimeClassLoader(opt, javaClassLoader);
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
        internal static RuntimeJavaType GetJavaTypeFromType(Type type)
        {
#if IMPORTER
            if (type.__ContainsMissingType)
            {
                return new RuntimeUnloadableJavaType(type);
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
                wrapper = new RuntimeUnloadableJavaType("Missing/" + type.Assembly.FullName);
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
                if (RuntimeAnonymousJavaType.IsAnonymous(type))
                {
                    var typeToTypeWrapper = globalTypeToTypeWrapper;
                    var tw = new RuntimeAnonymousJavaType(type);
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
                wrapper = RuntimeAssemblyClassLoaderFactory.FromAssembly(asm).GetJavaTypeFromAssemblyType(type);
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

        internal static RuntimeClassLoader GetGenericClassLoader(RuntimeJavaType wrapper)
        {
            Type type = wrapper.TypeAsTBD;
            Debug.Assert(type.IsGenericType);
            Debug.Assert(!type.ContainsGenericParameters);

            List<RuntimeClassLoader> list = new List<RuntimeClassLoader>();
            list.Add(RuntimeAssemblyClassLoaderFactory.FromAssembly(type.Assembly));
            foreach (Type arg in type.GetGenericArguments())
            {
                RuntimeClassLoader loader = GetJavaTypeFromType(arg).GetClassLoader();
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

        internal static RuntimeJavaType LoadClassCritical(string name)
        {
#if IMPORTER
            var wrapper = GetBootstrapClassLoader().TryLoadClassByName(name);
            if (wrapper == null)
                throw new FatalCompilerErrorException(Message.CriticalClassNotFound, name);

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

        static RuntimeClassLoader GetGenericClassLoaderByKey(RuntimeClassLoader[] key)
        {
            lock (wrapperLock)
            {
                genericClassLoaders ??= new List<RuntimeGenericClassLoader>();

                foreach (RuntimeGenericClassLoader loader in genericClassLoaders)
                    if (loader.Matches(key))
                        return loader;

#if IMPORTER || EXPORTER || FIRST_PASS
                var newLoader = new RuntimeGenericClassLoader(key, null);
#else
                var javaClassLoader = new ikvm.runtime.GenericClassLoader();
                var newLoader = new RuntimeGenericClassLoader(key, javaClassLoader);
                SetWrapperForClassLoader(javaClassLoader, newLoader);
#endif
                genericClassLoaders.Add(newLoader);
                return newLoader;
            }
        }

#if !IMPORTER && !EXPORTER

        public static void SetWrapperForClassLoader(java.lang.ClassLoader javaClassLoader, RuntimeClassLoader wrapper)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            javaClassLoader.wrapper = wrapper;
#endif
        }

#endif

#if !IMPORTER && !EXPORTER

        internal static RuntimeClassLoader GetGenericClassLoaderByName(string name)
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
                        list.Add(RuntimeClassLoaderFactory.GetAssemblyClassLoaderByName(name.Substring(start, i - start)));
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

        internal static RuntimeClassLoader GetAssemblyClassLoaderByName(string name)
        {
            if (name.StartsWith("[["))
                return GetGenericClassLoaderByName(name);

#if NETFRAMEWORK
            return RuntimeAssemblyClassLoaderFactory.FromAssembly(Assembly.Load(name));
#else
            return RuntimeAssemblyClassLoaderFactory.FromAssembly(AssemblyLoadContext.GetLoadContext(typeof(RuntimeClassLoader).Assembly).LoadFromAssemblyName(new AssemblyName(name)));
#endif
        }

#endif

        internal static int GetGenericClassLoaderId(RuntimeClassLoader wrapper)
        {
            lock (wrapperLock)
                return genericClassLoaders.IndexOf(wrapper as RuntimeGenericClassLoader);
        }

        internal static RuntimeClassLoader GetGenericClassLoaderById(int id)
        {
            lock (wrapperLock)
                return genericClassLoaders[id];
        }

        internal static void SetWrapperForType(Type type, RuntimeJavaType wrapper)
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
