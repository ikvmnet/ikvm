using System.Collections;
using System.Collections.Generic;

namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Represents a list of generic parameters on a <see cref="ManagedType"/>.
    /// </summary>
    internal readonly struct ManagedTypeGenericParameterList : IReadOnlyList<ManagedTypeGenericParameter>, IEnumerable<ManagedTypeGenericParameter>
    {

        /// <summary>
        /// Provides an enumerator that iterates over the items in the list.
        /// </summary>
        public struct Enumerator : IEnumerator<ManagedTypeGenericParameter>
        {

            readonly ManagedType type;
            int index = -1;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="type"></param>
            public Enumerator(ManagedType type)
            {
                this.type = type;
            }

            /// <summary>
            /// Moves to the next item.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext() => type.data.GenericParameters.Count > ++index;

            /// <summary>
            /// Gets a reference to the current item.
            /// </summary>
            public readonly ManagedTypeGenericParameter Current => new ManagedTypeGenericParameter(type, index);

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            public void Reset() => index = -1;

            /// <summary>
            /// Disposes of the enumerator.
            /// </summary>
            public readonly void Dispose() { }

            /// <inheritdoc />
            readonly ManagedTypeGenericParameter IEnumerator<ManagedTypeGenericParameter>.Current => Current;

            /// <inheritdoc />
            readonly object IEnumerator.Current => Current;

        }

        readonly ManagedType type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        public ManagedTypeGenericParameterList(ManagedType type) => this.type = type;

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly ManagedTypeGenericParameter this[int index] => new ManagedTypeGenericParameter(type, index);

        /// <summary>
        /// Gets the count of items in the list.
        /// </summary>
        public readonly int Count => type.data.GenericParameters.Count;

        /// <summary>
        /// Gets an enumerator that iterates over the items in the list.
        /// </summary>
        /// <returns></returns>
        public readonly Enumerator GetEnumerator() => new(type);

        /// <inheritdoc />
        ManagedTypeGenericParameter IReadOnlyList<ManagedTypeGenericParameter>.this[int index] => this[index];

        /// <inheritdoc />
        IEnumerator<ManagedTypeGenericParameter> IEnumerable<ManagedTypeGenericParameter>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}
