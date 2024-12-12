using System;
using System.Collections.Immutable;
using System.Security.Cryptography;

namespace IKVM.CoreLib.Symbols
{

    abstract class ModuleDefinition
    {

        /// <summary>
        /// Gets the fully qualified name of this module.
        /// </summary>
        /// <returns></returns>
        public abstract string GetFullyQualifiedName();

        /// <summary>
        /// Gets the scope name of this module.
        /// </summary>
        /// <returns></returns>
        public abstract string GetScopeName();

        /// <summary>
        /// Gets the MVID of this module.
        /// </summary>
        /// <returns></returns>
        public abstract Guid GetModuleVersionId();

        /// <summary>
        /// Gets whether this module is a resource module.
        /// </summary>
        /// <returns></returns>
        public abstract bool GetIsResource();

        /// <summary>
        /// Gets the fields declared of the module.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<FieldDefinition> GetFields();

        /// <summary>
        /// Gets the methods declared on the module.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<MethodDefinition> GetMethods();

        /// <summary>
        /// Gets the types declared on the module.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeDefinition> GetTypes();

        /// <summary>
        /// Gets the types declared on the module.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<CustomAttribute> GetCustomAttributes();

        /// <summary>
        /// Attempts to resolve the definition for the specified field.
        /// </summary>
        /// <param name="name"></param>
        internal abstract FieldDefinition? ResolveFieldDef(string name);

        /// <summary>
        /// Attempts to resolve the definition for the specified method.
        /// </summary>
        /// <param name="signature"></param>
        internal abstract MethodDefinition? ResolveMethodDef(MethodSymbolSignature signature);

        /// <summary>
        /// Attempts to resolve the definition for the specified type.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="name"></param>
        internal abstract TypeDefinition? ResolveTypeDef(string ns, string name);

    }

}