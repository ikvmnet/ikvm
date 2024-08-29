using System;
using System.IO;
using System.Linq;
using System.Text;

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
                return new StreamDiagnosticChannel(File.OpenWrite(spec), Encoding.UTF8, leaveOpen: false);

            if (spec is "1" or "stdout")
                return new StreamDiagnosticChannel(Console.OpenStandardOutput(), Console.OutputEncoding, leaveOpen: false);
            if (spec is "2" or "stderr")
                return new StreamDiagnosticChannel(Console.OpenStandardError(), Console.OutputEncoding, leaveOpen: false);

            return null;
        }

    }

}
