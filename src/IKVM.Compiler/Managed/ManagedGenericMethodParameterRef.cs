using System;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a reference to a generic parameter of a method.
    /// </summary>
    public readonly struct ManagedGenericMethodParameterRef
    {

        readonly ManagedTypeRef type;
        readonly int methodIndex;
        readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        public ManagedGenericMethodParameterRef(ManagedTypeRef type, int methodIndex, int index)
        {
            if (methodIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(methodIndex));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            this.type = type;
            this.methodIndex = methodIndex;
            this.index = index;
        }

        /// <summary>
        /// Gets the type of the generic parameter.
        /// </summary>
        public ManagedTypeRef Type => type;

        /// <summary>
        /// Gets the index of the method of the generic parameter.
        /// </summary>
        public int MethodIndex => methodIndex;

        /// <summary>
        /// Gets the index of the generic parameter on the method.
        /// </summary>
        public int Index => index;

    }

}