using System;

namespace IKVM.Compiler.Managed
{

    internal readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedTypeSignature value) => new ManagedSignature(true, value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedTypeSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedTypeSignature(in ManagedSignature value) => new ManagedTypeSignature(true, value.data);

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

    internal readonly partial struct ManagedTypeSignature : IEquatable<ManagedSignature>, IEquatable<ManagedTypeSignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedTypeSignature x, in ManagedTypeSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedTypeSignature x, in ManagedTypeSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedTypeSignature x, in ManagedSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedTypeSignature x, in ManagedSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedSignature x, in ManagedTypeSignature y) => y.Equals(x);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedSignature x, in ManagedTypeSignature y) => y.Equals(x) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedTypeSignature(bool copy, in ManagedSignatureData data)
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
        public readonly override int GetHashCode() => data.GetHashCode();
        
        /// <summary>
        /// Gets a string representation of the type.
        /// </summary>
        /// <returns></returns>
        public readonly override string ToString() => data.ToString();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedTypeSignature>.Equals(ManagedTypeSignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    internal readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedSZArraySignature value) => new ManagedSignature(true, value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSZArraySignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedSZArraySignature(in ManagedSignature value) => new ManagedSZArraySignature(true, value.data);

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

    internal readonly partial struct ManagedSZArraySignature : IEquatable<ManagedSignature>, IEquatable<ManagedSZArraySignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedSZArraySignature x, in ManagedSZArraySignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedSZArraySignature x, in ManagedSZArraySignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedSZArraySignature x, in ManagedSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedSZArraySignature x, in ManagedSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedSignature x, in ManagedSZArraySignature y) => y.Equals(x);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedSignature x, in ManagedSZArraySignature y) => y.Equals(x) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedSZArraySignature(bool copy, in ManagedSignatureData data)
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
        public readonly override int GetHashCode() => data.GetHashCode();
        
        /// <summary>
        /// Gets a string representation of the type.
        /// </summary>
        /// <returns></returns>
        public readonly override string ToString() => data.ToString();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSZArraySignature>.Equals(ManagedSZArraySignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    internal readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedArraySignature value) => new ManagedSignature(true, value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedArraySignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedArraySignature(in ManagedSignature value) => new ManagedArraySignature(true, value.data);

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

    internal readonly partial struct ManagedArraySignature : IEquatable<ManagedSignature>, IEquatable<ManagedArraySignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedArraySignature x, in ManagedArraySignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedArraySignature x, in ManagedArraySignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedArraySignature x, in ManagedSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedArraySignature x, in ManagedSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedSignature x, in ManagedArraySignature y) => y.Equals(x);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedSignature x, in ManagedArraySignature y) => y.Equals(x) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedArraySignature(bool copy, in ManagedSignatureData data)
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
        public readonly override int GetHashCode() => data.GetHashCode();
        
        /// <summary>
        /// Gets a string representation of the type.
        /// </summary>
        /// <returns></returns>
        public readonly override string ToString() => data.ToString();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedArraySignature>.Equals(ManagedArraySignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    internal readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedByRefSignature value) => new ManagedSignature(true, value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedByRefSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedByRefSignature(in ManagedSignature value) => new ManagedByRefSignature(true, value.data);

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

    internal readonly partial struct ManagedByRefSignature : IEquatable<ManagedSignature>, IEquatable<ManagedByRefSignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedByRefSignature x, in ManagedByRefSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedByRefSignature x, in ManagedByRefSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedByRefSignature x, in ManagedSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedByRefSignature x, in ManagedSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedSignature x, in ManagedByRefSignature y) => y.Equals(x);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedSignature x, in ManagedByRefSignature y) => y.Equals(x) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedByRefSignature(bool copy, in ManagedSignatureData data)
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
        public readonly override int GetHashCode() => data.GetHashCode();
        
        /// <summary>
        /// Gets a string representation of the type.
        /// </summary>
        /// <returns></returns>
        public readonly override string ToString() => data.ToString();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedByRefSignature>.Equals(ManagedByRefSignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    internal readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedGenericSignature value) => new ManagedSignature(true, value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedGenericSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedGenericSignature(in ManagedSignature value) => new ManagedGenericSignature(true, value.data);

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

    internal readonly partial struct ManagedGenericSignature : IEquatable<ManagedSignature>, IEquatable<ManagedGenericSignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedGenericSignature x, in ManagedGenericSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedGenericSignature x, in ManagedGenericSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedGenericSignature x, in ManagedSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedGenericSignature x, in ManagedSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedSignature x, in ManagedGenericSignature y) => y.Equals(x);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedSignature x, in ManagedGenericSignature y) => y.Equals(x) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedGenericSignature(bool copy, in ManagedSignatureData data)
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
        public readonly override int GetHashCode() => data.GetHashCode();
        
        /// <summary>
        /// Gets a string representation of the type.
        /// </summary>
        /// <returns></returns>
        public readonly override string ToString() => data.ToString();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedGenericSignature>.Equals(ManagedGenericSignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    internal readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedGenericConstraintSignature value) => new ManagedSignature(true, value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedGenericConstraintSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedGenericConstraintSignature(in ManagedSignature value) => new ManagedGenericConstraintSignature(true, value.data);

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

    internal readonly partial struct ManagedGenericConstraintSignature : IEquatable<ManagedSignature>, IEquatable<ManagedGenericConstraintSignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedGenericConstraintSignature x, in ManagedGenericConstraintSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedGenericConstraintSignature x, in ManagedGenericConstraintSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedGenericConstraintSignature x, in ManagedSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedGenericConstraintSignature x, in ManagedSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedSignature x, in ManagedGenericConstraintSignature y) => y.Equals(x);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedSignature x, in ManagedGenericConstraintSignature y) => y.Equals(x) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedGenericConstraintSignature(bool copy, in ManagedSignatureData data)
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
        public readonly override int GetHashCode() => data.GetHashCode();
        
        /// <summary>
        /// Gets a string representation of the type.
        /// </summary>
        /// <returns></returns>
        public readonly override string ToString() => data.ToString();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedGenericConstraintSignature>.Equals(ManagedGenericConstraintSignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    internal readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedGenericTypeParameterSignature value) => new ManagedSignature(true, value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedGenericTypeParameterSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedGenericTypeParameterSignature(in ManagedSignature value) => new ManagedGenericTypeParameterSignature(true, value.data);

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

    internal readonly partial struct ManagedGenericTypeParameterSignature : IEquatable<ManagedSignature>, IEquatable<ManagedGenericTypeParameterSignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedGenericTypeParameterSignature x, in ManagedGenericTypeParameterSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedGenericTypeParameterSignature x, in ManagedGenericTypeParameterSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedGenericTypeParameterSignature x, in ManagedSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedGenericTypeParameterSignature x, in ManagedSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedSignature x, in ManagedGenericTypeParameterSignature y) => y.Equals(x);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedSignature x, in ManagedGenericTypeParameterSignature y) => y.Equals(x) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedGenericTypeParameterSignature(bool copy, in ManagedSignatureData data)
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
        public readonly override int GetHashCode() => data.GetHashCode();
        
        /// <summary>
        /// Gets a string representation of the type.
        /// </summary>
        /// <returns></returns>
        public readonly override string ToString() => data.ToString();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedGenericTypeParameterSignature>.Equals(ManagedGenericTypeParameterSignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    internal readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedGenericMethodParameterSignature value) => new ManagedSignature(true, value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedGenericMethodParameterSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedGenericMethodParameterSignature(in ManagedSignature value) => new ManagedGenericMethodParameterSignature(true, value.data);

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

    internal readonly partial struct ManagedGenericMethodParameterSignature : IEquatable<ManagedSignature>, IEquatable<ManagedGenericMethodParameterSignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedGenericMethodParameterSignature x, in ManagedGenericMethodParameterSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedGenericMethodParameterSignature x, in ManagedGenericMethodParameterSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedGenericMethodParameterSignature x, in ManagedSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedGenericMethodParameterSignature x, in ManagedSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedSignature x, in ManagedGenericMethodParameterSignature y) => y.Equals(x);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedSignature x, in ManagedGenericMethodParameterSignature y) => y.Equals(x) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedGenericMethodParameterSignature(bool copy, in ManagedSignatureData data)
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
        public readonly override int GetHashCode() => data.GetHashCode();
        
        /// <summary>
        /// Gets a string representation of the type.
        /// </summary>
        /// <returns></returns>
        public readonly override string ToString() => data.ToString();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedGenericMethodParameterSignature>.Equals(ManagedGenericMethodParameterSignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    internal readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedModifiedSignature value) => new ManagedSignature(true, value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedModifiedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedModifiedSignature(in ManagedSignature value) => new ManagedModifiedSignature(true, value.data);

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

    internal readonly partial struct ManagedModifiedSignature : IEquatable<ManagedSignature>, IEquatable<ManagedModifiedSignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedModifiedSignature x, in ManagedModifiedSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedModifiedSignature x, in ManagedModifiedSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedModifiedSignature x, in ManagedSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedModifiedSignature x, in ManagedSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedSignature x, in ManagedModifiedSignature y) => y.Equals(x);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedSignature x, in ManagedModifiedSignature y) => y.Equals(x) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedModifiedSignature(bool copy, in ManagedSignatureData data)
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
        public readonly override int GetHashCode() => data.GetHashCode();
        
        /// <summary>
        /// Gets a string representation of the type.
        /// </summary>
        /// <returns></returns>
        public readonly override string ToString() => data.ToString();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedModifiedSignature>.Equals(ManagedModifiedSignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    internal readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedPointerSignature value) => new ManagedSignature(true, value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedPointerSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedPointerSignature(in ManagedSignature value) => new ManagedPointerSignature(true, value.data);

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

    internal readonly partial struct ManagedPointerSignature : IEquatable<ManagedSignature>, IEquatable<ManagedPointerSignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedPointerSignature x, in ManagedPointerSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedPointerSignature x, in ManagedPointerSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedPointerSignature x, in ManagedSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedPointerSignature x, in ManagedSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedSignature x, in ManagedPointerSignature y) => y.Equals(x);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedSignature x, in ManagedPointerSignature y) => y.Equals(x) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedPointerSignature(bool copy, in ManagedSignatureData data)
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
        public readonly override int GetHashCode() => data.GetHashCode();
        
        /// <summary>
        /// Gets a string representation of the type.
        /// </summary>
        /// <returns></returns>
        public readonly override string ToString() => data.ToString();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedPointerSignature>.Equals(ManagedPointerSignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

    internal readonly partial struct ManagedSignature
    {

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ManagedSignature(in ManagedFunctionPointerSignature value) => new ManagedSignature(true, value.data);

        /// <summary>
        /// Casts the given signature to a <see cref="ManagedFunctionPointerSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ManagedFunctionPointerSignature(in ManagedSignature value) => new ManagedFunctionPointerSignature(true, value.data);

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

    internal readonly partial struct ManagedFunctionPointerSignature : IEquatable<ManagedSignature>, IEquatable<ManagedFunctionPointerSignature>
    {
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedFunctionPointerSignature x, in ManagedFunctionPointerSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedFunctionPointerSignature x, in ManagedFunctionPointerSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedFunctionPointerSignature x, in ManagedSignature y) => x.Equals(y);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedFunctionPointerSignature x, in ManagedSignature y) => x.Equals(y) == false;
    
        /// <summary>
        /// Returns <c>true</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator ==(in ManagedSignature x, in ManagedFunctionPointerSignature y) => y.Equals(x);
        
        /// <summary>
        /// Returns <c>false</c> if the two signatures are equal to each other.
        /// </summary>
        /// <param name="x"></param
        /// <param name="y"></param>
        public static bool operator !=(in ManagedSignature x, in ManagedFunctionPointerSignature y) => y.Equals(x) == false;

        internal readonly ManagedSignatureData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        internal ManagedFunctionPointerSignature(bool copy, in ManagedSignatureData data)
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
        public readonly override int GetHashCode() => data.GetHashCode();
        
        /// <summary>
        /// Gets a string representation of the type.
        /// </summary>
        /// <returns></returns>
        public readonly override string ToString() => data.ToString();

        /// <inheritdoc />
        readonly bool IEquatable<ManagedFunctionPointerSignature>.Equals(ManagedFunctionPointerSignature other) => Equals(other);

        /// <inheritdoc />
        readonly bool IEquatable<ManagedSignature>.Equals(ManagedSignature other) => Equals(other);

    }

}