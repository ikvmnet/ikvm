using System;
using System.Collections.Immutable;
using System.Reflection.Metadata;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Decodes signature information given the specified context.
    /// </summary>
    class MetadataSignatureTypeProvider : ISignatureTypeProvider<ManagedTypeSignature, MetadataGenericContext>
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

        public ManagedTypeSignature GetArrayType(ManagedTypeSignature elementType, ArrayShape shape)
        {
            return elementType.Array(shape.Rank, shape.Sizes, shape.LowerBounds);
        }

        public ManagedTypeSignature GetByReferenceType(ManagedTypeSignature elementType)
        {
            return elementType.ByRef();
        }

        public ManagedTypeSignature GetFunctionPointerType(MethodSignature<ManagedTypeSignature> signature)
        {
            var parameterTypes = new FixedValueList<ManagedTypeSignature>(signature.ParameterTypes.Length);
            for (int i = 0; i < signature.ParameterTypes.Length; i++)
                parameterTypes[i] = signature.ParameterTypes[i];

            return new ManagedFunctionPointerSignature(parameterTypes.AsReadOnly(), signature.ReturnType);
        }

        public ManagedTypeSignature GetGenericInstantiation(ManagedTypeSignature genericType, ImmutableArray<ManagedTypeSignature> typeArguments)
        {
            return genericType.Generic(typeArguments);
        }

        public ManagedTypeSignature GetGenericTypeParameter(MetadataGenericContext genericContext, int index)
        {
            return new ManagedGenericTypeParameterTypeSignature(genericContext.TypeParameters[index]);
        }

        public ManagedTypeSignature GetGenericMethodParameter(MetadataGenericContext genericContext, int index)
        {
            if (genericContext.MethodParameters == null)
                throw new ManagedTypeException("No generic method context.");

            return new ManagedGenericMethodParameterTypeSignature(genericContext.MethodParameters.Value[index]);
        }

        public ManagedTypeSignature GetModifiedType(ManagedTypeSignature modifier, ManagedTypeSignature unmodifiedType, bool isRequired)
        {
            return new ManagedModifiedTypeSignature(unmodifiedType, modifier, isRequired);
        }

        public ManagedTypeSignature GetPinnedType(ManagedTypeSignature elementType)
        {
            throw new System.NotImplementedException();
        }

        public ManagedTypeSignature GetPointerType(ManagedTypeSignature elementType)
        {
            return new ManagedPointerTypeSignature(elementType);
        }

        public ManagedTypeSignature GetPrimitiveType(PrimitiveTypeCode typeCode) => typeCode switch
        {
            PrimitiveTypeCode.Boolean => ManagedTypeSignature.Boolean,
            PrimitiveTypeCode.Byte => ManagedTypeSignature.Byte,
            PrimitiveTypeCode.SByte => ManagedTypeSignature.SByte,
            PrimitiveTypeCode.Char => ManagedTypeSignature.Char,
            PrimitiveTypeCode.Int16 => ManagedTypeSignature.Int16,
            PrimitiveTypeCode.UInt16 => ManagedTypeSignature.UInt16,
            PrimitiveTypeCode.Int32 => ManagedTypeSignature.Int32,
            PrimitiveTypeCode.UInt32 => ManagedTypeSignature.UInt32,
            PrimitiveTypeCode.Int64 => ManagedTypeSignature.Int64,
            PrimitiveTypeCode.UInt64 => ManagedTypeSignature.UInt64,
            PrimitiveTypeCode.Single => ManagedTypeSignature.Single,
            PrimitiveTypeCode.Double => ManagedTypeSignature.Double,
            PrimitiveTypeCode.IntPtr => ManagedTypeSignature.IntPtr,
            PrimitiveTypeCode.UIntPtr => ManagedTypeSignature.UIntPtr,
            PrimitiveTypeCode.Object => ManagedTypeSignature.Object,
            PrimitiveTypeCode.String => ManagedTypeSignature.String,
            PrimitiveTypeCode.TypedReference => ManagedTypeSignature.TypedReference,
            PrimitiveTypeCode.Void => ManagedTypeSignature.Void,
            _ => throw new ManagedTypeException(),
        };

        public ManagedTypeSignature GetSZArrayType(ManagedTypeSignature elementType)
        {
            return elementType.Array();
        }

        public ManagedTypeSignature GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
        {
            var typeReference = context.ResolveTypeReference(reader, handle, MetadataGenericContext.Empty);
            return new ManagedTypeRefSignature(typeReference);
        }

        public ManagedTypeSignature GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
        {
            var typeReference = context.ResolveTypeReference(reader, handle, MetadataGenericContext.Empty);
            return new ManagedTypeRefSignature(typeReference);
        }

        public ManagedTypeSignature GetTypeFromSpecification(MetadataReader reader, MetadataGenericContext genericContext, TypeSpecificationHandle handle, byte rawTypeKind)
        {
            throw new NotImplementedException();
        }

    }

}
