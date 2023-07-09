using System;

namespace IKVM.Compiler.Managed
{

    public readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedTypeSignature value) => new ManagedSignature(value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedTypeSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedTypeSignature(in ManagedSignature value) => new ManagedTypeSignature(value.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedTypeSignature obj) => obj.Equals(this);
        
        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <inheritdoc />
        bool IEquatable<ManagedTypeSignature>.Equals(ManagedTypeSignature other) => Equals(other);

    }

    public readonly partial struct ManagedTypeSignature : IEquatable<ManagedSignature>, IEquatable<ManagedTypeSignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator ==(in ManagedTypeSignature x, in ManagedTypeSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator !=(in ManagedTypeSignature x, in ManagedTypeSignature y) => x.Equals(y) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedTypeSignature(in ManagedSignatureData data)
        {
            var c = data.GetLastCode();
            if (c.Data.Kind != ManagedSignatureKind.Type)
                throw new InvalidCastException($"Signature of type {c.Data.Kind} is not of kind Type.");

            this.data = data;
        }

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override readonly bool Equals(object obj) => obj switch
        {
            ManagedSignature s => Equals(s),
            ManagedTypeSignature s => Equals(s),
            _ => false,
        };

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedTypeSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Gets a unique hash code for this signature.
        /// </summary>
        /// <returns></returns>
        public override readonly int GetHashCode() => data.GetHashCode();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedTypeSignature>.Equals(ManagedTypeSignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    public readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedPrimitiveTypeSignature value) => new ManagedSignature(value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedPrimitiveTypeSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedPrimitiveTypeSignature(in ManagedSignature value) => new ManagedPrimitiveTypeSignature(value.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedPrimitiveTypeSignature obj) => obj.Equals(this);
        
        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <inheritdoc />
        bool IEquatable<ManagedPrimitiveTypeSignature>.Equals(ManagedPrimitiveTypeSignature other) => Equals(other);

    }

    public readonly partial struct ManagedPrimitiveTypeSignature : IEquatable<ManagedSignature>, IEquatable<ManagedPrimitiveTypeSignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator ==(in ManagedPrimitiveTypeSignature x, in ManagedPrimitiveTypeSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator !=(in ManagedPrimitiveTypeSignature x, in ManagedPrimitiveTypeSignature y) => x.Equals(y) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedPrimitiveTypeSignature(in ManagedSignatureData data)
        {
            var c = data.GetLastCode();
            if (c.Data.Kind != ManagedSignatureKind.PrimitiveType)
                throw new InvalidCastException($"Signature of type {c.Data.Kind} is not of kind PrimitiveType.");

            this.data = data;
        }

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override readonly bool Equals(object obj) => obj switch
        {
            ManagedSignature s => Equals(s),
            ManagedPrimitiveTypeSignature s => Equals(s),
            _ => false,
        };

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedPrimitiveTypeSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Gets a unique hash code for this signature.
        /// </summary>
        /// <returns></returns>
        public override readonly int GetHashCode() => data.GetHashCode();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedPrimitiveTypeSignature>.Equals(ManagedPrimitiveTypeSignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    public readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedSZArraySignature value) => new ManagedSignature(value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSZArraySignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedSZArraySignature(in ManagedSignature value) => new ManagedSZArraySignature(value.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedSZArraySignature obj) => obj.Equals(this);
        
        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <inheritdoc />
        bool IEquatable<ManagedSZArraySignature>.Equals(ManagedSZArraySignature other) => Equals(other);

    }

    public readonly partial struct ManagedSZArraySignature : IEquatable<ManagedSignature>, IEquatable<ManagedSZArraySignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator ==(in ManagedSZArraySignature x, in ManagedSZArraySignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator !=(in ManagedSZArraySignature x, in ManagedSZArraySignature y) => x.Equals(y) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedSZArraySignature(in ManagedSignatureData data)
        {
            var c = data.GetLastCode();
            if (c.Data.Kind != ManagedSignatureKind.SZArray)
                throw new InvalidCastException($"Signature of type {c.Data.Kind} is not of kind SZArray.");

            this.data = data;
        }

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override readonly bool Equals(object obj) => obj switch
        {
            ManagedSignature s => Equals(s),
            ManagedSZArraySignature s => Equals(s),
            _ => false,
        };

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedSZArraySignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Gets a unique hash code for this signature.
        /// </summary>
        /// <returns></returns>
        public override readonly int GetHashCode() => data.GetHashCode();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSZArraySignature>.Equals(ManagedSZArraySignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    public readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedArraySignature value) => new ManagedSignature(value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedArraySignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedArraySignature(in ManagedSignature value) => new ManagedArraySignature(value.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedArraySignature obj) => obj.Equals(this);
        
        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <inheritdoc />
        bool IEquatable<ManagedArraySignature>.Equals(ManagedArraySignature other) => Equals(other);

    }

    public readonly partial struct ManagedArraySignature : IEquatable<ManagedSignature>, IEquatable<ManagedArraySignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator ==(in ManagedArraySignature x, in ManagedArraySignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator !=(in ManagedArraySignature x, in ManagedArraySignature y) => x.Equals(y) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedArraySignature(in ManagedSignatureData data)
        {
            var c = data.GetLastCode();
            if (c.Data.Kind != ManagedSignatureKind.Array)
                throw new InvalidCastException($"Signature of type {c.Data.Kind} is not of kind Array.");

            this.data = data;
        }

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override readonly bool Equals(object obj) => obj switch
        {
            ManagedSignature s => Equals(s),
            ManagedArraySignature s => Equals(s),
            _ => false,
        };

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedArraySignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Gets a unique hash code for this signature.
        /// </summary>
        /// <returns></returns>
        public override readonly int GetHashCode() => data.GetHashCode();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedArraySignature>.Equals(ManagedArraySignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    public readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedByRefSignature value) => new ManagedSignature(value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedByRefSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedByRefSignature(in ManagedSignature value) => new ManagedByRefSignature(value.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedByRefSignature obj) => obj.Equals(this);
        
        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <inheritdoc />
        bool IEquatable<ManagedByRefSignature>.Equals(ManagedByRefSignature other) => Equals(other);

    }

    public readonly partial struct ManagedByRefSignature : IEquatable<ManagedSignature>, IEquatable<ManagedByRefSignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator ==(in ManagedByRefSignature x, in ManagedByRefSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator !=(in ManagedByRefSignature x, in ManagedByRefSignature y) => x.Equals(y) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedByRefSignature(in ManagedSignatureData data)
        {
            var c = data.GetLastCode();
            if (c.Data.Kind != ManagedSignatureKind.ByRef)
                throw new InvalidCastException($"Signature of type {c.Data.Kind} is not of kind ByRef.");

            this.data = data;
        }

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override readonly bool Equals(object obj) => obj switch
        {
            ManagedSignature s => Equals(s),
            ManagedByRefSignature s => Equals(s),
            _ => false,
        };

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedByRefSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Gets a unique hash code for this signature.
        /// </summary>
        /// <returns></returns>
        public override readonly int GetHashCode() => data.GetHashCode();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedByRefSignature>.Equals(ManagedByRefSignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    public readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedGenericSignature value) => new ManagedSignature(value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedGenericSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedGenericSignature(in ManagedSignature value) => new ManagedGenericSignature(value.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedGenericSignature obj) => obj.Equals(this);
        
        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <inheritdoc />
        bool IEquatable<ManagedGenericSignature>.Equals(ManagedGenericSignature other) => Equals(other);

    }

    public readonly partial struct ManagedGenericSignature : IEquatable<ManagedSignature>, IEquatable<ManagedGenericSignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator ==(in ManagedGenericSignature x, in ManagedGenericSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator !=(in ManagedGenericSignature x, in ManagedGenericSignature y) => x.Equals(y) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedGenericSignature(in ManagedSignatureData data)
        {
            var c = data.GetLastCode();
            if (c.Data.Kind != ManagedSignatureKind.Generic)
                throw new InvalidCastException($"Signature of type {c.Data.Kind} is not of kind Generic.");

            this.data = data;
        }

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override readonly bool Equals(object obj) => obj switch
        {
            ManagedSignature s => Equals(s),
            ManagedGenericSignature s => Equals(s),
            _ => false,
        };

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedGenericSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Gets a unique hash code for this signature.
        /// </summary>
        /// <returns></returns>
        public override readonly int GetHashCode() => data.GetHashCode();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedGenericSignature>.Equals(ManagedGenericSignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    public readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedGenericConstraintSignature value) => new ManagedSignature(value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedGenericConstraintSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedGenericConstraintSignature(in ManagedSignature value) => new ManagedGenericConstraintSignature(value.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedGenericConstraintSignature obj) => obj.Equals(this);
        
        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <inheritdoc />
        bool IEquatable<ManagedGenericConstraintSignature>.Equals(ManagedGenericConstraintSignature other) => Equals(other);

    }

    public readonly partial struct ManagedGenericConstraintSignature : IEquatable<ManagedSignature>, IEquatable<ManagedGenericConstraintSignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator ==(in ManagedGenericConstraintSignature x, in ManagedGenericConstraintSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator !=(in ManagedGenericConstraintSignature x, in ManagedGenericConstraintSignature y) => x.Equals(y) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedGenericConstraintSignature(in ManagedSignatureData data)
        {
            var c = data.GetLastCode();
            if (c.Data.Kind != ManagedSignatureKind.GenericConstraint)
                throw new InvalidCastException($"Signature of type {c.Data.Kind} is not of kind GenericConstraint.");

            this.data = data;
        }

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override readonly bool Equals(object obj) => obj switch
        {
            ManagedSignature s => Equals(s),
            ManagedGenericConstraintSignature s => Equals(s),
            _ => false,
        };

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedGenericConstraintSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Gets a unique hash code for this signature.
        /// </summary>
        /// <returns></returns>
        public override readonly int GetHashCode() => data.GetHashCode();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedGenericConstraintSignature>.Equals(ManagedGenericConstraintSignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    public readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedGenericTypeParameterSignature value) => new ManagedSignature(value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedGenericTypeParameterSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedGenericTypeParameterSignature(in ManagedSignature value) => new ManagedGenericTypeParameterSignature(value.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedGenericTypeParameterSignature obj) => obj.Equals(this);
        
        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <inheritdoc />
        bool IEquatable<ManagedGenericTypeParameterSignature>.Equals(ManagedGenericTypeParameterSignature other) => Equals(other);

    }

    public readonly partial struct ManagedGenericTypeParameterSignature : IEquatable<ManagedSignature>, IEquatable<ManagedGenericTypeParameterSignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator ==(in ManagedGenericTypeParameterSignature x, in ManagedGenericTypeParameterSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator !=(in ManagedGenericTypeParameterSignature x, in ManagedGenericTypeParameterSignature y) => x.Equals(y) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedGenericTypeParameterSignature(in ManagedSignatureData data)
        {
            var c = data.GetLastCode();
            if (c.Data.Kind != ManagedSignatureKind.GenericTypeParameter)
                throw new InvalidCastException($"Signature of type {c.Data.Kind} is not of kind GenericTypeParameter.");

            this.data = data;
        }

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override readonly bool Equals(object obj) => obj switch
        {
            ManagedSignature s => Equals(s),
            ManagedGenericTypeParameterSignature s => Equals(s),
            _ => false,
        };

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedGenericTypeParameterSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Gets a unique hash code for this signature.
        /// </summary>
        /// <returns></returns>
        public override readonly int GetHashCode() => data.GetHashCode();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedGenericTypeParameterSignature>.Equals(ManagedGenericTypeParameterSignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    public readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedGenericMethodParameterSignature value) => new ManagedSignature(value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedGenericMethodParameterSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedGenericMethodParameterSignature(in ManagedSignature value) => new ManagedGenericMethodParameterSignature(value.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedGenericMethodParameterSignature obj) => obj.Equals(this);
        
        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <inheritdoc />
        bool IEquatable<ManagedGenericMethodParameterSignature>.Equals(ManagedGenericMethodParameterSignature other) => Equals(other);

    }

    public readonly partial struct ManagedGenericMethodParameterSignature : IEquatable<ManagedSignature>, IEquatable<ManagedGenericMethodParameterSignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator ==(in ManagedGenericMethodParameterSignature x, in ManagedGenericMethodParameterSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator !=(in ManagedGenericMethodParameterSignature x, in ManagedGenericMethodParameterSignature y) => x.Equals(y) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedGenericMethodParameterSignature(in ManagedSignatureData data)
        {
            var c = data.GetLastCode();
            if (c.Data.Kind != ManagedSignatureKind.GenericMethodParameter)
                throw new InvalidCastException($"Signature of type {c.Data.Kind} is not of kind GenericMethodParameter.");

            this.data = data;
        }

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override readonly bool Equals(object obj) => obj switch
        {
            ManagedSignature s => Equals(s),
            ManagedGenericMethodParameterSignature s => Equals(s),
            _ => false,
        };

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedGenericMethodParameterSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Gets a unique hash code for this signature.
        /// </summary>
        /// <returns></returns>
        public override readonly int GetHashCode() => data.GetHashCode();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedGenericMethodParameterSignature>.Equals(ManagedGenericMethodParameterSignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    public readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedModifiedSignature value) => new ManagedSignature(value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedModifiedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedModifiedSignature(in ManagedSignature value) => new ManagedModifiedSignature(value.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedModifiedSignature obj) => obj.Equals(this);
        
        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <inheritdoc />
        bool IEquatable<ManagedModifiedSignature>.Equals(ManagedModifiedSignature other) => Equals(other);

    }

    public readonly partial struct ManagedModifiedSignature : IEquatable<ManagedSignature>, IEquatable<ManagedModifiedSignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator ==(in ManagedModifiedSignature x, in ManagedModifiedSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator !=(in ManagedModifiedSignature x, in ManagedModifiedSignature y) => x.Equals(y) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedModifiedSignature(in ManagedSignatureData data)
        {
            var c = data.GetLastCode();
            if (c.Data.Kind != ManagedSignatureKind.Modified)
                throw new InvalidCastException($"Signature of type {c.Data.Kind} is not of kind Modified.");

            this.data = data;
        }

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override readonly bool Equals(object obj) => obj switch
        {
            ManagedSignature s => Equals(s),
            ManagedModifiedSignature s => Equals(s),
            _ => false,
        };

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedModifiedSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Gets a unique hash code for this signature.
        /// </summary>
        /// <returns></returns>
        public override readonly int GetHashCode() => data.GetHashCode();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedModifiedSignature>.Equals(ManagedModifiedSignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    public readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedPointerSignature value) => new ManagedSignature(value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedPointerSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedPointerSignature(in ManagedSignature value) => new ManagedPointerSignature(value.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedPointerSignature obj) => obj.Equals(this);
        
        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <inheritdoc />
        bool IEquatable<ManagedPointerSignature>.Equals(ManagedPointerSignature other) => Equals(other);

    }

    public readonly partial struct ManagedPointerSignature : IEquatable<ManagedSignature>, IEquatable<ManagedPointerSignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator ==(in ManagedPointerSignature x, in ManagedPointerSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator !=(in ManagedPointerSignature x, in ManagedPointerSignature y) => x.Equals(y) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedPointerSignature(in ManagedSignatureData data)
        {
            var c = data.GetLastCode();
            if (c.Data.Kind != ManagedSignatureKind.Pointer)
                throw new InvalidCastException($"Signature of type {c.Data.Kind} is not of kind Pointer.");

            this.data = data;
        }

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override readonly bool Equals(object obj) => obj switch
        {
            ManagedSignature s => Equals(s),
            ManagedPointerSignature s => Equals(s),
            _ => false,
        };

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedPointerSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Gets a unique hash code for this signature.
        /// </summary>
        /// <returns></returns>
        public override readonly int GetHashCode() => data.GetHashCode();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedPointerSignature>.Equals(ManagedPointerSignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    public readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedFunctionPointerSignature value) => new ManagedSignature(value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedFunctionPointerSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedFunctionPointerSignature(in ManagedSignature value) => new ManagedFunctionPointerSignature(value.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedFunctionPointerSignature obj) => obj.Equals(this);
        
        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <inheritdoc />
        bool IEquatable<ManagedFunctionPointerSignature>.Equals(ManagedFunctionPointerSignature other) => Equals(other);

    }

    public readonly partial struct ManagedFunctionPointerSignature : IEquatable<ManagedSignature>, IEquatable<ManagedFunctionPointerSignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator ==(in ManagedFunctionPointerSignature x, in ManagedFunctionPointerSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="value"></param>
        public static bool operator !=(in ManagedFunctionPointerSignature x, in ManagedFunctionPointerSignature y) => x.Equals(y) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedFunctionPointerSignature(in ManagedSignatureData data)
        {
            var c = data.GetLastCode();
            if (c.Data.Kind != ManagedSignatureKind.FunctionPointer)
                throw new InvalidCastException($"Signature of type {c.Data.Kind} is not of kind FunctionPointer.");

            this.data = data;
        }

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override readonly bool Equals(object obj) => obj switch
        {
            ManagedSignature s => Equals(s),
            ManagedFunctionPointerSignature s => Equals(s),
            _ => false,
        };

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly bool Equals(in ManagedFunctionPointerSignature obj) => data.Equals(obj.data);

        /// <summary>
        /// Gets a unique hash code for this signature.
        /// </summary>
        /// <returns></returns>
        public override readonly int GetHashCode() => data.GetHashCode();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedFunctionPointerSignature>.Equals(ManagedFunctionPointerSignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

}