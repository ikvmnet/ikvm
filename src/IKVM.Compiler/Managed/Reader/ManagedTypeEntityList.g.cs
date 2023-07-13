using System.Collections;
using System.Collections.Generic;

namespace IKVM.Compiler.Managed.Reader
{

    internal readonly partial struct ManagedTypeCustomAttributeList : IReadOnlyList<ManagedTypeCustomAttribute>, IEnumerable<ManagedTypeCustomAttribute>
    {

        /// <summary>
        /// Provides an enumerator that iterates over the items in the list.
        /// </summary>
        public struct Enumerator : IEnumerator<ManagedTypeCustomAttribute>
        {

            readonly ManagedType type;
            int index = -1;

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
            public bool MoveNext() => type.data.CustomAttributes.Count > ++index;

            /// <summary>
            /// Gets a reference to the current item.
            /// </summary>
            public readonly ManagedTypeCustomAttribute Current => new ManagedTypeCustomAttribute(type, index);

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            public void Reset() => index = -1;

            /// <summary>
            /// Disposes of the enumerator.
            /// </summary>
            public readonly void Dispose() { }

            /// <inheritdoc />
            readonly ManagedTypeCustomAttribute IEnumerator<ManagedTypeCustomAttribute>.Current => Current;

            /// <inheritdoc />
            readonly object IEnumerator.Current => Current;

        }

        readonly ManagedType type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        public ManagedTypeCustomAttributeList(ManagedType type) => this.type = type;

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly ManagedTypeCustomAttribute this[int index] => new ManagedTypeCustomAttribute(type, index);

        /// <summary>
        /// Gets the count of items in the list.
        /// </summary>
        public readonly int Count => type.data.CustomAttributes.Count;

        /// <summary>
        /// Gets an enumerator that iterates over the items in the list.
        /// </summary>
        /// <returns></returns>
        public readonly Enumerator GetEnumerator() => new(type);

        /// <inheritdoc />
        ManagedTypeCustomAttribute IReadOnlyList<ManagedTypeCustomAttribute>.this[int index] => this[index];

        /// <inheritdoc />
        IEnumerator<ManagedTypeCustomAttribute> IEnumerable<ManagedTypeCustomAttribute>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

    internal readonly partial struct ManagedTypeInterfaceList : IReadOnlyList<ManagedTypeInterface>, IEnumerable<ManagedTypeInterface>
    {

        /// <summary>
        /// Provides an enumerator that iterates over the items in the list.
        /// </summary>
        public struct Enumerator : IEnumerator<ManagedTypeInterface>
        {

            readonly ManagedType type;
            int index = -1;

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
            public bool MoveNext() => type.data.Interfaces.Count > ++index;

            /// <summary>
            /// Gets a reference to the current item.
            /// </summary>
            public readonly ManagedTypeInterface Current => new ManagedTypeInterface(type, index);

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            public void Reset() => index = -1;

            /// <summary>
            /// Disposes of the enumerator.
            /// </summary>
            public readonly void Dispose() { }

            /// <inheritdoc />
            readonly ManagedTypeInterface IEnumerator<ManagedTypeInterface>.Current => Current;

            /// <inheritdoc />
            readonly object IEnumerator.Current => Current;

        }

        readonly ManagedType type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        public ManagedTypeInterfaceList(ManagedType type) => this.type = type;

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly ManagedTypeInterface this[int index] => new ManagedTypeInterface(type, index);

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
        ManagedTypeInterface IReadOnlyList<ManagedTypeInterface>.this[int index] => this[index];

        /// <inheritdoc />
        IEnumerator<ManagedTypeInterface> IEnumerable<ManagedTypeInterface>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

    internal readonly partial struct ManagedTypeFieldList : IReadOnlyList<ManagedTypeField>, IEnumerable<ManagedTypeField>
    {

        /// <summary>
        /// Provides an enumerator that iterates over the items in the list.
        /// </summary>
        public struct Enumerator : IEnumerator<ManagedTypeField>
        {

            readonly ManagedType type;
            int index = -1;

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
            public bool MoveNext() => type.data.Fields.Count > ++index;

            /// <summary>
            /// Gets a reference to the current item.
            /// </summary>
            public readonly ManagedTypeField Current => new ManagedTypeField(type, index);

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            public void Reset() => index = -1;

            /// <summary>
            /// Disposes of the enumerator.
            /// </summary>
            public readonly void Dispose() { }

            /// <inheritdoc />
            readonly ManagedTypeField IEnumerator<ManagedTypeField>.Current => Current;

            /// <inheritdoc />
            readonly object IEnumerator.Current => Current;

        }

        readonly ManagedType type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        public ManagedTypeFieldList(ManagedType type) => this.type = type;

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly ManagedTypeField this[int index] => new ManagedTypeField(type, index);

        /// <summary>
        /// Gets the count of items in the list.
        /// </summary>
        public readonly int Count => type.data.Fields.Count;

        /// <summary>
        /// Gets an enumerator that iterates over the items in the list.
        /// </summary>
        /// <returns></returns>
        public readonly Enumerator GetEnumerator() => new(type);

        /// <inheritdoc />
        ManagedTypeField IReadOnlyList<ManagedTypeField>.this[int index] => this[index];

        /// <inheritdoc />
        IEnumerator<ManagedTypeField> IEnumerable<ManagedTypeField>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

    internal readonly partial struct ManagedTypeMethodList : IReadOnlyList<ManagedTypeMethod>, IEnumerable<ManagedTypeMethod>
    {

        /// <summary>
        /// Provides an enumerator that iterates over the items in the list.
        /// </summary>
        public struct Enumerator : IEnumerator<ManagedTypeMethod>
        {

            readonly ManagedType type;
            int index = -1;

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
            public bool MoveNext() => type.data.Methods.Count > ++index;

            /// <summary>
            /// Gets a reference to the current item.
            /// </summary>
            public readonly ManagedTypeMethod Current => new ManagedTypeMethod(type, index);

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            public void Reset() => index = -1;

            /// <summary>
            /// Disposes of the enumerator.
            /// </summary>
            public readonly void Dispose() { }

            /// <inheritdoc />
            readonly ManagedTypeMethod IEnumerator<ManagedTypeMethod>.Current => Current;

            /// <inheritdoc />
            readonly object IEnumerator.Current => Current;

        }

        readonly ManagedType type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        public ManagedTypeMethodList(ManagedType type) => this.type = type;

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly ManagedTypeMethod this[int index] => new ManagedTypeMethod(type, index);

        /// <summary>
        /// Gets the count of items in the list.
        /// </summary>
        public readonly int Count => type.data.Methods.Count;

        /// <summary>
        /// Gets an enumerator that iterates over the items in the list.
        /// </summary>
        /// <returns></returns>
        public readonly Enumerator GetEnumerator() => new(type);

        /// <inheritdoc />
        ManagedTypeMethod IReadOnlyList<ManagedTypeMethod>.this[int index] => this[index];

        /// <inheritdoc />
        IEnumerator<ManagedTypeMethod> IEnumerable<ManagedTypeMethod>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

    internal readonly partial struct ManagedTypePropertyList : IReadOnlyList<ManagedTypeProperty>, IEnumerable<ManagedTypeProperty>
    {

        /// <summary>
        /// Provides an enumerator that iterates over the items in the list.
        /// </summary>
        public struct Enumerator : IEnumerator<ManagedTypeProperty>
        {

            readonly ManagedType type;
            int index = -1;

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
            public bool MoveNext() => type.data.Properties.Count > ++index;

            /// <summary>
            /// Gets a reference to the current item.
            /// </summary>
            public readonly ManagedTypeProperty Current => new ManagedTypeProperty(type, index);

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            public void Reset() => index = -1;

            /// <summary>
            /// Disposes of the enumerator.
            /// </summary>
            public readonly void Dispose() { }

            /// <inheritdoc />
            readonly ManagedTypeProperty IEnumerator<ManagedTypeProperty>.Current => Current;

            /// <inheritdoc />
            readonly object IEnumerator.Current => Current;

        }

        readonly ManagedType type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        public ManagedTypePropertyList(ManagedType type) => this.type = type;

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly ManagedTypeProperty this[int index] => new ManagedTypeProperty(type, index);

        /// <summary>
        /// Gets the count of items in the list.
        /// </summary>
        public readonly int Count => type.data.Properties.Count;

        /// <summary>
        /// Gets an enumerator that iterates over the items in the list.
        /// </summary>
        /// <returns></returns>
        public readonly Enumerator GetEnumerator() => new(type);

        /// <inheritdoc />
        ManagedTypeProperty IReadOnlyList<ManagedTypeProperty>.this[int index] => this[index];

        /// <inheritdoc />
        IEnumerator<ManagedTypeProperty> IEnumerable<ManagedTypeProperty>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

    internal readonly partial struct ManagedTypeEventList : IReadOnlyList<ManagedTypeEvent>, IEnumerable<ManagedTypeEvent>
    {

        /// <summary>
        /// Provides an enumerator that iterates over the items in the list.
        /// </summary>
        public struct Enumerator : IEnumerator<ManagedTypeEvent>
        {

            readonly ManagedType type;
            int index = -1;

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
            public bool MoveNext() => type.data.Events.Count > ++index;

            /// <summary>
            /// Gets a reference to the current item.
            /// </summary>
            public readonly ManagedTypeEvent Current => new ManagedTypeEvent(type, index);

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            public void Reset() => index = -1;

            /// <summary>
            /// Disposes of the enumerator.
            /// </summary>
            public readonly void Dispose() { }

            /// <inheritdoc />
            readonly ManagedTypeEvent IEnumerator<ManagedTypeEvent>.Current => Current;

            /// <inheritdoc />
            readonly object IEnumerator.Current => Current;

        }

        readonly ManagedType type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        public ManagedTypeEventList(ManagedType type) => this.type = type;

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly ManagedTypeEvent this[int index] => new ManagedTypeEvent(type, index);

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
        ManagedTypeEvent IReadOnlyList<ManagedTypeEvent>.this[int index] => this[index];

        /// <inheritdoc />
        IEnumerator<ManagedTypeEvent> IEnumerable<ManagedTypeEvent>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}