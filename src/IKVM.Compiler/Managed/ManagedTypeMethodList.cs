using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a list of methods on a <see cref="ManagedType"/>.
    /// </summary>
    internal readonly struct ManagedTypeMethodList : IReadOnlyList<ManagedMethod>, IEnumerable<ManagedMethod>
    {

        /// <summary>
        /// Provides an enumerator that iterates over the items in the list.
        /// </summary>
        public struct Enumerator : IEnumerator<ManagedMethod>
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
            public bool MoveNext() => type.data.Methods.Count > ++pos;

            /// <summary>
            /// Gets a reference to the current item.
            /// </summary>
            public readonly ref readonly ManagedMethod Current => ref type.data.Methods.GetItemRef(pos);

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            public void Reset() => pos = -1;

            /// <summary>
            /// Disposes of the enumerator.
            /// </summary>
            public readonly void Dispose() { }

            /// <inheritdoc />
            readonly ManagedMethod IEnumerator<ManagedMethod>.Current => Current;

            /// <inheritdoc />
            readonly object IEnumerator.Current => Current;

        }

        readonly ManagedType type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        public ManagedTypeMethodList(ManagedType type) => this.type = type;

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly ref readonly ManagedMethod this[int index] => ref type.data.Methods.GetItemRef(index);

        /// <summary>
        /// Gets the count of items in the list.
        /// </summary>
        public readonly int Count => type.data.Methods.Count;

        /// <summary>
        /// Gets the first method, if any, on the type with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public readonly ref readonly ManagedMethod Resolve(string name)
        {
            foreach (ref readonly var m in this)
                if (m.Name == name)
                    return ref m;

            return ref ManagedMethod.Nil;
        }

        /// <summary>
        /// Gets the first method, if any, on the type with the specified name and return type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public readonly ref readonly ManagedMethod Resolve(string name, ReadOnlySequence<ManagedSignature> parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the first method, if any, on the type with the specified name, return type and parameters.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="returnType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public readonly ref readonly ManagedMethod Resolve(string name, ManagedSignature returnType, ReadOnlySequence<ManagedSignature> parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an enumerator that iterates over the items in the list.
        /// </summary>
        /// <returns></returns>
        public readonly Enumerator GetEnumerator() => new(type);

        /// <inheritdoc />
        ManagedMethod IReadOnlyList<ManagedMethod>.this[int index] => this[index];

        /// <inheritdoc />
        IEnumerator<ManagedMethod> IEnumerable<ManagedMethod>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}
