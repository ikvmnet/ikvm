using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionPropertySymbolBuilderWriter
    {

        readonly ReflectionSymbolContext _context;
        readonly PropertySymbolBuilder _builder;

        ReflectionSymbolState _state = ReflectionSymbolState.Default;
        TypeBuilder? _parentTypeBuilder;
        PropertyBuilder? _propertyBuilder;
        Type? _parentType;
        PropertyInfo? _property;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public ReflectionPropertySymbolBuilderWriter(ReflectionSymbolContext context, PropertySymbolBuilder builder)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public PropertyInfo? Property => _property ?? _propertyBuilder;

        /// <summary>
        /// Advances the builder to the given phase.
        /// </summary>
        /// <param name="state"></param>
        public void AdvanceTo(ReflectionSymbolState state)
        {
            lock (this)
            {
                if (state >= ReflectionSymbolState.Defined && _state < ReflectionSymbolState.Defined)
                    _parentTypeBuilder = (TypeBuilder)_context.ResolveType((TypeSymbolBuilder)_builder.DeclaringType, ReflectionSymbolState.Emitted);

                if (state >= ReflectionSymbolState.Defined && _state < ReflectionSymbolState.Defined)
                    Define();

                if (state >= ReflectionSymbolState.Emitted && _state < ReflectionSymbolState.Emitted)
                    Emit();

                if (state >= ReflectionSymbolState.Finished && _state < ReflectionSymbolState.Finished)
                    _parentType = _context.ResolveType((TypeSymbolBuilder)_builder.DeclaringType, ReflectionSymbolState.Finished);

                if (state >= ReflectionSymbolState.Finished && _state < ReflectionSymbolState.Finished)
                    Finish();
            }
        }

        /// <summary>
        /// Executes the declare phase.
        /// </summary>
        void Define()
        {
            if (_state != ReflectionSymbolState.Default)
                throw new InvalidOperationException();

            var returnType = _context.ResolveType(_builder.PropertyType, ReflectionSymbolState.Defined);
            var returnRequiredCustomModifiers = _context.ResolveTypes(_builder.PropertyType.GetRequiredCustomModifiers(), ReflectionSymbolState.Defined);
            var returnOptionalCustomModifiers = _context.ResolveTypes(_builder.PropertyType.GetOptionalCustomModifiers(), ReflectionSymbolState.Defined);

            var parameterTypes = Array.Empty<Type>();
            var parameterRequiredCustomModifiers = Array.Empty<Type[]>();
            var parameterOptionalCustomModifiers = Array.Empty<Type[]>();
            if (_builder.GetIndexParameters().Length > 0)
            {
                parameterTypes = _context.ResolveTypes(_builder.GetIndexParameters().Select(i => i.ParameterType).ToImmutableArray(), ReflectionSymbolState.Defined);
                parameterRequiredCustomModifiers = new Type[_builder.GetIndexParameters().Length][];
                parameterOptionalCustomModifiers = new Type[_builder.GetIndexParameters().Length][];
                for (int i = 0; i < _builder.GetIndexParameters().Length; i++)
                {
                    parameterRequiredCustomModifiers[i] = _context.ResolveTypes(_builder.GetIndexParameters()[i].GetRequiredCustomModifiers(), ReflectionSymbolState.Defined);
                    parameterOptionalCustomModifiers[i] = _context.ResolveTypes(_builder.GetIndexParameters()[i].GetOptionalCustomModifiers(), ReflectionSymbolState.Defined);
                }
            }

            // define property
            if (_parentTypeBuilder is not null)
                _propertyBuilder = _parentTypeBuilder.DefineProperty(_builder.Name, _builder.Attributes, returnType, returnRequiredCustomModifiers, returnOptionalCustomModifiers, parameterTypes, parameterRequiredCustomModifiers, parameterOptionalCustomModifiers);
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
            if (_propertyBuilder == null)
                throw new InvalidOperationException();

            // freeze and set properties
            _builder.Freeze();
            _propertyBuilder.SetConstant(_builder.GetRawConstantValue());

            // set get method
            if (_builder.GetMethod is MethodSymbolBuilder addMethod)
                _propertyBuilder.SetGetMethod((MethodBuilder)_context.ResolveMethod(addMethod, ReflectionSymbolState.Defined));

            // set set method
            if (_builder.SetMethod is MethodSymbolBuilder removeMethod)
                _propertyBuilder.SetSetMethod((MethodBuilder)_context.ResolveMethod(removeMethod, ReflectionSymbolState.Defined));

            // set custom attributes
            foreach (var customAttribute in _builder.GetDeclaredCustomAttributes())
                _propertyBuilder.SetCustomAttribute(_context.CreateCustomAttributeBuilder(customAttribute));

            _state = ReflectionSymbolState.Emitted;
        }

        /// <summary>
        /// Executes the export phase.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        void Finish()
        {
            if (_state != ReflectionSymbolState.Emitted)
                throw new InvalidOperationException();

            // set state
            _state = ReflectionSymbolState.Finished;

            if (_parentType is not null)
                _property = _parentType.GetProperty(_builder.Name, TypeSymbol.DeclaredOnlyLookup, null, _context.ResolveType(_builder.PropertyType), _context.ResolveTypes(_builder.GetIndexParameters().Select(i => i.ParameterType).ToImmutableArray()), null) ?? throw new InvalidOperationException();
            else
                throw new InvalidOperationException();
        }

    }

}
