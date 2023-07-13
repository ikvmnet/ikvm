using System.Collections;
using System.Collections.Generic;

namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Represents a list of generic parameters on a <see cref="ManagedType"/>.
    /// </summary>
    internal readonly struct ManagedTypeGenericParameterConstraintList : IReadOnlyList<ManagedTypeGenericParameterConstraint>, IEnumerable<ManagedTypeGenericParameterConstraint>
    {

        /// <summary>
        /// Provides an enumerator that iterates over the items in the list.
        /// </summary>
        public struct Enumerator : IEnumerator<ManagedTypeGenericParameterConstraint>
        {

            readonly ManagedTypeGenericParameter parameter;
            int index = -1;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="parameter"></param>
            public Enumerator(ManagedTypeGenericParameter parameter)
            {
                this.parameter = parameter;
            }

            /// <summary>
            /// Moves to the next item.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext() => parameter.type.GenericParameters.Count > ++index;

            /// <summary>
            /// Gets a reference to the current item.
            /// </summary>
            public readonly ManagedTypeGenericParameterConstraint Current => new ManagedTypeGenericParameterConstraint(parameter, index);

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            public void Reset() => index = -1;

            /// <summary>
            /// Disposes of the enumerator.
            /// </summary>
            public readonly void Dispose() { }

            /// <inheritdoc />
            readonly ManagedTypeGenericParameterConstraint IEnumerator<ManagedTypeGenericParameterConstraint>.Current => Current;

            /// <inheritdoc />
            readonly object IEnumerator.Current => Current;

        }

        readonly ManagedTypeGenericParameter parameter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parameter"></param>
        public ManagedTypeGenericParameterConstraintList(ManagedTypeGenericParameter parameter) => this.parameter = parameter;

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly ManagedTypeGenericParameterConstraint this[int index] => new ManagedTypeGenericParameterConstraint(parameter, index);

        /// <summary>
        /// Gets the count of items in the list.
        /// </summary>
        public readonly int Count => parameter.type.GenericParameters.Count;

        /// <summary>
        /// Gets an enumerator that iterates over the items in the list.
        /// </summary>
        /// <returns></returns>
        public readonly Enumerator GetEnumerator() => new(parameter);

        /// <inheritdoc />
        ManagedTypeGenericParameterConstraint IReadOnlyList<ManagedTypeGenericParameterConstraint>.this[int index] => this[index];

        /// <inheritdoc />
        IEnumerator<ManagedTypeGenericParameterConstraint> IEnumerable<ManagedTypeGenericParameterConstraint>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}
