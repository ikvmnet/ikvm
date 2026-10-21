using System;
using System.Collections.Concurrent;

using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Linking;
using IKVM.CoreLib.Symbols;

#if IMPORTER
using IKVM.Tools.Importer;
#endif

#if EXPORTER
using IKVM.Tools.Exporter;
#endif

#if IMPORTER == false
using IKVM.Runtime.StubGen;
#endif

namespace IKVM.Runtime
{

    /// <summary>
    /// Maintains services relevant to an instance of the IKVM runtime.
    /// </summary>
    class RuntimeContext : ILinkingContext<RuntimeJavaType, RuntimeJavaMember, RuntimeJavaField, RuntimeJavaMethod>
    {

        readonly RuntimeContextOptions options;
        readonly IDiagnosticHandler diagnostics;
        readonly ISymbolResolver resolver;
        readonly bool bootstrap;
        readonly ConcurrentDictionary<Type, object> singletons = new();

        Types types;
        CoreClasses javaBase;
        AttributeHelper attributeHelper;
        RuntimeClassLoaderFactory classLoaderFactory;
        RuntimeAssemblyClassLoaderFactory assemblyClassLoaderFactory;
        RuntimeManagedJavaTypeFactory managedJavaTypeFactory;
        RuntimeManagedByteCodeJavaTypeFactory managedByteCodeJavaTypeFactory;
        RuntimePrimitiveJavaTypeFactory primitiveJavaTypeFactory;
        RuntimeVerifierJavaTypeFactory verifierJavaTypeFactory;
        Intrinsics intrinsics;

#if IMPORTER == false && EXPORTER == false
        ExceptionHelper exceptionHelper;
#endif

#if IMPORTER == false
        StubGenerator stubGenerator;
#endif

#if EXPORTER == false
        CodeEmitterFactory codeEmitterFactory;
        DynamicClassLoaderFactory dynamicClassLoaderFactory;
        ByteCodeHelperMethods byteCodeHelperMethods;
        Serialization serialization;
        InterlockedMethods interlockedMethods;
        CompilerFactory compilerFactory;
        MethodAnalyzerFactory methodAnalyzerFactory;
        MethodHandleUtil methodHandleUtil;
        Boxer boxer;
#endif

#if IMPORTER
        ProxyGenerator proxyGenerator;
#endif

#if IMPORTER || EXPORTER
        StaticCompiler staticCompiler;
        FakeTypes fakeTypes;
#endif

#if IMPORTER || EXPORTER

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="diagnostics"></param>
        /// <param name="bootstrap"></param>
        /// <param name="staticCompiler"></param>
        public RuntimeContext(RuntimeContextOptions options, IDiagnosticHandler diagnostics, ISymbolResolver resolver, bool bootstrap, StaticCompiler staticCompiler)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.bootstrap = bootstrap;
            this.staticCompiler = staticCompiler;
        }

#else

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="diagnostics"></param>
        /// <param name="resolver"></param>
        /// <param name="bootstrap"></param>
        public RuntimeContext(RuntimeContextOptions options, IDiagnosticHandler diagnostics, ISymbolResolver resolver, bool bootstrap)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.bootstrap = bootstrap;
        }

#endif

        /// <summary>
        /// Gets the <see cref="IDiagnosticHandler"/> where events should be sent.
        /// </summary>
        public IDiagnosticHandler Diagnostics => diagnostics;

        /// <summary>
        /// Gets or creates a new instance in a thread safe manner.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        T GetOrCreateSingleton<T>(ref T value, Func<T> create)
        {
            if (value == null)
                lock (this)
                    value ??= create();

            return value;
        }

        /// <summary>
        /// Gets or creates an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="create"></param>
        /// <returns></returns>
        public T GetOrCreateSingleton<T>(Func<T> create) => (T)singletons.GetOrAdd(typeof(T), _ => create());

        /// <summary>
        /// Gets the <see cref="RuntimeContextOptions"/> associated with this instance of the runtime.
        /// </summary>
        public RuntimeContextOptions Options => options;

        /// <summary>
        /// Gets the <see cref="ISymbolResolver"/> associated with this instance of the runtime.
        /// </summary>
        public ISymbolResolver Resolver => resolver;

        /// <summary>
        /// Gets whether or not the runtime is running in bootstrap mode; that is, we are compiling the Java base assembly itself.
        /// </summary>
        public bool Bootstrap => bootstrap;

        /// <summary>
        /// Gets the <see cref="Types"/> associated with this instance of the runtime.
        /// </summary>
        public Types Types => GetOrCreateSingleton(ref types, () => new Types(this));

        /// <summary>
        /// Gets the <see cref="CoreClasses"/> associated with this instance of the runtime.
        /// </summary>
        public CoreClasses JavaBase => GetOrCreateSingleton(ref javaBase, () => new CoreClasses(this));

        /// <summary>
        /// Gets the <see cref="AttributeHelper"/> associated with this instance of the runtime.
        /// </summary>
        public AttributeHelper AttributeHelper => GetOrCreateSingleton(ref attributeHelper, () => new AttributeHelper(this));

        /// <summary>
        /// Gets the <see cref="RuntimePrimitiveJavaTypeFactory"/> associated with this instance of the runtime.
        /// </summary>
        public RuntimePrimitiveJavaTypeFactory PrimitiveJavaTypeFactory => GetOrCreateSingleton(ref primitiveJavaTypeFactory, () => new RuntimePrimitiveJavaTypeFactory(this));

        /// <summary>
        /// Gets the <see cref="RuntimeVerifierJavaTypeFactory"/> associated with this instance of the runtime.
        /// </summary>
        public RuntimeVerifierJavaTypeFactory VerifierJavaTypeFactory => GetOrCreateSingleton(ref verifierJavaTypeFactory, () => new RuntimeVerifierJavaTypeFactory(this));

        /// <summary>
        /// Gets the <see cref="Intrinsics"/> associated with this instance.
        /// </summary>
        public Intrinsics Intrinsics => GetOrCreateSingleton(ref intrinsics, () => new Intrinsics(this));

#if IMPORTER == false

        /// <summary>
        /// Gets the <see cref="StubGenerator"/> associated with this instance of the runtime.
        /// </summary>
        public StubGenerator StubGenerator => GetOrCreateSingleton(ref stubGenerator, () => new StubGenerator(this));

#endif

#if EXPORTER == false

        /// <summary>
        /// Gets the <see cref="CodeEmitterFactory"/> associated with this instance of the runtime.
        /// </summary>
        public CodeEmitterFactory CodeEmitterFactory => GetOrCreateSingleton(ref codeEmitterFactory, () => new CodeEmitterFactory(this));

        /// <summary>
        /// Gets the <see cref="DynamicClassLoaderFactory"/> associated with this instance of the runtime.
        /// </summary>
        public DynamicClassLoaderFactory DynamicClassLoaderFactory => GetOrCreateSingleton(ref dynamicClassLoaderFactory, () => new DynamicClassLoaderFactory(this));

        /// <summary>
        /// Gets the <see cref="ByteCodeHelperMethods"/> associated with this instance of the runtime.
        /// </summary>
        public ByteCodeHelperMethods ByteCodeHelperMethods => GetOrCreateSingleton(ref byteCodeHelperMethods, () => new ByteCodeHelperMethods(this));

        /// <summary>
        /// Gets the <see cref="Serialization"/> associated with this instance of the runtime.
        /// </summary>
        public Serialization Serialization => GetOrCreateSingleton(ref serialization, () => new Serialization(this));

        /// <summary>
        /// Gets the <see cref="CompilerFactory"/> associated with this instance of the runtime.
        /// </summary>
        /// 
        public CompilerFactory CompilerFactory => GetOrCreateSingleton(ref compilerFactory, () => new CompilerFactory(this, bootstrap));

        /// <summary>
        /// Gets the <see cref="InterlockedMethods"/> associated with this instance of the runtime.
        /// </summary>
        public InterlockedMethods InterlockedMethods => GetOrCreateSingleton(ref interlockedMethods, () => new InterlockedMethods(this));

        /// <summary>
        /// Gets the <see cref="MethodAnalyzerFactory"/> associated with this instance of the runtime.
        /// </summary>
        public MethodAnalyzerFactory MethodAnalyzerFactory => GetOrCreateSingleton(ref methodAnalyzerFactory, () => new MethodAnalyzerFactory(this));

        /// <summary>
        /// Gets the <see cref="MethodHandleUtil"/> associated with this instance of the runtime.
        /// </summary>
        public MethodHandleUtil MethodHandleUtil => GetOrCreateSingleton(ref methodHandleUtil, () => new MethodHandleUtil(this));

        /// <summary>
        /// Gets the <see cref="Boxer"/> associated with this instance of the runtime.
        /// </summary>
        public Boxer Boxer => GetOrCreateSingleton(ref boxer, () => new Boxer(this));

#endif

#if IMPORTER == false && EXPORTER == false

        /// <summary>
        /// Gets the <see cref="ExceptionHelper"/> associated with this instance of the runtime.
        /// </summary>
        public ExceptionHelper ExceptionHelper => GetOrCreateSingleton(ref exceptionHelper, () => new ExceptionHelper(this));

#endif

        /// <summary>
        /// Gets the <see cref="RuntimeClassLoaderFactory"/> associated with this instance of the runtime.
        /// </summary>
        public RuntimeClassLoaderFactory ClassLoaderFactory => GetOrCreateSingleton(ref classLoaderFactory, () => new RuntimeClassLoaderFactory(this));

        /// <summary>
        /// Gets the <see cref="RuntimeAssemblyClassLoaderFactory"/> associated with this instance of the runtime.
        /// </summary>
        public RuntimeAssemblyClassLoaderFactory AssemblyClassLoaderFactory => GetOrCreateSingleton(ref assemblyClassLoaderFactory, () => new RuntimeAssemblyClassLoaderFactory(this));

        /// <summary>
        /// Gets the <see cref="RuntimeManagedJavaTypeFactory"/> associated with this instance of the runtime.
        /// </summary>
        public RuntimeManagedJavaTypeFactory ManagedJavaTypeFactory => GetOrCreateSingleton(ref managedJavaTypeFactory, () => new RuntimeManagedJavaTypeFactory(this));

        /// <summary>
        /// Gets the <see cref="RuntimeManagedJavaTypeFactory"/> associated with this instance of the runtime.
        /// </summary>
        public RuntimeManagedByteCodeJavaTypeFactory ManagedByteCodeJavaTypeFactory => GetOrCreateSingleton(ref managedByteCodeJavaTypeFactory, () => new RuntimeManagedByteCodeJavaTypeFactory(this));

#if IMPORTER || EXPORTER

        /// <summary>
        /// Gets the <see cref="StaticCompiler"/> associated with this instance of the runtime.
        /// </summary>
        public StaticCompiler StaticCompiler => staticCompiler;

        /// <summary>
        /// Gets the <see cref="FakeTypes"/> associated with this instance of the runtime.
        /// </summary>
        public FakeTypes FakeTypes => GetOrCreateSingleton(ref fakeTypes, () => new FakeTypes(this));

#endif

#if IMPORTER

        /// <summary>
        /// Gets the <see cref="ProxyGenerator"/> associated with this instance of the runtime.
        /// </summary>
        public ProxyGenerator ProxyGenerator => GetOrCreateSingleton(ref proxyGenerator, () => new ProxyGenerator(this));

#endif

        #region LinkingContext<RuntimeJavaType, RuntimeJavaMember, RuntimeJavaField, RuntimeJavaMethod>

        /// <inheritdoc />
        RuntimeJavaType ILinkingContext<RuntimeJavaType, RuntimeJavaMember, RuntimeJavaField, RuntimeJavaMethod>.CreateUnloadableType(string name) => new RuntimeUnloadableJavaType(this, name);

#if IMPORTER

        /// <inheritdoc />
        bool ILinkingContext<RuntimeJavaType, RuntimeJavaMember, RuntimeJavaField, RuntimeJavaMethod>.IsImporter => true;

#else

        /// <inheritdoc />
        bool ILinkingContext<RuntimeJavaType, RuntimeJavaMember, RuntimeJavaField, RuntimeJavaMethod>.IsImporter => false;

#endif

        /// <inheritdoc />
        RuntimeJavaType ILinkingContext<RuntimeJavaType, RuntimeJavaMember, RuntimeJavaField, RuntimeJavaMethod>.TypeOfJavaLangObject => JavaBase.TypeOfJavaLangObject;

        /// <inheritdoc />
        RuntimeJavaType ILinkingContext<RuntimeJavaType, RuntimeJavaMember, RuntimeJavaField, RuntimeJavaMethod>.TypeOfVerifierNull => VerifierJavaTypeFactory.Null;

#if EXPORTER == false

        /// <inheritdoc />
        bool ILinkingContext<RuntimeJavaType, RuntimeJavaMember, RuntimeJavaField, RuntimeJavaMethod>.AllowNonVirtualCalls => JVM.AllowNonVirtualCalls;

#else

        /// <inheritdoc />
        bool ILinkingContext<RuntimeJavaType, RuntimeJavaMember, RuntimeJavaField, RuntimeJavaMethod>.AllowNonVirtualCalls => false;

#endif

        #endregion

    }

}
