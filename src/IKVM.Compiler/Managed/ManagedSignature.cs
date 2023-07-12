using System;
using System.Collections.Generic;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a managed signature.
    /// </summary>
    internal readonly partial struct ManagedSignature :
        IEquatable<ManagedSignature>,
        IEquatable<ManagedTypeSignature>,
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

        /// <summary>
        /// Creates a new type signature.
        /// </summary>
        /// <param name="typeRef"></param>
        /// <returns></returns>
        public static ManagedTypeSignature Type(ManagedType typeRef)
        {
            return new ManagedTypeSignature(typeRef);
        }

        /// <summary>
        /// Creates a new function pointer signature.
        /// </summary>
        /// <param name="parameterTypes"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public static ManagedFunctionPointerSignature FunctionPointer(in ManagedSignature returnType, in FixedValueList4<ManagedSignature> parameterTypes)
        {
            parameterTypes.ToDataList4(out var parameterTypes_);
            return new ManagedFunctionPointerSignature(parameterTypes_, returnType.data);
        }

        /// <summary>
        /// Creates a new function pointer signature.
        /// </summary>
        /// <param name="parameterTypes"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public static ManagedFunctionPointerSignature FunctionPointer(in ManagedSignature returnType, ReadOnlySpan<ManagedSignature> parameterTypes)
        {
            parameterTypes.ToDataList4(out var parameterTypes_);
            return new ManagedFunctionPointerSignature(parameterTypes_, returnType.data);
        }

        /// <summary>
        /// Creates a new function pointer signature.
        /// </summary>
        /// <param name="parameterTypes"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public static ManagedFunctionPointerSignature FunctionPointer(in ManagedSignature returnType, IReadOnlyList<ManagedSignature> parameterTypes)
        {
            parameterTypes.ToDataList4(out var parameterTypes_);
            return new ManagedFunctionPointerSignature(parameterTypes_, returnType.data);
        }

        /// <summary>
        /// Creates a new function pointer signature.
        /// </summary>
        /// <param name="parameterTypes"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public static ManagedFunctionPointerSignature FunctionPointer(in ManagedSignature returnType, params ManagedSignature[] parameterTypes)
        {
            parameterTypes.ToDataList4(out var parameterTypes_);
            return new ManagedFunctionPointerSignature(parameterTypes_, returnType.data);
        }

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedSignature(bool copy, in ManagedSignatureData data)
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
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

        /// <summary>
        /// Gets a unique hash code for this signature.
        /// </summary>
        /// <returns></returns>
        public override readonly int GetHashCode() => Kind switch
        {
            ManagedSignatureKind.Type => ((ManagedTypeSignature)this).GetHashCode(),
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
            _ => throw new ManagedException("Invalid signature kind."),
        };

        /// <inheritdoc />
        public override readonly string? ToString() => data.ToString();

    }

}
