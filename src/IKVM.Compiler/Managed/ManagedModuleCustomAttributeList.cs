using System.Collections;
using System.Collections.Generic;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a list of attributes on a <see cref="ManagedModule"/>.
    /// </summary>
    internal readonly struct ManagedModuleCustomAttributeList : IReadOnlyList<ManagedCustomAttributeData>, IEnumerable<ManagedCustomAttributeData>
    {

        /// <summary>
        /// Provides an enumerator that iterates over the items in the list.
        /// </summary>
        public struct Enumerator : IEnumerator<ManagedCustomAttributeData>
        {

            readonly ManagedModule module;
            int pos = -1;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="assembly"></param>
            public Enumerator(ManagedModule assembly)
            {
                this.module = assembly;
            }

            /// <summary>
            /// Moves to the next item.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext() => module.customAttributes.Count > ++pos;

            /// <summary>
            /// Gets a reference to the current item.
            /// </summary>
            public readonly ref readonly ManagedCustomAttributeData Current => ref module.customAttributes.GetItemRef(pos);

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            public void Reset() => pos = -1;

            /// <summary>
            /// Disposes of the enumerator.
            /// </summary>
            public readonly void Dispose() { }

            /// <inheritdoc />
            readonly ManagedCustomAttributeData IEnumerator<ManagedCustomAttributeData>.Current => Current;

            /// <inheritdoc />
            readonly object IEnumerator.Current => Current;

        }

        readonly ManagedModule module;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        public ManagedModuleCustomAttributeList(ManagedModule assembly) => this.module = assembly;

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly ref readonly ManagedCustomAttributeData this[int index] => ref module.customAttributes.GetItemRef(index);

        /// <summary>
        /// Gets the count of items in the list.
        /// </summary>
        public readonly int Count => module.customAttributes.Count;

        /// <summary>
        /// Gets an enumerator that iterates over the items in the list.
        /// </summary>
        /// <returns></returns>
        public readonly Enumerator GetEnumerator() => new(module);

        /// <inheritdoc />
        ManagedCustomAttributeData IReadOnlyList<ManagedCustomAttributeData>.this[int index] => this[index];

        /// <inheritdoc />
        IEnumerator<ManagedCustomAttributeData> IEnumerable<ManagedCustomAttributeData>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}
