using System.Reflection;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// A discriminated union containing the fields required to store a single code.
    /// </summary>
    readonly struct ManagedSignatureCodeData
    {

        public readonly ManagedSignatureCodeKind Kind;
        public readonly IManagedAssemblyContext? Type_Context;
        public readonly AssemblyName? Type_AssemblyName;
        public readonly string? Type_TypeName;
        public readonly ManagedType? Type_Cache;
        public readonly ManagedArrayShape? Array_Shape;
        public readonly ManagedGenericTypeParameterRef? GenericTypeParameter_Parameter;
        public readonly ManagedGenericMethodParameterRef? GenericMethodParameter_Parameter;
        public readonly bool? Modified_Required;
        public readonly ManagedPrimitiveType? Primitive_Type;

        /// <summary>
        /// Initializes a new instance for a Type kind.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        /// <param name="typeName"></param>
        /// <param name="type"></param>
        public ManagedSignatureCodeData(ManagedSignatureCodeKind kind, IManagedAssemblyContext context, AssemblyName assembly, string typeName, ManagedType? type)
        {
            Kind = kind;
            Type_Context = context;
            Type_AssemblyName = assembly;
            Type_TypeName = typeName;
            Type_Cache = type;
        }

        /// <summary>
        /// Initializes a new instance for an Array kind.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="shape"></param>
        public ManagedSignatureCodeData(ManagedSignatureCodeKind kind, ManagedArrayShape shape)
        {
            Kind = kind;
            Array_Shape = shape;
        }

        /// <summary>
        /// Initializes a new instance for a GenericTypeParameter kind.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="parameter"></param>
        public ManagedSignatureCodeData(ManagedSignatureCodeKind kind, ManagedGenericTypeParameterRef parameter)
        {
            Kind = kind;
            GenericTypeParameter_Parameter = parameter;
        }

        /// <summary>
        /// Initializes a new instance for a GenericMethodParameter kind.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="parameter"></param>
        public ManagedSignatureCodeData(ManagedSignatureCodeKind kind, ManagedGenericMethodParameterRef parameter)
        {
            Kind = kind;
            GenericMethodParameter_Parameter = parameter;
        }

        /// <summary>
        /// Initializes a new instance for a Modified kind.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="required"></param>
        public ManagedSignatureCodeData(ManagedSignatureCodeKind kind, bool required)
        {
            Kind = kind;
            Modified_Required = required;
        }

        /// <summary>
        /// Initializes a new instance for a PrimitiveType kind.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="type"></param>
        public ManagedSignatureCodeData(ManagedSignatureCodeKind kind, ManagedPrimitiveType type)
        {
            Kind = kind;
            Primitive_Type = type;
        }

    }

}
