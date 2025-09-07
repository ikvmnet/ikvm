using System;
using System.Collections.Immutable;
using System.Linq;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionPropertySymbolBuilderWriter
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly PropertySymbolBuilder _builder;

        IkvmReflectionSymbolState _state = IkvmReflectionSymbolState.Default;
        TypeBuilder? _parentTypeBuilder;
        PropertyBuilder? _propertyBuilder;
        Type? _parentType;
        PropertyInfo? _property;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public IkvmReflectionPropertySymbolBuilderWriter(IkvmReflectionSymbolContext context, PropertySymbolBuilder builder)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public PropertyInfo? Property => _property ?? _propertyBuilder;

        /// <summary>
        /// Advances the builder to the given phase.
        /// </summary>
        /// <param name="state"></param>
        public void AdvanceTo(IkvmReflectionSymbolState state)
        {
            if (state >= IkvmReflectionSymbolState.Defined && _state < IkvmReflectionSymbolState.Defined)
                _parentTypeBuilder = (TypeBuilder)_context.ResolveType((TypeSymbolBuilder)_builder.DeclaringType, IkvmReflectionSymbolState.Emitted);

            if (state >= IkvmReflectionSymbolState.Defined && _state < IkvmReflectionSymbolState.Defined)
                Define();

            if (state >= IkvmReflectionSymbolState.Emitted && _state < IkvmReflectionSymbolState.Emitted)
                Emit();

            if (state >= IkvmReflectionSymbolState.Finished && _state < IkvmReflectionSymbolState.Finished)
                _parentType = _context.ResolveType((TypeSymbolBuilder)_builder.DeclaringType, IkvmReflectionSymbolState.Finished);

            if (state >= IkvmReflectionSymbolState.Finished && _state < IkvmReflectionSymbolState.Finished)
                Finish();
        }

        /// <summary>
        /// Executes the declare phase.
        /// </summary>
        void Define()
        {
            if (_state != IkvmReflectionSymbolState.Default)
                throw new InvalidOperationException();

            var returnType = _context.ResolveType(_builder.PropertyType, IkvmReflectionSymbolState.Defined);
            var returnRequiredCustomModifiers = _context.ResolveTypes(_builder.PropertyType.GetRequiredCustomModifiers(), IkvmReflectionSymbolState.Defined);
            var returnOptionalCustomModifiers = _context.ResolveTypes(_builder.PropertyType.GetOptionalCustomModifiers(), IkvmReflectionSymbolState.Defined);

            var parameterTypes = Array.Empty<Type>();
            var parameterRequiredCustomModifiers = Array.Empty<Type[]>();
            var parameterOptionalCustomModifiers = Array.Empty<Type[]>();
            if (_builder.GetIndexParameters().Length > 0)
            {
                parameterTypes = _context.ResolveTypes(_builder.GetIndexParameters().Select(i => i.ParameterType).ToImmutableArray(), IkvmReflectionSymbolState.Defined);
                parameterRequiredCustomModifiers = new Type[_builder.GetIndexParameters().Length][];
                parameterOptionalCustomModifiers = new Type[_builder.GetIndexParameters().Length][];
                for (int i = 0; i < _builder.GetIndexParameters().Length; i++)
                {
                    parameterRequiredCustomModifiers[i] = _context.ResolveTypes(_builder.GetIndexParameters()[i].GetRequiredCustomModifiers(), IkvmReflectionSymbolState.Defined);
                    parameterOptionalCustomModifiers[i] = _context.ResolveTypes(_builder.GetIndexParameters()[i].GetOptionalCustomModifiers(), IkvmReflectionSymbolState.Defined);
                }
            }

            // define property
            if (_parentTypeBuilder is not null)
                _propertyBuilder = _parentTypeBuilder.DefineProperty(_builder.Name, (IKVM.Reflection.PropertyAttributes)_builder.Attributes, returnType, returnRequiredCustomModifiers, returnOptionalCustomModifiers, parameterTypes, parameterRequiredCustomModifiers, parameterOptionalCustomModifiers);
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
            if (_propertyBuilder == null)
                throw new InvalidOperationException();

            // freeze and set properties
            _builder.Freeze();
            _propertyBuilder.SetConstant(_builder.GetRawConstantValue());

            // set get method
            if (_builder.GetMethod is MethodSymbolBuilder addMethod)
                _propertyBuilder.SetGetMethod((MethodBuilder)_context.ResolveMethod((MethodSymbol)addMethod, IkvmReflectionSymbolState.Defined));

            // set set method
            if (_builder.SetMethod is MethodSymbolBuilder removeMethod)
                _propertyBuilder.SetSetMethod((MethodBuilder)_context.ResolveMethod((MethodSymbol)removeMethod, IkvmReflectionSymbolState.Defined));

            // set custom attributes
            foreach (var customAttribute in _builder.GetDeclaredCustomAttributes())
                _propertyBuilder.SetCustomAttribute(_context.CreateCustomAttributeBuilder(customAttribute));

            _state = IkvmReflectionSymbolState.Emitted;
        }

        /// <summary>
        /// Executes the export phase.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        void Finish()
        {
            if (_state != IkvmReflectionSymbolState.Emitted)
                throw new InvalidOperationException();

            // set state
            _state = IkvmReflectionSymbolState.Finished;

            if (_parentType is not null)
                _property = _parentType.GetProperty(_builder.Name, (BindingFlags)TypeSymbol.DeclaredOnlyLookup, null, _context.ResolveType(_builder.PropertyType), _context.ResolveTypes(_builder.GetIndexParameters().Select(i => i.ParameterType).ToImmutableArray()), null) ?? throw new InvalidOperationException();
            else
                throw new InvalidOperationException();
        }

    }

}
