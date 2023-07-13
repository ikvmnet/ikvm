using System.Collections;
using System.Collections.Generic;
using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Reader
{

    internal readonly partial struct ManagedTypeMethodGenericParameterConstraintList : IReadOnlyList<ManagedTypeMethodGenericParameterConstraint>, IEnumerable<ManagedTypeMethodGenericParameterConstraint>
    {

        /// <summary>
        /// Provides an enumerator that iterates over the items in the list.
        /// </summary>
        public struct Enumerator : IEnumerator<ManagedTypeMethodGenericParameterConstraint>
        {

            readonly ManagedTypeMethodGenericParameter parameter;
            int index = -1;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="parameter"></param>
            public Enumerator(ManagedTypeMethodGenericParameter parameter)
            {
                this.parameter = parameter;
            }

            /// <summary>
            /// Moves to the next item.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext() => parameter.method.type.data.Methods.GetItemRef(parameter.index).Parameters.Count > ++index;

            /// <summary>
            /// Gets a reference to the current item.
            /// </summary>
            public readonly ManagedTypeMethodGenericParameterConstraint Current => new ManagedTypeMethodGenericParameterConstraint(parameter, index);

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            public void Reset() => index = -1;

            /// <summary>
            /// Disposes of the enumerator.
            /// </summary>
            public readonly void Dispose() { }

            /// <inheritdoc />
            readonly ManagedTypeMethodGenericParameterConstraint IEnumerator<ManagedTypeMethodGenericParameterConstraint>.Current => Current;

            /// <inheritdoc />
            readonly object IEnumerator.Current => Current;

        }

        readonly ManagedTypeMethodGenericParameter parameter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parameter"></param>
        public ManagedTypeMethodGenericParameterConstraintList(ManagedTypeMethodGenericParameter parameter) => this.parameter = parameter;

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly ManagedTypeMethodGenericParameterConstraint this[int index] => new ManagedTypeMethodGenericParameterConstraint(parameter, index);

        /// <summary>
        /// Gets the count of items in the list.
        /// </summary>
        public readonly int Count => parameter.method.type.data.Methods.GetItemRef(parameter.index).Parameters.Count;

        /// <summary>
        /// Gets an enumerator that iterates over the items in the list.
        /// </summary>
        /// <returns></returns>
        public readonly Enumerator GetEnumerator() => new(parameter);

        /// <inheritdoc />
        ManagedTypeMethodGenericParameterConstraint IReadOnlyList<ManagedTypeMethodGenericParameterConstraint>.this[int index] => this[index];

        /// <inheritdoc />
        IEnumerator<ManagedTypeMethodGenericParameterConstraint> IEnumerable<ManagedTypeMethodGenericParameterConstraint>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
