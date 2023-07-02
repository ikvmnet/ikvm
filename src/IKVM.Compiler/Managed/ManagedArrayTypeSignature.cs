using System;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes an array of a type signature.
    /// </summary>
    public sealed class ManagedArrayTypeSignature : ManagedTypeSignature
    {

        readonly ManagedTypeSignature elementType;
        readonly ManagedArrayShape shape;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="shape"></param>
        ManagedArrayTypeSignature(ManagedTypeSignature elementType, ManagedArrayShape shape)
        {
            this.elementType = elementType;
            this.shape = shape;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="lowerBound0"></param>
        /// <param name="lowerBound1"></param>
        public ManagedArrayTypeSignature(ManagedTypeSignature elementType, int lowerBound0 = 0, int lowerBound1 = 0) :
            this(elementType, new ManagedArrayShape(2, lowerBound0, lowerBound1))
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="lowerBounds"></param>
        public ManagedArrayTypeSignature(ManagedTypeSignature elementType, ReadOnlySpan<int> lowerBounds) :
            this(elementType, new ManagedArrayShape(lowerBounds))
        {

        }

        /// <summary>
        /// Gets the element type of the array.
        /// </summary>
        public ManagedTypeSignature ElementType => elementType;

        /// <summary>
        /// Gets the number of dimensions in the array.
        /// </summary>
        public int Rank => shape.Rank;

    }

}
