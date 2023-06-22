using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Maintains a context that loaded a particular set of metadata items.
    /// </summary>
    internal class MetadataContext
    {

        readonly MetadataReader reader;
        readonly IManagedTypeResolver resolver;

        readonly MetadataAssembly assembly;
        readonly MetadataSignatureTypeProvider signatureTypeProvider;
        readonly ConcurrentDictionary<TypeDefinitionHandle, MetadataType> types = new ConcurrentDictionary<TypeDefinitionHandle, MetadataType>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="resolver"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MetadataContext(MetadataReader reader, IManagedTypeResolver resolver)
        {
            this.resolver = resolver;
            this.reader = reader;

            assembly = new MetadataAssembly(this, reader.GetAssemblyDefinition());
            signatureTypeProvider = new MetadataSignatureTypeProvider(this);
        }

        /// <summary>
        /// Gets the <see cref="MetadataReader"/> associated with the context.
        /// </summary>
        public MetadataReader Reader => reader;

        /// <summary>
        /// Gets the assembly that defines this context.
        /// </summary>
        public MetadataAssembly Assembly => assembly;

        /// <summary>
        /// Gets the <see cref="ISignatureTypeProvider{TType, TGenericContext}"/> for the current context.
        /// </summary>
        public MetadataSignatureTypeProvider SignatureTypeProvider => signatureTypeProvider;

        /// <summary>
        /// Attempts to resolve the type handle into a <see cref="MetadataType"/> within this assembly.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal MetadataType ResolveType(TypeDefinitionHandle handle) => handle.IsNil == false ? types.GetOrAdd(handle, h => new MetadataType(this, h)) : throw new ArgumentNullException();

        /// <summary>
        /// Attempts to resolve the type reference. Type may be within the current type, module, assembly, or external.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal IManagedType ResolveType(TypeReferenceHandle handle)
        {
            if (handle.IsNil)
                throw new ArgumentNullException();

            var type = reader.GetTypeReference(handle);

            // reference should be resolved within the declaring type
            if (type.ResolutionScope.Kind == HandleKind.TypeReference)
            {
                // resolve declaring type
                var declaringType = ResolveType((TypeReferenceHandle)type.ResolutionScope);
                if (declaringType == null)
                    throw new InvalidOperationException();

                // scan nested types for type with specified name
                var name = GetTypeName(type);
                return declaringType.NestedTypes.FirstOrDefault(i => i.Name == name);
            }

            // reference should be resolved within the specified module
            if (type.ResolutionScope.Kind == HandleKind.ModuleReference)
                throw new NotImplementedException();

            // reference should be resolved within the specified module
            if (type.ResolutionScope.Kind == HandleKind.ModuleDefinition)
                throw new NotImplementedException();

            // reference should be resolved in global scope, it refers to another assembly
            if (type.ResolutionScope.Kind == HandleKind.AssemblyReference)
            {
                // resolution requires assembly information
                var assemblyReference = reader.GetAssemblyReference((AssemblyReferenceHandle)type.ResolutionScope);
                var assemblyName = assemblyReference.GetAssemblyName();
                return resolver.Resolve(assemblyName, GetTypeName(type));
            }

            throw new ArgumentException("Could not resolve type reference handle.");
        }

        /// <summary>
        /// Generates a full type name string for the given type reference.
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        string GetTypeName(TypeReference reference)
        {
            var f = new StringBuilder(reference.Namespace.IsNil ? "" : reader.GetString(reference.Namespace), 32);
            if (f.Length > 0)
                f.Append(".");
            f.Append(reader.GetString(reference.Name));
            return f.ToString();
        }

    }

}
