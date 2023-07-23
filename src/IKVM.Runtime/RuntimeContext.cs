using System;
using System.Collections.Concurrent;

#if IMPORTER
using IKVM.Tools.Importer;
#endif

#if EXPORTER
using IKVM.Tools.Exporter;
#endif

#if IMPORTER ==false
using IKVM.StubGen;
#endif

namespace IKVM.Runtime
{

    /// <summary>
    /// Maintains services relevant to an instane of the IKVM runtime.
    /// </summary>
    class RuntimeContext
    {

        readonly IManagedTypeResolver resolver;
        readonly bool bootstrap;
        readonly ConcurrentDictionary<Type, object> singletons = new ConcurrentDictionary<Type, object>();

        Types types;
        CoreClasses javaBase;
        AttributeHelper attributeHelper;
        RuntimeClassLoaderFactory classLoaderFactory;
        RuntimeAssemblyClassLoaderFactory assemblyClassLoaderFactory;
        RuntimeManagedJavaTypeFactory managedJavaTypeFactory;
        RuntimeManagedByteCodeJavaTypeFactory managedByteCodeJavaTypeFactory;
        RuntimePrimitiveJavaTypeFactory primitiveJavaTypeFactory;
        RuntimeVerifierJavaTypeFactory verifierJavaTypeFactory;

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
        /// <param name="bootstrap"></param>
        /// <param name="staticCompiler"></param>
        public RuntimeContext(IManagedTypeResolver resolver, bool bootstrap, StaticCompiler staticCompiler)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.bootstrap = bootstrap;
            this.staticCompiler = staticCompiler;
        }

#else

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="bootstrap"></param>
        public RuntimeContext(IManagedTypeResolver resolver, bool bootstrap)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.bootstrap = bootstrap;
        }

#endif

        /// <summary>
        /// Gets or creates an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="create"></param>
        /// <returns></returns>
        public T GetOrCreateSingleton<T>(Func<T> create) => (T)singletons.GetOrAdd(typeof(T), () => create());

        /// <summary>
        /// Gets the <see cref="IManagedTypeResolver"/> associated with this instance of the runtime.
        /// </summary>
        public IManagedTypeResolver Resolver => resolver;

        /// <summary>
        /// Gets whether or not the runtime is running in bootstrap mode; that is, we are compiling the Java base assembly itself.
        /// </summary>
        public bool Bootstrap => bootstrap;

        /// <summary>
        /// Gets the <see cref="Types"/> associated with this instance of the runtime.
        /// </summary>
        public Types Types => types ??= new Types(this);

        /// <summary>
        /// Gets the <see cref="CoreClasses"/> associated with this instance of the runtime.
        /// </summary>
        public CoreClasses JavaBase => javaBase ??= new CoreClasses(this);

        /// <summary>
        /// Gets the <see cref="AttributeHelper"/> associated with this instance of the runtime.
        /// </summary>
        public AttributeHelper AttributeHelper => attributeHelper ??= new AttributeHelper(this);

        /// <summary>
        /// Gets the <see cref="RuntimePrimitiveJavaTypeFactory"/> associated with this instance of the runtime.
        /// </summary>
        public RuntimePrimitiveJavaTypeFactory PrimitiveJavaTypeFactory => primitiveJavaTypeFactory ??= new RuntimePrimitiveJavaTypeFactory(this);

        /// <summary>
        /// Gets the <see cref="RuntimeVerifierJavaTypeFactory"/> associated with this instance of the runtime.
        /// </summary>
        public RuntimeVerifierJavaTypeFactory VerifierJavaTypeFactory => verifierJavaTypeFactory ??= new RuntimeVerifierJavaTypeFactory(this);

#if IMPORTER == false

        /// <summary>
        /// Gets the <see cref="StubGenerator"/> associated with this instance of the runtime.
        /// </summary>
        public StubGenerator StubGenerator => stubGenerator ??= new StubGenerator(this);

#endif

#if EXPORTER == false

        /// <summary>
        /// Gets the <see cref="CodeEmitterFactory"/> associated with this instance of the runtime.
        /// </summary>
        public CodeEmitterFactory CodeEmitterFactory => codeEmitterFactory ??= new CodeEmitterFactory(this);

        /// <summary>
        /// Gets the <see cref="DynamicClassLoaderFactory"/> associated with this instance of the runtime.
        /// </summary>
        public DynamicClassLoaderFactory DynamicClassLoaderFactory => dynamicClassLoaderFactory ??= new DynamicClassLoaderFactory(this);

        /// <summary>
        /// Gets the <see cref="ByteCodeHelperMethods"/> associated with this instance of the runtime.
        /// </summary>
        public ByteCodeHelperMethods ByteCodeHelperMethods => byteCodeHelperMethods ??= new ByteCodeHelperMethods(this);

        /// <summary>
        /// Gets the <see cref="Serialization"/> associated with this instance of the runtime.
        /// </summary>
        public Serialization Serialization => serialization ??= new Serialization(this);

        /// <summary>
        /// Gets the <see cref="CompilerFactory"/> associated with this instance of the runtime.
        /// </summary>
        public CompilerFactory CompilerFactory => compilerFactory ??= new CompilerFactory(this, bootstrap);

        /// <summary>
        /// Gets the <see cref="InterlockedMethods"/> associated with this instance of the runtime.
        /// </summary>
        public InterlockedMethods InterlockedMethods => interlockedMethods ??= new InterlockedMethods(this);

        /// <summary>
        /// Gets the <see cref="MethodAnalyzerFactory"/> associated with this instance of the runtime.
        /// </summary>
        public MethodAnalyzerFactory MethodAnalyzerFactory => methodAnalyzerFactory ??= new MethodAnalyzerFactory(this);

        /// <summary>
        /// Gets the <see cref="MethodHandleUtil"/> associated with this instance of the runtime.
        /// </summary>
        public MethodHandleUtil MethodHandleUtil => methodHandleUtil ??= new MethodHandleUtil(this);

        /// <summary>
        /// Gets the <see cref="Boxer"/> associated with this instance of the runtime.
        /// </summary>
        public Boxer Boxer => boxer ??= new Boxer(this);

#endif

#if IMPORTER == false && EXPORTER == false

        /// <summary>
        /// Gets the <see cref="ExceptionHelper"/> associated with this instance of the runtime.
        /// </summary>
        public ExceptionHelper ExceptionHelper => exceptionHelper ??= new ExceptionHelper(this);

#endif

        /// <summary>
        /// Gets the <see cref="RuntimeClassLoaderFactory"/> associated with this instance of the runtime.
        /// </summary>
        public RuntimeClassLoaderFactory ClassLoaderFactory => classLoaderFactory ??= new RuntimeClassLoaderFactory(this);

        /// <summary>
        /// Gets the <see cref="RuntimeAssemblyClassLoaderFactory"/> associated with this instance of the runtime.
        /// </summary>
        public RuntimeAssemblyClassLoaderFactory AssemblyClassLoaderFactory => assemblyClassLoaderFactory ??= new RuntimeAssemblyClassLoaderFactory(this);

        /// <summary>
        /// Gets the <see cref="RuntimeManagedJavaTypeFactory"/> associated with this instance of the runtime.
        /// </summary>
        public RuntimeManagedJavaTypeFactory ManagedJavaTypeFactory => managedJavaTypeFactory ??= new RuntimeManagedJavaTypeFactory(this);

        /// <summary>
        /// Gets the <see cref="RuntimeManagedJavaTypeFactory"/> associated with this instance of the runtime.
        /// </summary>
        public RuntimeManagedByteCodeJavaTypeFactory ManagedByteCodeJavaTypeFactory => managedByteCodeJavaTypeFactory ??= new RuntimeManagedByteCodeJavaTypeFactory(this);

#if IMPORTER || EXPORTER

        /// <summary>
        /// Gets the <see cref="StaticCompiler"/> associated with this instance of the runtime.
        /// </summary>
        public StaticCompiler StaticCompiler => staticCompiler;

        /// <summary>
        /// Gets the <see cref="FakeTypes"/> associated with this instance of the runtime.
        /// </summary>
        public FakeTypes FakeTypes => fakeTypes ??= new FakeTypes(this);

#endif

#if IMPORTER

        /// <summary>
        /// Gets the <see cref="ProxyGenerator"/> associated with this instance of the runtime.
        /// </summary>
        public ProxyGenerator ProxyGenerator => proxyGenerator ??= new ProxyGenerator(this);

#endif

    }

}
