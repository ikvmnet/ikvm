using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.PortableExecutable;

using FluentAssertions;

using IKVM.Tests.Util;

namespace IKVM.Reflection.Tests
{

    /// <summary>
    /// Bundles a <see cref="Universe"/> configured for a target framework with the tools needed to verify what is
    /// written by it: a temporary output directory, ILVerify, and a <see cref="MetadataLoadContext"/> to read output back
    /// independently of IKVM.Reflection.
    /// </summary>
    sealed class TestUniverse : IDisposable
    {

        /// <summary>
        /// Provides resolution for ILVerify.
        /// </summary>
        sealed class VerifyResolver : ILVerify.IResolver, IDisposable
        {

            readonly TestAssemblyResolver _resolver;
            readonly ConcurrentDictionary<string, PEReader?> _cache = new();

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="resolver">The resolver that locates assembly files.</param>
            /// <exception cref="ArgumentNullException"><paramref name="resolver"/> is <see langword="null"/>.</exception>
            public VerifyResolver(TestAssemblyResolver resolver)
            {
                _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            }

            /// <inheritdoc />
            public PEReader? ResolveAssembly(System.Reflection.AssemblyName assemblyName) => Open(assemblyName.Name!);

            /// <inheritdoc />
            public PEReader? ResolveModule(System.Reflection.AssemblyName referencingAssembly, string fileName) => Open(fileName);

            /// <summary>
            /// Opens a <see cref="PEReader"/> over the named assembly or module, caching the result so each file is opened
            /// once.
            /// </summary>
            /// <param name="name">The assembly simple name or module file name.</param>
            /// <returns>The reader, or <see langword="null"/> if the file cannot be found.</returns>
            PEReader? Open(string name) => _cache.GetOrAdd(name, n => _resolver.Resolve(n) is string s ? new PEReader(File.OpenRead(s)) : null);

            /// <summary>
            /// Disposes every reader that was opened.
            /// </summary>
            public void Dispose()
            {
                foreach (var i in _cache.Values)
                    i?.Dispose();
            }

        }

        /// <summary>
        /// Provides resolution for <see cref="MetadataLoadContext"/>.
        /// </summary>
        sealed class LoadContextResolver : MetadataAssemblyResolver
        {

            readonly TestAssemblyResolver _resolver;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="resolver">The resolver that locates assembly files.</param>
            /// <exception cref="ArgumentNullException"><paramref name="resolver"/> is <see langword="null"/>.</exception>
            public LoadContextResolver(TestAssemblyResolver resolver)
            {
                _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            }

            /// <inheritdoc />
            public override System.Reflection.Assembly? Resolve(MetadataLoadContext context, System.Reflection.AssemblyName assemblyName)
            {
                return _resolver.Resolve(assemblyName.Name!) is string s ? context.LoadFromAssemblyPath(s) : null;
            }

        }

        /// <summary>
        /// Creates a new instance for the specified target framework.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies the universe resolves against.</param>
        /// <param name="searchPaths">Extra directories searched before the reference assemblies, or <see langword="null"/> for none.</param>
        /// <returns>The new instance, which the caller disposes.</returns>
        public static TestUniverse Create(string tfm, IEnumerable<string>? searchPaths = null) => new(FrameworkSpec.Get(tfm), UniverseOptions.None, searchPaths);

        /// <summary>
        /// Creates a new instance for the specified target framework with the given options.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies the universe resolves against.</param>
        /// <param name="options">The options the universe is created with.</param>
        /// <returns>The new instance, which the caller disposes.</returns>
        public static TestUniverse Create(string tfm, UniverseOptions options) => new(FrameworkSpec.Get(tfm), options, null);

        /// <summary>
        /// Creates a new instance for the specified target framework, configured the way the IKVM importer configures its
        /// universe.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies the universe resolves against.</param>
        /// <param name="searchPaths">Extra directories searched before the reference assemblies, or <see langword="null"/> for none.</param>
        /// <returns>The new instance, which the caller disposes.</returns>
        public static TestUniverse CreateLikeImporter(string tfm, IEnumerable<string>? searchPaths = null) => new(FrameworkSpec.Get(tfm), UniverseOptions.ResolveMissingMembers | UniverseOptions.EnableFunctionPointers, searchPaths);

        readonly FrameworkSpec _framework;
        readonly Universe _universe;
        readonly TestAssemblyResolver _resolver;
        readonly string _tempPath;
        VerifyResolver? _verifyResolver;
        ILVerify.Verifier? _verifier;
        MetadataLoadContext? _loadContext;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="framework">The target framework whose reference assemblies the universe resolves against.</param>
        /// <param name="options">The options the universe is created with.</param>
        /// <param name="searchPaths">Extra directories searched before the reference assemblies, or <see langword="null"/> for none.</param>
        TestUniverse(FrameworkSpec framework, UniverseOptions options, IEnumerable<string>? searchPaths)
        {
            _framework = framework;
            _universe = new Universe(options, DotNetSdkUtil.GetCoreLibName(framework.Tfm, framework.TargetFrameworkIdentifier, framework.TargetFrameworkVersion));
            _resolver = new TestAssemblyResolver(_universe, framework.Tfm, framework.TargetFrameworkIdentifier, framework.TargetFrameworkVersion, searchPaths);
            _tempPath = Path.Combine(Path.GetTempPath(), "IKVM.Reflection.Tests", Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Gets the target framework.
        /// </summary>
        public FrameworkSpec Framework => _framework;

        /// <summary>
        /// Gets the <see cref="Universe"/> under test.
        /// </summary>
        public Universe Universe => _universe;

        /// <summary>
        /// Gets the resolver used to locate reference assemblies.
        /// </summary>
        public TestAssemblyResolver Resolver => _resolver;

        /// <summary>
        /// Gets a temporary directory for output, created on first access.
        /// </summary>
        public string TempPath
        {
            get
            {
                Directory.CreateDirectory(_tempPath);
                return _tempPath;
            }
        }

        /// <summary>
        /// Gets an independent <see cref="MetadataLoadContext"/> over the same reference assemblies.
        /// </summary>
        public MetadataLoadContext LoadContext => _loadContext ??= new MetadataLoadContext(new LoadContextResolver(_resolver), _universe.CoreLibName);

        /// <summary>
        /// Imports the given runtime type into the universe. Types the .NET reference assemblies define outside the core
        /// library, such as the interop attributes, are looked up in their reference assembly.
        /// </summary>
        /// <param name="type">The runtime type to import.</param>
        /// <returns>The matching type in the universe.</returns>
        public Type Import(System.Type type)
        {
            if (_framework.TargetFrameworkIdentifier == ".NET" && type.Namespace == "System.Runtime.InteropServices" && _universe.Load(_universe.CoreLibName).GetType(type.FullName!) == null)
                return _universe.Load("System.Runtime.InteropServices").GetType(type.FullName!, true)!;

            return _universe.Import(type);
        }

        /// <summary>
        /// Gets the type with the given full name from the core library of the <see cref="LoadContext"/>.
        /// </summary>
        /// <param name="name">The full name of the type.</param>
        /// <returns>The type, read through the <see cref="LoadContext"/>.</returns>
        public System.Type LoadContextType(string name) => LoadContext.CoreAssembly!.GetType(name, true)!;

        /// <summary>
        /// Defines a new dynamic assembly saved into <see cref="TempPath"/>.
        /// </summary>
        /// <param name="name">The simple name of the assembly.</param>
        /// <returns>The new assembly builder.</returns>
        public IKVM.Reflection.Emit.AssemblyBuilder DefineAssembly(string name = "Test") => _universe.DefineDynamicAssembly(new AssemblyName(name), IKVM.Reflection.Emit.AssemblyBuilderAccess.Save, TempPath);

        /// <summary>
        /// Runs ILVerify over the given file in <see cref="TempPath"/> and asserts no errors were reported.
        /// </summary>
        /// <param name="fileName">The name of the file, relative to <see cref="TempPath"/>.</param>
        public void Verify(string fileName)
        {
            _verifyResolver ??= new VerifyResolver(_resolver);
            _verifier ??= CreateVerifier(_verifyResolver);

            using var pe = new PEReader(File.OpenRead(Path.Combine(TempPath, fileName)));
            foreach (var v in _verifier.Verify(pe))
                v.Code.Should().Be(ILVerify.VerifierError.None, string.Format(v.Message ?? "", v.Args ?? []));
        }

        /// <summary>
        /// Creates the ILVerify verifier, with the core library of the universe as its system module.
        /// </summary>
        /// <param name="resolver">The resolver the verifier locates referenced assemblies with.</param>
        /// <returns>The new verifier.</returns>
        ILVerify.Verifier CreateVerifier(VerifyResolver resolver)
        {
            var v = new ILVerify.Verifier(resolver, new ILVerify.VerifierOptions() { IncludeMetadataTokensInErrorMessages = true, SanityChecks = true });
            v.SetSystemModuleName(new System.Reflection.AssemblyName(_universe.CoreLibName));
            return v;
        }

        /// <summary>
        /// Verifies the given file in <see cref="TempPath"/> and loads it into the <see cref="LoadContext"/>.
        /// </summary>
        /// <param name="fileName">The name of the file, relative to <see cref="TempPath"/>.</param>
        /// <returns>The assembly, loaded into the <see cref="LoadContext"/>.</returns>
        public System.Reflection.Assembly VerifyAndLoad(string fileName)
        {
            Verify(fileName);
            return LoadContext.LoadFromAssemblyPath(Path.Combine(TempPath, fileName));
        }

        /// <summary>
        /// Disposes the load context, the verifier readers and the universe, and deletes the temporary directory where it
        /// can.
        /// </summary>
        public void Dispose()
        {
            _loadContext?.Dispose();
            _verifyResolver?.Dispose();
            _universe.Dispose();

            try
            {
                if (Directory.Exists(_tempPath))
                    Directory.Delete(_tempPath, true);
            }
            catch (IOException)
            {
                // best effort; a file may still be mapped
            }
        }

    }

}
