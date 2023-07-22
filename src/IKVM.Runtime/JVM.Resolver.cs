using System;
using System.Reflection;

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
            public Type ResolveType(string typeName)
            {
                return typeof(object).Assembly.GetType(typeName);
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
