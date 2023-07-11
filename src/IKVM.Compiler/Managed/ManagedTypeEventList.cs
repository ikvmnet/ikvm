using System.Collections;
using System.Collections.Generic;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a list of events on a <see cref="ManagedType"/>.
    /// </summary>
    internal readonly struct ManagedTypeEventList : IReadOnlyList<ManagedEvent>, IEnumerable<ManagedEvent>
    {

        /// <summary>
        /// Provides an enumerator that iterates over the items in the list.
        /// </summary>
        public struct Enumerator : IEnumerator<ManagedEvent>
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
            public bool MoveNext() => type.data.Events.Count > ++pos;

            /// <summary>
            /// Gets a reference to the current item.
            /// </summary>
            public readonly ref readonly ManagedEvent Current => ref type.data.Events.GetItemRef(pos);

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            public void Reset() => pos = -1;

            /// <summary>
            /// Disposes of the enumerator.
            /// </summary>
            public readonly void Dispose() { }

            /// <inheritdoc />
            readonly ManagedEvent IEnumerator<ManagedEvent>.Current => Current;

            /// <inheritdoc />
            readonly object IEnumerator.Current => Current;

        }

        readonly ManagedType type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        public ManagedTypeEventList(ManagedType type) => this.type = type;

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly ref readonly ManagedEvent this[int index] => ref type.data.Events.GetItemRef(index);

        /// <summary>
        /// Gets the count of items in the list.
        /// </summary>
        public readonly int Count => type.data.Events.Count;

        /// <summary>
        /// Gets an enumerator that iterates over the items in the list.
        /// </summary>
        /// <returns></returns>
        public readonly Enumerator GetEnumerator() => new(type);

        /// <inheritdoc />
        ManagedEvent IReadOnlyList<ManagedEvent>.this[int index] => this[index];

        /// <inheritdoc />
        IEnumerator<ManagedEvent> IEnumerable<ManagedEvent>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}
