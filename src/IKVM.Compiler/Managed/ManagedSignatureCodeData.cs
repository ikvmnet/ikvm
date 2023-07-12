using System.Runtime.InteropServices;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// A discriminated union containing the fields required to store a single code.
    /// </summary>
    internal readonly struct ManagedSignatureCodeData
    {

        /// <summary>
        /// Merges the optional unmanaged fields.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        internal struct Union
        {

            [FieldOffset(0)]
            public readonly ManagedArrayShape Array_Shape;

            [FieldOffset(0)]
            public readonly ManagedGenericTypeParameterRef GenericTypeParameter_Parameter;

            [FieldOffset(0)]
            public readonly ManagedGenericMethodParameterRef GenericMethodParameter_Parameter;

            [FieldOffset(0)]
            public readonly bool Modified_Required;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="arrayShape"></param>
            public Union(ManagedArrayShape arrayShape)
            {
                Array_Shape = arrayShape;
            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="genericTypeParameter"></param>
            public Union(ManagedGenericTypeParameterRef genericTypeParameter)
            {
                GenericTypeParameter_Parameter = genericTypeParameter;
            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="genericMethodParameter"></param>
            public Union(ManagedGenericMethodParameterRef genericMethodParameter)
            {
                GenericMethodParameter_Parameter = genericMethodParameter;
            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="required"></param>
            public Union(bool required)
            {
                Modified_Required = required;
            }

        }

        public readonly ManagedSignatureKind Kind;
        public readonly ManagedType? Type;
        public readonly Union Data;

        /// <summary>
        /// Initializes a new instance for a kind with no data.
        /// </summary>
        /// <param name="kind"></param>
        public ManagedSignatureCodeData(ManagedSignatureKind kind)
        {
            Kind = kind;
        }

        /// <summary>
        /// Initializes a new instance for a Type kind.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="type"></param>
        public ManagedSignatureCodeData(ManagedSignatureKind kind, ManagedType type)
        {
            Kind = kind;
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance for an Array kind.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="shape"></param>
        public ManagedSignatureCodeData(ManagedSignatureKind kind, ManagedArrayShape shape)
        {
            Kind = kind;
            Data = new Union(shape);
        }

        /// <summary>
        /// Initializes a new instance for a GenericTypeParameter kind.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="parameter"></param>
        public ManagedSignatureCodeData(ManagedSignatureKind kind, ManagedGenericTypeParameterRef parameter)
        {
            Kind = kind;
            Data = new Union(parameter);
        }

        /// <summary>
        /// Initializes a new instance for a GenericMethodParameter kind.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="parameter"></param>
        public ManagedSignatureCodeData(ManagedSignatureKind kind, ManagedGenericMethodParameterRef parameter)
        {
            Kind = kind;
            Data = new Union(parameter);
        }

        /// <summary>
        /// Initializes a new instance for a Modified kind.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="required"></param>
        public ManagedSignatureCodeData(ManagedSignatureKind kind, bool required)
        {
            Kind = kind;
            Data = new Union(required);
        }

    }

}
