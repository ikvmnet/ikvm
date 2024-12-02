using System;
using System.Linq;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    /// <summary>
    /// Writes the symbol builder to the underlying type builder and type.
    /// </summary>
    class IkvmReflectionTypeSymbolBuilderWriter
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly TypeSymbolBuilder _builder;

        IkvmReflectionSymbolState _state = IkvmReflectionSymbolState.Default;
        ModuleBuilder? _parentModuleBuilder;
        TypeBuilder? _parentTypeBuilder;
        TypeBuilder? _typeBuilder;
        Type? _type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public IkvmReflectionTypeSymbolBuilderWriter(IkvmReflectionSymbolContext context, TypeSymbolBuilder builder)
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
        public void AdvanceTo(IkvmReflectionSymbolState state)
        {
            if (state >= IkvmReflectionSymbolState.Declared && _state < IkvmReflectionSymbolState.Declared)
            {
                // require parent to be finished; this is reentrant
                if (_builder.DeclaringType == null)
                    _parentModuleBuilder = (ModuleBuilder)_context.ResolveModule((ModuleSymbolBuilder)_builder.Module, IkvmReflectionSymbolState.Finished);
                else
                    _parentTypeBuilder = (TypeBuilder)_context.ResolveType((TypeSymbolBuilder)_builder.DeclaringType, IkvmReflectionSymbolState.Finished);
            }

            if (state >= IkvmReflectionSymbolState.Declared && _state < IkvmReflectionSymbolState.Declared)
                Declare();

            if (state >= IkvmReflectionSymbolState.Finished && _state < IkvmReflectionSymbolState.Finished)
                Finish();

            if (state >= IkvmReflectionSymbolState.Completed && _state < IkvmReflectionSymbolState.Completed)
                Complete();
        }

        /// <summary>
        /// Executes the Declare phase, which declares the real builder and sets basic properties.
        /// </summary>
        void Declare()
        {
            if (_state != IkvmReflectionSymbolState.Default)
                throw new InvalidOperationException();

            if (_parentModuleBuilder is not null)
                _typeBuilder = _parentModuleBuilder.DefineType(_builder.FullName, (TypeAttributes)_builder.Attributes, null, (PackingSize)_builder.PackingSize, _builder.TypeSize);
            else if (_parentTypeBuilder is not null)
                _typeBuilder = _parentTypeBuilder.DefineNestedType(_builder.Name, (TypeAttributes)_builder.Attributes, null, (PackingSize)_builder.PackingSize, _builder.TypeSize);
            else
                throw new InvalidOperationException();

            _state = IkvmReflectionSymbolState.Declared;
        }

        /// <summary>
        /// Advances to the Emitted phase by emitting the declarations.
        /// </summary>
        void Finish()
        {
            if (_state != IkvmReflectionSymbolState.Declared)
                throw new InvalidOperationException();
            if (_typeBuilder == null)
                throw new InvalidOperationException();

            // freeze builder and set state
            _builder.Freeze();
            _state = IkvmReflectionSymbolState.Finished;

            // declare nested types: this is done first because other things may depend on them, such as the parent type
            foreach (var type in _builder.GetDeclaredNestedTypes())
                _context.ResolveType(type, IkvmReflectionSymbolState.Declared);

            // define various properties
            _typeBuilder.SetParent(_context.ResolveType(_builder.BaseType, IkvmReflectionSymbolState.Declared));

            // freeze the set of generic parameter builders
            var genericParameterBuilders = _builder.GenericArguments.CastArray<GenericTypeParameterTypeSymbolBuilder>();
            for (int i = 0; i < genericParameterBuilders.Length; i++)
                genericParameterBuilders[i].Freeze();

            // define generic parameters
            var realGenericParameters = _typeBuilder.DefineGenericParameters(genericParameterBuilders.Select(i => i.Name).ToArray());
            for (int i = 0; i < genericParameterBuilders.Length; i++)
            {
                var genericParameterBuilder = genericParameterBuilders[i];
                var realGenericParameter = realGenericParameters[i];

                // set properties and constraints
                realGenericParameter.SetGenericParameterAttributes((IKVM.Reflection.GenericParameterAttributes)genericParameterBuilder.GenericParameterAttributes);
                realGenericParameter.SetBaseTypeConstraint(_context.ResolveType(genericParameterBuilder.BaseType, IkvmReflectionSymbolState.Declared));
                realGenericParameter.SetInterfaceConstraints(_context.ResolveTypes(genericParameterBuilder.GetDeclaredInterfaces(), IkvmReflectionSymbolState.Declared));

                // set custom attributes
                foreach (var customAttribute in genericParameterBuilder.GetDeclaredCustomAttributes())
                    realGenericParameter.SetCustomAttribute(_context.CreateCustomAttributeBuilder(customAttribute));
            }

            // define interfaces
            var interfaces = _builder.GetDeclaredInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
                _typeBuilder.AddInterfaceImplementation(_context.ResolveType(interfaces[i], IkvmReflectionSymbolState.Declared));

            // declare fields
            foreach (var field in _builder.GetDeclaredFields())
                _context.ResolveField(field, IkvmReflectionSymbolState.Declared);

            // declare methods
            foreach (var method in _builder.GetDeclaredMethods())
                _context.ResolveMethod(method, IkvmReflectionSymbolState.Declared);

            // declare properties
            foreach (var property in _builder.GetDeclaredProperties())
                _context.ResolveProperty(property, IkvmReflectionSymbolState.Declared);

            // declare events
            foreach (var evt in _builder.GetDeclaredEvents())
                _context.ResolveEvent(evt, IkvmReflectionSymbolState.Declared);

            // set custom attributes
            foreach (var customAttribute in _builder.GetDeclaredCustomAttributes())
                _typeBuilder.SetCustomAttribute(_context.CreateCustomAttributeBuilder(customAttribute));
        }

        /// <summary>
        /// Executes the complete phase.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        void Complete()
        {
            if (_state != IkvmReflectionSymbolState.Finished)
                throw new InvalidOperationException();
            if (_typeBuilder == null)
                throw new InvalidOperationException();

            // complete nested types
            foreach (var type in _builder.GetDeclaredNestedTypes())
                _context.ResolveType(type, IkvmReflectionSymbolState.Finished);

            // complete fields
            foreach (var field in _builder.GetDeclaredFields())
                _context.ResolveField(field, IkvmReflectionSymbolState.Finished);

            // complete methods
            foreach (var method in _builder.GetDeclaredMethods())
                _context.ResolveMethod(method, IkvmReflectionSymbolState.Finished);

            // complete properties
            foreach (var property in _builder.GetDeclaredProperties())
                _context.ResolveProperty(property, IkvmReflectionSymbolState.Finished);

            // complete events
            foreach (var evt in _builder.GetDeclaredEvents())
                _context.ResolveEvent(evt, IkvmReflectionSymbolState.Finished);

            // bake type and set state
            _type = _typeBuilder.CreateType();
            _state = IkvmReflectionSymbolState.Completed;

            // complete nested types
            foreach (var type in _builder.GetDeclaredNestedTypes())
                _context.ResolveType(type, IkvmReflectionSymbolState.Completed);

            // complete fields
            foreach (var field in _builder.GetDeclaredFields())
                _context.ResolveField(field, IkvmReflectionSymbolState.Completed);

            // complete methods
            foreach (var method in _builder.GetDeclaredMethods())
                _context.ResolveMethod(method, IkvmReflectionSymbolState.Completed);

            // complete properties
            foreach (var property in _builder.GetDeclaredProperties())
                _context.ResolveProperty(property, IkvmReflectionSymbolState.Completed);

            // complete events
            foreach (var evt in _builder.GetDeclaredEvents())
                _context.ResolveEvent(evt, IkvmReflectionSymbolState.Completed);
        }

    }

}
