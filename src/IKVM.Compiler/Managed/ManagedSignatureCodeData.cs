namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// A discriminated union containing the fields required to store a single code.
    /// </summary>
    internal readonly struct ManagedSignatureCodeData 
    {

        public readonly ManagedSignatureKind Kind;
        public readonly ManagedTypeRef? Type_Type;
        public readonly ManagedArrayShape? Array_Shape;
        public readonly ManagedGenericTypeParameterRef? GenericTypeParameter_Parameter;
        public readonly ManagedGenericMethodParameterRef? GenericMethodParameter_Parameter;
        public readonly bool? Modified_Required;
        public readonly ManagedPrimitiveTypeCode? Primitive_TypeCode;

        /// <summary>
        /// Initializes a new instance for a Type kind.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="type"></param>
        public ManagedSignatureCodeData(ManagedSignatureKind kind, ManagedTypeRef type)
        {
            Kind = kind;
            Type_Type = type;
        }

        /// <summary>
        /// Initializes a new instance for an Array kind.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="shape"></param>
        public ManagedSignatureCodeData(ManagedSignatureKind kind, ManagedArrayShape shape)
        {
            Kind = kind;
            Array_Shape = shape;
        }

        /// <summary>
        /// Initializes a new instance for a GenericTypeParameter kind.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="parameter"></param>
        public ManagedSignatureCodeData(ManagedSignatureKind kind, ManagedGenericTypeParameterRef parameter)
        {
            Kind = kind;
            GenericTypeParameter_Parameter = parameter;
        }

        /// <summary>
        /// Initializes a new instance for a GenericMethodParameter kind.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="parameter"></param>
        public ManagedSignatureCodeData(ManagedSignatureKind kind, ManagedGenericMethodParameterRef parameter)
        {
            Kind = kind;
            GenericMethodParameter_Parameter = parameter;
        }

        /// <summary>
        /// Initializes a new instance for a Modified kind.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="required"></param>
        public ManagedSignatureCodeData(ManagedSignatureKind kind, bool required)
        {
            Kind = kind;
            Modified_Required = required;
        }

        /// <summary>
        /// Initializes a new instance for a PrimitiveType kind.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="type"></param>
        public ManagedSignatureCodeData(ManagedSignatureKind kind, ManagedPrimitiveTypeCode type)
        {
            Kind = kind;
            Primitive_TypeCode = type;
        }

        /// <summary>
        /// Initializes a new instance for a kind with no data.
        /// </summary>
        /// <param name="kind"></param>
        public ManagedSignatureCodeData(ManagedSignatureKind kind)
        {
            Kind = kind;
        }

    }

}
