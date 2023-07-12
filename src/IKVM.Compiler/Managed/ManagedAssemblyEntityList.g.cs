using System;
using System.Collections;
using System.Collections.Generic;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    internal readonly partial struct ManagedAssemblyReferenceList : IReadOnlyList<ManagedAssemblyReference>, IEnumerable<ManagedAssemblyReference>
    {

        /// <summary>
        /// Provides an enumerator that iterates over the items in the list.
        /// </summary>
        public struct Enumerator : IEnumerator<ManagedAssemblyReference>
        {

            readonly ManagedAssembly assembly;
            int index = -1;

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
            public bool MoveNext() => assembly.data.References.Count > ++index;

            /// <summary>
            /// Gets a reference to the current item.
            /// </summary>
            public readonly ManagedAssemblyReference Current => new ManagedAssemblyReference(assembly, index);

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            public void Reset() => index = -1;

            /// <summary>
            /// Disposes of the enumerator.
            /// </summary>
            public readonly void Dispose() { }

            /// <inheritdoc />
            readonly ManagedAssemblyReference IEnumerator<ManagedAssemblyReference>.Current => Current;

            /// <inheritdoc />
            readonly object IEnumerator.Current => Current;

        }

        readonly ManagedAssembly assembly;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        public ManagedAssemblyReferenceList(ManagedAssembly assembly) => this.assembly = assembly;

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly ManagedAssemblyReference this[int index] => new ManagedAssemblyReference(assembly, index);

        /// <summary>
        /// Gets the count of items in the list.
        /// </summary>
        public readonly int Count => assembly.data.References.Count;

        /// <summary>
        /// Gets an enumerator that iterates over the items in the list.
        /// </summary>
        /// <returns></returns>
        public readonly Enumerator GetEnumerator() => new(assembly);

        /// <inheritdoc />
        ManagedAssemblyReference IReadOnlyList<ManagedAssemblyReference>.this[int index] => this[index];

        /// <inheritdoc />
        IEnumerator<ManagedAssemblyReference> IEnumerable<ManagedAssemblyReference>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

    internal readonly partial struct ManagedAssemblyCustomAttributeList : IReadOnlyList<ManagedAssemblyCustomAttribute>, IEnumerable<ManagedAssemblyCustomAttribute>
    {

        /// <summary>
        /// Provides an enumerator that iterates over the items in the list.
        /// </summary>
        public struct Enumerator : IEnumerator<ManagedAssemblyCustomAttribute>
        {

            readonly ManagedAssembly assembly;
            int index = -1;

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
            public bool MoveNext() => assembly.data.CustomAttributes.Count > ++index;

            /// <summary>
            /// Gets a reference to the current item.
            /// </summary>
            public readonly ManagedAssemblyCustomAttribute Current => new ManagedAssemblyCustomAttribute(assembly, index);

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            public void Reset() => index = -1;

            /// <summary>
            /// Disposes of the enumerator.
            /// </summary>
            public readonly void Dispose() { }

            /// <inheritdoc />
            readonly ManagedAssemblyCustomAttribute IEnumerator<ManagedAssemblyCustomAttribute>.Current => Current;

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
        public readonly ManagedAssemblyCustomAttribute this[int index] => new ManagedAssemblyCustomAttribute(assembly, index);

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
        ManagedAssemblyCustomAttribute IReadOnlyList<ManagedAssemblyCustomAttribute>.this[int index] => this[index];

        /// <inheritdoc />
        IEnumerator<ManagedAssemblyCustomAttribute> IEnumerable<ManagedAssemblyCustomAttribute>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}