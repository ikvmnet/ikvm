#if NETCOREAPP

using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyModel.Resolution;

using System.Text.Json;

namespace IKVM.Tools.Exporter
{

    public partial class IkvmExporterContext
    {

        /// <summary>
        /// Invokes the static instance on the internal type.
        /// </summary>
        class IkvmExporterDispatcher
        {

            readonly IkvmExporterOptions options;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="options"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public IkvmExporterDispatcher(string options)
            {
                if (options is null)
                    throw new ArgumentNullException(nameof(options));

                this.options = JsonSerializer.Deserialize<IkvmExporterOptions>(options);
            }

            /// <summary>
            /// Executes the exporter.
            /// </summary>
            /// <returns></returns>
            public int Execute()
            {
                return IkvmExporterInternal.Execute(options);
            }

        }

        class IsolatedAssemblyLoadContext : AssemblyLoadContext
        {

            readonly AssemblyDependencyResolver resolver;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="isCollectible"></param>
            public IsolatedAssemblyLoadContext(string name, bool isCollectible = false) :
                base(name, isCollectible)
            {
                resolver = new AssemblyDependencyResolver(Assembly.GetEntryAssembly().Location);
            }

            protected override Assembly Load(AssemblyName assemblyName)
            {
                return resolver.ResolveAssemblyToPath(assemblyName) is string p ? base.LoadFromAssemblyPath(p) : base.Load(assemblyName);
            }

        }

        IsolatedAssemblyLoadContext context;
        object dispatcher;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmExporterContext(IkvmExporterOptions options)
        {
            // load the exporter in a nested assembly context
            context = new IsolatedAssemblyLoadContext("IkvmExporter", true);
            var asm = context.LoadFromAssemblyName(typeof(IkvmExporterDispatcher).Assembly.GetName());
            var typ = asm.GetType(typeof(IkvmExporterDispatcher).FullName);
            dispatcher = Activator.CreateInstance(typ, new[] { JsonSerializer.Serialize(options) });
        }

        /// <summary>
        /// Creates a new context for the execution.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public partial async Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(() => (int)dispatcher.GetType().GetMethod(nameof(IkvmExporterDispatcher.Execute), BindingFlags.Public | BindingFlags.Instance).Invoke(dispatcher, Array.Empty<object>()));
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            try
            {
                dispatcher = null;
                if (context != null)
                    context.Unload();
            }
            finally
            {
                context = null;
            }

            GC.SuppressFinalize(this);
        }

    }

}

#endif
