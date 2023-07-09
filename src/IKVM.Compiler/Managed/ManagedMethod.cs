using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed method.
    /// </summary>
    public readonly struct ManagedMethod
    {

        /// <summary>
        /// Gets a reference to a nil version of the type.
        /// </summary>
        public static readonly ManagedMethod Nil = new ManagedMethod();

        /// <summary>
        /// Gest whether or not the value is null.
        /// </summary>
        public readonly bool IsNil = true;

        /// <summary>
        /// Gets the name of the managed method.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Gets the attributes of the method.
        /// </summary>
        public readonly MethodAttributes Attributes;

        /// <summary>
        /// Gets the implementation attributes of the method.
        /// </summary>
        public readonly MethodImplAttributes ImplAttributes;

        /// <summary>
        /// Gets the set of generic parameters on the method.
        /// </summary>
        public readonly ReadOnlyFixedValueList1<ManagedGenericParameter> GenericParameters;

        /// <summary>
        /// Gets the set of custom attributes applied to the method.
        /// </summary>
        public readonly ReadOnlyFixedValueList1<ManagedCustomAttribute> CustomAttributes;

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        public readonly ManagedSignature ReturnType;

        /// <summary>
        /// Gets the set of parameters of the method.
        /// </summary>
        public readonly ReadOnlyFixedValueList4<ManagedParameter> Parameters;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="implAttributes"></param>
        /// <param name="genericParameters"></param>
        /// <param name="customAttributes"></param>
        /// <param name="returnType"></param>
        /// <param name="parameters"></param>
        public ManagedMethod(string name, MethodAttributes attributes, MethodImplAttributes implAttributes, in ReadOnlyFixedValueList1<ManagedGenericParameter> genericParameters, in ReadOnlyFixedValueList1<ManagedCustomAttribute> customAttributes, in ManagedSignature returnType, in ReadOnlyFixedValueList4<ManagedParameter> parameters)
        {
            IsNil = false;
            Name = name;
            Attributes = attributes;
            ImplAttributes = implAttributes;
            CustomAttributes = customAttributes;
            GenericParameters = genericParameters;
            ReturnType = returnType;
            Parameters = parameters;
        }

        /// <inhericdoc />
        public override readonly string ToString() => Name;

    }

}
