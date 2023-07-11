using System;
using System.Collections.Immutable;
using System.Reflection.Metadata;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Decodes signature information given the specified context.
    /// </summary>
    class MetadataSignatureTypeProvider : ISignatureTypeProvider<ManagedSignature, MetadataGenericContext>
    {

        readonly MetadataAssemblyContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public MetadataSignatureTypeProvider(MetadataAssemblyContext context)
        {
            this.context = context;
        }

        public ManagedSignature GetArrayType(ManagedSignature elementType, ArrayShape shape)
        {
            return elementType.CreateArray(shape.Rank, shape.Sizes, shape.LowerBounds);
        }

        public ManagedSignature GetByReferenceType(ManagedSignature elementType)
        {
            return elementType.CreateByRef();
        }

        public ManagedSignature GetFunctionPointerType(MethodSignature<ManagedSignature> signature)
        {
            return ManagedSignature.FunctionPointer(signature.ReturnType, signature.ParameterTypes);
        }

        public ManagedSignature GetGenericInstantiation(ManagedSignature genericType, ImmutableArray<ManagedSignature> typeArguments)
        {
            return genericType.CreateGeneric(typeArguments);
        }

        public ManagedSignature GetGenericTypeParameter(MetadataGenericContext genericContext, int index)
        {
            return new ManagedGenericTypeParameterSignature(genericContext.TypeParameters[index]);
        }

        public ManagedSignature GetGenericMethodParameter(MetadataGenericContext genericContext, int index)
        {
            if (genericContext.MethodParameters == null)
                throw new ManagedTypeException("No generic method context.");

            return new ManagedGenericMethodParameterSignature(genericContext.MethodParameters.Value[index]);
        }

        public ManagedSignature GetModifiedType(ManagedSignature modifier, ManagedSignature unmodifiedType, bool isRequired)
        {
            return unmodifiedType.CreateModified(modifier, isRequired);
        }

        public ManagedSignature GetPinnedType(ManagedSignature elementType)
        {
            throw new System.NotImplementedException();
        }

        public ManagedSignature GetPointerType(ManagedSignature elementType)
        {
            return elementType.CreatePointer();
        }

        public ManagedSignature GetPrimitiveType(PrimitiveTypeCode typeCode) => typeCode switch
        {
            PrimitiveTypeCode.Boolean => ManagedSignature.Boolean,
            PrimitiveTypeCode.Byte => ManagedSignature.Byte,
            PrimitiveTypeCode.SByte => ManagedSignature.SByte,
            PrimitiveTypeCode.Char => ManagedSignature.Char,
            PrimitiveTypeCode.Int16 => ManagedSignature.Int16,
            PrimitiveTypeCode.UInt16 => ManagedSignature.UInt16,
            PrimitiveTypeCode.Int32 => ManagedSignature.Int32,
            PrimitiveTypeCode.UInt32 => ManagedSignature.UInt32,
            PrimitiveTypeCode.Int64 => ManagedSignature.Int64,
            PrimitiveTypeCode.UInt64 => ManagedSignature.UInt64,
            PrimitiveTypeCode.Single => ManagedSignature.Single,
            PrimitiveTypeCode.Double => ManagedSignature.Double,
            PrimitiveTypeCode.IntPtr => ManagedSignature.IntPtr,
            PrimitiveTypeCode.UIntPtr => ManagedSignature.UIntPtr,
            PrimitiveTypeCode.Object => ManagedSignature.Object,
            PrimitiveTypeCode.String => ManagedSignature.String,
            PrimitiveTypeCode.TypedReference => ManagedSignature.TypedReference,
            PrimitiveTypeCode.Void => ManagedSignature.Void,
            _ => throw new ManagedTypeException(),
        };

        public ManagedSignature GetSZArrayType(ManagedSignature elementType)
        {
            return elementType.CreateArray();
        }

        public ManagedSignature GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
        {
            var typeReference = context.ResolveTypeReference(reader, handle, MetadataGenericContext.Empty);
            return ManagedSignature.Type(typeReference);
        }

        public ManagedSignature GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
        {
            var typeReference = context.ResolveTypeReference(reader, handle, MetadataGenericContext.Empty);
            return ManagedSignature.Type(typeReference);
        }

        public ManagedSignature GetTypeFromSpecification(MetadataReader reader, MetadataGenericContext genericContext, TypeSpecificationHandle handle, byte rawTypeKind)
        {
            throw new NotImplementedException();
        }

    }

}
