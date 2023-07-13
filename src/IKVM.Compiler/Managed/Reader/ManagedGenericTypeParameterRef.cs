using System;

namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Describes a reference to a generic parameter of a type.
    /// </summary>
    internal readonly struct ManagedGenericTypeParameterRef
    {

        readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="index"></param>
        public ManagedGenericTypeParameterRef(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            this.index = index;
        }

        /// <summary>
        /// Gets the index of the generic parameter on the type.
        /// </summary>
        public int Index => index;

    }

}