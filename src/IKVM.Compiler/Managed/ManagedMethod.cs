using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed method.
    /// </summary>
    public readonly struct ManagedMethod
    {

        readonly string name;
        readonly MethodAttributes attributes;
        readonly MethodImplAttributes implAttributes;
        readonly ReadOnlyFixedValueList<ManagedCustomAttribute> customAttributes;
        readonly ReadOnlyFixedValueList<ManagedGenericParameter> genericParameters;
        readonly ManagedTypeSignature returnType;
        readonly ReadOnlyFixedValueList<ManagedParameter> parameters;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="implAttributes"></param>
        /// <param name="customAttributes"></param>
        /// <param name="genericParameters"></param>
        /// <param name="returnType"></param>
        /// <param name="parameters"></param>
        public ManagedMethod(string name, MethodAttributes attributes, MethodImplAttributes implAttributes, in ReadOnlyFixedValueList<ManagedCustomAttribute> customAttributes, in ReadOnlyFixedValueList<ManagedGenericParameter> genericParameters, ManagedTypeSignature returnType, in ReadOnlyFixedValueList<ManagedParameter> parameters)
        {
            this.name = name;
            this.attributes = attributes;
            this.implAttributes = implAttributes;
            this.customAttributes = customAttributes;
            this.genericParameters = genericParameters;
            this.returnType = returnType;
            this.parameters = parameters;
        }

        /// <summary>
        /// Gets the name of the managed method.
        /// </summary>
        public readonly string Name => name;

        /// <summary>
        /// Gets the attributes of the method.
        /// </summary>
        public readonly MethodAttributes Attributes => attributes;

        /// <summary>
        /// Gets the implementation attributes of the method.
        /// </summary>
        public readonly MethodImplAttributes ImplAttributes => implAttributes;

        /// <summary>
        /// Gets the set of custom attributes applied to the method.
        /// </summary>
        public readonly ReadOnlyFixedValueList<ManagedCustomAttribute> CustomAttributes => customAttributes;

        /// <summary>
        /// Gets the set of generic parameters on the method.
        /// </summary>
        public readonly ReadOnlyFixedValueList<ManagedGenericParameter> GenericParameters => genericParameters;

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        public readonly ManagedTypeSignature ReturnType => returnType;

        /// <summary>
        /// Gets the set of parameters of the method.
        /// </summary>
        public readonly ReadOnlyFixedValueList<ManagedParameter> Parameters => parameters;

        /// <inhericdoc />
        public override readonly string ToString() => name;

    }

}
