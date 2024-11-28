using System;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Resources;

namespace IKVM.CoreLib.Symbols.Emit
{

    public class ModuleSymbolBuilder : ModuleSymbol, ICustomAttributeBuilder
    {

        readonly string _name;
        readonly string? _fileName;

        ulong _imageBase;
        uint _fileAlignment;
        DllCharacteristics _dllCharacteristics;

        readonly ImmutableArray<FieldSymbolBuilder>.Builder _fields = ImmutableArray.CreateBuilder<FieldSymbolBuilder>();
        readonly ImmutableArray<MethodSymbolBuilder>.Builder _methods = ImmutableArray.CreateBuilder<MethodSymbolBuilder>();
        readonly ImmutableArray<TypeSymbolBuilder>.Builder _types = ImmutableArray.CreateBuilder<TypeSymbolBuilder>();
        readonly ImmutableArray<CustomAttribute>.Builder _customAttributes = ImmutableArray.CreateBuilder<CustomAttribute>();

        ImmutableArray<FieldSymbol> _fieldsCache;
        ImmutableArray<MethodSymbol> _methodsCache;
        ImmutableArray<TypeSymbol> _typesCache;

        readonly ImmutableArray<SourceDocument>.Builder _sourceDocuments = ImmutableArray.CreateBuilder<SourceDocument>();

        bool _frozen;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        internal ModuleSymbolBuilder(SymbolContext context, AssemblySymbolBuilder assembly, string name, string? fileName) :
            base(context, assembly)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        }

        /// <inheritdoc />
        public sealed override string FullyQualifiedName => throw new NotImplementedException();

        /// <inheritdoc />
        public sealed override string Name => _name;

        /// <summary>
        /// Name of the file to produce for the module.
        /// </summary>
        public string? FileName => _fileName;

        /// <inheritdoc />
        public sealed override Guid ModuleVersionId => Guid.Empty;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool IsComplete => false;

        /// <inheritdoc />
        public override bool IsResource()
        {
            return false;
        }

        /// <inheritdoc />
        internal override ImmutableArray<FieldSymbol> GetDeclaredFields()
        {
            if (_fieldsCache == default)
                ImmutableInterlocked.InterlockedInitialize(ref _fieldsCache, _fields.ToImmutable().CastArray<FieldSymbol>());

            return _fieldsCache;
        }

        /// <inheritdoc />
        internal override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            if (_methodsCache == default)
                ImmutableInterlocked.InterlockedInitialize(ref _methodsCache, _methods.ToImmutable().CastArray<MethodSymbol>());

            return _methodsCache;
        }

        /// <inheritdoc />
        internal override ImmutableArray<TypeSymbol> GetDeclaredTypes()
        {
            if (_typesCache == default)
                ImmutableInterlocked.InterlockedInitialize(ref _typesCache, _types.ToImmutable().CastArray<TypeSymbol>());

            return _typesCache;
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return _customAttributes.ToImmutable();
        }

        /// <summary>
        /// Throws an exception if this module is frozen.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        void ThrowIfFrozen()
        {
            if (_frozen)
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Freezes the module.
        /// </summary>
        internal void SetFrozen()
        {
            _frozen = true;
        }

        /// <summary>
        /// Gets or sets the preferred address of the first byte of the image when it is loaded into memory.
        /// </summary>
        public ulong ImageBase
        {
            get => _imageBase;
            set { ThrowIfFrozen(); _imageBase = value; }
        }

        /// <summary>
        /// Gets or sets the alignment factor (in bytes) that is used to align the raw data of sections in the image file.
        /// </summary>
        public uint FileAlignment
        {
            get => _fileAlignment;
            set { ThrowIfFrozen(); _fileAlignment = value; }
        }

        /// <summary>
        /// Gets or sets the characteristics of a dynamic link library.
        /// </summary>
        public DllCharacteristics DllCharacteristics
        {
            get => _dllCharacteristics;
            set { ThrowIfFrozen(); _dllCharacteristics = value; }
        }

        /// <summary>
        /// Defines a document for source symbols.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="language"></param>
        /// <param name="languageVendor"></param>
        /// <param name="documentType"></param>
        /// <returns></returns>
        public SourceDocument DefineSymbolDocument(string url, Guid language, Guid languageVendor, Guid documentType)
        {
            ThrowIfFrozen();

            var d = new SourceDocument(Context, this, url, language, languageVendor, documentType);
            _sourceDocuments.Add(d);
            return d;
        }

        /// <summary>
        /// Defines a binary large object (BLOB) that represents a manifest resource to be embedded in the dynamic assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        /// <param name="attribute"></param>
        public void DefineManifestResource(string name, Stream stream, ResourceAttributes attribute)
        {
            ThrowIfFrozen();

            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines the named managed embedded resource to be stored in this module.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public IResourceWriter DefineResource(string name, string description)
        {
            ThrowIfFrozen();

            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines the named managed embedded resource with the given attributes that is to be stored in this module.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public IResourceWriter DefineResource(string name, string description, ResourceAttributes attribute)
        {
            ThrowIfFrozen();

            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a global method with the specified name, attributes, calling convention, return type, and parameter types.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        public MethodSymbolBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, TypeSymbol? returnType, ImmutableArray<TypeSymbol> parameterTypes)
        {
            return DefineGlobalMethod(name, attributes, callingConvention, returnType, parameterTypes, [], [], [], []);
        }

        /// <summary>
        /// Defines a global method with the specified name, attributes, return type, and parameter types.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        public MethodSymbolBuilder DefineGlobalMethod(string name, MethodAttributes attributes, TypeSymbol? returnType, ImmutableArray<TypeSymbol> parameterTypes)
        {
            return DefineGlobalMethod(name, attributes, CallingConventions.Standard, returnType, parameterTypes);
        }

        /// <summary>
        /// Defines a global method with the specified name, attributes, calling convention, return type, custom modifiers for the return type, parameter types, and custom modifiers for the parameter types.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="requiredReturnCustomModifiers"></param>
        /// <param name="optionalReturnCustomModifiers"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="requiredParameterCustomModifiers"></param>
        /// <param name="optionalParameterCustomModifiers"></param>
        /// <returns></returns>
        public MethodSymbolBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, TypeSymbol? returnType, ImmutableArray<TypeSymbol> requiredReturnCustomModifiers, ImmutableArray<TypeSymbol> optionalReturnCustomModifiers, ImmutableArray<TypeSymbol> parameterTypes, ImmutableArray<ImmutableArray<TypeSymbol>> requiredParameterCustomModifiers, ImmutableArray<ImmutableArray<TypeSymbol>> optionalParameterCustomModifiers)
        {
            ThrowIfFrozen();

            var b = new MethodSymbolBuilder(Context, this, null, name, attributes, callingConvention, returnType ?? Context.ResolveCoreType("System.Void"), requiredReturnCustomModifiers, optionalReturnCustomModifiers, parameterTypes, requiredParameterCustomModifiers, optionalParameterCustomModifiers);
            _methods.Add(b);
            _methodsCache = default;
            return b;
        }

        /// <summary>
        /// Constructs a TypeBuilder for a private type with the specified name in this module.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TypeSymbolBuilder DefineType(string name)
        {
            return DefineType(name, TypeAttributes.Class);
        }

        /// <summary>
        /// Constructs a TypeBuilder given the type name, the attributes, the type that the defined type extends, and the total size of the type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="parent"></param>
        /// <param name="typesize"></param>
        /// <returns></returns>
        public TypeSymbolBuilder DefineType(string name, TypeAttributes attributes, TypeSymbol? parent, int typesize)
        {
            return DefineType(name, attributes, parent, PackingSize.Unspecified, typesize);
        }

        /// <summary>
        /// Constructs a TypeBuilder given type name, its attributes, and the type that the defined type extends.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public TypeSymbolBuilder DefineType(string name, TypeAttributes attributes, TypeSymbol? parent)
        {
            return DefineType(name, attributes, parent, PackingSize.Unspecified, -1);
        }

        /// <summary>
        /// Constructs a TypeBuilder given the type name and the type attributes.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public TypeSymbolBuilder DefineType(string name, TypeAttributes attributes)
        {
            return DefineType(name, attributes, null);
        }

        /// <summary>
        /// Constructs a TypeBuilder given the type name, the attributes, the type that the defined type extends, and the packing size of the type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="parent"></param>
        /// <param name="packingSize"></param>
        /// <returns></returns>
        public TypeSymbolBuilder DefineType(string name, TypeAttributes attributes, TypeSymbol? parent, PackingSize packingSize)
        {
            return DefineType(name, attributes, parent, packingSize, -1);
        }

        /// <summary>
        /// Constructs a TypeBuilder given the type name, attributes, the type that the defined type extends, the packing size of the defined type, and the total size of the defined type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="parent"></param>
        /// <param name="packingSize"></param>
        /// <param name="typesize"></param>
        /// <returns></returns>
        public TypeSymbolBuilder DefineType(string name, TypeAttributes attributes, TypeSymbol? parent, PackingSize packingSize, int typesize)
        {
            ThrowIfFrozen();

            var b = new TypeSymbolBuilder(Context, this, name, attributes, parent, [], packingSize, typesize, null);
            _types.Add(b);
            _typesCache = default;
            return b;
        }

        /// <summary>
        /// Constructs a TypeBuilder given the type name, attributes, the type that the defined type extends, and the interfaces that the defined type implements.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="parent"></param>
        /// <param name="interfaces"></param>
        /// <returns></returns>
        public TypeSymbolBuilder DefineType(string name, TypeAttributes attributes, TypeSymbol? parent, ImmutableArray<TypeSymbol> interfaces)
        {
            ThrowIfFrozen();

            var b = new TypeSymbolBuilder(Context, this, name, attributes, parent, interfaces, PackingSize.Unspecified, -1, null);
            _types.Add(b);
            _typesCache = default;
            return b;
        }

        /// <summary>
        /// Explicitely adds a reference to the specified assembly.
        /// </summary>
        /// <param name="assembly"></param>
        public void AddReference(AssemblySymbol assembly)
        {
            ThrowIfFrozen();
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            ThrowIfFrozen();
            _customAttributes.Add(attribute);
        }

    }

}
