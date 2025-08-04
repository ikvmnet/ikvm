using System;
using System.Collections.Immutable;
using System.Linq;

using IKVM.ByteCode;
using IKVM.CoreLib.Collections;

namespace IKVM.CoreLib.Modules
{


    /// <summary>
    /// A package opened by a module, may be qualified or unqualified.
    /// </summary>
    public readonly struct ModuleOpens : IComparable<ModuleOpens>
    {

        readonly ModuleOpensFlag _modifiers;
        readonly string _source;
        readonly ImmutableHashSet<string> _targets;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="source"></param>
        /// <param name="targets"></param>
        public ModuleOpens(ModuleOpensFlag modifiers, string source, ImmutableHashSet<string> targets)
        {
            _modifiers = modifiers;
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _targets = targets ?? throw new ArgumentNullException(nameof(targets));
        }

        /// <summary>
        /// Gets the set of modifiers.
        /// </summary>
        public ModuleOpensFlag Modifiers => _modifiers;

        /// <summary>
        /// Gets the package name.
        /// </summary>
        public string Source => _source;

        /// <summary>
        /// For a qualified export, returns the non-empty and immutable set of the module names to which the
        /// package is exported.For an unqualified export, returns an empty set.
        /// </summary>
        public ImmutableHashSet<string> Targets => _targets;

        /// <inheritdoc />
        public int CompareTo(ModuleOpens other)
        {
            int c = _source.CompareTo(other._source);
            if (c != 0)
                return c;

            // modifiers
            c = _modifiers.CompareTo(other.Modifiers);
            if (c != 0)
                return c;

            // targets
            var t1 = _targets.ToArray();
            var t2 = other._targets.ToArray();
            Array.Sort(t1);
            Array.Sort(t2);
            return LexicographicListComparer<string, StringComparer, string[]>.Default.Compare(t1, t2);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is ModuleOpens other && Equals(other);
        }

        /// <inheritdoc />
        public bool Equals(ModuleOpens other)
        {
            return _modifiers == other._modifiers && _source == other._source && _targets.SetEquals(other._targets);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hc = new HashCode();
            hc.Add(_modifiers);
            hc.Add(_source);

            foreach (var t in _targets)
                hc.Add(t);

            return hc.ToHashCode();
        }

    }

}