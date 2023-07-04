using System;
using System.Collections.Generic;
using System.Linq;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a type including any modifiers.
    /// </summary>
    public abstract class ManagedTypeSignature
    {

        public static readonly ManagedPrimitiveTypeSignature Void = new(ManagedPrimitiveType.Void);
        public static readonly ManagedPrimitiveTypeSignature Boolean = new(ManagedPrimitiveType.Boolean);
        public static readonly ManagedPrimitiveTypeSignature Byte = new(ManagedPrimitiveType.Byte);
        public static readonly ManagedPrimitiveTypeSignature SByte = new(ManagedPrimitiveType.SByte);
        public static readonly ManagedPrimitiveTypeSignature Char = new(ManagedPrimitiveType.Char);
        public static readonly ManagedPrimitiveTypeSignature Int16 = new(ManagedPrimitiveType.Int16);
        public static readonly ManagedPrimitiveTypeSignature UInt16 = new(ManagedPrimitiveType.UInt16);
        public static readonly ManagedPrimitiveTypeSignature Int32 = new(ManagedPrimitiveType.Int32);
        public static readonly ManagedPrimitiveTypeSignature UInt32 = new(ManagedPrimitiveType.UInt32);
        public static readonly ManagedPrimitiveTypeSignature Int64 = new(ManagedPrimitiveType.Int64);
        public static readonly ManagedPrimitiveTypeSignature UInt64 = new(ManagedPrimitiveType.UInt64);
        public static readonly ManagedPrimitiveTypeSignature Single = new(ManagedPrimitiveType.Single);
        public static readonly ManagedPrimitiveTypeSignature Double = new(ManagedPrimitiveType.Double);
        public static readonly ManagedPrimitiveTypeSignature IntPtr = new(ManagedPrimitiveType.IntPtr);
        public static readonly ManagedPrimitiveTypeSignature UIntPtr = new(ManagedPrimitiveType.UIntPtr);
        public static readonly ManagedPrimitiveTypeSignature Object = new(ManagedPrimitiveType.Object);
        public static readonly ManagedPrimitiveTypeSignature String = new(ManagedPrimitiveType.String);
        public static readonly ManagedPrimitiveTypeSignature TypedReference = new(ManagedPrimitiveType.TypedReference);

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected ManagedTypeSignature()
        {

        }

        /// <summary>
        /// Creates a new fixed size, zero-indexed array. This is a standard .NET array type.
        /// </summary>
        /// <returns></returns>
        public ManagedTypeSignature Array() => new ManagedSzArrayTypeSignature(this);

        /// <summary>
        /// Creates a new multidimensional array.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public ManagedTypeSignature Array(int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds) => new ManagedArrayTypeSignature(this, new ManagedArrayShape(rank, sizes, lowerBounds));

        /// <summary>
        /// Creates a new multidimensional array.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        /// <returns></returns>
        public ManagedTypeSignature Array(int rank, IList<int> sizes, IList<int> lowerBounds) => new ManagedArrayTypeSignature(this, new ManagedArrayShape(rank, sizes, lowerBounds));

        /// <summary>
        /// Creates a new by-ref type.
        /// </summary>
        /// <returns></returns>
        public ManagedTypeSignature ByRef() => new ManagedByRefTypeSignature(this);

        /// <summary>
        /// Creates a new pointer type.
        /// </summary>
        /// <returns></returns>
        public ManagedTypeSignature Pointer() => new ManagedPointerTypeSignature(this);

        /// <summary>
        /// Creates a new generic type.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ManagedTypeSignature Generic(in ReadOnlyFixedValueList<ManagedTypeSignature> parameters) => new ManagedGenericTypeSignature(this, parameters);

        /// <summary>
        /// Creates a new generic type.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ManagedTypeSignature Generic(ReadOnlySpan<ManagedTypeSignature> parameters) => Generic(new ReadOnlyFixedValueList<ManagedTypeSignature>(parameters));

        /// <summary>
        /// Creates a new generic type.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ManagedTypeSignature Generic(params ManagedTypeSignature[] parameters) => Generic(parameters.AsSpan());

        /// <summary>
        /// Creates a new generic type.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ManagedTypeSignature Generic(IEnumerable<ManagedTypeSignature> parameters) => Generic(parameters.ToArray().AsSpan());

    }

}
