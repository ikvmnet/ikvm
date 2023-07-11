using System.Collections;
using System.Collections.Generic;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a list of interfaces on a <see cref="ManagedType"/>.
    /// </summary>
    internal readonly struct ManagedTypeInterfaceList : IReadOnlyList<ManagedInterface>, IEnumerable<ManagedInterface>
    {

        /// <summary>
        /// Provides an enumerator that iterates over the items in the list.
        /// </summary>
        internal struct Enumerator : IEnumerator<ManagedInterface>
        {

            readonly ManagedType type;
            int pos = -1;

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
            public bool MoveNext() => type.data.Interfaces.Count > ++pos;

            /// <summary>
            /// Gets a reference to the current item.
            /// </summary>
            public readonly ref readonly ManagedInterface Current => ref type.data.Interfaces.GetItemRef(pos);

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            public void Reset() => pos = -1;

            /// <summary>
            /// Disposes of the enumerator.
            /// </summary>
            public readonly void Dispose() { }

            /// <inheritdoc />
            readonly ManagedInterface IEnumerator<ManagedInterface>.Current => Current;

            /// <inheritdoc />
            readonly object IEnumerator.Current => Current;

        }

        readonly ManagedType type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        public ManagedTypeInterfaceList(ManagedType type) => this.type = type;

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly ref readonly ManagedInterface this[int index] => ref type.data.Interfaces.GetItemRef(index);

        /// <summary>
        /// Gets the count of items in the list.
        /// </summary>
        public readonly int Count => type.data.Interfaces.Count;

        /// <summary>
        /// Gets an enumerator that iterates over the items in the list.
        /// </summary>
        /// <returns></returns>
        public readonly Enumerator GetEnumerator() => new(type);

        /// <inheritdoc />
        ManagedInterface IReadOnlyList<ManagedInterface>.this[int index] => this[index];

        /// <inheritdoc />
        IEnumerator<ManagedInterface> IEnumerable<ManagedInterface>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}
