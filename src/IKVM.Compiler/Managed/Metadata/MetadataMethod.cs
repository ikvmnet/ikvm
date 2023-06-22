using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Represents a method from metadata.
    /// </summary>
    internal sealed class MetadataMethod : MetadataMember, IManagedMethod
    {

        readonly MethodDefinitionHandle handle;
        readonly ReadOnlyListMap<MetadataCustomAttribute, CustomAttributeHandle> customAttributes;
        readonly ReadOnlyListMap<MetadataParameter, ParameterHandle> parameters;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="handle"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal MetadataMethod(MetadataType declaringType, MethodDefinitionHandle handle) :
            base(declaringType)
        {
            this.handle = handle;

            customAttributes = new ReadOnlyListMap<MetadataCustomAttribute, CustomAttributeHandle>(new ReadOnlyCollectionList<CustomAttributeHandle>(Data.GetCustomAttributes()), (a, i) => new MetadataCustomAttribute(this, a));
            parameters = new ReadOnlyListMap<MetadataParameter, ParameterHandle>(new ReadOnlyCollectionList<ParameterHandle>(Data.GetParameters()), (p, i) => new MetadataParameter(this, p));
        }

        MethodDefinition Data => Context.Reader.GetMethodDefinition(handle);

        MethodSignature<ManagedTypeSignature> Signature => Data.DecodeSignature(Context.SignatureTypeProvider, declaringType);

        /// <inheritdoc />
        public override string Name => Context.Reader.GetString(Data.Name);

        /// <inheritdoc />
        public ManagedTypeSignature ReturnType => Signature.ReturnType;

        /// <inheritdoc />
        public IReadOnlyList<IManagedParameter> Parameters => parameters;

        /// <inheritdoc />
        public MethodAttributes Attributes => Data.Attributes;

        /// <inheritdoc />
        public override IReadOnlyList<IManagedCustomAttribute> CustomAttributes => customAttributes;

    }

}
