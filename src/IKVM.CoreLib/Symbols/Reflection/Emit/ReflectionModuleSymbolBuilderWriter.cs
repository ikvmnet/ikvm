using System;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionModuleSymbolBuilderWriter
    {

        readonly ReflectionSymbolContext _context;
        readonly ModuleSymbolBuilder _builder;

        ReflectionSymbolState _state = ReflectionSymbolState.Default;
        AssemblyBuilder? _parentAssemblyBuilder;
        Assembly? _parentAssembly;
        ModuleBuilder? _moduleBuilder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public ReflectionModuleSymbolBuilderWriter(ReflectionSymbolContext context, ModuleSymbolBuilder builder)
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
        public void AdvanceTo(ReflectionSymbolState state)
        {
            lock (this)
            {
                if (state >= ReflectionSymbolState.Defined && _state < ReflectionSymbolState.Defined)
                    _parentAssemblyBuilder = (AssemblyBuilder)_context.ResolveAssembly((AssemblySymbolBuilder)_builder.Assembly, ReflectionSymbolState.Emitted);

                if (state >= ReflectionSymbolState.Defined && _state < ReflectionSymbolState.Defined)
                    Define();

                if (state >= ReflectionSymbolState.Emitted && _state < ReflectionSymbolState.Emitted)
                    Emit();

                if (state >= ReflectionSymbolState.Finished && _state < ReflectionSymbolState.Finished)
                    _parentAssembly = _context.ResolveAssembly((AssemblySymbolBuilder)_builder.Assembly, ReflectionSymbolState.Finished);

                if (state >= ReflectionSymbolState.Finished && _state < ReflectionSymbolState.Finished)
                    Finish();
            }
        }

        /// <summary>
        /// Executes the finish phase.
        /// </summary>
        void Define()
        {
            if (_state != ReflectionSymbolState.Default)
                throw new InvalidOperationException();

            // define module
            if (_parentAssemblyBuilder is not null)
#if NETFRAMEWORK
                _moduleBuilder = _parentAssemblyBuilder.DefineDynamicModule(_builder.ScopeName, _builder.Name, _context.Options.EmitDebugInfo);
#else
                _moduleBuilder = _parentAssemblyBuilder.DefineDynamicModule(_builder.ScopeName);
#endif
            else
                throw new InvalidOperationException();

            _state = ReflectionSymbolState.Defined;
        }

        /// <summary>
        /// Executes the finish phase.
        /// </summary>
        void Emit()
        {
            if (_state != ReflectionSymbolState.Defined)
                throw new InvalidOperationException();
            if (_moduleBuilder == null)
                throw new InvalidOperationException();

            // lock builder and set state
            _builder.Freeze();
            _state = ReflectionSymbolState.Emitted;

            // declare global fields
            foreach (var field in _builder.GetDeclaredFields())
                _context.ResolveField(field, ReflectionSymbolState.Defined);

            // declare global methods
            foreach (var method in _builder.GetDeclaredMethods())
                _context.ResolveMethod(method, ReflectionSymbolState.Defined);

            // declare global types
            foreach (var type in _builder.GetDeclaredTypes())
                _context.ResolveType(type, ReflectionSymbolState.Defined);

#if NETFRAMEWORK
            // define documents
            foreach (var document in _builder.SourceDocuments)
                _moduleBuilder.DefineDocument(document.Url, document.Language, document.LanguageVendor, document.DocumentType);
#endif

            // set custom attributes
            foreach (var customAttribute in _builder.GetDeclaredCustomAttributes())
                _moduleBuilder.SetCustomAttribute(_context.CreateCustomAttributeBuilder(customAttribute));
        }

        /// <summary>
        /// Executes the complete phase.
        /// </summary>
        void Finish()
        {
            if (_state != ReflectionSymbolState.Emitted)
                throw new InvalidOperationException();
            if (_moduleBuilder == null)
                throw new InvalidOperationException();

            // finish global fields
            foreach (var field in _builder.GetDeclaredFields())
                _context.ResolveField(field, ReflectionSymbolState.Emitted);

            // finish global methods
            foreach (var method in _builder.GetDeclaredMethods())
                _context.ResolveMethod(method, ReflectionSymbolState.Emitted);

            // finish global types
            foreach (var type in _builder.GetDeclaredTypes())
                _context.ResolveType(type, ReflectionSymbolState.Emitted);

            // create the global functions
            _moduleBuilder.CreateGlobalFunctions();
            _state = ReflectionSymbolState.Finished;

            // finish global fields
            foreach (var field in _builder.GetDeclaredFields())
                _context.ResolveField(field, ReflectionSymbolState.Finished);

            // finish global methods
            foreach (var method in _builder.GetDeclaredMethods())
                _context.ResolveMethod(method, ReflectionSymbolState.Finished);

            // finish global types
            foreach (var type in _builder.GetDeclaredTypes())
                _context.ResolveType(type, ReflectionSymbolState.Finished);
        }

    }

}
