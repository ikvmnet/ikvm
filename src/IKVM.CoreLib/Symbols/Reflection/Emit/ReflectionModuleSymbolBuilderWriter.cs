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
            if (state >= ReflectionSymbolState.Declared && _state < ReflectionSymbolState.Declared)
                _parentAssemblyBuilder = (AssemblyBuilder)_context.ResolveAssembly((AssemblySymbolBuilder)_builder.Assembly, ReflectionSymbolState.Finished);

            if (state >= ReflectionSymbolState.Declared && _state < ReflectionSymbolState.Declared)
                Declare();

            if (state >= ReflectionSymbolState.Finished && _state < ReflectionSymbolState.Finished)
                Finish();

            if (state >= ReflectionSymbolState.Completed && _state < ReflectionSymbolState.Completed)
                _parentAssembly = _context.ResolveAssembly((AssemblySymbolBuilder)_builder.Assembly, ReflectionSymbolState.Completed);

            if (state >= ReflectionSymbolState.Completed && _state < ReflectionSymbolState.Completed)
                Complete();
        }

        /// <summary>
        /// Executes the finish phase.
        /// </summary>
        void Declare()
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

            _state = ReflectionSymbolState.Declared;
        }

        /// <summary>
        /// Executes the finish phase.
        /// </summary>
        void Finish()
        {
            if (_state != ReflectionSymbolState.Declared)
                throw new InvalidOperationException();
            if (_moduleBuilder == null)
                throw new InvalidOperationException();

            // lock builder and set state
            _builder.Freeze();
            _state = ReflectionSymbolState.Finished;

            // declare global fields
            foreach (var field in _builder.GetDeclaredFields())
                _context.ResolveField(field, ReflectionSymbolState.Declared);

            // declare global methods
            foreach (var method in _builder.GetDeclaredMethods())
                _context.ResolveMethod(method, ReflectionSymbolState.Declared);

            // declare global types
            foreach (var type in _builder.GetDeclaredTypes())
                _context.ResolveType(type, ReflectionSymbolState.Declared);

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
        void Complete()
        {
            if (_state != ReflectionSymbolState.Finished)
                throw new InvalidOperationException();
            if (_moduleBuilder == null)
                throw new InvalidOperationException();

            // finish global fields
            foreach (var field in _builder.GetDeclaredFields())
                _context.ResolveField(field, ReflectionSymbolState.Finished);

            // finish global methods
            foreach (var method in _builder.GetDeclaredMethods())
                _context.ResolveMethod(method, ReflectionSymbolState.Finished);

            // finish global types
            foreach (var type in _builder.GetDeclaredTypes())
                _context.ResolveType(type, ReflectionSymbolState.Finished);

            // create the global functions
            _moduleBuilder.CreateGlobalFunctions();
            _state = ReflectionSymbolState.Completed;

            // finish global fields
            foreach (var field in _builder.GetDeclaredFields())
                _context.ResolveField(field, ReflectionSymbolState.Completed);

            // finish global methods
            foreach (var method in _builder.GetDeclaredMethods())
                _context.ResolveMethod(method, ReflectionSymbolState.Completed);

            // finish global types
            foreach (var type in _builder.GetDeclaredTypes())
                _context.ResolveType(type, ReflectionSymbolState.Completed);
        }

    }

}
