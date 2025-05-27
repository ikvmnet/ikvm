using System;
using System.Diagnostics;
using System.Linq;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionMethodSymbolBuilderWriter
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly MethodSymbolBuilder _builder;

        IkvmReflectionSymbolState _state = IkvmReflectionSymbolState.Default;

        ModuleBuilder? _parentModuleBuilder;
        TypeBuilder? _parentTypeBuilder;
        MethodBuilder? _methodBuilder;
        Module? _parentModule;
        Type? _parentType;
        MethodInfo? _method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public IkvmReflectionMethodSymbolBuilderWriter(IkvmReflectionSymbolContext context, MethodSymbolBuilder builder)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the most up to date underlying method object.
        /// </summary>
        public MethodInfo? Method => _method ?? _methodBuilder;

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
            {
                // require parent to be finished; this is reentrant
                if (_builder.DeclaringType == null)
                    _parentModule = _context.ResolveModule((ModuleSymbolBuilder)_builder.Module, IkvmReflectionSymbolState.Completed);
                else
                    _parentType = _context.ResolveType((TypeSymbolBuilder)_builder.DeclaringType, IkvmReflectionSymbolState.Completed);
            }

            if (state >= IkvmReflectionSymbolState.Completed && _state < IkvmReflectionSymbolState.Completed)
                Complete();
        }

        /// <summary>
        /// Executes the declare phase.
        /// </summary>
        void Declare()
        {
            Debug.Assert(_state == IkvmReflectionSymbolState.Default);

            _state = IkvmReflectionSymbolState.Declared;

            var returnType = _context.ResolveType(_builder.ReturnType, IkvmReflectionSymbolState.Declared);
            var returnRequiredCustomModifiers = _context.ResolveTypes(_builder.ReturnParameter.GetRequiredCustomModifiers(), IkvmReflectionSymbolState.Declared);
            var returnOptionalCustomModifiers = _context.ResolveTypes(_builder.ReturnParameter.GetOptionalCustomModifiers(), IkvmReflectionSymbolState.Declared);

            var parameterTypes = Array.Empty<Type>();
            var parameterRequiredCustomModifiers = Array.Empty<Type[]>();
            var parameterOptionalCustomModifiers = Array.Empty<Type[]>();
            if (_builder.Parameters.Length > 0)
            {
                parameterTypes = _context.ResolveTypes(_builder.ParameterTypes, IkvmReflectionSymbolState.Declared);
                parameterRequiredCustomModifiers = new Type[_builder.Parameters.Length][];
                parameterOptionalCustomModifiers = new Type[_builder.Parameters.Length][];
                for (int i = 0; i < _builder.Parameters.Length; i++)
                {
                    parameterRequiredCustomModifiers[i] = _context.ResolveTypes(_builder.Parameters[i].GetRequiredCustomModifiers(), IkvmReflectionSymbolState.Declared);
                    parameterOptionalCustomModifiers[i] = _context.ResolveTypes(_builder.Parameters[i].GetOptionalCustomModifiers(), IkvmReflectionSymbolState.Declared);
                }
            }

            // declare method
            if (_parentModuleBuilder is not null)
                _methodBuilder = _parentModuleBuilder.DefineGlobalMethod(_builder.Name, (IKVM.Reflection.MethodAttributes)_builder.Attributes, (IKVM.Reflection.CallingConventions)_builder.CallingConvention, returnType, returnRequiredCustomModifiers, returnOptionalCustomModifiers, parameterTypes, parameterRequiredCustomModifiers, parameterOptionalCustomModifiers);
            else if (_parentTypeBuilder is not null)
                _methodBuilder = _parentTypeBuilder.DefineMethod(_builder.Name, (IKVM.Reflection.MethodAttributes)_builder.Attributes, (IKVM.Reflection.CallingConventions)_builder.CallingConvention, returnType, returnRequiredCustomModifiers, returnOptionalCustomModifiers, parameterTypes, parameterRequiredCustomModifiers, parameterOptionalCustomModifiers);
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Executes the finish phase.
        /// </summary>
        void Finish()
        {
            if (_state != IkvmReflectionSymbolState.Declared)
                throw new InvalidOperationException();
            if (_methodBuilder == null)
                throw new InvalidOperationException();

            // lock builder and set state
            _builder.Freeze();
            _state = IkvmReflectionSymbolState.Finished;

            // define various properties
            _methodBuilder.InitLocals = _builder.InitLocals;
            _methodBuilder.SetImplementationFlags((IKVM.Reflection.MethodImplAttributes)_builder.MethodImplementationFlags);

            // freeze the set of generic parameter builders
            var genericParameterBuilders = _builder.GenericArguments.CastArray<GenericMethodParameterTypeSymbolBuilder>();
            for (int i = 0; i < genericParameterBuilders.Length; i++)
                genericParameterBuilders[i].Freeze();

            // define generic parameters
            var realGenericParameters = _methodBuilder.DefineGenericParameters(genericParameterBuilders.Select(i => i.Name).ToArray());
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

            // define return parameter
            var realReturnBuilder = _methodBuilder.DefineParameter(0, (IKVM.Reflection.ParameterAttributes)_builder.ReturnParameter.Attributes, _builder.ReturnParameter.Name);
            foreach (var customAttribute in _builder.ReturnParameter.GetDeclaredCustomAttributes())
                realReturnBuilder.SetCustomAttribute(_context.CreateCustomAttributeBuilder(customAttribute));

            // define parameters
            var parameterBuilders = _builder.Parameters.CastArray<ParameterSymbolBuilder>();
            for (int i = 0; i < parameterBuilders.Length; i++)
            {
                var parameterBuilder = parameterBuilders[i];
                var realParameterBuilder = _methodBuilder.DefineParameter(i + 1, (IKVM.Reflection.ParameterAttributes)parameterBuilder.Attributes, parameterBuilder.Name);
                realParameterBuilder.SetConstant(parameterBuilder.DefaultValue);

                // set custom attributes
                foreach (var customAttribute in parameterBuilder.GetDeclaredCustomAttributes())
                    realParameterBuilder.SetCustomAttribute(_context.CreateCustomAttributeBuilder(customAttribute));
            }

            // emit method body
            var ilgen = _builder._il;
            if (ilgen is not null)
                ilgen.Write(new IkvmReflectionILGenerationWriter(_context, _methodBuilder.GetILGenerator()));
        }

        /// <summary>
        /// Executes the complete phase.
        /// </summary>
        void Complete()
        {
            if (_state != IkvmReflectionSymbolState.Finished)
                throw new InvalidOperationException();

            // set state
            _state = IkvmReflectionSymbolState.Completed;

            if (_parentModule is not null)
                _method = _parentModule.GetMethod(_builder.Name, (BindingFlags)TypeSymbol.DeclaredOnlyLookup, null, CallingConventions.Any, _context.ResolveTypes(_builder.ParameterTypes), null) ?? throw new InvalidOperationException();
            else if (_parentType is not null)
                _method = _parentType.GetMethod(_builder.Name, (BindingFlags)TypeSymbol.DeclaredOnlyLookup, null, CallingConventions.Any, _context.ResolveTypes(_builder.ParameterTypes), null) ?? throw new InvalidOperationException();
            else
                throw new InvalidOperationException();
        }

    }

}
