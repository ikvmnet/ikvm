using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace IKVM.Runtime
{

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

    static partial class JVM
    {

        /// <summary>
        /// Provides support for resolving managed types from the current JVM environment.
        /// </summary>
        public class Resolver : IManagedTypeResolver
        {

            /// <summary>
            /// Gets the set of assemblies from which to load core types.
            /// </summary>
            /// <returns></returns>
            static IEnumerable<Assembly> GetCoreAssemblies()
            {
                yield return typeof(object).Assembly;
                yield return typeof(RuntimeCompatibilityAttribute).Assembly;
                yield return typeof(Console).Assembly;
                yield return typeof(System.Threading.Interlocked).Assembly;
                yield return typeof(EditorBrowsableAttribute).Assembly;
                yield return typeof(System.Collections.IEnumerable).Assembly;
                yield return typeof(IEnumerable<>).Assembly;
                yield return typeof(Environment).Assembly;
            }

            readonly Assembly[] coreAssemblies = GetCoreAssemblies().Distinct().ToArray();
            readonly ConcurrentDictionary<string, Type> typeCache = new();


            /// <inheritdoc />
            public Assembly ResolveAssembly(string assemblyName)
            {
                return Assembly.Load(assemblyName);
            }

            /// <inheritdoc />
            public Assembly ResolveBaseAssembly()
            {
                return typeof(java.lang.Object).Assembly;
            }

            /// <inheritdoc />
            public Type ResolveCoreType(string typeName)
            {
                return typeCache.GetOrAdd(typeName, ResolveCoreTypeImpl);
            }

            Type ResolveCoreTypeImpl(string typeName)
            {
                // loop over core assemblies searching for type
                foreach (var assembly in coreAssemblies)
                    if (assembly.GetType(typeName) is Type t)
                        return t;

                return null;
            }

            /// <inheritdoc />
            public Type ResolveRuntimeType(string typeName)
            {
                return typeof(JVM).Assembly.GetType(typeName);
            }

        }

    }

#endif

}
