using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Represents a list of <see cref="TypeSymbolSelector"/>s.
    /// </summary>
    /// <param name="Indexes"></param>
    [CollectionBuilder(typeof(TypeSymbolSelectorList), nameof(Create))]
    public readonly record struct TypeSymbolSelectorList(ImmutableArray<TypeSymbolSelector> Indexes) : IReadOnlyList<TypeSymbolSelector>
    {

        /// <summary>
        /// Creates a new instance of the selector list.
        /// </summary>
        /// <param name="types"></param>
        public static implicit operator TypeSymbolSelectorList(ImmutableArray<TypeSymbol> types)
        {
            var b = ImmutableArray.CreateBuilder<TypeSymbolSelector>(types.Length);
            foreach (var type in types)
                b.Add(type);

            return new(b.DrainToImmutable());
        }

        /// <summary>
        /// Creates a new instance of the selector list.
        /// </summary>
        /// <param name="types"></param>
        public static implicit operator TypeSymbolSelectorList(ReadOnlySpan<TypeSymbol> types)
        {
            var b = ImmutableArray.CreateBuilder<TypeSymbolSelector>(types.Length);
            foreach (var type in types)
                b.Add(type);

            return new(b.DrainToImmutable());
        }

        /// <summary>
        /// Creates a new instance of the selector list.
        /// </summary>
        /// <param name="types"></param>
        public static implicit operator TypeSymbolSelectorList(ImmutableArray<TypeSymbolSelector> types)
        {
            var b = ImmutableArray.CreateBuilder<TypeSymbolSelector>(types.Length);
            foreach (var type in types)
                b.Add(type);

            return new(b.DrainToImmutable());
        }

        /// <summary>
        /// Creates a new instance of the selector list.
        /// </summary>
        /// <param name="types"></param>
        public static implicit operator TypeSymbolSelectorList(ReadOnlySpan<TypeSymbolSelector> types)
        {
            var b = ImmutableArray.CreateBuilder<TypeSymbolSelector>(types.Length);
            foreach (var type in types)
                b.Add(type);

            return new(b.DrainToImmutable());
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static TypeSymbolSelectorList Create(ReadOnlySpan<TypeSymbol> items)
        {
            return items;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static TypeSymbolSelectorList Create(ReadOnlySpan<TypeSymbolSelector> items)
        {
            return items;
        }

        /// <summary>
        /// Gets the indexes.
        /// </summary>
        public readonly ImmutableArray<TypeSymbolSelector> Indexes = Indexes;

        /// <inheritdoc />
        public int Count => Indexes.Length;

        /// <inheritdoc />
        public TypeSymbolSelector this[int index] => Indexes[index];

        /// <inheritdoc />
        public IEnumerator<TypeSymbolSelector> GetEnumerator() => ((IEnumerable<TypeSymbolSelector>)Indexes).GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}
