/*
  Copyright (C) 2002-2013 Jeroen Frijters

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
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

using IKVM.Attributes;
using IKVM.Runtime;
using IKVM.Runtime.Syntax;

#if IMPORTER || EXPORTER
using IKVM.Reflection;

using Type = IKVM.Reflection.Type;
#else
using IKVM.Runtime.Vfs;

using System.Reflection;
#endif

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Internal
{

    internal class AssemblyClassLoader : ClassLoaderWrapper
    {

        /// <summary>
        /// Maps existing <see cref="AssemblyClassLoader"/> instances to <see cref="Assembly"/> instances. Allows
        /// assemblies to be unloaded.
        /// </summary>
        static readonly ConditionalWeakTable<Assembly, AssemblyClassLoader> assemblyClassLoaders = new();

#if !IMPORTER && !EXPORTER && !FIRST_PASS
        static Dictionary<string, string> customClassLoaderRedirects;
#endif

        /// <summary>
        /// Obtains the <see cref="AssemblyClassLoader"/> for the given <see cref="Assembly"/>. This method should not
        /// be used with dynamic Java assemblies
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        internal static AssemblyClassLoader FromAssembly(Assembly assembly)
        {
            if (assemblyClassLoaders.TryGetValue(assembly, out AssemblyClassLoader loader))
                return loader;

            loader = Create(assembly);

            lock (assemblyClassLoaders)
            {
                if (assemblyClassLoaders.TryGetValue(assembly, out var existing))
                    loader = existing;
                else
                    assemblyClassLoaders.Add(assembly, loader);
            }

            return loader;
        }

        /// <summary>
        /// Creates a new <see cref="AssemblyClassLoader"/> for the given assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        static AssemblyClassLoader Create(Assembly assembly)
        {
            // If the assembly is a part of a multi-assembly shared class loader,
            // it will export the __<MainAssembly> type from the main assembly in the group.
            var forwarder = assembly.GetType("__<MainAssembly>");
            if (forwarder != null)
            {
                var mainAssembly = forwarder.Assembly;
                if (mainAssembly != assembly)
                    return FromAssembly(mainAssembly);
            }

#if IMPORTER

            if (JVM.CoreAssembly == null && CompilerClassLoader.IsCoreAssembly(assembly))
            {
                JVM.CoreAssembly = assembly;
                ClassLoaderWrapper.LoadRemappedTypes();
            }

#endif

            if (assembly == JVM.CoreAssembly)
            {
                // This cast is necessary for ikvmc and a no-op for the runtime.
                // Note that the cast cannot fail, because ikvmc will only return a non AssemblyClassLoader
                // from GetBootstrapClassLoader() when compiling the core assembly and in that case JVM.CoreAssembly
                // will be null.
                return (AssemblyClassLoader)GetBootstrapClassLoader();
            }

            return new AssemblyClassLoader(assembly);
        }

        AssemblyLoader assemblyLoader;
        string[] references;
        AssemblyClassLoader[] delegates;
#if !IMPORTER && !EXPORTER && !FIRST_PASS
        JavaClassLoaderConstructionInProgress jclcip;
        java.security.ProtectionDomain protectionDomain;
        byte hasCustomClassLoader;  /* 0 = unknown, 1 = yes, 2 = no */
#endif
        Dictionary<int, List<int>> exports;
        string[] exportedAssemblyNames;
        AssemblyLoader[] exportedAssemblies;
        Dictionary<Assembly, AssemblyLoader> exportedLoaders;

        /// <summary>
        /// Manages a loaded assembly.
        /// </summary>
        sealed class AssemblyLoader
        {

            readonly Assembly assembly;

            bool[] isJavaModule;
            Module[] modules;
            Dictionary<string, string> nameMap;
            bool hasDotNetModule;
            AssemblyName[] internalsVisibleTo;
            string[] jarList;
#if !IMPORTER && !EXPORTER && !FIRST_PASS
            sun.misc.URLClassPath urlClassPath;
#endif

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="assembly"></param>
            internal AssemblyLoader(Assembly assembly)
            {
                this.assembly = assembly;

                modules = assembly.GetModules(false);
                isJavaModule = new bool[modules.Length];

                for (int i = 0; i < modules.Length; i++)
                {
                    var attr = AttributeHelper.GetJavaModuleAttributes(modules[i]);
                    if (attr.Length > 0)
                    {
                        isJavaModule[i] = true;

                        foreach (JavaModuleAttribute jma in attr)
                        {
                            var map = jma.GetClassMap();
                            if (map != null)
                            {
                                if (nameMap == null)
                                    nameMap = new Dictionary<string, string>();

                                for (int j = 0; j < map.Length; j += 2)
                                {
                                    var key = map[j];
                                    var val = map[j + 1];

                                    // TODO if there is a name clash between modules, this will throw.
                                    // Figure out how to handle that.
                                    nameMap.Add(key, val);
                                }
                            }

                            var jars = jma.Jars;
                            if (jars != null)
                            {
                                if (jarList == null)
                                {
                                    jarList = jars;
                                }
                                else
                                {
                                    var newList = new string[jarList.Length + jars.Length];
                                    Array.Copy(jarList, newList, jarList.Length);
                                    Array.Copy(jars, 0, newList, jarList.Length, jars.Length);
                                    jarList = newList;
                                }
                            }
                        }
                    }
                    else
                    {
                        hasDotNetModule = true;
                    }
                }
            }

            internal Assembly Assembly
            {
                get { return assembly; }
            }

            internal bool HasJavaModule
            {
                get
                {
                    for (int i = 0; i < isJavaModule.Length; i++)
                        if (isJavaModule[i])
                            return true;

                    return false;
                }
            }

            /// <summary>
            /// Attempts to load a type with the given name from the assembly.
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            Type GetType(string name)
            {
                try
                {
                    return assembly.GetType(name);
                }
                catch (ArgumentException)
                {

                }
                catch (FileLoadException e)
                {
                    // this can only happen if the assembly was loaded in the ReflectionOnly
                    // context and the requested type references a type in another assembly
                    // that cannot be found in the ReflectionOnly context
                    // TODO figure out what other exceptions Assembly.GetType() can throw
                    Tracer.Info(Tracer.Runtime, e.Message);
                }

                return null;
            }

            /// <summary>
            /// Attempts to load a type with the specified name from the given <see cref="Module"/>.
            /// </summary>
            /// <param name="module"></param>
            /// <param name="name"></param>
            /// <returns></returns>
            Type GetType(Module module, string name)
            {
                try
                {
                    return module.GetType(name);
                }
                catch (ArgumentException)
                {

                }
                catch (FileLoadException e)
                {
                    // this can only happen if the assembly was loaded in the ReflectionOnly
                    // context and the requested type references a type in another assembly
                    // that cannot be found in the ReflectionOnly context
                    // TODO figure out what other exceptions Assembly.GetType() can throw
                    Tracer.Info(Tracer.Runtime, e.Message);
                }

                return null;
            }

            Type GetJavaType(Module module, string name)
            {
                try
                {
                    string n = null;
                    if (nameMap != null)
                        nameMap.TryGetValue(name, out n);

                    var t = GetType(module, n ?? name);
                    if (t == null)
                    {
                        n = name.Replace('$', '+');
                        if (!ReferenceEquals(n, name))
                            t = GetType(n);
                    }

                    if (t != null
                        && !AttributeHelper.IsHideFromJava(t)
                        && !t.IsArray
                        && !t.IsPointer
                        && !t.IsByRef)
                        return t;
                }
                catch (ArgumentException x)
                {
                    // we can end up here because we replace the $ with a plus sign
                    // (or client code did a Class.forName() on an invalid name)
                    Tracer.Info(Tracer.Runtime, x.Message);
                }

                return null;
            }

            internal TypeWrapper DoLoad(string name)
            {
                for (int i = 0; i < modules.Length; i++)
                {
                    if (isJavaModule[i])
                    {
                        var type = GetJavaType(modules[i], name);
                        if (type != null)
                        {
                            // check the name to make sure that the canonical name was used
                            if (CompiledTypeWrapper.GetName(type) == name)
                            {
                                return CompiledTypeWrapper.newInstance(name, type);
                            }
                        }
                    }
                    else
                    {
                        var type = GetType(modules[i], DotNetTypeWrapper.DemangleTypeName(name));

                        // type could be loaded from this assembly, but ended up forwarded to a different assembly
                        // this class loader isn't responsible for it
                        if (type != null && type.Assembly != assembly)
                            return null;

                        // type was loaded successfully
                        if (type != null && DotNetTypeWrapper.IsAllowedOutside(type))
                        {
                            // check the name to make sure that the canonical name was used
                            if (DotNetTypeWrapper.GetName(type) == name)
                            {
                                return DotNetTypeWrapper.Create(type, name);
                            }
                        }
                    }
                }

                if (hasDotNetModule)
                {
                    // for fake types, we load the declaring outer type (the real one) and
                    // let that generated the manufactured nested classes
                    // (note that for generic outer types, we need to duplicate this in ClassLoaderWrapper.LoadGenericClass)
                    TypeWrapper outer = null;
                    if (name.EndsWith(DotNetTypeWrapper.DelegateInterfaceSuffix))
                    {
                        outer = DoLoad(name.Substring(0, name.Length - DotNetTypeWrapper.DelegateInterfaceSuffix.Length));
                    }
                    else if (name.EndsWith(DotNetTypeWrapper.AttributeAnnotationSuffix))
                    {
                        outer = DoLoad(name.Substring(0, name.Length - DotNetTypeWrapper.AttributeAnnotationSuffix.Length));
                    }
                    else if (name.EndsWith(DotNetTypeWrapper.AttributeAnnotationReturnValueSuffix))
                    {
                        outer = DoLoad(name.Substring(0, name.Length - DotNetTypeWrapper.AttributeAnnotationReturnValueSuffix.Length));
                    }
                    else if (name.EndsWith(DotNetTypeWrapper.AttributeAnnotationMultipleSuffix))
                    {
                        outer = DoLoad(name.Substring(0, name.Length - DotNetTypeWrapper.AttributeAnnotationMultipleSuffix.Length));
                    }
                    else if (name.EndsWith(DotNetTypeWrapper.EnumEnumSuffix))
                    {
                        outer = DoLoad(name.Substring(0, name.Length - DotNetTypeWrapper.EnumEnumSuffix.Length));
                    }

                    if (outer != null && outer.IsFakeTypeContainer)
                        foreach (var tw in outer.InnerClasses)
                            if (tw.Name == name)
                                return tw;
                }

                return null;
            }

            /// <summary>
            /// Gets the Java type name for the specified type.
            /// </summary>
            /// <param name="type"></param>
            /// <param name="isJavaType"></param>
            /// <returns></returns>
            internal JavaTypeName? GetTypeNameAndType(Type type, out bool isJavaType)
            {
                // find the module index of the type's module
                var module = type.Module;
                int moduleIndex = -1;
                for (int i = 0; i < modules.Length; i++)
                {
                    if (modules[i] == module)
                    {
                        moduleIndex = i;
                        break;
                    }
                }

                // if the type is associated with a Java module, the type is a Java type
                if (isJavaModule[moduleIndex])
                {
                    isJavaType = true;

                    // types which should be hidden from Java should not have Java names
                    if (AttributeHelper.IsHideFromJava(type))
                        return null;

                    return CompiledTypeWrapper.GetName(type);
                }
                else
                {
                    isJavaType = false;

                    // type is a .NET type, but not allowed visibilty to Java
                    if (DotNetTypeWrapper.IsAllowedOutside(type) == false)
                        return null;

                    return DotNetTypeWrapper.GetName(type);
                }
            }

            internal TypeWrapper CreateWrapperForAssemblyType(Type type)
            {
                var name = GetTypeNameAndType(type, out bool isJavaType);
                if (name == null)
                    return null;

                if (isJavaType)
                {
                    // since this type was compiled from Java source, we have to look for our attributes
                    return CompiledTypeWrapper.newInstance(name, type);
                }
                else
                {
                    // since this type was not compiled from Java source, we don't need to
                    // look for our attributes, but we do need to filter unrepresentable
                    // stuff (and transform some other stuff)
                    return DotNetTypeWrapper.Create(type, name);
                }
            }

            internal bool InternalsVisibleTo(AssemblyName otherName)
            {
                if (internalsVisibleTo == null)
                    Interlocked.CompareExchange(ref internalsVisibleTo, AttributeHelper.GetInternalsVisibleToAttributes(assembly), null);

                foreach (var name in internalsVisibleTo)
                {
                    // we match the simple name and PublicKeyToken (because the AssemblyName constructor used
                    // by GetInternalsVisibleToAttributes() only sets the PublicKeyToken, even if a PublicKey is specified)
                    if (ReflectUtil.MatchNameAndPublicKeyToken(name, otherName))
                    {
                        return true;
                    }
                }

                return false;
            }

#if !IMPORTER && !EXPORTER && !FIRST_PASS

            internal java.util.Enumeration FindResources(string name)
            {
                if (urlClassPath == null)
                {
                    if (jarList == null)
                        return gnu.java.util.EmptyEnumeration.getInstance();

                    var urls = new List<java.net.URL>();
                    foreach (var jar in jarList)
                        urls.Add(MakeResourceURL(assembly, jar));

                    Interlocked.CompareExchange(ref urlClassPath, new sun.misc.URLClassPath(urls.ToArray()), null);
                }

                return urlClassPath.findResources(name, true);
            }

#endif
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        internal AssemblyClassLoader(Assembly assembly) :
            this(assembly, null)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="fixedReferences"></param>
        internal AssemblyClassLoader(Assembly assembly, string[] fixedReferences) :
            base(CodeGenOptions.None, null)
        {
            this.assemblyLoader = new AssemblyLoader(assembly);
            this.references = fixedReferences;
        }

#if IMPORTER

        internal static void PreloadExportedAssemblies(Assembly assembly)
        {
            if (assembly.GetManifestResourceInfo("ikvm.exports") != null)
            {
                using (Stream stream = assembly.GetManifestResourceStream("ikvm.exports"))
                {
                    BinaryReader rdr = new BinaryReader(stream);
                    int assemblyCount = rdr.ReadInt32();
                    for (int i = 0; i < assemblyCount; i++)
                    {
                        string assemblyName = rdr.ReadString();
                        int typeCount = rdr.ReadInt32();
                        if (typeCount != 0)
                        {
                            for (int j = 0; j < typeCount; j++)
                            {
                                rdr.ReadInt32();
                            }
                            try
                            {
                                StaticCompiler.LoadFile(assembly.Location + "/../" + new AssemblyName(assemblyName).Name + ".dll");
                            }
                            catch { }
                        }
                    }
                }
            }
        }

#endif

        void DoInitializeExports()
        {
            lock (this)
            {
                if (delegates == null)
                {
                    if (!ReflectUtil.IsDynamicAssembly(assemblyLoader.Assembly) && assemblyLoader.Assembly.GetManifestResourceInfo("ikvm.exports") != null)
                    {
                        var wildcardExports = new List<string>();

                        using (var stream = assemblyLoader.Assembly.GetManifestResourceStream("ikvm.exports"))
                        {
                            var rdr = new BinaryReader(stream);
                            var assemblyCount = rdr.ReadInt32();
                            exports = new Dictionary<int, List<int>>();
                            exportedAssemblies = new AssemblyLoader[assemblyCount];
                            exportedAssemblyNames = new string[assemblyCount];
                            exportedLoaders = new Dictionary<Assembly, AssemblyLoader>();

                            for (int i = 0; i < assemblyCount; i++)
                            {
                                exportedAssemblyNames[i] = string.Intern(rdr.ReadString());

                                int typeCount = rdr.ReadInt32();
                                if (typeCount == 0 && references == null)
                                    wildcardExports.Add(exportedAssemblyNames[i]);

                                for (int j = 0; j < typeCount; j++)
                                {
                                    int hash = rdr.ReadInt32();
                                    if (!exports.TryGetValue(hash, out List<int> assemblies))
                                    {
                                        assemblies = new List<int>();
                                        exports.Add(hash, assemblies);
                                    }
                                    assemblies.Add(i);
                                }
                            }
                        }

                        if (references == null)
                            references = wildcardExports.ToArray();
                    }
                    else
                    {
                        AssemblyName[] refNames = assemblyLoader.Assembly.GetReferencedAssemblies();
                        references = new string[refNames.Length];
                        for (int i = 0; i < references.Length; i++)
                        {
                            references[i] = refNames[i].FullName;
                        }
                    }

                    Interlocked.Exchange(ref delegates, new AssemblyClassLoader[references.Length]);
                }
            }
        }

        void LazyInitExports()
        {
            if (delegates == null)
                DoInitializeExports();
        }

        internal Assembly MainAssembly => assemblyLoader.Assembly;

        internal Assembly GetAssembly(TypeWrapper wrapper)
        {
            Debug.Assert(wrapper.GetClassLoader() == this);

            while (wrapper.IsFakeNestedType)
                wrapper = wrapper.DeclaringTypeWrapper;

            return wrapper.TypeAsBaseType.Assembly;
        }

        Assembly LoadAssemblyOrClearName(ref string name, bool exported)
        {
            // previous load attempt failed
            if (name == null)
                return null;

            try
            {
#if IMPORTER || EXPORTER
                return StaticCompiler.Load(name);
#else
                return Assembly.Load(name);
#endif
            }
            catch
            {
                // cache failure by clearing out the name the caller uses
                name = null;
                // should we issue a warning error (in ikvmc)?
                return null;
            }
        }

        internal TypeWrapper DoLoad(string name)
        {
            var tw = assemblyLoader.DoLoad(name);
            if (tw != null)
                return RegisterInitiatingLoader(tw);

            LazyInitExports();

            if (exports != null && exports.TryGetValue(JVM.PersistableHash(name), out var assemblies))
            {
                foreach (int index in assemblies)
                {
                    var loader = TryGetLoaderByIndex(index);
                    if (loader != null)
                    {
                        tw = loader.DoLoad(name);
                        if (tw != null)
                            return RegisterInitiatingLoader(tw);
                    }
                }
            }

            return null;
        }

        internal JavaTypeName? GetTypeNameAndType(Type type, out bool isJavaType)
        {
            return GetLoader(type.Assembly).GetTypeNameAndType(type, out isJavaType);
        }

        AssemblyLoader TryGetLoaderByIndex(int index)
        {
            var loader = exportedAssemblies[index];
            if (loader == null)
            {
                var asm = LoadAssemblyOrClearName(ref exportedAssemblyNames[index], true);
                if (asm != null)
                    loader = exportedAssemblies[index] = GetLoaderForExportedAssembly(asm);
            }

            return loader;
        }

        internal List<Assembly> GetAllAvailableAssemblies()
        {
            var list = new List<Assembly>();
            list.Add(assemblyLoader.Assembly);

            LazyInitExports();

            if (exportedAssemblies != null)
            {
                for (int i = 0; i < exportedAssemblies.Length; i++)
                {
                    var loader = TryGetLoaderByIndex(i);
                    if (loader != null && FromAssembly(loader.Assembly) == this)
                        list.Add(loader.Assembly);
                }
            }

            return list;
        }

        AssemblyLoader GetLoader(Assembly assembly)
        {
            if (assemblyLoader.Assembly == assembly)
                return assemblyLoader;

            return GetLoaderForExportedAssembly(assembly);
        }

        AssemblyLoader GetLoaderForExportedAssembly(Assembly assembly)
        {
            LazyInitExports();

            AssemblyLoader loader;
            lock (exportedLoaders)
                exportedLoaders.TryGetValue(assembly, out loader);

            if (loader == null)
            {
                loader = new AssemblyLoader(assembly);

                lock (exportedLoaders)
                {
                    if (exportedLoaders.TryGetValue(assembly, out AssemblyLoader existing))
                    {
                        // another thread beat us to it
                        loader = existing;
                    }
                    else
                    {
                        exportedLoaders.Add(assembly, loader);
                    }
                }
            }

            return loader;
        }

        /// <summary>
        /// Gets the type wrapper for the given type located in this assembly.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        internal virtual TypeWrapper GetWrapperFromAssemblyType(Type type)
        {
            if (type.Name.EndsWith("[]"))
                throw new InternalException();
            if (AssemblyClassLoader.FromAssembly(type.Assembly) != this)
                throw new InternalException();

            var wrapper = GetLoader(type.Assembly).CreateWrapperForAssemblyType(type);
            if (wrapper != null)
            {
                if (type.IsGenericType && !type.IsGenericTypeDefinition)
                {
                    // in the case of "magic" implementation generic type instances we'll end up here as well,
                    // but then wrapper.GetClassLoader() will return this anyway
                    wrapper = wrapper.GetClassLoader().RegisterInitiatingLoader(wrapper);
                }
                else
                {
                    wrapper = RegisterInitiatingLoader(wrapper);
                }

                // this really shouldn't happen, it means that we have two different types in our assembly that both
                // have the same Java name
                if (wrapper.TypeAsTBD != type && (!wrapper.IsRemapped || wrapper.TypeAsBaseType != type))
                {
#if IMPORTER
                    throw new FatalCompilerErrorException(Message.AssemblyContainsDuplicateClassNames, type.FullName, wrapper.TypeAsTBD.FullName, wrapper.Name, type.Assembly.FullName);
#else
                    throw new InternalException($"\nType \"{type.FullName}\" and \"{wrapper.TypeAsTBD.FullName}\" both map to the same name \"{wrapper.Name}\".");
#endif
                }

                return wrapper;
            }

            return null;
        }

        protected override TypeWrapper LoadClassImpl(string name, LoadMode mode)
        {
            var tw = FindLoadedClass(name);
            if (tw != null)
                return tw;

#if !IMPORTER && !EXPORTER && !FIRST_PASS

            while (hasCustomClassLoader != 2)
            {
                if (hasCustomClassLoader == 0)
                {
                    var customClassLoader = GetCustomClassLoaderType();
                    if (customClassLoader == null)
                    {
                        hasCustomClassLoader = 2;
                        break;
                    }

                    WaitInitializeJavaClassLoader(customClassLoader);
                    hasCustomClassLoader = 1;
                }
                return base.LoadClassImpl(name, mode);
            }

#endif

            return LoadBootstrapIfNonJavaAssembly(name)
                ?? LoadDynamic(name)
                ?? FindOrLoadGenericClass(name, LoadMode.LoadOrNull);
        }

        /// <summary>
        /// This implements ikvm.runtime.AssemblyClassLoader.loadClass(), so unlike the above LoadClassImpl, it
        /// doesn't delegate to Java, but otherwise it should be the same algorithm.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal TypeWrapper LoadClass(string name)
        {
            return FindLoadedClass(name)
                ?? LoadBootstrapIfNonJavaAssembly(name)
                ?? LoadDynamic(name)
                ?? FindOrLoadGenericClass(name, LoadMode.LoadOrNull);
        }

        TypeWrapper LoadBootstrapIfNonJavaAssembly(string name)
        {
            if (!assemblyLoader.HasJavaModule)
                return GetBootstrapClassLoader().LoadClassByDottedNameFast(name);

            return null;
        }

        TypeWrapper LoadDynamic(string name)
        {
#if !IMPORTER && !EXPORTER && !FIRST_PASS
            var classFile = name.Replace('.', '/') + ".class";
            foreach (var res in GetBootstrapClassLoader().FindDelegateResources(classFile))
                return res.Loader.DefineDynamic(name, res.URL);
            foreach (var res in FindDelegateResources(classFile))
                return res.Loader.DefineDynamic(name, res.URL);
            foreach (var url in FindResources(classFile))
                return DefineDynamic(name, url);
#endif
            return null;
        }

#if !IMPORTER && !EXPORTER && !FIRST_PASS

        TypeWrapper DefineDynamic(string name, java.net.URL url)
        {
            byte[] buf;

            using (var inp = url.openStream())
            {
                buf = new byte[inp.available()];
                for (int pos = 0; pos < buf.Length;)
                {
                    int read = inp.read(buf, pos, buf.Length - pos);
                    if (read <= 0)
                        break;

                    pos += read;
                }
            }

            // when the VM initiates a class load, it doesn't go through ClassLoader.loadClass() for non-custom Assembly class loaders (for efficiency)
            // so when we dynamically attempt to define a class, we have to explicitly obtain the class loading lock to prevent race conditions
            var loader = GetJavaClassLoader();
            lock (loader == null ? this : loader.getClassLoadingLock(name))
            {
                // make sure the class wasn't defined since we last checked and before we acquired the lock
                var tw = FindLoadedClass(name);
                if (tw != null)
                    return tw;

                return TypeWrapper.FromClass(IKVM.Java.Externs.java.lang.ClassLoader.defineClass1(loader, name, buf, 0, buf.Length, GetProtectionDomain(), null));
            }
        }
#endif

        TypeWrapper FindReferenced(string name)
        {
            for (int i = 0; i < delegates.Length; i++)
            {
                if (delegates[i] == null)
                {
                    var asm = LoadAssemblyOrClearName(ref references[i], false);
                    if (asm != null)
                        delegates[i] = AssemblyClassLoader.FromAssembly(asm);
                }
                if (delegates[i] != null)
                {
                    var tw = delegates[i].DoLoad(name);
                    if (tw != null)
                        return RegisterInitiatingLoader(tw);
                }
            }

            return null;
        }

#if !IMPORTER && !EXPORTER

        static java.net.URL MakeResourceURL(Assembly asm, string name)
        {
#if FIRST_PASS
            return null;
#else
            return new java.io.File(Path.Combine(VfsTable.Default.GetAssemblyResourcesPath(asm), name)).toURI().toURL();
#endif
        }

        internal IEnumerable<java.net.URL> FindResources(string unmangledName)
        {
            // cannot find resources in dynamic assembly
            if (assemblyLoader.Assembly.IsDynamic)
                yield break;

            var found = false;

#if !FIRST_PASS
            var urls = assemblyLoader.FindResources(unmangledName);
            while (urls.hasMoreElements())
            {
                found = true;
                yield return (java.net.URL)urls.nextElement();
            }
#endif

            // assembly is not a Java assembly
            if (assemblyLoader.HasJavaModule == false)
            {
                // attempt to find an assembly resource with the exact name
                if (unmangledName != "" && assemblyLoader.Assembly.GetManifestResourceInfo(unmangledName) != null)
                {
                    found = true;
                    yield return MakeResourceURL(assemblyLoader.Assembly, unmangledName);
                }

                // the JavaResourceAttribute can be used to manufacture a named Java resource
                foreach (var res in assemblyLoader.Assembly.GetCustomAttributes<JavaResourceAttribute>())
                {
                    if (res.JavaName == unmangledName)
                    {
                        found = true;
                        yield return MakeResourceURL(assemblyLoader.Assembly, res.ResourceName);
                    }
                }
            }

            // find an assembly resource with the managed resource name
            var name = JVM.MangleResourceName(unmangledName);
            if (assemblyLoader.Assembly.GetManifestResourceInfo(name) != null)
            {
                found = true;
                yield return MakeResourceURL(assemblyLoader.Assembly, name);
            }

            LazyInitExports();

            if (exports != null && exports.TryGetValue(JVM.PersistableHash(unmangledName), out var assemblies))
            {
                foreach (int index in assemblies)
                {
                    var loader = exportedAssemblies[index];
                    if (loader == null)
                    {
                        var asm = LoadAssemblyOrClearName(ref exportedAssemblyNames[index], true);
                        if (asm == null)
                            continue;

                        loader = exportedAssemblies[index] = GetLoaderForExportedAssembly(asm);
                    }
#if !FIRST_PASS
                    urls = loader.FindResources(unmangledName);
                    while (urls.hasMoreElements())
                    {
                        found = true;
                        yield return (java.net.URL)urls.nextElement();
                    }
#endif
                    if (loader.Assembly.GetManifestResourceInfo(name) != null)
                    {
                        found = true;
                        yield return MakeResourceURL(loader.Assembly, name);
                    }
                }
            }

#if !FIRST_PASS

            // if asked for a '.class' resource, we can return the appropriate stub
            if (!found && unmangledName.EndsWith(".class", StringComparison.Ordinal) && unmangledName.IndexOf('.') == unmangledName.Length - 6)
            {
                var tw = FindLoadedClass(unmangledName.Substring(0, unmangledName.Length - 6).Replace('/', '.'));
                if (tw != null && tw.GetClassLoader() == this && !tw.IsArray && !tw.IsDynamic)
                    yield return new java.io.File(Path.Combine(VfsTable.Default.GetAssemblyClassesPath(assemblyLoader.Assembly), unmangledName)).toURI().toURL();
            }

#endif
        }

        protected struct Resource
        {

            internal readonly java.net.URL URL;
            internal readonly AssemblyClassLoader Loader;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="url"></param>
            /// <param name="loader"></param>
            internal Resource(java.net.URL url, AssemblyClassLoader loader)
            {
                this.URL = url;
                this.Loader = loader;
            }

        }

        /// <summary>
        /// Looks up resources from delegated assembly loaders.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected IEnumerable<Resource> FindDelegateResources(string name)
        {
            LazyInitExports();

            for (int i = 0; i < delegates.Length; i++)
            {
                if (delegates[i] == null)
                {
                    var asm = LoadAssemblyOrClearName(ref references[i], false);
                    if (asm != null)
                        delegates[i] = AssemblyClassLoader.FromAssembly(asm);
                }

                if (delegates[i] != null && delegates[i] != GetBootstrapClassLoader())
                    foreach (java.net.URL url in delegates[i].FindResources(name))
                        yield return new Resource(url, delegates[i]);
            }
        }

        /// <summary>
        /// Searches the class loader for a resource with the given name, starting at the bootstrap classloader, then
        /// consulting any delegated resources, and finally the managed assembly itself.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal virtual IEnumerable<java.net.URL> GetResources(string name)
        {
            foreach (var url in GetBootstrapClassLoader().GetResources(name))
                yield return url;

            foreach (var res in FindDelegateResources(name))
                yield return res.URL;

            foreach (var url in FindResources(name))
                yield return url;
        }

#endif // !IMPORTER

#if !IMPORTER && !FIRST_PASS && !EXPORTER

        private sealed class JavaClassLoaderConstructionInProgress
        {
            internal readonly Thread Thread = Thread.CurrentThread;
            internal java.lang.ClassLoader javaClassLoader;
            internal int recursion;
        }

        private java.lang.ClassLoader WaitInitializeJavaClassLoader(Type customClassLoader)
        {
            Interlocked.CompareExchange(ref jclcip, new JavaClassLoaderConstructionInProgress(), null);
            JavaClassLoaderConstructionInProgress curr = jclcip;
            if (curr != null)
            {
                if (curr.Thread == Thread.CurrentThread)
                {
                    if (curr.javaClassLoader != null)
                    {
                        // we were recursively invoked during the class loader construction,
                        // so we have to return the partialy constructed class loader
                        return curr.javaClassLoader;
                    }
                    curr.recursion++;
                    try
                    {
                        if (javaClassLoader == null)
                        {
                            InitializeJavaClassLoader(curr, customClassLoader);
                        }
                    }
                    finally
                    {
                        // We only publish the class loader from the outer most invocation, otherwise
                        // an invocation of getClassLoader in the static initializer or constructor
                        // of the custom class loader would result in prematurely publishing it.
                        if (--curr.recursion == 0)
                        {
                            lock (this)
                            {
                                jclcip = null;
                                Monitor.PulseAll(this);
                            }
                        }
                    }
                }
                else
                {
                    lock (this)
                    {
                        while (jclcip != null)
                        {
                            Monitor.Wait(this);
                        }
                    }
                }
            }
            return javaClassLoader;
        }

        internal override java.lang.ClassLoader GetJavaClassLoader()
        {
            return javaClassLoader ?? WaitInitializeJavaClassLoader(GetCustomClassLoaderType());
        }

        internal virtual java.security.ProtectionDomain GetProtectionDomain()
        {
            if (protectionDomain == null)
                Interlocked.CompareExchange(ref protectionDomain, new java.security.ProtectionDomain(assemblyLoader.Assembly), null);

            return protectionDomain;
        }
#endif

        protected override TypeWrapper FindLoadedClassLazy(string name)
        {
            return DoLoad(name)
                ?? FindReferenced(name)
                ?? FindOrLoadGenericClass(name, LoadMode.Find);
        }

        internal override bool InternalsVisibleToImpl(TypeWrapper wrapper, TypeWrapper friend)
        {
            var other = friend.GetClassLoader();
            if (this == other)
            {
#if IMPORTER || EXPORTER
                return true;
#else
                // we're OK if the type being accessed (wrapper) is a dynamic type
                // or if the dynamic assembly has internal access
                return GetAssembly(wrapper).Equals(GetTypeWrapperFactory().ModuleBuilder.Assembly)
                    || GetTypeWrapperFactory().HasInternalAccess;
#endif
            }
            AssemblyName otherName;
#if IMPORTER
            CompilerClassLoader ccl = other as CompilerClassLoader;
            if (ccl == null)
            {
                return false;
            }
            otherName = ccl.GetAssemblyName();
#else
            AssemblyClassLoader acl = other as AssemblyClassLoader;
            if (acl == null)
            {
                return false;
            }
            otherName = acl.GetAssembly(friend).GetName();
#endif
            return GetLoader(GetAssembly(wrapper)).InternalsVisibleTo(otherName);
        }

        internal void AddDelegate(AssemblyClassLoader acl)
        {
            LazyInitExports();
            lock (this)
            {
                delegates = ArrayUtil.Concat(delegates, acl);
            }
        }

#if !IMPORTER && !EXPORTER

        internal List<KeyValuePair<string, string[]>> GetPackageInfo()
        {
            var list = new List<KeyValuePair<string, string[]>>();
            foreach (var m in assemblyLoader.Assembly.GetModules(false))
            {
                var attr = m.GetCustomAttributes<PackageListAttribute>();
                foreach (var p in attr)
                    list.Add(new KeyValuePair<string, string[]>(p.jar, p.packages));
            }

            return list;
        }

#endif

#if !IMPORTER && !FIRST_PASS && !EXPORTER

        Type GetCustomClassLoaderType()
        {
            LoadCustomClassLoaderRedirects();

            var assembly = assemblyLoader.Assembly;
            var assemblyName = assembly.FullName;

            foreach (var kv in customClassLoaderRedirects)
            {
                var asm = kv.Key;

                // FXBUG
                // We only support matching on the assembly's simple name,
                // because there appears to be no viable alternative.
                // There is AssemblyName.ReferenceMatchesDefinition()
                // but it is completely broken.
                if (assemblyName.StartsWith(asm + ","))
                {
                    try
                    {
                        return Type.GetType(kv.Value, true);
                    }
                    catch (Exception x)
                    {
                        Tracer.Error(Tracer.Runtime, "Unable to load custom class loader {0} specified in app.config for assembly {1}: {2}", kv.Value, assembly, x);
                    }

                    break;
                }
            }

            var attribs = assembly.GetCustomAttributes(typeof(CustomAssemblyClassLoaderAttribute), false);
            if (attribs.Length == 1)
                return ((CustomAssemblyClassLoaderAttribute)attribs[0]).Type;

            return null;
        }

        void InitializeJavaClassLoader(JavaClassLoaderConstructionInProgress jclcip, Type customClassLoaderClass)
        {
            var assembly = assemblyLoader.Assembly;

            if (customClassLoaderClass != null)
            {
                try
                {
                    if (!customClassLoaderClass.IsPublic && !customClassLoaderClass.Assembly.Equals(assembly))
                        throw new InternalException($"Custom class loader type is not accessible: '{customClassLoaderClass}'.");

                    var customClassLoaderCtor = customClassLoaderClass.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(Assembly) }, null);
                    if (customClassLoaderCtor == null)
                        throw new InternalException($"Custom class loader type has no empty constructor: '{customClassLoaderClass}'.");

                    if (!customClassLoaderCtor.IsPublic && !customClassLoaderClass.Assembly.Equals(assembly))
                        throw new InternalException($"Custom class loader constructor is not accessible: '{customClassLoaderClass}'.");

                    // NOTE we're creating an uninitialized instance of the custom class loader here, so that getClassLoader will return the proper object
                    // when it is called during the construction of the custom class loader later on. This still doesn't make it safe to use the custom
                    // class loader before it is constructed, but at least the object instance is available and should anyone cache it, they will get the
                    // right object to use later on.
                    // Note that creating the unitialized instance will (unfortunately) trigger the static initializer. The static initializer can
                    // trigger a call to getClassLoader(), which means we can end up here recursively.
                    var newJavaClassLoader = (java.lang.ClassLoader)GetUninitializedObject(customClassLoaderClass);

                    // check if we weren't invoked recursively and the nested invocation already did the work
                    if (jclcip.javaClassLoader == null)
                    {
                        jclcip.javaClassLoader = newJavaClassLoader;
                        SetWrapperForClassLoader(jclcip.javaClassLoader, this);
                        DoPrivileged(new CustomClassLoaderCtorCaller(customClassLoaderCtor, jclcip.javaClassLoader, assembly));
                        Tracer.Info(Tracer.Runtime, "Created custom assembly class loader {0} for assembly {1}", customClassLoaderClass.FullName, assembly);
                    }
                    else
                    {
                        // we didn't initialize the object, so there is no need to finalize it
                        GC.SuppressFinalize(newJavaClassLoader);
                    }
                }
                catch (Exception x)
                {
                    Tracer.Error(Tracer.Runtime, "Unable to create custom assembly class loader {0} for {1}: {2}", customClassLoaderClass.FullName, assembly, x);
                }
            }

            if (jclcip.javaClassLoader == null)
            {
                jclcip.javaClassLoader = new ikvm.runtime.AssemblyClassLoader();
                SetWrapperForClassLoader(jclcip.javaClassLoader, this);
            }

            // finally we publish the class loader for other threads to see
            Thread.MemoryBarrier();
            javaClassLoader = jclcip.javaClassLoader;
        }

        // separate method to avoid LinkDemand killing the caller
        // and to bridge transparent -> critical boundary
        [System.Security.SecuritySafeCritical]
        static object GetUninitializedObject(Type type)
        {
            return FormatterServices.GetUninitializedObject(type);
        }

        static void LoadCustomClassLoaderRedirects()
        {
            if (customClassLoaderRedirects == null)
            {
                var dict = new Dictionary<string, string>();

                try
                {
#if NETFRAMEWORK
                    foreach (var key in System.Configuration.ConfigurationManager.AppSettings.AllKeys)
                    {
                        const string prefix = "ikvm-classloader:";
                        if (key.StartsWith(prefix))
                            dict[key.Substring(prefix.Length)] = System.Configuration.ConfigurationManager.AppSettings.Get(key);
                    }
#endif
                }
                catch (Exception x)
                {
                    Tracer.Error(Tracer.Runtime, "Error while reading custom class loader redirects: {0}", x);
                }
                finally
                {
                    Interlocked.CompareExchange(ref customClassLoaderRedirects, dict, null);
                }
            }
        }

        /// <summary>
        /// Uses reflection to invoke the constructor of am uninitialized class loader instance.
        /// </summary>
        sealed class CustomClassLoaderCtorCaller : java.security.PrivilegedAction
        {

            readonly ConstructorInfo ctor;
            readonly object classLoader;
            readonly Assembly assembly;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="ctor"></param>
            /// <param name="classLoader"></param>
            /// <param name="assembly"></param>
            internal CustomClassLoaderCtorCaller(ConstructorInfo ctor, object classLoader, Assembly assembly)
            {
                this.ctor = ctor ?? throw new ArgumentNullException(nameof(ctor));
                this.classLoader = classLoader ?? throw new ArgumentNullException(nameof(classLoader));
                this.assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            }

            public object run()
            {
                ctor.Invoke(classLoader, new object[] { assembly });
                return null;
            }
        }

#endif

    }

    /// <summary>
    /// Represents the bootstrap class loader of the system, containing only built-in .NET assemblies.
    /// </summary>
    sealed class BootstrapClassLoader : AssemblyClassLoader
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal BootstrapClassLoader() :
            base(JVM.CoreAssembly, new string[] { typeof(object).Assembly.FullName, typeof(Uri).Assembly.FullName })
        {

        }

        internal override TypeWrapper GetWrapperFromAssemblyType(Type type)
        {
            // we have to special case the fake types here
            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                var outer = ClassLoaderWrapper.GetWrapperFromType(type.GetGenericArguments()[0]);

                foreach (var inner in outer.InnerClasses)
                {
                    if (inner.TypeAsTBD == type)
                        return inner;

                    foreach (var inner2 in inner.InnerClasses)
                        if (inner2.TypeAsTBD == type)
                            return inner2;
                }

                return null;
            }

            return base.GetWrapperFromAssemblyType(type);
        }

        protected override void CheckProhibitedPackage(string className)
        {

        }

#if !FIRST_PASS && !IMPORTER && !EXPORTER

        internal override java.lang.ClassLoader GetJavaClassLoader()
        {
            return null;
        }

        internal override java.security.ProtectionDomain GetProtectionDomain()
        {
            return null;
        }

        internal override IEnumerable<java.net.URL> GetResources(string name)
        {
            foreach (java.net.URL url in FindResources(name))
                yield return url;

            foreach (var res in FindDelegateResources(name))
                yield return res.URL;
        }

#endif

    }

}
