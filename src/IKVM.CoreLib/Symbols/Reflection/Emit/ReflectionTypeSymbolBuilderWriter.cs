using System;
using System.Linq;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    /// <summary>
    /// Writes the symbol builder to the underlying type builder and type.
    /// </summary>
    class ReflectionTypeSymbolBuilderWriter
    {

        readonly ReflectionSymbolContext _context;
        readonly TypeSymbolBuilder _builder;

        ReflectionSymbolState _state = ReflectionSymbolState.Default;
        ModuleBuilder? _parentModuleBuilder;
        TypeBuilder? _parentTypeBuilder;
        TypeBuilder? _typeBuilder;
        Type? _type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public ReflectionTypeSymbolBuilderWriter(ReflectionSymbolContext context, TypeSymbolBuilder builder)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the most complete type.
        /// </summary>
        public Type? Type => _type ?? _typeBuilder;

        /// <summary>
        /// Advances the builder to the given phase.
        /// </summary>
        /// <param name="state"></param>
        public void AdvanceTo(ReflectionSymbolState state)
        {
            lock (this)
            {
                if (state >= ReflectionSymbolState.Defined && _state < ReflectionSymbolState.Defined)
                {
                    // require parent to be finished; this is reentrant
                    if (_builder.DeclaringType == null)
                        _parentModuleBuilder = (ModuleBuilder)_context.ResolveModule((ModuleSymbolBuilder)_builder.Module, ReflectionSymbolState.Emitted);
                    else
                        _parentTypeBuilder = (TypeBuilder)_context.ResolveType((TypeSymbolBuilder)_builder.DeclaringType, ReflectionSymbolState.Emitted);
                }

                if (state >= ReflectionSymbolState.Defined && _state < ReflectionSymbolState.Defined)
                    Define();

                if (state >= ReflectionSymbolState.Emitted && _state < ReflectionSymbolState.Emitted)
                    Emit();

                if (state >= ReflectionSymbolState.Finished && _state < ReflectionSymbolState.Finished)
                    Finish();
            }
        }

        /// <summary>
        /// Executes the Declare phase, which declares the real builder and sets basic properties.
        /// </summary>
        void Define()
        {
            if (_state != ReflectionSymbolState.Default)
                throw new InvalidOperationException();

            if (_parentModuleBuilder is not null)
                _typeBuilder = _parentModuleBuilder.DefineType(_builder.FullName, _builder.Attributes, null, (PackingSize)_builder.PackingSize, _builder.TypeSize);
            else if (_parentTypeBuilder is not null)
                _typeBuilder = _parentTypeBuilder.DefineNestedType(_builder.Name, _builder.Attributes, null, (PackingSize)_builder.PackingSize, _builder.TypeSize);
            else
                throw new InvalidOperationException();

            _state = ReflectionSymbolState.Defined;
        }

        /// <summary>
        /// Advances to the Emitted phase by emitting the declarations.
        /// </summary>
        void Emit()
        {
            if (_state != ReflectionSymbolState.Defined)
                throw new InvalidOperationException();
            if (_typeBuilder == null)
                throw new InvalidOperationException();

            // freeze builder and set state
            _builder.Freeze();
            _state = ReflectionSymbolState.Emitted;

            // declare nested types: this is done first because other things may depend on them, such as the parent type
            foreach (var type in _builder.GetDeclaredNestedTypes())
                _context.ResolveType(type, ReflectionSymbolState.Defined);

            // define various properties
            _typeBuilder.SetParent(_context.ResolveType(_builder.BaseType, ReflectionSymbolState.Defined));

            // freeze the set of generic parameter builders
            var genericParameterBuilders = _builder.GenericParameters.CastArray<GenericTypeParameterTypeSymbolBuilder>();
            for (int i = 0; i < genericParameterBuilders.Length; i++)
                genericParameterBuilders[i].Freeze();

            // define generic parameters
            var realGenericParameters = _typeBuilder.DefineGenericParameters(genericParameterBuilders.Select(i => i.Name).ToArray());
            for (int i = 0; i < genericParameterBuilders.Length; i++)
            {
                var genericParameterBuilder = genericParameterBuilders[i];
                var realGenericParameter = realGenericParameters[i];

                // set properties and constraints
                realGenericParameter.SetGenericParameterAttributes(genericParameterBuilder.GenericParameterAttributes);
                realGenericParameter.SetBaseTypeConstraint(_context.ResolveType(genericParameterBuilder.BaseType, ReflectionSymbolState.Defined));
                realGenericParameter.SetInterfaceConstraints(_context.ResolveTypes(genericParameterBuilder.GetDeclaredInterfaces(), ReflectionSymbolState.Defined));

                // set custom attributes
                foreach (var customAttribute in genericParameterBuilder.GetDeclaredCustomAttributes())
                    realGenericParameter.SetCustomAttribute(_context.CreateCustomAttributeBuilder(customAttribute));
            }

            // define interfaces
            var interfaces = _builder.GetDeclaredInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
                _typeBuilder.AddInterfaceImplementation(_context.ResolveType(interfaces[i], ReflectionSymbolState.Defined));

            // declare fields
            foreach (var field in _builder.GetDeclaredFields())
                _context.ResolveField(field, ReflectionSymbolState.Defined);

            // declare methods
            foreach (var method in _builder.GetDeclaredMethods())
                _context.ResolveMethod(method, ReflectionSymbolState.Defined);

            // declare properties
            foreach (var property in _builder.GetDeclaredProperties())
                _context.ResolveProperty(property, ReflectionSymbolState.Defined);

            // declare events
            foreach (var evt in _builder.GetDeclaredEvents())
                _context.ResolveEvent(evt, ReflectionSymbolState.Defined);

            // set custom attributes
            foreach (var customAttribute in _builder.GetDeclaredCustomAttributes())
                _typeBuilder.SetCustomAttribute(_context.CreateCustomAttributeBuilder(customAttribute));
        }

        /// <summary>
        /// Executes the complete phase.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        void Finish()
        {
            if (_state != ReflectionSymbolState.Emitted)
                throw new InvalidOperationException();
            if (_typeBuilder == null)
                throw new InvalidOperationException();

            // complete nested types
            foreach (var type in _builder.GetDeclaredNestedTypes())
                _context.ResolveType(type, ReflectionSymbolState.Emitted);

            // complete fields
            foreach (var field in _builder.GetDeclaredFields())
                _context.ResolveField(field, ReflectionSymbolState.Emitted);

            // complete methods
            foreach (var method in _builder.GetDeclaredMethods())
                _context.ResolveMethod(method, ReflectionSymbolState.Emitted);

            // complete properties
            foreach (var property in _builder.GetDeclaredProperties())
                _context.ResolveProperty(property, ReflectionSymbolState.Emitted);

            // complete events
            foreach (var evt in _builder.GetDeclaredEvents())
                _context.ResolveEvent(evt, ReflectionSymbolState.Emitted);

            // bake type and set state
            _type = _typeBuilder.CreateType();
            _state = ReflectionSymbolState.Finished;

            // complete nested types
            foreach (var type in _builder.GetDeclaredNestedTypes())
                _context.ResolveType(type, ReflectionSymbolState.Finished);

            // complete fields
            foreach (var field in _builder.GetDeclaredFields())
                _context.ResolveField(field, ReflectionSymbolState.Finished);

            // complete methods
            foreach (var method in _builder.GetDeclaredMethods())
                _context.ResolveMethod(method, ReflectionSymbolState.Finished);

            // complete properties
            foreach (var property in _builder.GetDeclaredProperties())
                _context.ResolveProperty(property, ReflectionSymbolState.Finished);

            // complete events
            foreach (var evt in _builder.GetDeclaredEvents())
                _context.ResolveEvent(evt, ReflectionSymbolState.Finished);
        }

    }

}
