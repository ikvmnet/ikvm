using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed method.
    /// </summary>
    internal struct ManagedMethodData
    {

        /// <summary>
        /// Gets a reference to a nil version of the type.
        /// </summary>
        public static readonly ManagedMethodData Nil = new ManagedMethodData(true);

        /// <summary>
        /// Gest whether or not the value is null.
        /// </summary>
        public readonly bool IsNil;

        /// <summary>
        /// Gets the name of the managed method.
        /// </summary>
        public string? Name;

        /// <summary>
        /// Gets the attributes of the method.
        /// </summary>
        public MethodAttributes Attributes;

        /// <summary>
        /// Gets the implementation attributes of the method.
        /// </summary>
        public MethodImplAttributes ImplAttributes;

        /// <summary>
        /// Gets the set of generic parameters on the method.
        /// </summary>
        public FixedValueList1<ManagedGenericParameter> GenericParameters;

        /// <summary>
        /// Gets the set of custom attributes applied to the method.
        /// </summary>
        public FixedValueList1<ManagedCustomAttributeData> CustomAttributes;

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        public ManagedSignature ReturnType;

        /// <summary>
        /// Gets the set of parameters of the method.
        /// </summary>
        public FixedValueList4<ManagedParameterData> Parameters;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="isNil"></param>
        public ManagedMethodData(bool isNil)
        {
            IsNil = isNil;
        }

    }

}
