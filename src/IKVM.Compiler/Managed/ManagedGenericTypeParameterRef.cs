using System;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a reference to a generic parameter of a type.
    /// </summary>
    public readonly struct ManagedGenericTypeParameterRef
    {

        readonly ManagedTypeRef type;
        readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        public ManagedGenericTypeParameterRef(ManagedTypeRef type, int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            this.type = type;
            this.index = index;
        }

        /// <summary>
        /// Gets the type of the generic parameter.
        /// </summary>
        public ManagedTypeRef Type => type;

        /// <summary>
        /// Gets the index of the generic parameter on the type.
        /// </summary>
        public int Index => index;

    }

}