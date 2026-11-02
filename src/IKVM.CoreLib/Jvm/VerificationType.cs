using System;
using System.Runtime.CompilerServices;

namespace IKVM.CoreLib.Jvm
{

    /// <summary>
    /// The kind of a <see cref="VerificationType{TType}"/>. These are the verification types of the JVM specification
    /// (4.10.1.2), with <see cref="Int"/> standing for every type that is an int on the stack.
    /// </summary>
    internal enum VerificationTypeKind : byte
    {

        /// <summary>
        /// No usable value: an unassigned local, the second half of a long or double in the locals, or the result of
        /// merging values that have nothing in common. This is the default.
        /// </summary>
        Top = 0,

        Int,

        Float,

        Long,

        Double,

        /// <summary>
        /// The type of the null reference.
        /// </summary>
        Null,

        /// <summary>
        /// An object whose constructor has not run yet: the result of a 'new' instruction, or the receiver of a
        /// constructor before it calls another constructor.
        /// </summary>
        Uninitialized,

        /// <summary>
        /// A reference to an initialized object of a known type.
        /// </summary>
        Reference,

    }

    /// <summary>
    /// Annotations that IKVM adds to a verification type. They are not part of the JVM's verification type system, but
    /// record facts about a value that the compiler needs.
    /// </summary>
    [Flags]
    internal enum VerificationTypeFlags : byte
    {

        None = 0,

        /// <summary>
        /// A <see cref="VerificationTypeKind.Float"/> or <see cref="VerificationTypeKind.Double"/> that is the result of
        /// arithmetic and may hold more precision than its type.
        /// </summary>
        Extended = 1,

        /// <summary>
        /// A <see cref="VerificationTypeKind.Reference"/> that is the 'this' reference of an instance method, loaded
        /// directly from local 0.
        /// </summary>
        This = 2,

        /// <summary>
        /// A <see cref="VerificationTypeKind.Reference"/> to the exception caught by a catch-any handler that may turn
        /// out to be a fault block. <see cref="VerificationType{TType}.Index"/> holds the handler's instruction index.
        /// </summary>
        FaultBlock = 4,

    }

    /// <summary>
    /// The type of a value in a local variable or on the operand stack, as tracked by the verifier. This is a value, so it
    /// can be compared and copied freely and needs no allocation.
    /// </summary>
    /// <typeparam name="TType">The representation of a type in the type system.</typeparam>
    internal readonly struct VerificationType<TType> : IEquatable<VerificationType<TType>>
        where TType : class
    {

        /// <summary>
        /// The <see cref="VerificationType{TType}.Index"/> of the uninitialized 'this' of a constructor.
        /// </summary>
        const int UninitializedThisIndex = -1;

        /// <summary>
        /// Gets the top type, which is also the default value.
        /// </summary>
        public static VerificationType<TType> Top => default;

        public static VerificationType<TType> Int => new(VerificationTypeKind.Int);

        public static VerificationType<TType> Float => new(VerificationTypeKind.Float);

        public static VerificationType<TType> ExtendedFloat => new(VerificationTypeKind.Float, VerificationTypeFlags.Extended);

        public static VerificationType<TType> Long => new(VerificationTypeKind.Long);

        public static VerificationType<TType> Double => new(VerificationTypeKind.Double);

        public static VerificationType<TType> ExtendedDouble => new(VerificationTypeKind.Double, VerificationTypeFlags.Extended);

        public static VerificationType<TType> Null => new(VerificationTypeKind.Null);

        /// <summary>
        /// Gets the type of the receiver of a constructor before it has called another constructor.
        /// </summary>
        public static VerificationType<TType> UninitializedThis => new(VerificationTypeKind.Uninitialized, VerificationTypeFlags.None, null, UninitializedThisIndex);

        /// <summary>
        /// Gets a reference to an initialized object of the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static VerificationType<TType> Reference(TType type) => new(VerificationTypeKind.Reference, VerificationTypeFlags.None, type ?? throw new ArgumentNullException(nameof(type)), 0);

        /// <summary>
        /// Gets the 'this' reference of an instance method of the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static VerificationType<TType> This(TType type) => new(VerificationTypeKind.Reference, VerificationTypeFlags.This, type ?? throw new ArgumentNullException(nameof(type)), 0);

        /// <summary>
        /// Gets the type of the object created by the 'new' instruction at the given instruction index.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instructionIndex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static VerificationType<TType> Uninitialized(TType type, int instructionIndex)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (instructionIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(instructionIndex));

            return new(VerificationTypeKind.Uninitialized, VerificationTypeFlags.None, type, instructionIndex);
        }

        /// <summary>
        /// Gets the type of the exception caught by the catch-any handler at the given instruction index, while it may
        /// still be a fault block.
        /// </summary>
        /// <param name="throwable">The type of 'java.lang.Throwable'.</param>
        /// <param name="handlerIndex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static VerificationType<TType> FaultBlockException(TType throwable, int handlerIndex)
        {
            if (throwable is null)
                throw new ArgumentNullException(nameof(throwable));
            if (handlerIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(handlerIndex));

            return new(VerificationTypeKind.Reference, VerificationTypeFlags.FaultBlock, throwable, handlerIndex);
        }

        readonly TType? _type;
        readonly int _index;
        readonly VerificationTypeKind _kind;
        readonly VerificationTypeFlags _flags;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="flags"></param>
        /// <param name="type"></param>
        /// <param name="index"></param>
        VerificationType(VerificationTypeKind kind, VerificationTypeFlags flags = VerificationTypeFlags.None, TType? type = null, int index = 0)
        {
            _kind = kind;
            _flags = flags;
            _type = type;
            _index = index;
        }

        /// <summary>
        /// Gets the kind of this type.
        /// </summary>
        public VerificationTypeKind Kind => _kind;

        /// <summary>
        /// Gets the IKVM annotations on this type.
        /// </summary>
        public VerificationTypeFlags Flags => _flags;

        /// <summary>
        /// Gets the class type of a reference, or the type being constructed by an uninitialized 'new', or <c>null</c>.
        /// </summary>
        public TType? Type => _type;

        /// <summary>
        /// Gets the instruction index of the 'new' of an uninitialized object, or of the handler of a fault block
        /// exception.
        /// </summary>
        public int Index => _index;

        public bool IsTop => _kind == VerificationTypeKind.Top;

        public bool IsInt => _kind == VerificationTypeKind.Int;

        /// <summary>
        /// Gets whether this is a float, of either precision.
        /// </summary>
        public bool IsFloat => _kind == VerificationTypeKind.Float;

        public bool IsLong => _kind == VerificationTypeKind.Long;

        /// <summary>
        /// Gets whether this is a double, of either precision.
        /// </summary>
        public bool IsDouble => _kind == VerificationTypeKind.Double;

        /// <summary>
        /// Gets whether this is a float or double that may hold extended precision.
        /// </summary>
        public bool IsExtended => (_flags & VerificationTypeFlags.Extended) != 0;

        public bool IsNull => _kind == VerificationTypeKind.Null;

        /// <summary>
        /// Gets whether this is an int, float, long or double.
        /// </summary>
        public bool IsPrimitive => _kind is VerificationTypeKind.Int or VerificationTypeKind.Float or VerificationTypeKind.Long or VerificationTypeKind.Double;

        /// <summary>
        /// Gets whether this is a long or double, which take two words.
        /// </summary>
        public bool IsCategory2 => _kind is VerificationTypeKind.Long or VerificationTypeKind.Double;

        /// <summary>
        /// Gets whether this is a reference to an initialized object, including 'this' and a fault block exception.
        /// </summary>
        public bool IsReference => _kind == VerificationTypeKind.Reference;

        /// <summary>
        /// Gets whether this is the 'this' reference of an instance method.
        /// </summary>
        public bool IsThis => (_flags & VerificationTypeFlags.This) != 0;

        /// <summary>
        /// Gets whether this is the exception of a catch-any handler that may be a fault block.
        /// </summary>
        public bool IsFaultBlockException => (_flags & VerificationTypeFlags.FaultBlock) != 0;

        /// <summary>
        /// Gets whether this is an object whose constructor has not run, either from 'new' or the receiver of a
        /// constructor.
        /// </summary>
        public bool IsUninitialized => _kind == VerificationTypeKind.Uninitialized;

        /// <summary>
        /// Gets whether this is the receiver of a constructor before it has called another constructor.
        /// </summary>
        public bool IsUninitializedThis => _kind == VerificationTypeKind.Uninitialized && _index == UninitializedThisIndex;

        /// <summary>
        /// Gets whether this is the result of a 'new' whose constructor has not run.
        /// </summary>
        public bool IsUninitializedNew => _kind == VerificationTypeKind.Uninitialized && _index != UninitializedThisIndex;

        /// <summary>
        /// Returns this type without IKVM's annotations: a plain float or double, or a plain reference.
        /// </summary>
        /// <returns></returns>
        public VerificationType<TType> WithoutFlags()
        {
            if (_flags == VerificationTypeFlags.None)
                return this;

            return new(_kind, VerificationTypeFlags.None, _type, (_flags & VerificationTypeFlags.FaultBlock) != 0 ? 0 : _index);
        }

        /// <inheritdoc />
        public bool Equals(VerificationType<TType> other)
        {
            return _kind == other._kind && _flags == other._flags && _index == other._index && ReferenceEquals(_type, other._type);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is VerificationType<TType> other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(_kind, _flags, _index, _type is null ? 0 : RuntimeHelpers.GetHashCode(_type));
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var extended = IsExtended ? "!" : "";
            return _kind switch
            {
                VerificationTypeKind.Top => "top",
                VerificationTypeKind.Int => "I",
                VerificationTypeKind.Float => "F" + extended,
                VerificationTypeKind.Long => "J",
                VerificationTypeKind.Double => "D" + extended,
                VerificationTypeKind.Null => "null",
                VerificationTypeKind.Uninitialized when IsUninitializedThis => "uninitThis",
                VerificationTypeKind.Uninitialized => $"new@{_index}:{_type}",
                VerificationTypeKind.Reference when IsThis => $"this:{_type}",
                VerificationTypeKind.Reference when IsFaultBlockException => $"fault@{_index}",
                VerificationTypeKind.Reference => $"{_type}",
                _ => throw new InvalidOperationException(),
            };
        }

        public static bool operator ==(VerificationType<TType> left, VerificationType<TType> right) => left.Equals(right);

        public static bool operator !=(VerificationType<TType> left, VerificationType<TType> right) => !left.Equals(right);

    }

}
