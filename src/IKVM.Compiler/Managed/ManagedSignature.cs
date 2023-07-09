using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a managed signature.
    /// </summary>
    public readonly partial struct ManagedSignature :
        IEquatable<ManagedSignature>,
        IEquatable<ManagedTypeSignature>,
        IEquatable<ManagedPrimitiveTypeSignature>,
        IEquatable<ManagedSZArraySignature>,
        IEquatable<ManagedArraySignature>,
        IEquatable<ManagedByRefSignature>,
        IEquatable<ManagedGenericSignature>,
        IEquatable<ManagedGenericConstraintSignature>,
        IEquatable<ManagedGenericTypeParameterSignature>,
        IEquatable<ManagedGenericMethodParameterSignature>,
        IEquatable<ManagedModifiedSignature>,
        IEquatable<ManagedPointerSignature>,
        IEquatable<ManagedFunctionPointerSignature>
    {

        public static readonly ManagedPrimitiveTypeSignature Void = ManagedPrimitiveTypeSignature.Create(ManagedPrimitiveTypeCode.Void);
        public static readonly ManagedPrimitiveTypeSignature Boolean = ManagedPrimitiveTypeSignature.Create(ManagedPrimitiveTypeCode.Boolean);
        public static readonly ManagedPrimitiveTypeSignature Byte = ManagedPrimitiveTypeSignature.Create(ManagedPrimitiveTypeCode.Byte);
        public static readonly ManagedPrimitiveTypeSignature SByte = ManagedPrimitiveTypeSignature.Create(ManagedPrimitiveTypeCode.SByte);
        public static readonly ManagedPrimitiveTypeSignature Char = ManagedPrimitiveTypeSignature.Create(ManagedPrimitiveTypeCode.Char);
        public static readonly ManagedPrimitiveTypeSignature Int16 = ManagedPrimitiveTypeSignature.Create(ManagedPrimitiveTypeCode.Int16);
        public static readonly ManagedPrimitiveTypeSignature UInt16 = ManagedPrimitiveTypeSignature.Create(ManagedPrimitiveTypeCode.UInt16);
        public static readonly ManagedPrimitiveTypeSignature Int32 = ManagedPrimitiveTypeSignature.Create(ManagedPrimitiveTypeCode.Int32);
        public static readonly ManagedPrimitiveTypeSignature UInt32 = ManagedPrimitiveTypeSignature.Create(ManagedPrimitiveTypeCode.UInt32);
        public static readonly ManagedPrimitiveTypeSignature Int64 = ManagedPrimitiveTypeSignature.Create(ManagedPrimitiveTypeCode.Int64);
        public static readonly ManagedPrimitiveTypeSignature UInt64 = ManagedPrimitiveTypeSignature.Create(ManagedPrimitiveTypeCode.UInt64);
        public static readonly ManagedPrimitiveTypeSignature Single = ManagedPrimitiveTypeSignature.Create(ManagedPrimitiveTypeCode.Single);
        public static readonly ManagedPrimitiveTypeSignature Double = ManagedPrimitiveTypeSignature.Create(ManagedPrimitiveTypeCode.Double);
        public static readonly ManagedPrimitiveTypeSignature IntPtr = ManagedPrimitiveTypeSignature.Create(ManagedPrimitiveTypeCode.IntPtr);
        public static readonly ManagedPrimitiveTypeSignature UIntPtr = ManagedPrimitiveTypeSignature.Create(ManagedPrimitiveTypeCode.UIntPtr);
        public static readonly ManagedPrimitiveTypeSignature Object = ManagedPrimitiveTypeSignature.Create(ManagedPrimitiveTypeCode.Object);
        public static readonly ManagedPrimitiveTypeSignature String = ManagedPrimitiveTypeSignature.Create(ManagedPrimitiveTypeCode.String);
        public static readonly ManagedPrimitiveTypeSignature TypedReference = ManagedPrimitiveTypeSignature.Create(ManagedPrimitiveTypeCode.TypedReference);

        /// <summary>
        /// Creates a new function pointer signature.
        /// </summary>
        /// <param name="parameterTypes"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public static ManagedFunctionPointerSignature FunctionPointer(ReadOnlyFixedValueList4<ManagedSignature> parameterTypes, ManagedSignature returnType) => ManagedFunctionPointerSignature.Create(parameterTypes.ToDataList4(), returnType.data);

        /// <summary>
        /// Creates a new function pointer signature.
        /// </summary>
        /// <param name="parameterTypes"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public static ManagedFunctionPointerSignature FunctionPointer(ReadOnlySpan<ManagedSignature> parameterTypes, ManagedSignature returnType) => ManagedFunctionPointerSignature.Create(parameterTypes.ToDataList4(), returnType.data);

        /// <summary>
        /// Creates a new function pointer signature.
        /// </summary>
        /// <param name="parameterTypes"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public static ManagedFunctionPointerSignature FunctionPointer(IReadOnlyList<ManagedSignature> parameterTypes, ManagedSignature returnType) => ManagedFunctionPointerSignature.Create(parameterTypes.ToDataList4(), returnType.data);

        /// <summary>
        /// Creates a new function pointer signature.
        /// </summary>
        /// <param name="parameterTypes"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public static ManagedFunctionPointerSignature FunctionPointer(ManagedSignature[] parameterTypes, ManagedSignature returnType) => ManagedFunctionPointerSignature.Create(parameterTypes.ToDataList4(), returnType.data);

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="elementType"></param>
        internal ManagedSignature(in ManagedSignatureData data)
        {
            this.data = data;
        }

        /// <summary>
        /// Gets the kind of this signature.
        /// </summary>
        public readonly ManagedSignatureKind Kind => data.GetLastCode().Data.Kind;

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly override bool Equals(object? obj) => obj switch
        {
            ManagedSignature s => Equals(s),
            _ => false,
        };

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedSignature obj) => Kind switch
        {
            ManagedSignatureKind.Type => obj.Kind == ManagedSignatureKind.Type && Equals((ManagedTypeSignature)obj),
            ManagedSignatureKind.PrimitiveType => obj.Kind == ManagedSignatureKind.PrimitiveType && Equals(((ManagedPrimitiveTypeSignature)obj)),
            ManagedSignatureKind.SZArray => obj.Kind == ManagedSignatureKind.SZArray && Equals((ManagedSZArraySignature)obj),
            ManagedSignatureKind.Array => obj.Kind == ManagedSignatureKind.Array && Equals((ManagedArraySignature)obj),
            ManagedSignatureKind.ByRef => obj.Kind == ManagedSignatureKind.ByRef && Equals((ManagedByRefSignature)obj),
            ManagedSignatureKind.Generic => obj.Kind == ManagedSignatureKind.Generic && Equals((ManagedGenericSignature)obj),
            ManagedSignatureKind.GenericConstraint => obj.Kind == ManagedSignatureKind.GenericConstraint && Equals((ManagedGenericConstraintSignature)obj),
            ManagedSignatureKind.GenericTypeParameter => obj.Kind == ManagedSignatureKind.GenericTypeParameter && Equals((ManagedGenericTypeParameterSignature)obj),
            ManagedSignatureKind.GenericMethodParameter => obj.Kind == ManagedSignatureKind.GenericMethodParameter && Equals((ManagedGenericMethodParameterSignature)obj),
            ManagedSignatureKind.Modified => obj.Kind == ManagedSignatureKind.Modified && Equals((ManagedModifiedSignature)obj),
            ManagedSignatureKind.Pointer => obj.Kind == ManagedSignatureKind.Pointer && Equals((ManagedPointerSignature)obj),
            ManagedSignatureKind.FunctionPointer => obj.Kind == ManagedSignatureKind.FunctionPointer && Equals((ManagedFunctionPointerSignature)obj),
            _ => false
        };

        /// <inheritdoc />
        bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

        /// <summary>
        /// Gets a unique hash code for this signature.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => Kind switch
        {
            ManagedSignatureKind.Type => ((ManagedTypeSignature)this).GetHashCode(),
            ManagedSignatureKind.PrimitiveType => ((ManagedPrimitiveTypeSignature)this).GetHashCode(),
            ManagedSignatureKind.SZArray => ((ManagedSZArraySignature)this).GetHashCode(),
            ManagedSignatureKind.Array => ((ManagedArraySignature)this).GetHashCode(),
            ManagedSignatureKind.ByRef => ((ManagedByRefSignature)this).GetHashCode(),
            ManagedSignatureKind.Generic => ((ManagedGenericSignature)this).GetHashCode(),
            ManagedSignatureKind.GenericConstraint => ((ManagedGenericConstraintSignature)this).GetHashCode(),
            ManagedSignatureKind.GenericTypeParameter => ((ManagedGenericTypeParameterSignature)this).GetHashCode(),
            ManagedSignatureKind.GenericMethodParameter => ((ManagedGenericMethodParameterSignature)this).GetHashCode(),
            ManagedSignatureKind.Modified => ((ManagedModifiedSignature)this).GetHashCode(),
            ManagedSignatureKind.Pointer => ((ManagedPointerSignature)this).GetHashCode(),
            ManagedSignatureKind.FunctionPointer => ((ManagedFunctionPointerSignature)this).GetHashCode(),
            _ => throw new ManagedTypeException("Invalid signature kind."),
        };

        /// <inheritdoc />
        public override string? ToString() => Kind switch
        {
            ManagedSignatureKind.Type => ((ManagedTypeSignature)this).ToString(),
            ManagedSignatureKind.PrimitiveType => ((ManagedPrimitiveTypeSignature)this).ToString(),
            ManagedSignatureKind.SZArray => ((ManagedSZArraySignature)this).ToString(),
            ManagedSignatureKind.Array => ((ManagedArraySignature)this).ToString(),
            ManagedSignatureKind.ByRef => ((ManagedByRefSignature)this).ToString(),
            ManagedSignatureKind.Generic => ((ManagedGenericSignature)this).ToString(),
            ManagedSignatureKind.GenericConstraint => ((ManagedGenericConstraintSignature)this).ToString(),
            ManagedSignatureKind.GenericTypeParameter => ((ManagedGenericTypeParameterSignature)this).ToString(),
            ManagedSignatureKind.GenericMethodParameter => ((ManagedGenericMethodParameterSignature)this).ToString(),
            ManagedSignatureKind.Modified => ((ManagedModifiedSignature)this).ToString(),
            ManagedSignatureKind.Pointer => ((ManagedPointerSignature)this).ToString(),
            ManagedSignatureKind.FunctionPointer => ((ManagedFunctionPointerSignature)this).ToString(),
            _ => throw new ManagedTypeException("Invalid signature kind."),
        };

    }

}
