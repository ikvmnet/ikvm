using System.Collections;
using System.Collections.Generic;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Represents a list of attributes on a <see cref="ManagedModule"/>.
    /// </summary>
    internal readonly struct ManagedModuleCustomAttributeList : IReadOnlyList<ManagedModuleCustomAttribute>, IEnumerable<ManagedModuleCustomAttribute>
    {

        /// <summary>
        /// Provides an enumerator that iterates over the items in the list.
        /// </summary>
        public struct Enumerator : IEnumerator<ManagedModuleCustomAttribute>
        {

            readonly ManagedModule module;
            int index = -1;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="assembly"></param>
            public Enumerator(ManagedModule assembly)
            {
                module = assembly;
            }

            /// <summary>
            /// Moves to the next item.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext() => module.assembly.data.Modules.GetItemRef(module.index).CustomAttributes.Count > ++index;

            /// <summary>
            /// Gets a reference to the current item.
            /// </summary>
            public readonly ManagedModuleCustomAttribute Current => new ManagedModuleCustomAttribute(module, index);

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            public void Reset() => index = -1;

            /// <summary>
            /// Disposes of the enumerator.
            /// </summary>
            public readonly void Dispose() { }

            /// <inheritdoc />
            readonly ManagedModuleCustomAttribute IEnumerator<ManagedModuleCustomAttribute>.Current => Current;

            /// <inheritdoc />
            readonly object IEnumerator.Current => Current;

        }

        readonly ManagedModule module;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        public ManagedModuleCustomAttributeList(ManagedModule module) => this.module = module;

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly ManagedModuleCustomAttribute this[int index] => new ManagedModuleCustomAttribute(module, index);

        /// <summary>
        /// Gets the count of items in the list.
        /// </summary>
        public readonly int Count => module.assembly.data.Modules.GetItemRef(module.index).CustomAttributes.Count;

        /// <summary>
        /// Gets an enumerator that iterates over the items in the list.
        /// </summary>
        /// <returns></returns>
        public readonly Enumerator GetEnumerator() => new(module);

        /// <inheritdoc />
        ManagedModuleCustomAttribute IReadOnlyList<ManagedModuleCustomAttribute>.this[int index] => this[index];

        /// <inheritdoc />
        IEnumerator<ManagedModuleCustomAttribute> IEnumerable<ManagedModuleCustomAttribute>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}
