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

            readonly TestAssemblyResolver resolver;
            readonly ConcurrentDictionary<string, PEReader?> cache = new();

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="resolver"></param>
            public VerifyResolver(TestAssemblyResolver resolver)
            {
                this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            }

            public PEReader? ResolveAssembly(System.Reflection.AssemblyName assemblyName) => Open(assemblyName.Name!);

            public PEReader? ResolveModule(System.Reflection.AssemblyName referencingAssembly, string fileName) => Open(fileName);

            PEReader? Open(string name) => cache.GetOrAdd(name, n => resolver.Resolve(n) is string s ? new PEReader(File.OpenRead(s)) : null);

            public void Dispose()
            {
                foreach (var i in cache.Values)
                    i?.Dispose();
            }

        }

        /// <summary>
        /// Provides resolution for <see cref="MetadataLoadContext"/>.
        /// </summary>
        sealed class LoadContextResolver : MetadataAssemblyResolver
        {

            readonly TestAssemblyResolver resolver;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="resolver"></param>
            public LoadContextResolver(TestAssemblyResolver resolver)
            {
                this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            }

            public override System.Reflection.Assembly? Resolve(MetadataLoadContext context, System.Reflection.AssemblyName assemblyName)
            {
                return resolver.Resolve(assemblyName.Name!) is string s ? context.LoadFromAssemblyPath(s) : null;
            }

        }

        /// <summary>
        /// Creates a new instance for the specified target framework.
        /// </summary>
        /// <param name="tfm"></param>
        /// <param name="searchPaths"></param>
        /// <returns></returns>
        public static TestUniverse Create(string tfm, IEnumerable<string>? searchPaths = null) => new(FrameworkSpec.Get(tfm), UniverseOptions.None, searchPaths);

        /// <summary>
        /// Creates a new instance for the specified target framework with the given options.
        /// </summary>
        /// <param name="tfm"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static TestUniverse Create(string tfm, UniverseOptions options) => new(FrameworkSpec.Get(tfm), options, null);

        /// <summary>
        /// Creates a new instance for the specified target framework, configured the way the IKVM importer configures its
        /// universe.
        /// </summary>
        /// <param name="tfm"></param>
        /// <param name="searchPaths"></param>
        /// <returns></returns>
        public static TestUniverse CreateLikeImporter(string tfm, IEnumerable<string>? searchPaths = null) => new(FrameworkSpec.Get(tfm), UniverseOptions.ResolveMissingMembers | UniverseOptions.EnableFunctionPointers, searchPaths);

        readonly FrameworkSpec framework;
        readonly Universe universe;
        readonly TestAssemblyResolver resolver;
        readonly string tempPath;
        VerifyResolver? verifyResolver;
        ILVerify.Verifier? verifier;
        MetadataLoadContext? loadContext;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="framework"></param>
        /// <param name="options"></param>
        /// <param name="searchPaths"></param>
        TestUniverse(FrameworkSpec framework, UniverseOptions options, IEnumerable<string>? searchPaths)
        {
            this.framework = framework;
            universe = new Universe(options, DotNetSdkUtil.GetCoreLibName(framework.Tfm, framework.TargetFrameworkIdentifier, framework.TargetFrameworkVersion));
            resolver = new TestAssemblyResolver(universe, framework.Tfm, framework.TargetFrameworkIdentifier, framework.TargetFrameworkVersion, searchPaths);
            tempPath = Path.Combine(Path.GetTempPath(), "IKVM.Reflection.Tests", Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Gets the target framework.
        /// </summary>
        public FrameworkSpec Framework => framework;

        /// <summary>
        /// Gets the <see cref="Universe"/> under test.
        /// </summary>
        public Universe Universe => universe;

        /// <summary>
        /// Gets the resolver used to locate reference assemblies.
        /// </summary>
        public TestAssemblyResolver Resolver => resolver;

        /// <summary>
        /// Gets a temporary directory for output, created on first access.
        /// </summary>
        public string TempPath
        {
            get
            {
                Directory.CreateDirectory(tempPath);
                return tempPath;
            }
        }

        /// <summary>
        /// Gets an independent <see cref="MetadataLoadContext"/> over the same reference assemblies.
        /// </summary>
        public MetadataLoadContext LoadContext => loadContext ??= new MetadataLoadContext(new LoadContextResolver(resolver), universe.CoreLibName);

        /// <summary>
        /// Imports the given runtime type into the universe. Types the .NET reference assemblies define outside the core
        /// library, such as the interop attributes, are looked up in their reference assembly.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Type Import(System.Type type)
        {
            if (framework.TargetFrameworkIdentifier == ".NET" && type.Namespace == "System.Runtime.InteropServices" && universe.Load(universe.CoreLibName).GetType(type.FullName!) == null)
                return universe.Load("System.Runtime.InteropServices").GetType(type.FullName!, true)!;

            return universe.Import(type);
        }

        /// <summary>
        /// Gets the type with the given full name from the core library of the <see cref="LoadContext"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public System.Type LoadContextType(string name) => LoadContext.CoreAssembly!.GetType(name, true)!;

        /// <summary>
        /// Defines a new dynamic assembly saved into <see cref="TempPath"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IKVM.Reflection.Emit.AssemblyBuilder DefineAssembly(string name = "Test") => universe.DefineDynamicAssembly(new AssemblyName(name), IKVM.Reflection.Emit.AssemblyBuilderAccess.Save, TempPath);

        /// <summary>
        /// Runs ILVerify over the given file in <see cref="TempPath"/> and asserts no errors were reported.
        /// </summary>
        /// <param name="fileName"></param>
        public void Verify(string fileName)
        {
            verifyResolver ??= new VerifyResolver(resolver);
            verifier ??= CreateVerifier(verifyResolver);

            using var pe = new PEReader(File.OpenRead(Path.Combine(TempPath, fileName)));
            foreach (var v in verifier.Verify(pe))
                v.Code.Should().Be(ILVerify.VerifierError.None, string.Format(v.Message ?? "", v.Args ?? []));
        }

        ILVerify.Verifier CreateVerifier(VerifyResolver resolver)
        {
            var v = new ILVerify.Verifier(resolver, new ILVerify.VerifierOptions() { IncludeMetadataTokensInErrorMessages = true, SanityChecks = true });
            v.SetSystemModuleName(new System.Reflection.AssemblyName(universe.CoreLibName));
            return v;
        }

        /// <summary>
        /// Verifies the given file in <see cref="TempPath"/> and loads it into the <see cref="LoadContext"/>.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public System.Reflection.Assembly VerifyAndLoad(string fileName)
        {
            Verify(fileName);
            return LoadContext.LoadFromAssemblyPath(Path.Combine(TempPath, fileName));
        }

        public void Dispose()
        {
            loadContext?.Dispose();
            verifyResolver?.Dispose();
            universe.Dispose();

            try
            {
                if (Directory.Exists(tempPath))
                    Directory.Delete(tempPath, true);
            }
            catch (IOException)
            {
                // best effort; a file may still be mapped
            }
        }

    }

}
