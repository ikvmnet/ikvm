#if NETCOREAPP

using System;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IKVM.Tools.Importer
{

    partial class ExecutionContext
    {

        /// <summary>
        /// Invokes the static instance on the internal type.
        /// </summary>
        class ExecutionContextDispatcher
        {

            readonly string[] _args;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="args"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public ExecutionContextDispatcher(string args)
            {
                if (args is null)
                    throw new ArgumentNullException(nameof(args));

                _args = JsonSerializer.Deserialize<string[]>(args);
            }

            /// <summary>
            /// Executes the exporter.
            /// </summary>
            /// <returns></returns>
            public int Execute()
            {
                return Task.Run(() => ImportTool.ExecuteInContext(_args)).GetAwaiter().GetResult();
            }

        }

        /// <summary>
        /// Manages a separate isolated copy of the assemblies.
        /// </summary>
        class IsolatedAssemblyLoadContext : AssemblyLoadContext
        {

            readonly AssemblyDependencyResolver _resolver;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="isCollectible"></param>
            public IsolatedAssemblyLoadContext(string name, bool isCollectible = true) :
                base(name, isCollectible)
            {
                _resolver = new AssemblyDependencyResolver(Assembly.GetEntryAssembly().Location);
            }

            protected override Assembly Load(AssemblyName assemblyName)
            {
                return _resolver.ResolveAssemblyToPath(assemblyName) is string p ? base.LoadFromAssemblyPath(p) : base.Load(assemblyName);
            }

        }

        IsolatedAssemblyLoadContext _context;
        object _dispatcher;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="args"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ExecutionContext(string[] args)
        {
            // load the importer in a nested assembly context
            _context = new IsolatedAssemblyLoadContext("IkvmImporter", true);
            var asm = _context.LoadFromAssemblyName(typeof(ExecutionContextDispatcher).Assembly.GetName());
            var typ = asm.GetType(typeof(ExecutionContextDispatcher).FullName);
            _dispatcher = Activator.CreateInstance(typ, [JsonSerializer.Serialize(args)]);
        }

        /// <summary>
        /// Creates a new context for the execution.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public partial async Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(() => (int)_dispatcher.GetType().GetMethod(nameof(ExecutionContextDispatcher.Execute), BindingFlags.Public | BindingFlags.Instance).Invoke(_dispatcher, []));
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            try
            {
                _dispatcher = null;
                if (_context != null)
                    _context.Unload();
            }
            finally
            {
                _context = null;
            }

            GC.SuppressFinalize(this);
        }

    }

}

#endif
