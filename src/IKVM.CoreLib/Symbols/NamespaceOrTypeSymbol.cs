using System;
using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

    abstract class NamespaceOrTypeSymbol : Symbol, INamespaceOrTypeSymbol
    {



        /// <summary>
        /// Returns true if this symbol is "virtual", has an implementation, and does not override a
        /// base class member; i.e., declared with the "virtual" modifier. Does not return true for
        /// members declared as abstract or override.
        /// </summary>
        /// <returns>
        /// Always returns false.
        /// </returns>
        public sealed override bool IsVirtual
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if this symbol was declared to override a base class member; i.e., declared
        /// with the "override" modifier. Still returns true if member was declared to override
        /// something, but (erroneously) no member to override exists.
        /// </summary>
        /// <returns>
        /// Always returns false.
        /// </returns>
        public sealed override bool IsOverride
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if this symbol has external implementation; i.e., declared with the 
        /// "extern" modifier. 
        /// </summary>
        /// <returns>
        /// Always returns false.
        /// </returns>
        public sealed override bool IsExtern
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Get all the members of this symbol.
        /// </summary>
        /// <returns>An ImmutableArray containing all the members of this symbol. If this symbol has no members,
        /// returns an empty ImmutableArray. Never returns null.</returns>
        public abstract ImmutableArray<Symbol> GetMembers();

        /// <summary>
        /// Get all the members of this symbol that have a particular name.
        /// </summary>
        /// <returns>An ImmutableArray containing all the members of this symbol with the given name. If there are
        /// no members with this name, returns an empty ImmutableArray. Never returns null.</returns>
        public abstract ImmutableArray<Symbol> GetMembers(string name);

        /// <summary>
        /// Get all the members of this symbol that are types.
        /// </summary>
        /// <returns>An ImmutableArray containing all the types that are members of this symbol. If this symbol has no type members,
        /// returns an empty ImmutableArray. Never returns null.</returns>
        public abstract ImmutableArray<NamedTypeSymbol> GetTypeMembers();

        /// <summary>
        /// Get all the members of this symbol that are types that have a particular name, of any arity.
        /// </summary>
        /// <returns>An ImmutableArray containing all the types that are members of this symbol with the given name.
        /// If this symbol has no type members with this name,
        /// returns an empty ImmutableArray. Never returns null.</returns>
        public ImmutableArray<NamedTypeSymbol> GetTypeMembers(string name)
            => GetTypeMembers(name.AsMemory());

        /// <summary>
        /// Get all the members of this symbol that are types that have a particular name and arity
        /// </summary>
        /// <returns>An IEnumerable containing all the types that are members of this symbol with the given name and arity.
        /// If this symbol has no type members with this name and arity,
        /// returns an empty IEnumerable. Never returns null.</returns>
        public ImmutableArray<NamedTypeSymbol> GetTypeMembers(string name, int arity)
            => GetTypeMembers(name.AsMemory(), arity);

        /// <inheritdoc cref="GetTypeMembers(string)"/>
        public abstract ImmutableArray<NamedTypeSymbol> GetTypeMembers(ReadOnlyMemory<char> name);

        /// <inheritdoc cref="GetTypeMembers(string, int)"/>
        public virtual ImmutableArray<NamedTypeSymbol> GetTypeMembers(ReadOnlyMemory<char> name, int arity)
        {
            // default implementation does a post-filter. We can override this if its a performance burden, but 
            // experience is that it won't be.
            return GetTypeMembers(name).WhereAsArray(static (t, arity) => t.Arity == arity, arity);
        }

    }

}