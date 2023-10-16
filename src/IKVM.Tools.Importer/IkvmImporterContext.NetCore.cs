#if NETCOREAPP

using System;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

using System.Text.Json;

namespace IKVM.Tools.Importer
{

    public partial class IkvmImporterContext
    {

        /// <summary>
        /// Invokes the static instance on the internal type.
        /// </summary>
        class IkvmImporterDispatcher
        {

            readonly string[] args;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="args"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public IkvmImporterDispatcher(string args)
            {
                if (args is null)
                    throw new ArgumentNullException(nameof(args));

                this.args = JsonSerializer.Deserialize<string[]>(args);
            }

            /// <summary>
            /// Executes the exporter.
            /// </summary>
            /// <returns></returns>
            public int Execute()
            {
                return IkvmImporterInternal.Execute(args);
            }

        }

        /// <summary>
        /// Manages a separate isolated copy of the assemblies.
        /// </summary>
        class IsolatedAssemblyLoadContext : AssemblyLoadContext
        {

            readonly AssemblyDependencyResolver resolver;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="isCollectible"></param>
            public IsolatedAssemblyLoadContext(string name, bool isCollectible = true) :
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
        /// <param name="args"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmImporterContext(string[] args)
        {
            // load the importer in a nested assembly context
            context = new IsolatedAssemblyLoadContext("IkvmImporter", true);
            var asm = context.LoadFromAssemblyName(typeof(IkvmImporterDispatcher).Assembly.GetName());
            var typ = asm.GetType(typeof(IkvmImporterDispatcher).FullName);
            dispatcher = Activator.CreateInstance(typ, new[] { JsonSerializer.Serialize(args) });
        }

        /// <summary>
        /// Creates a new context for the execution.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public partial async Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(() => (int)dispatcher.GetType().GetMethod(nameof(IkvmImporterDispatcher.Execute), BindingFlags.Public | BindingFlags.Instance).Invoke(dispatcher, Array.Empty<object>()));
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
