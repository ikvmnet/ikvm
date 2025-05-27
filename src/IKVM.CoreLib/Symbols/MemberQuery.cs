using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;


namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Stores the result of a member filtering that's filtered by name and visibility from base class (as defined by the Type.Get*() family of apis).
    ///
    /// The results are as if you'd passed in a bindingFlags value of "Public | NonPublic | Instance | Static | FlattenHierarchy"
    ///
    /// Results are sorted by declaring type. The members declared by the most derived type appear first, then those declared by its base class, and so on.
    /// The Disambiguation logic takes advantage of this.
    ///
    /// This object is a good candidate for long term caching.
    ///
    /// MemberQuery's come in two flavors: ImmediateTypeOnly and full. The immediateTypeOnly only holds the results for one type, not any of its
    /// base types. This is used when the binding flags passed to a Get() API limit the search to the immediate type only in order to avoid triggering
    /// unnecessary assembly resolving and a lot of unnecessary ParameterInfo creation and comparison checks.
    /// </summary>
    struct MemberQuery<TParentSymbol, TMemberSymbol> : IEnumerable<TMemberSymbol>
        where TParentSymbol : Symbol
        where TMemberSymbol : MemberSymbol
    {

        static readonly MemberSymbolPolicy<TParentSymbol, TMemberSymbol> _policy = MemberSymbolPolicy<TParentSymbol, TMemberSymbol>.Default;

        /// <summary>
        /// Returns whether, for the specified <see cref="BindingFlags"/>, the query logic needs only to search the the immediate type.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        static bool NeedToSearchImmediateTypeOnly(BindingFlags bindingFlags)
        {
            if ((bindingFlags & (BindingFlags.Static | BindingFlags.FlattenHierarchy)) == (BindingFlags.Static | BindingFlags.FlattenHierarchy))
                return false;

            if ((bindingFlags & (BindingFlags.Instance | BindingFlags.DeclaredOnly)) == BindingFlags.Instance)
                return false;

            return true;
        }

        readonly TParentSymbol _parent;
        readonly string? _name;
        readonly BindingFlags _bindingFlags;
        readonly bool _immediateParentOnly;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="bindingFlags"></param>
        internal MemberQuery(TParentSymbol parent, string? name, BindingFlags bindingFlags)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _name = name;
            _bindingFlags = _policy.ModifyBindingFlags(bindingFlags);
            _immediateParentOnly = NeedToSearchImmediateTypeOnly(_bindingFlags);
        }

        /// <inheritdoc />
        public readonly IEnumerator<TMemberSymbol> GetEnumerator()
        {
            return Query().GetEnumerator();
        }

        /// <summary>
        /// Returns a single member, null or throws <see cref="AmbiguousMatchException"/>, for the TypeSymbol.Get*(string name,...) family of apis.
        /// </summary>
        /// <returns></returns>
        public readonly TMemberSymbol? Disambiguate()
        {
            TMemberSymbol? match = null;

            foreach (var challenger in Query())
            {
                if (match != null)
                {
                    // Assuming the policy says it's ok to ignore the ambiguity, we're to resolve in favor of the member
                    // declared by the most derived type. Since QueriedMemberLists are sorted in order of decreasing derivation,
                    // that means we let the first match win - unless, of course, they're both the "most derived member".
                    if (match.DeclaringType!.Equals(challenger.DeclaringType))
                        throw new AmbiguousMatchException($"Ambiguous match found for '{match.DeclaringType} {match}'.");

                    if (!_policy.OkToIgnoreAmbiguity(match, challenger))
                        throw new AmbiguousMatchException($"Ambiguous match found for '{match.DeclaringType} {match}'.");
                }
                else
                {
                    match = challenger;
                }
            }

            return match;
        }

        /// <summary>
        /// Check that <paramref name="mask"/> are all set on <paramref name="symbol"/>.
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly bool BindingFlagsMatch(BindingFlags mask)
        {
            return (_bindingFlags & mask) == mask;
        }

        /// <inheritdoc />
        readonly IEnumerable<TMemberSymbol> Query()
        {
            // query immediate
            var priorMembers = new List<TMemberSymbol>();
            foreach (var i in QueryImmediate(priorMembers))
                yield return i;

            // query inheritable
            if (_immediateParentOnly == false && _policy.AlwaysTreatAsDeclaredOnly == false)
                for (var currentParent = _policy.GetInheritedParent(_parent); currentParent != null; currentParent = _policy.GetInheritedParent(currentParent))
                    foreach (var i in QueryInherited(currentParent, priorMembers))
                        yield return i;
        }

        /// <summary>
        /// Enumerates the results of the query against the immediate parent.
        /// </summary>
        /// <param name="priorMembers"></param>
        /// <returns></returns>
        readonly IEnumerable<TMemberSymbol> QueryImmediate(List<TMemberSymbol> priorMembers)
        {
            foreach (var member in _policy.GetDeclaredMembers(_parent))
            {
                // skip unmatched name
                if (_name != null && _name != member.Name)
                    continue;

                _policy.GetMemberAttributes(member, out MethodAttributes visibility, out bool isStatic, out _, out _);

                // member might exclude future members
                priorMembers.Add(member);

                // check that query includes flags required by member
                var matchFlags = default(BindingFlags);
                matchFlags |= isStatic ? BindingFlags.Static : BindingFlags.Instance;
                matchFlags |= visibility == MethodAttributes.Public ? BindingFlags.Public : BindingFlags.NonPublic;
                if (BindingFlagsMatch(matchFlags) == false)
                    continue;

                yield return member;
            }
        }

        /// <summary>
        /// Enumerates the results of the query against the inherited parent(s).
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="priorMembers"></param>
        /// <returns></returns>
        readonly IEnumerable<TMemberSymbol> QueryInherited(TParentSymbol parent, List<TMemberSymbol> priorMembers)
        {
            foreach (var member in _policy.GetDeclaredMembers(parent))
            {
                // skip unmatched name
                if (_name != null && _name != member.Name)
                    continue;

                _policy.GetMemberAttributes(member, out MethodAttributes visibility, out bool isStatic, out _, out _);

                // skip private members in inherited query
                if (visibility == MethodAttributes.Private)
                    continue;

                // skip members that are excluded by prior members
                if (priorMembers.Count != 0 && _policy.IsSuppressedByMoreDerivedMember(member, priorMembers))
                    continue;

                // member might exclude future members
                priorMembers.Add(member);

                // check that query includes flags required by member
                var matchFlags = default(BindingFlags);
                matchFlags |= isStatic ? BindingFlags.Static | BindingFlags.FlattenHierarchy : BindingFlags.Instance;
                matchFlags |= visibility == MethodAttributes.Public ? BindingFlags.Public : BindingFlags.NonPublic;
                if (BindingFlagsMatch(matchFlags) == false)
                    continue;

                yield return member;
            }
        }

        /// <inheritdoc />
        readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}