/*
  Copyright (C) 2002-2014 Jeroen Frijters

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
using System.Runtime.CompilerServices;
using System.Collections.Concurrent;

using IKVM.CoreLib.Exceptions;

#if IMPORTER
using IKVM.Tools.Importer;
#else
using System.Reflection;
#endif

namespace IKVM.Runtime
{

    /// <summary>
    /// Maintains instances of <see cref="DynamicClassLoader"/>.
    /// </summary>
    class DynamicClassLoaderFactory
    {

        readonly RuntimeContext context;
        internal readonly ConcurrentDictionary<string, RuntimeJavaType> dynamicTypes = new();
        DynamicClassLoader instance;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DynamicClassLoaderFactory(RuntimeContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

#if IMPORTER == false
            // we attach to the AppDomain.TypeResolve event to be notified when the CLR cannot find a type, and we must provide it
            AppDomain.CurrentDomain.TypeResolve += new ResolveEventHandler(OnTypeResolve);

            // exclude '<Module>' as a valid type name, as it overlaps with the pseudo-type for global fields and methods
            dynamicTypes.TryAdd("<Module>", null);
#endif
        }

#if IMPORTER == false

        /// <summary>
        /// Invoked when the runtime cannot load a type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Assembly OnTypeResolve(object sender, ResolveEventArgs args)
        {
            return Resolve(dynamicTypes, args.Name);
        }

        /// <summary>
        /// Attempts to resolve the given type name from the specified dictionary.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Assembly Resolve(ConcurrentDictionary<string, RuntimeJavaType> dict, string name)
        {
            if (dict.TryGetValue(name, out var type))
            {
                try
                {
                    type.Finish();
                    return type.TypeAsTBD.Assembly;
                }
                catch (TranslatableJavaException e)
                {
                    throw context.ExceptionHelper.MapException<Exception>(e, true, false);
                }
            }

            return null;
        }

#endif

        /// <summary>
        /// Gets the <see cref="DynamicClassLoader"/> instance which should be used for dynamic classes emitted by the given class loader.
        /// </summary>
        /// <param name="loader"></param>
        /// <returns></returns>
        [System.Security.SecuritySafeCritical]
        public DynamicClassLoader GetOrCreate(RuntimeClassLoader loader)
        {
#if IMPORTER
            // importer uses only one class loader, and we can just return a single dynamic class loader
            return new DynamicClassLoader(context, loader.Diagnostics, ((ImportClassLoader)loader).CreateModuleBuilder(), false);
#else
            // each assembly class loader gets its own dynamic class loader
            if (loader is RuntimeAssemblyClassLoader acl)
            {
                var name = acl.MainAssembly.GetName().Name + context.Options.DynamicAssemblySuffixAndPublicKey;
                foreach (var attr in acl.MainAssembly.GetCustomAttributes<InternalsVisibleToAttribute>())
                {
                    if (attr.AssemblyName == name)
                    {
                        var n = new AssemblyName(name);
#if NETFRAMEWORK
                        n.KeyPair = DynamicClassLoader.ForgedKeyPair.Instance;
#endif
                        return new DynamicClassLoader(context, loader.Diagnostics, DynamicClassLoader.CreateModuleBuilder(context, n), true);
                    }
                }
            }

            return instance ??= new DynamicClassLoader(context, loader.Diagnostics, DynamicClassLoader.CreateModuleBuilder(context), false);
#endif
        }

        /// <summary>
        /// Reserves the given type name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ReserveName(string name)
        {
            return dynamicTypes.TryAdd(name, null);
        }

        /// <summary>
        /// Associated the given Java type with the given type name, ensuring the actual stored name is unique.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="name"></param>
        /// <param name="javaType"></param>
        /// <returns></returns>
        public static string TypeNameMangleImpl(ConcurrentDictionary<string, RuntimeJavaType> dict, string name, RuntimeJavaType javaType)
        {
            // the CLR maximum type name length is 1023 characters, but we need to leave some room for the suffix that we may need to append to make the name unique
            const int MaxLength = 1000;

            if (name.Length > MaxLength)
                name = name.Substring(0, MaxLength) + "/truncated";

            var mangledTypeName = TypeNameUtil.ReplaceIllegalCharacters(name);
            var baseTypeName = mangledTypeName;
            int instanceId = 0;

            // advance through unique type names until we find a free one
            while (dict.TryAdd(mangledTypeName, javaType) == false)
            {
                javaType.ClassLoader.Diagnostics.GenericCompilerWarning($"Class name clash: {mangledTypeName}");
                mangledTypeName = baseTypeName + "/" + (++instanceId);
            }

            return mangledTypeName;
        }

    }

}
