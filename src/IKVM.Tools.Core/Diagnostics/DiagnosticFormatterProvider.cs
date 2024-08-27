using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Provides the capability to look up diagnostic channels.
    /// </summary>
    class DiagnosticFormatterProvider
    {

        /// <summary>
        /// Adds the <see cref="DiagnosticFormatterProvider"> to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceCollection Add(IServiceCollection services)
        {
            return services.AddSingleton<DiagnosticFormatterProvider>();
        }

        readonly IEnumerable<IDiagnosticFormatterFactory> _factories;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="factories"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DiagnosticFormatterProvider(IEnumerable<IDiagnosticFormatterFactory> factories)
        {
            _factories = factories ?? throw new ArgumentNullException(nameof(factories));
        }

        /// <summary>
        /// Attempts to parse the specification into a diagnostic channel.
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public IDiagnosticFormatter? FindChannel(string spec)
        {
            return _factories.Select(i => i.GetFormatter(spec)).FirstOrDefault(i => i != null);
        }

    }

}
