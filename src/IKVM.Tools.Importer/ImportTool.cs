using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Symbols.IkvmReflection;
using IKVM.Reflection;
using IKVM.Reflection.Diagnostics;
using IKVM.Runtime;
using IKVM.Tools.Core.CommandLine;
using IKVM.Tools.Core.Diagnostics;

using Microsoft.Extensions.DependencyInjection;

namespace IKVM.Tools.Importer
{

    /// <summary>
    /// Main entry point for the application.
    /// </summary>
    public partial class ImportTool
    {

        /// <summary>
        /// Executes the importer against a set of command line options.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<int> InvokeAsync(string[] args, CancellationToken cancellationToken = default)
        {
            using var context = new ExecutionContext(args);
            return await context.ExecuteAsync(cancellationToken);
        }

        /// <summary>
        /// Executes the importer against a set of command line options.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        internal static async Task<int> ExecuteInContext(string[] args, CancellationToken cancellationToken = default)
        {
            return await new ImportTool().InvokeImplAsync(args, cancellationToken);
        }

        /// <summary>
        /// Executes the importer.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<int> ExecuteImplAsync(ImportOptions options, CancellationToken cancellationToken)
        {
            var services = new ServiceCollection();
            services.AddToolsDiagnostics();
            services.AddSingleton(options);
            services.AddSingleton<DiagnosticOptions>(p => GetDiagnosticOptions(p, p.GetRequiredService<ImportOptions>()));
            services.AddSingleton<ImportDiagnosticHandler>(p => GetDiagnostics(p, p.GetRequiredService<ImportOptions>().Log));
            services.AddSingleton<IDiagnosticHandler>(p => p.GetRequiredService<ImportDiagnosticHandler>());
            services.AddSingleton(p => CreateResolver(p.GetRequiredService<IDiagnosticHandler>(), p.GetRequiredService<ImportOptions>()));
            services.AddSingleton(p => p.GetRequiredService<ImportAssemblyResolver>().Universe);
            services.AddSingleton<IkvmReflectionSymbolContext>();
            services.AddSingleton<IRuntimeSymbolResolver, ImportRuntimeSymbolResolver>();
            services.AddSingleton<RuntimeContextOptions>(p => CreateContextOptions(p));
            services.AddSingleton<RuntimeContext>();
            services.AddSingleton<StaticCompiler>();
            services.AddSingleton<ImportContextFactory>();
            using var provider = services.BuildServiceProvider();

            try
            {
                try
                {
                    // convert options to imports
                    var imports = provider.GetRequiredService<ImportContextFactory>().Create(options);
                    if (imports.Count == 0)
                        throw new DiagnosticEventException(DiagnosticEvent.NoTargetsFound());

                    // execute the compiler
                    return await Task.Run(() => Execute(
                        provider.GetRequiredService<RuntimeContext>(),
                        provider.GetRequiredService<StaticCompiler>(),
                        provider.GetRequiredService<IDiagnosticHandler>(),
                        imports));
                }
                catch (DiagnosticEventException e)
                {
                    ExceptionDispatchInfo.Capture(e).Throw();
                    throw;
                }
                catch (Exception e)
                {
                    throw new DiagnosticEventException(DiagnosticEvent.GenericCompilerError(e.Message));
                }
            }
            catch (DiagnosticEventException e)
            {
                provider.GetRequiredService<ImportDiagnosticHandler>().Report(e.Event);
                return e.Event.Diagnostic.Id;
            }
        }

        /// <summary>
        /// Executes the imports.
        /// </summary>
        /// <param name="runtime"></param>
        /// <param name="compiler"></param>
        /// <param name="diagnostic"></param>
        /// <param name="imports"></param>
        /// <returns></returns>
        int Execute(RuntimeContext runtime, StaticCompiler compiler, IDiagnosticHandler diagnostic, List<ImportContext> imports)
        {
            return ImportClassLoader.Compile(runtime, compiler, diagnostic, imports);
        }

        /// <summary>
        /// Creates the universe of types.
        /// </summary>
        /// <returns></returns>
        ImportAssemblyResolver CreateResolver(IDiagnosticHandler diagnostics, ImportOptions options)
        {
            if (diagnostics is null)
                throw new ArgumentNullException(nameof(diagnostics));
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            var universeOptions = UniverseOptions.ResolveMissingMembers | UniverseOptions.EnableFunctionPointers;
            if (options.Deterministic == false)
                universeOptions |= UniverseOptions.DeterministicOutput;

            // discover the core lib from the references
            var coreLibName = FindCoreLibName(options.References, options.Libraries);
            if (coreLibName == null)
            {
                diagnostics.CoreClassesMissing();
                throw new Exception();
            }

            // create a new universe of types
            var universe = new Universe(universeOptions, coreLibName);

            // warn when unable to resolve a member
            universe.ResolvedMissingMember += (Module requestingModule, MemberInfo member) =>
            {
                if (requestingModule != null && member is IKVM.Reflection.Type type)
                    diagnostics.UnableToResolveType(requestingModule.Name, type.FullName, member.Module.FullyQualifiedName);
            };

            // enable embedded symbol writer
            if (options.Debug == ImportDebug.Portable)
                universe.SetSymbolWriterFactory(module => new PortablePdbSymbolWriter(module));

            // universe resolver calls back into import
            var resolver = new ImportAssemblyResolver(universe, options, diagnostics);
            universe.AssemblyResolve += resolver.AssemblyResolve;

            return resolver;
        }

        /// <summary>
        /// Finds the first potential core library in the reference set.
        /// </summary>
        /// <param name="references"></param>
        /// <param name="libpaths"></param>
        /// <returns></returns>
        string FindCoreLibName(IList<string> references, IList<DirectoryInfo> libpaths)
        {
            if (references != null)
                foreach (var reference in references)
                    if (GetAssemblyNameIfCoreLib(reference) is string coreLibName)
                        return coreLibName;

            if (libpaths != null)
                foreach (var libpath in libpaths)
                    foreach (var dll in libpath.GetFiles("*.dll"))
                        if (GetAssemblyNameIfCoreLib(dll.FullName) is string coreLibName)
                            return coreLibName;

            return null;
        }

        /// <summary>
        /// Returns <c>true</c> if the given assembly is a core library.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        string GetAssemblyNameIfCoreLib(string file)
        {
            if (File.Exists(file) == false)
                return null;

            using var st = File.OpenRead(file);
            using var pe = new PEReader(st);
            var mr = pe.GetMetadataReader();

            foreach (var handle in mr.TypeDefinitions)
                if (IsSystemObject(mr, handle))
                    return mr.GetString(mr.GetAssemblyDefinition().Name);

            return null;
        }

        /// <summary>
        /// Returns <c>true</c> if the given type definition handle refers to "System.Object".
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="th"></param>
        /// <returns></returns>
        bool IsSystemObject(MetadataReader reader, TypeDefinitionHandle th)
        {
            var td = reader.GetTypeDefinition(th);
            var ns = reader.GetString(td.Namespace);
            var nm = reader.GetString(td.Name);

            return ns == "System" && nm == "Object";
        }

        /// <summary>
        /// Executes the program.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ExecuteAsync(ImportOptions options, CancellationToken cancellationToken = default)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            return ExecuteImplAsync(options, cancellationToken);
        }

        /// <summary>
        /// Generates a diagnostic instance from the diagnostics options.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="spec"></param>
        /// <returns></returns>
        static ImportDiagnosticHandler GetDiagnostics(IServiceProvider services, string spec)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            if (string.IsNullOrWhiteSpace(spec))
                throw new ArgumentException($"'{nameof(spec)}' cannot be null or whitespace.", nameof(spec));

            return ActivatorUtilities.CreateInstance<ImportDiagnosticHandler>(services, spec);
        }

        /// <summary>
        /// Generates a diagnostic instance from the diagnostics options.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        static DiagnosticOptions GetDiagnosticOptions(IServiceProvider services, ImportOptions options)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            return new DiagnosticOptions()
            {
                NoWarn = options.NoWarn != null && options.NoWarn.Length == 0,
                NoWarnDiagnostics = options.NoWarn?.ToImmutableArray() ?? [],
                WarnAsError = options.WarnAsError != null && options.WarnAsError.Length == 0,
                WarnAsErrorDiagnostics = options.WarnAsError?.ToImmutableArray() ?? [],
            };
        }

        /// <summary>
        /// Creates the <see cref="RuntimeContextOptions"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        static RuntimeContextOptions CreateContextOptions(IServiceProvider services)
        {
            var u = services.GetRequiredService<Universe>();
            var isNetFX = u.CoreLibName == "mscorlib";
            var dynamicSuffix = isNetFX ? RuntimeContextOptions.SignedDefaultDynamicAssemblySuffixAndPublicKey : RuntimeContextOptions.UnsignedDefaultDynamicAssemblySuffixAndPublicKey;
            return new RuntimeContextOptions(services.GetRequiredService<ImportOptions>().Bootstrap, dynamicSuffix);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ImportTool()
        {

        }

        /// <summary>
        /// Executes the exporter.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<int> InvokeImplAsync(string[] args, CancellationToken cancellationToken)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            var stack = new Stack<ImportArgLevel>();
            var level = new ImportArgLevel(0);

            foreach (var token in new Parser().Parse(args).Tokens)
            {
                // open new level, set as current
                if (token.Value == "{")
                {
                    stack.Push(level);
                    level = new ImportArgLevel(level.Depth + 1);
                    continue;
                }

                // close current level, parent becomes current
                if (token.Value == "}")
                {
                    var parent = stack.Pop();
                    parent.Nested.Add(level);
                    level = parent;
                    continue;
                }

                // add arg to current level
                level.Args.Add(token.Value);
            }

            // root command binds the first level, and accepts the nested levels
            var command = new ImportCommand();
            command.SetHandler(options => ExecuteAsync(options), new ImportOptionsBinding(level.Nested.ToArray(), null));

            // execute command
            return await new CommandLineBuilder(command)
                .UseHelp()
                .UseParseDirective()
                .UseTypoCorrections()
                .UseToolParseError()
                .UseExceptionHandler()
                .CancelOnProcessTermination()
                .UseToolErrorExceptionHandler()
                .Build()
                .InvokeAsync(level.Args.ToArray());
        }

    }

}
