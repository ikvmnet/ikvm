using System;
using System.Collections.Immutable;
using System.Linq;

using IKVM.CoreLib.Collections;

namespace IKVM.CoreLib.Modules
{


    /// <summary>
    /// A service that a module provides one or more implementations of.
    /// </summary>
    public readonly struct ModuleProvides : IComparable<ModuleProvides>
    {

        readonly string _service;
        readonly ImmutableArray<string> _providers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="providers"></param>
        public ModuleProvides(string service, ImmutableArray<string> providers)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _providers = providers;
        }

        /// <summary>
        /// Gets the binary name of the service type.
        /// </summary>
        public string Service => _service;

        /// <summary>
        /// Gets the binar name of the providers or provider factories.
        /// </summary>
        public ImmutableArray<string> Providers => _providers;

        /// <inheritdoc />
        public int CompareTo(ModuleProvides other)
        {
            // service
            int c = _service.CompareTo(other._service);
            if (c != 0)
                return c;

            // targets
            return LexicographicListComparer<string, StringComparer, ImmutableArray<string>>.Default.Compare(_providers, other._providers);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is ModuleProvides other && Equals(other);
        }

        /// <inheritdoc />
        public bool Equals(ModuleProvides other)
        {
            return _service == other._service && _providers.SequenceEqual(other._providers);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hc = new HashCode();
            hc.Add(_service);

            foreach (var t in _providers)
                hc.Add(t);

            return hc.ToHashCode();
        }

    }

}