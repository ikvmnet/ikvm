using System.Collections.Immutable;
using System.Reflection.Metadata;

namespace IKVM.Compiler.Managed.Metadata
{

    class MetadataSignatureTypeProvider : ISignatureTypeProvider<ManagedTypeSignature, object>
    {

        public ManagedTypeSignature GetPrimitiveType(PrimitiveTypeCode typeCode)
        {
            throw new System.NotImplementedException();
        }

        public ManagedTypeSignature GetArrayType(ManagedTypeSignature elementType, ArrayShape shape)
        {
            throw new System.NotImplementedException();
        }

        public ManagedTypeSignature GetByReferenceType(ManagedTypeSignature elementType)
        {
            throw new System.NotImplementedException();
        }

        public ManagedTypeSignature GetFunctionPointerType(MethodSignature<ManagedTypeSignature> signature)
        {
            throw new System.NotImplementedException();
        }

        public ManagedTypeSignature GetGenericInstantiation(ManagedTypeSignature genericType, ImmutableArray<ManagedTypeSignature> typeArguments)
        {
            throw new System.NotImplementedException();
        }

        public ManagedTypeSignature GetGenericMethodParameter(object genericContext, int index)
        {
            throw new System.NotImplementedException();
        }

        public ManagedTypeSignature GetGenericTypeParameter(object genericContext, int index)
        {
            throw new System.NotImplementedException();
        }

        public ManagedTypeSignature GetModifiedType(ManagedTypeSignature modifier, ManagedTypeSignature unmodifiedType, bool isRequired)
        {
            throw new System.NotImplementedException();
        }

        public ManagedTypeSignature GetPinnedType(ManagedTypeSignature elementType)
        {
            throw new System.NotImplementedException();
        }

        public ManagedTypeSignature GetPointerType(ManagedTypeSignature elementType)
        {
            throw new System.NotImplementedException();
        }

        public ManagedTypeSignature GetSZArrayType(ManagedTypeSignature elementType)
        {
            throw new System.NotImplementedException();
        }

        public ManagedTypeSignature GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
        {
            throw new System.NotImplementedException();
        }

        public ManagedTypeSignature GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
        {
            throw new System.NotImplementedException();
        }

        public ManagedTypeSignature GetTypeFromSpecification(MetadataReader reader, object genericContext, TypeSpecificationHandle handle, byte rawTypeKind)
        {
            throw new System.NotImplementedException();
        }

    }

}
