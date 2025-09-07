using System;
using System.IO;
using System.Linq;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionModuleSymbolBuilderWriter
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly ModuleSymbolBuilder _builder;

        IkvmReflectionSymbolState _state = IkvmReflectionSymbolState.Default;
        AssemblyBuilder? _parentAssemblyBuilder;
        Assembly? _parentAssembly;
        ModuleBuilder? _moduleBuilder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public IkvmReflectionModuleSymbolBuilderWriter(IkvmReflectionSymbolContext context, ModuleSymbolBuilder builder)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the most completed module.
        /// </summary>
        public Module? Module => _moduleBuilder;

        /// <summary>
        /// Advances the builder to the given phase.
        /// </summary>
        /// <param name="state"></param>
        public void AdvanceTo(IkvmReflectionSymbolState state)
        {
            if (state >= IkvmReflectionSymbolState.Defined && _state < IkvmReflectionSymbolState.Defined)
                _parentAssemblyBuilder = (AssemblyBuilder)_context.ResolveAssembly((AssemblySymbolBuilder)_builder.Assembly, IkvmReflectionSymbolState.Emitted);

            if (state >= IkvmReflectionSymbolState.Defined && _state < IkvmReflectionSymbolState.Defined)
                Define();

            if (state >= IkvmReflectionSymbolState.Emitted && _state < IkvmReflectionSymbolState.Emitted)
                Emit();

            if (state >= IkvmReflectionSymbolState.Finished && _state < IkvmReflectionSymbolState.Finished)
                _parentAssembly = _context.ResolveAssembly((AssemblySymbolBuilder)_builder.Assembly, IkvmReflectionSymbolState.Finished);

            if (state >= IkvmReflectionSymbolState.Finished && _state < IkvmReflectionSymbolState.Finished)
                Finish();
        }

        /// <summary>
        /// Executes the finish phase.
        /// </summary>
        void Define()
        {
            if (_state != IkvmReflectionSymbolState.Default)
                throw new InvalidOperationException();

            // define module
            if (_parentAssemblyBuilder is not null)
                _moduleBuilder = _parentAssemblyBuilder.DefineDynamicModule(_builder.ScopeName, _builder.Name, _context.Options.EmitDebugInfo);
            else
                throw new InvalidOperationException();

            _state = IkvmReflectionSymbolState.Defined;
        }

        /// <summary>
        /// Executes the finish phase.
        /// </summary>
        void Emit()
        {
            if (_state != IkvmReflectionSymbolState.Defined)
                throw new InvalidOperationException();
            if (_moduleBuilder == null)
                throw new InvalidOperationException();

            // lock builder and set state
            _builder.Freeze();
            _state = IkvmReflectionSymbolState.Emitted;

            // set properties
            _moduleBuilder.__ImageBase = _builder.ImageBase;
            _moduleBuilder.__FileAlignment = _builder.FileAlignment;
            _moduleBuilder.__DllCharacteristics = (IKVM.Reflection.DllCharacteristics)_builder.DllCharacteristics;

            // declare global fields
            foreach (var field in _builder.GetDeclaredFields())
                _context.ResolveField(field, IkvmReflectionSymbolState.Defined);

            // declare global methods
            foreach (var method in _builder.GetDeclaredMethods())
                _context.ResolveMethod(method, IkvmReflectionSymbolState.Defined);

            // declare global types
            foreach (var type in _builder.GetDeclaredTypes())
                _context.ResolveType(type, IkvmReflectionSymbolState.Defined);

            // define resources
            foreach (var resource in _builder.GetManifestResources())
                _moduleBuilder.DefineManifestResource(resource.Name, new MemoryStream(resource.Data.ToArray()), (ResourceAttributes)resource.Attributes);

            // define documents
            foreach (var document in _builder.SourceDocuments)
                _moduleBuilder.DefineDocument(document.Url, document.Language, document.LanguageVendor, document.DocumentType);

            // set custom attributes
            foreach (var customAttribute in _builder.GetDeclaredCustomAttributes())
                _moduleBuilder.SetCustomAttribute(_context.CreateCustomAttributeBuilder(customAttribute));
        }

        /// <summary>
        /// Executes the complete phase.
        /// </summary>
        void Finish()
        {
            if (_state != IkvmReflectionSymbolState.Emitted)
                throw new InvalidOperationException();
            if (_moduleBuilder == null)
                throw new InvalidOperationException();

            // finish global fields
            foreach (var field in _builder.GetDeclaredFields())
                _context.ResolveField(field, IkvmReflectionSymbolState.Emitted);

            // finish global methods
            foreach (var method in _builder.GetDeclaredMethods())
                _context.ResolveMethod(method, IkvmReflectionSymbolState.Emitted);

            // finish global types
            foreach (var type in _builder.GetDeclaredTypes())
                _context.ResolveType(type, IkvmReflectionSymbolState.Emitted);

            // create the global functions
            _moduleBuilder.CreateGlobalFunctions();
            _state = IkvmReflectionSymbolState.Finished;

            // finish global fields
            foreach (var field in _builder.GetDeclaredFields())
                _context.ResolveField(field, IkvmReflectionSymbolState.Finished);

            // finish global methods
            foreach (var method in _builder.GetDeclaredMethods())
                _context.ResolveMethod(method, IkvmReflectionSymbolState.Finished);

            // finish global types
            foreach (var type in _builder.GetDeclaredTypes())
                _context.ResolveType(type, IkvmReflectionSymbolState.Finished);
        }

    }

}
