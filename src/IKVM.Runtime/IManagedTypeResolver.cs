using System;

#if IMPORTER || EXPORTER
using IKVM.Reflection;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
#endif

namespace IKVM.Runtime
{

    /// <summary>
    /// Provides an interface to resolve a managed type.
    /// </summary>
    public interface IManagedTypeResolver
    {

        /// <summary>
        /// Resolves the named type from the IKVM runtime assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        Type ResolveRuntimeType(string typeName);

        /// <summary>
        /// Resolves the named assembly from any reference source.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        Assembly ResolveAssembly(string assemblyName);

        /// <summary>
        /// Resolves the named type from any reference source.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        Type ResolveCoreType(string typeName);

        /// <summary>
        /// Resolves the known Java base assembly.
        /// </summary>
        /// <returns></returns>
        Assembly ResolveBaseAssembly();

    }

}
