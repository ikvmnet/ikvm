using System;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Resolves a "file" diagnostic output value.
    /// </summary>
    class FileDiagnosticChannelFactory : IDiagnosticChannelFactory
    {

        public const string STDOUT = "stdout";
        public const string STDERR = "stderr";

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
            if (spec is null)
                throw new ArgumentNullException(nameof(spec));

            return spec switch
            {
                "1" or STDOUT => new StreamDiagnosticChannel(Console.OpenStandardOutput(), Console.OutputEncoding, leaveOpen: false),
                "2" or STDERR => new StreamDiagnosticChannel(Console.OpenStandardError(), Console.OutputEncoding, leaveOpen: false),
                _ when spec.Contains(Path.DirectorySeparatorChar) => new StreamDiagnosticChannel(File.OpenWrite(spec), Encoding.UTF8, leaveOpen: false),
                _ => null,
            };
        }

    }

}
