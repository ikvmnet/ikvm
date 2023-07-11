using System.Collections;
using System.Collections.Generic;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a list of attributes on a <see cref="ManagedAssembly"/>.
    /// </summary>
    internal readonly struct ManagedAssemblyCustomAttributeList : IReadOnlyList<ManagedCustomAttribute>, IEnumerable<ManagedCustomAttribute>
    {

        /// <summary>
        /// Provides an enumerator that iterates over the items in the list.
        /// </summary>
        public struct Enumerator : IEnumerator<ManagedCustomAttribute>
        {

            readonly ManagedAssembly assembly;
            int pos = -1;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="assembly"></param>
            public Enumerator(ManagedAssembly assembly)
            {
                this.assembly = assembly;
            }

            /// <summary>
            /// Moves to the next item.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext() => assembly.data.CustomAttributes.Count > ++pos;

            /// <summary>
            /// Gets a reference to the current item.
            /// </summary>
            public readonly ref readonly ManagedCustomAttribute Current => ref assembly.data.CustomAttributes.GetItemRef(pos);

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            public void Reset() => pos = -1;

            /// <summary>
            /// Disposes of the enumerator.
            /// </summary>
            public readonly void Dispose() { }

            /// <inheritdoc />
            readonly ManagedCustomAttribute IEnumerator<ManagedCustomAttribute>.Current => Current;

            /// <inheritdoc />
            readonly object IEnumerator.Current => Current;

        }

        readonly ManagedAssembly assembly;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        public ManagedAssemblyCustomAttributeList(ManagedAssembly assembly) => this.assembly = assembly;

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly ref readonly ManagedCustomAttribute this[int index] => ref assembly.data.CustomAttributes.GetItemRef(index);

        /// <summary>
        /// Gets the count of items in the list.
        /// </summary>
        public readonly int Count => assembly.data.CustomAttributes.Count;

        /// <summary>
        /// Gets an enumerator that iterates over the items in the list.
        /// </summary>
        /// <returns></returns>
        public readonly Enumerator GetEnumerator() => new(assembly);

        /// <inheritdoc />
        ManagedCustomAttribute IReadOnlyList<ManagedCustomAttribute>.this[int index] => this[index];

        /// <inheritdoc />
        IEnumerator<ManagedCustomAttribute> IEnumerable<ManagedCustomAttribute>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}
