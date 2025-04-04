using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Provides the capability to look up diagnostic channels.
    /// </summary>
    class DiagnosticChannelProvider : IDisposable
    {

        readonly IEnumerable<IDiagnosticChannelFactory> _factories;
        readonly ConcurrentDictionary<string, IDiagnosticChannel?> _cache = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="factories"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DiagnosticChannelProvider(IEnumerable<IDiagnosticChannelFactory> factories)
        {
            _factories = factories ?? throw new ArgumentNullException(nameof(factories));
        }

        /// <summary>
        /// Attempts to parse the specification into a diagnostic channel.
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public IDiagnosticChannel? GetOrCreateChannel(string spec)
        {
            return _cache.GetOrAdd(spec, _ => _factories.Select(i => i.CreateChannel(_)).FirstOrDefault(i => i != null));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            foreach (var i in _cache.Values)
                if (i is IDisposable d)
                    d.Dispose();

            _cache.Clear();
        }

    }

}
