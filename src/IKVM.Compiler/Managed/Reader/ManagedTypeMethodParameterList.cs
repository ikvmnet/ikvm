using System.Collections;
using System.Collections.Generic;
using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Reader
{

    internal readonly partial struct ManagedTypeMethodParameterList : IReadOnlyList<ManagedTypeMethodParameter>, IEnumerable<ManagedTypeMethodParameter>
    {

        /// <summary>
        /// Provides an enumerator that iterates over the items in the list.
        /// </summary>
        public struct Enumerator : IEnumerator<ManagedTypeMethodParameter>
        {

            readonly ManagedTypeMethod method;
            int index = -1;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="method"></param>
            public Enumerator(ManagedTypeMethod method)
            {
                this.method = method;
            }

            /// <summary>
            /// Moves to the next item.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext() => method.type.data.Methods.GetItemRef(method.index).Parameters.Count > ++index;

            /// <summary>
            /// Gets a reference to the current item.
            /// </summary>
            public readonly ManagedTypeMethodParameter Current => new ManagedTypeMethodParameter(method, index);

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            public void Reset() => index = -1;

            /// <summary>
            /// Disposes of the enumerator.
            /// </summary>
            public readonly void Dispose() { }

            /// <inheritdoc />
            readonly ManagedTypeMethodParameter IEnumerator<ManagedTypeMethodParameter>.Current => Current;

            /// <inheritdoc />
            readonly object IEnumerator.Current => Current;

        }

        readonly ManagedTypeMethod method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        public ManagedTypeMethodParameterList(ManagedTypeMethod method) => this.method = method;

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly ManagedTypeMethodParameter this[int index] => new ManagedTypeMethodParameter(method, index);

        /// <summary>
        /// Gets the count of items in the list.
        /// </summary>
        public readonly int Count => method.type.data.Methods.GetItemRef(method.index).Parameters.Count;

        /// <summary>
        /// Gets an enumerator that iterates over the items in the list.
        /// </summary>
        /// <returns></returns>
        public readonly Enumerator GetEnumerator() => new(method);

        /// <inheritdoc />
        ManagedTypeMethodParameter IReadOnlyList<ManagedTypeMethodParameter>.this[int index] => this[index];

        /// <inheritdoc />
        IEnumerator<ManagedTypeMethodParameter> IEnumerable<ManagedTypeMethodParameter>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
