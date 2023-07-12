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
        readonly string coreAssemblyName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public MetadataSignatureTypeProvider(MetadataAssemblyContext context, string coreAssemblyName)
        {
            this.context = context;
            this.coreAssemblyName = coreAssemblyName;
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
                throw new ManagedException("No generic method context.");

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

        public ManagedSignature GetPrimitiveType(PrimitiveTypeCode typeCode)
        {
            var assembly = context.ResolveAssembly(coreAssemblyName);
            if (assembly == null)
                throw new ManagedResolveException("Cannot resolve core assembly.");

            var typeName = GetPrimitiveTypeName(typeCode);
            var type = assembly.ResolveType(typeName);
            if (type == null)
                throw new ManagedResolveException($"Cannot resolve primitive type '{typeName}'.");

            return ManagedSignature.Type(type);
        }

        string GetPrimitiveTypeName(PrimitiveTypeCode typeCode) => typeCode switch
        {
            PrimitiveTypeCode.Boolean => "System.Boolean",
            PrimitiveTypeCode.Byte => "System.Byte",
            PrimitiveTypeCode.SByte => "System.SByte",
            PrimitiveTypeCode.Char => "System.Char",
            PrimitiveTypeCode.Int16 => "System.Int16",
            PrimitiveTypeCode.UInt16 => "System.UInt16",
            PrimitiveTypeCode.Int32 => "System.Int32",
            PrimitiveTypeCode.UInt32 => "System.UInt32",
            PrimitiveTypeCode.Int64 => "System.Int64",
            PrimitiveTypeCode.UInt64 => "System.UInt64",
            PrimitiveTypeCode.Single => "System.Single",
            PrimitiveTypeCode.Double => "System.Double",
            PrimitiveTypeCode.IntPtr => "System.IntPtr",
            PrimitiveTypeCode.UIntPtr => "System.UIntPtr",
            PrimitiveTypeCode.Object => "System.Object",
            PrimitiveTypeCode.String => "System.String",
            PrimitiveTypeCode.TypedReference => "System.TypedReference",
            PrimitiveTypeCode.Void => "System.Void",
            _ => throw new ManagedException(),
        };

        public ManagedSignature GetSZArrayType(ManagedSignature elementType)
        {
            return elementType.CreateArray();
        }

        public ManagedSignature GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
        {
            var typeReference = context.ResolveType(reader, handle, MetadataGenericContext.Empty);
            return ManagedSignature.Type(typeReference);
        }

        public ManagedSignature GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
        {
            var typeReference = context.ResolveType(reader, handle, MetadataGenericContext.Empty);
            return ManagedSignature.Type(typeReference);
        }

        public ManagedSignature GetTypeFromSpecification(MetadataReader reader, MetadataGenericContext genericContext, TypeSpecificationHandle handle, byte rawTypeKind)
        {
            throw new NotImplementedException();
        }

    }

}
