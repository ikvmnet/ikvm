using System;
using System.IO;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

namespace IKVM.Tools.Core.Diagnostics
{

    class FileDiagnosticChannelFactory : IDiagnosticChannelFactory
    {

        /// <summary>
        /// Adds the JSON diagnostic formater to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection Add(IServiceCollection services)
        {
            return services.AddSingleton<IDiagnosticChannelFactory, FileDiagnosticChannelFactory>();
        }

        /// <inheritdoc />
        public IDiagnosticChannel? GetChannel(string spec)
        {
            if (spec.Contains(Path.DirectorySeparatorChar))
                return new StreamDiagnosticChannel(File.OpenWrite(spec), leaveOpen: false);

            if (spec is "1" or "stdout")
                return new StreamDiagnosticChannel(Console.OpenStandardOutput(), leaveOpen: false);
            if (spec is "2" or "stderr")
                return new StreamDiagnosticChannel(Console.OpenStandardError(), leaveOpen: false);

            return null;
        }

    }

}
