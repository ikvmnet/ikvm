using System.Collections.Immutable;
using System.Reflection.Metadata;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Decodes signature information given the specified context.
    /// </summary>
    class MetadataSignatureTypeProvider : ISignatureTypeProvider<ManagedTypeSignature, IMetadataGenericTypeContext>
    {

        readonly MetadataContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public MetadataSignatureTypeProvider(MetadataContext context)
        {
            this.context = context;
        }

        public ManagedTypeSignature GetArrayType(ManagedTypeSignature elementType, ArrayShape shape)
        {
            return new ManagedArrayTypeSignature(elementType, shape.Rank);
        }

        public ManagedTypeSignature GetByReferenceType(ManagedTypeSignature elementType)
        {
            return new ManagedByRefTypeSignature(elementType);
        }

        public ManagedTypeSignature GetFunctionPointerType(MethodSignature<ManagedTypeSignature> signature)
        {
            throw new System.NotImplementedException();
        }

        public ManagedTypeSignature GetGenericInstantiation(ManagedTypeSignature genericType, ImmutableArray<ManagedTypeSignature> typeArguments)
        {
            throw new System.NotImplementedException();
        }

        public ManagedTypeSignature GetGenericMethodParameter(IMetadataGenericTypeContext genericContext, int index)
        {
            throw new System.NotImplementedException();
        }

        public ManagedTypeSignature GetGenericTypeParameter(IMetadataGenericTypeContext genericContext, int index)
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
            return new ManagedPointerTypeSignature(elementType);
        }

        public ManagedTypeSignature GetPrimitiveType(PrimitiveTypeCode typeCode)
        {
            return typeCode switch
            {
                PrimitiveTypeCode.Boolean => throw new System.NotImplementedException(),
                PrimitiveTypeCode.Byte => throw new System.NotImplementedException(),
                PrimitiveTypeCode.SByte => throw new System.NotImplementedException(),
                PrimitiveTypeCode.Char => throw new System.NotImplementedException(),
                PrimitiveTypeCode.Int16 => throw new System.NotImplementedException(),
                PrimitiveTypeCode.UInt16 => throw new System.NotImplementedException(),
                PrimitiveTypeCode.Int32 => throw new System.NotImplementedException(),
                PrimitiveTypeCode.UInt32 => throw new System.NotImplementedException(),
                PrimitiveTypeCode.Int64 => throw new System.NotImplementedException(),
                PrimitiveTypeCode.UInt64 => throw new System.NotImplementedException(),
                PrimitiveTypeCode.Single => throw new System.NotImplementedException(),
                PrimitiveTypeCode.Double => throw new System.NotImplementedException(),
                PrimitiveTypeCode.IntPtr => throw new System.NotImplementedException(),
                PrimitiveTypeCode.UIntPtr => throw new System.NotImplementedException(),
                PrimitiveTypeCode.Object => throw new System.NotImplementedException(),
                PrimitiveTypeCode.String => throw new System.NotImplementedException(),
                PrimitiveTypeCode.TypedReference => throw new System.NotImplementedException(),
                PrimitiveTypeCode.Void => throw new System.NotImplementedException(),
                _ => throw new System.NotImplementedException(),
            };
        }

        public ManagedTypeSignature GetSZArrayType(ManagedTypeSignature elementType)
        {
            throw new System.NotImplementedException();
        }

        public ManagedTypeSignature GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
        {
            return new ManagedTypeSignature(context.Resolve(handle));
        }

        public ManagedTypeSignature GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
        {
            return new ManagedTypeSignature(context.Resolve(handle));
        }

        public ManagedTypeSignature GetTypeFromSpecification(MetadataReader reader, IMetadataGenericTypeContext genericContext, TypeSpecificationHandle handle, byte rawTypeKind)
        {
            return new ManagedTypeSignature(context.Resolve(handle));
        }

    }

}
