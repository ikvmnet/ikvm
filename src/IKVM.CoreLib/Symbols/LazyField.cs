#nullable disable

using System.Runtime.CompilerServices;
using System.Threading;

namespace IKVM.CoreLib.Symbols
{

    struct LazyField
    {

        /// <summary>
        /// Sentinal value to represent null (non-default).
        /// </summary>
        internal static readonly object Null = new object();

    }

    struct LazyField<T>
        where T : class
    {

        /// <summary>
        /// Initializes the value of the field.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T InterlockedInitialize(ref LazyField<T> field, T value)
        {
            // we will store null as sentinal
            var v = value ?? LazyField.Null;

            // field is currently true-null (default)
            if (field._field is null)
                Interlocked.CompareExchange(ref field._field, v, null);

            // return nullable value
            return ReferenceEquals(field._field, LazyField.Null) ? null! : Unsafe.As<T>(field._field);
        }

        object _field;

        /// <summary>
        /// Returns whether the field is defaulted.
        /// </summary>
        public readonly bool IsDefault => _field is null;

        /// <summary>
        /// Returns whether the field is defaulted.
        /// </summary>
        public readonly T Value => ReferenceEquals(_field, LazyField.Null) ? null! : Unsafe.As<T>(_field!);

        /// <summary>
        /// Initializes the field with the given value and returns it, or returns the existing value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public T InterlockedInitialize(T value) => InterlockedInitialize(ref this, value);

    }

}

#nullable restore
