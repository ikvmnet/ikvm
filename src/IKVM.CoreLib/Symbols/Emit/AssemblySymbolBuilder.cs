using System;
using System.Collections.Immutable;
using System.IO;

namespace IKVM.CoreLib.Symbols.Emit
{

    public class AssemblySymbolBuilder : AssemblySymbol, ICustomAttributeBuilder
    {

        AssemblyIdentity _identity;

        ImmutableArray<ModuleSymbolBuilder>.Builder _modules = ImmutableArray.CreateBuilder<ModuleSymbolBuilder>();
        ImmutableArray<ModuleSymbol> _modulesCache;
        ImmutableArray<byte[]>.Builder _win32Icons = ImmutableArray.CreateBuilder<byte[]>();
        ImmutableArray<byte[]>.Builder _manifestResources = ImmutableArray.CreateBuilder<byte[]>();
        ImmutableArray<byte[]>.Builder _resources = ImmutableArray.CreateBuilder<byte[]>();

        ImmutableArray<TypeSymbol>.Builder _typeForwarder = ImmutableArray.CreateBuilder<TypeSymbol>();
        MethodSymbol? _entryPoint;
        PEFileKinds _fileKind;
        ImmutableArray<(string Name, string FileName)>.Builder _resourceFiles = ImmutableArray.CreateBuilder<(string, string)>();
        (string? product, string? productVersion, string? company, string? copyright, string? trademark)? _versionResource;
        ImmutableArray<AssemblyIdentity>.Builder _referencedAssemblies = ImmutableArray.CreateBuilder<AssemblyIdentity>();
        ImmutableArray<CustomAttribute>.Builder _customAttributes = ImmutableArray.CreateBuilder<CustomAttribute>();

        bool _frozen;
        internal object? _state1;
        internal object? _state2;

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
        public override ModuleSymbol ManifestModule => throw new NotSupportedException();

        /// <inheritdoc />
        public override MethodSymbol? EntryPoint => _entryPoint;

        /// <inheritdoc />
        public override bool IsMissing => false;

        /// <inheritdoc />
        public override ManifestResourceInfo? GetManifestResourceInfo(string resourceName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Stream? GetManifestResourceStream(string name)
        {
            throw new NotImplementedException();
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
        /// Defines a named module in this assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ModuleSymbolBuilder DefineModule(string name)
        {
            return DefineModule(name, null);
        }

        /// <summary>
        /// Defines a named module in this assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ModuleSymbolBuilder DefineModule(string name, string? fileName)
        {
            var b = new ModuleSymbolBuilder(Context, this, name, fileName);
            _modules.Add(b);
            _modulesCache = default;
            return b;
        }

        /// <summary>
        /// Sets a Win32 icon on the generated assembly.
        /// </summary>
        /// <param name="bytes"></param>
        public void DefineIconResource(byte[] bytes)
        {
            _win32Icons.Add(bytes);
        }

        /// <summary>
        /// Sets a manifest resource on the generated assembly.
        /// </summary>
        /// <param name="bytes"></param>
        public void DefineManifestResource(byte[] bytes)
        {
            _manifestResources.Add(bytes);
        }

        /// <summary>
        /// Sets a Win32 version info resource on the generated assembly.
        /// </summary>
        public void DefineVersionInfoResource()
        {
            _versionResource = (null, null, null, null, null);
        }

        /// <summary>
        /// Sets a Win32 version info resource on the generated assembly.
        /// </summary>
        public void DefineVersionInfoResource(string product, string productVersion, string company, string copyright, string trademark)
        {
            _versionResource = (product, productVersion, company, copyright, trademark);
        }

        /// <summary>
        /// Sets the entry point for this assembly, assuming that a console application is being built.
        /// </summary>
        /// <param name="entryMethod"></param>
        public void SetEntryPoint(MethodSymbol entryMethod)
        {
            SetEntryPoint(entryMethod, PEFileKinds.Dll);
        }

        /// <summary>
        /// Sets the entry point for this assembly and defines the type of the portable executable (PE file) being built.
        /// </summary>
        /// <param name="entryMethod"></param>
        /// <param name="fileKind"></param>
        public void SetEntryPoint(MethodSymbol entryMethod, PEFileKinds fileKind)
        {
            _entryPoint = entryMethod;
            _fileKind = fileKind;
        }

        /// <summary>
        /// Adds a forwarded type to this assembly.
        /// </summary>
        /// <param name="type"></param>
        public void AddTypeForwarder(TypeSymbol type)
        {
            _typeForwarder.Add(type);
        }

        /// <summary>
        /// Adds an external resource file to the assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        public void AddResourceFile(string name, string fileName)
        {
            _resourceFiles.Add((name, fileName));
        }

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            _customAttributes.Add(attribute);
        }

        /// <summary>
        /// Sets the assembly version.
        /// </summary>
        /// <param name="version"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetAssemblyVersion(Version version)
        {
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
            _identity = new AssemblyIdentity(
                _identity.Name,
                _identity.Version,
                cultureName,
                _identity.HasPublicKey ? _identity.PublicKey : _identity.PublicKeyToken,
                _identity.HasPublicKey,
                _identity.ContentType,
                _identity.ProcessorArchitecture);
        }

    }

}
