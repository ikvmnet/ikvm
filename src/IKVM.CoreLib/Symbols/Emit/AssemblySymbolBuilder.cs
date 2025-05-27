using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;

namespace IKVM.CoreLib.Symbols.Emit
{

    public class AssemblySymbolBuilder : AssemblySymbol, ICustomAttributeBuilder
    {

        AssemblyIdentity _identity;

        ModuleSymbolBuilder? _manifestModule;
        ImmutableArray<ModuleSymbolBuilder>.Builder _modules = ImmutableArray.CreateBuilder<ModuleSymbolBuilder>();
        ImmutableArray<ModuleSymbol> _modulesCache;
        ImmutableArray<byte[]>.Builder _win32Icons = ImmutableArray.CreateBuilder<byte[]>();
        ImmutableArray<byte[]>.Builder _manifestResources = ImmutableArray.CreateBuilder<byte[]>();
        ImmutableArray<byte[]>.Builder _resources = ImmutableArray.CreateBuilder<byte[]>();

        ImmutableArray<TypeSymbol>.Builder _typeForwarders = ImmutableArray.CreateBuilder<TypeSymbol>();
        MethodSymbolBuilder? _entryPoint;
        PEFileKinds _fileKind;
        ImmutableArray<(string Name, string FileName)>.Builder _resourceFiles = ImmutableArray.CreateBuilder<(string, string)>();
        (string? product, string? productVersion, string? company, string? copyright, string? trademark)? _versionResource;
        ImmutableArray<AssemblyIdentity>.Builder _referencedAssemblies = ImmutableArray.CreateBuilder<AssemblyIdentity>();
        ImmutableArray<CustomAttribute>.Builder _customAttributes = ImmutableArray.CreateBuilder<CustomAttribute>();

        bool _frozen;
        object? _writer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        internal AssemblySymbolBuilder(SymbolContext context, AssemblyIdentity identity) :
            base(context)
        {
            _identity = identity ?? throw new ArgumentNullException(nameof(identity));
        }

        /// <inheritdoc />
        public override AssemblyIdentity Identity => _identity;

        /// <inheritdoc />
        public override string ImageRuntimeVersion => throw new NotSupportedException();

        /// <inheritdoc />
        public override string Location => throw new NotSupportedException();

        /// <inheritdoc />
        public override ModuleSymbol ManifestModule => _manifestModule ?? throw new InvalidOperationException();

        /// <inheritdoc />
        public override MethodSymbol? EntryPoint => _entryPoint;

        /// <inheritdoc />
        public override bool IsMissing => false;

        /// <inheritdoc />
        public override ManifestResourceInfo? GetManifestResourceInfo(string resourceName)
        {
            var manifestModule = (ModuleSymbolBuilder)ManifestModule;
            foreach (var i in manifestModule.GetManifestResources())
                if (i.Name == resourceName)
                    return new ManifestResourceInfo(System.Reflection.ResourceLocation.Embedded, null, null);

            return null;
        }

        /// <inheritdoc />
        public override Stream GetManifestResourceStream(string name)
        {
            var manifestModule = (ModuleSymbolBuilder)ManifestModule;
            foreach (var i in manifestModule.GetManifestResources())
                if (i.Name == name)
                    return new MemoryStream(i.Data.ToArray());

            throw new FileNotFoundException();
        }

        /// <inheritdoc />
        public override ImmutableArray<ModuleSymbol> GetModules()
        {
            if (_modulesCache == default)
                ImmutableInterlocked.InterlockedInitialize(ref _modulesCache, _modules.ToImmutable().CastArray<ModuleSymbol>());

            return _modulesCache;
        }

        /// <inheritdoc />
        public override ImmutableArray<AssemblyIdentity> GetReferencedAssemblies()
        {
            return _referencedAssemblies.ToImmutable();
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return _customAttributes.ToImmutable();
        }

        /// <summary>
        /// Freezes the type builder.
        /// </summary>
        internal void Freeze()
        {
            _frozen = true;
        }

        /// <summary>
        /// Throws an exception if the builder is frozen.
        /// </summary>
        void ThrowIfFrozen()
        {
            if (_frozen)
                throw new InvalidOperationException("AssemblySymbolBuilder is frozen.");
        }

        /// <summary>
        /// Defines a named module in this assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ModuleSymbolBuilder DefineModule(string name, string fileName)
        {
            ThrowIfFrozen();
            var b = new ModuleSymbolBuilder(Context, this, name, fileName);
            _modules.Add(b);
            _modulesCache = default;
            _manifestModule ??= b;
            return b;
        }

        /// <summary>
        /// Sets a Win32 icon on the generated assembly.
        /// </summary>
        /// <param name="bytes"></param>
        public void DefineIconResource(byte[] bytes)
        {
            ThrowIfFrozen();
            _win32Icons.Add(bytes);
        }

        /// <summary>
        /// Sets a manifest resource on the generated assembly.
        /// </summary>
        /// <param name="bytes"></param>
        public void DefineManifestResource(byte[] bytes)
        {
            ThrowIfFrozen();
            _manifestResources.Add(bytes);
        }

        /// <summary>
        /// Sets a Win32 version info resource on the generated assembly.
        /// </summary>
        public void DefineVersionInfoResource()
        {
            ThrowIfFrozen();
            _versionResource = (null, null, null, null, null);
        }

        /// <summary>
        /// Sets a Win32 version info resource on the generated assembly.
        /// </summary>
        public void DefineVersionInfoResource(string product, string productVersion, string company, string copyright, string trademark)
        {
            ThrowIfFrozen();
            _versionResource = (product, productVersion, company, copyright, trademark);
        }

        /// <summary>
        /// Sets the entry point for this assembly, assuming that a console application is being built.
        /// </summary>
        /// <param name="entryMethod"></param>
        public void SetEntryPoint(MethodSymbolBuilder entryMethod)
        {
            SetEntryPoint(entryMethod, PEFileKinds.Dll);
        }

        /// <summary>
        /// Sets the entry point for this assembly and defines the type of the portable executable (PE file) being built.
        /// </summary>
        /// <param name="entryMethod"></param>
        /// <param name="fileKind"></param>
        public void SetEntryPoint(MethodSymbolBuilder entryMethod, PEFileKinds fileKind)
        {
            ThrowIfFrozen();
            _entryPoint = entryMethod;
            _fileKind = fileKind;
        }

        /// <summary>
        /// Adds a forwarded type to this assembly.
        /// </summary>
        /// <param name="type"></param>
        public void AddTypeForwarder(TypeSymbol type)
        {
            ThrowIfFrozen();
            _typeForwarders.Add(type);
        }

        /// <summary>
        /// Adds an external resource file to the assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        public void AddResourceFile(string name, string fileName)
        {
            ThrowIfFrozen();
            _resourceFiles.Add((name, fileName));
        }

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            ThrowIfFrozen();
            _customAttributes.Add(attribute);
        }

        /// <summary>
        /// Sets the assembly version.
        /// </summary>
        /// <param name="version"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetAssemblyVersion(Version version)
        {
            ThrowIfFrozen();

            _identity = new AssemblyIdentity(
                _identity.Name,
                version,
                _identity.CultureName,
                _identity.HasPublicKey ? _identity.PublicKey : _identity.PublicKeyToken,
                _identity.HasPublicKey,
                _identity.ContentType,
                _identity.ProcessorArchitecture);
        }

        /// <summary>
        /// Sets the assembly culture.
        /// </summary>
        /// <param name="cultureName"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetAssemblyCulture(string cultureName)
        {
            ThrowIfFrozen();

            _identity = new AssemblyIdentity(
                _identity.Name,
                _identity.Version,
                cultureName,
                _identity.HasPublicKey ? _identity.PublicKey : _identity.PublicKeyToken,
                _identity.HasPublicKey,
                _identity.ContentType,
                _identity.ProcessorArchitecture);
        }

        /// <summary>
        /// Gets the writer object associated with this builder.
        /// </summary>
        /// <typeparam name="TWriter"></typeparam>
        /// <param name="create"></param>
        /// <returns></returns>
        internal TWriter Writer<TWriter>(Func<AssemblySymbolBuilder, TWriter> create)
        {
            if (_writer is null)
                Interlocked.CompareExchange(ref _writer, create(this), null);

            return (TWriter)(_writer ?? throw new InvalidOperationException());
        }

    }

}
