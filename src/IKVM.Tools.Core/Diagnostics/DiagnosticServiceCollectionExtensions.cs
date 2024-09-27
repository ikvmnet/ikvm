using Microsoft.Extensions.DependencyInjection;

namespace IKVM.Tools.Core.Diagnostics
{

    public static class DiagnosticServiceCollectionExtensions
    {

        /// <summary>
        /// Adds the required services for diagnostics.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddToolsDiagnostics(this IServiceCollection services)
        {
            services.AddSingleton<DiagnosticOptions>();
            services.AddSingleton<DiagnosticChannelProvider>();
            services.AddSingleton<DiagnosticFormatterProvider>();
            services.AddSingleton<IDiagnosticChannelFactory, FileDiagnosticChannelFactory>();
            services.AddSingleton<IDiagnosticFormatterFactory, JsonDiagnosticFormatterFactory>();
            services.AddSingleton<IDiagnosticFormatterFactory, TextDiagnosticFormatterFactory>();
            services.AddSingleton<IDiagnosticFormatterFactory, ConsoleDiagnosticFormatterFactory>();
            services.AddSingleton<FormattedDiagnosticHandler>();
            return services;
        }

    }

}
