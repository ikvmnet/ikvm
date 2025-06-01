﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionMethodSymbolBuilderWriter
    {

        readonly ReflectionSymbolContext _context;
        readonly MethodSymbolBuilder _builder;

        ReflectionSymbolState _state = ReflectionSymbolState.Default;

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
        public ReflectionMethodSymbolBuilderWriter(ReflectionSymbolContext context, MethodSymbolBuilder builder)
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
                {
                    // require parent to be finished; this is reentrant
                    if (_builder.DeclaringType == null)
                        _parentModule = _context.ResolveModule((ModuleSymbolBuilder)_builder.Module, ReflectionSymbolState.Finished);
                    else
                        _parentType = _context.ResolveType((TypeSymbolBuilder)_builder.DeclaringType, ReflectionSymbolState.Finished);
                }

                if (state >= ReflectionSymbolState.Finished && _state < ReflectionSymbolState.Finished)
                    Finish();
            }
        }

        /// <summary>
        /// Executes the declare phase.
        /// </summary>
        void Define()
        {
            Debug.Assert(_state == ReflectionSymbolState.Default);

            _state = ReflectionSymbolState.Defined;

            var returnType = _context.ResolveType(_builder.ReturnType, ReflectionSymbolState.Defined);
            var returnRequiredCustomModifiers = _context.ResolveTypes(_builder.ReturnParameter.GetRequiredCustomModifiers(), ReflectionSymbolState.Defined);
            var returnOptionalCustomModifiers = _context.ResolveTypes(_builder.ReturnParameter.GetOptionalCustomModifiers(), ReflectionSymbolState.Defined);

            var parameterTypes = Array.Empty<Type>();
            var parameterRequiredCustomModifiers = Array.Empty<Type[]>();
            var parameterOptionalCustomModifiers = Array.Empty<Type[]>();
            if (_builder.Parameters.Length > 0)
            {
                parameterTypes = _context.ResolveTypes(_builder.ParameterTypes, ReflectionSymbolState.Defined);
                parameterRequiredCustomModifiers = new Type[_builder.Parameters.Length][];
                parameterOptionalCustomModifiers = new Type[_builder.Parameters.Length][];
                for (int i = 0; i < _builder.Parameters.Length; i++)
                {
                    parameterRequiredCustomModifiers[i] = _context.ResolveTypes(_builder.Parameters[i].GetRequiredCustomModifiers(), ReflectionSymbolState.Defined);
                    parameterOptionalCustomModifiers[i] = _context.ResolveTypes(_builder.Parameters[i].GetOptionalCustomModifiers(), ReflectionSymbolState.Defined);
                }
            }

            // declare method
            if (_parentModuleBuilder is not null)
                _methodBuilder = _parentModuleBuilder.DefineGlobalMethod(_builder.Name, _builder.Attributes, _builder.CallingConvention, returnType, returnRequiredCustomModifiers, returnOptionalCustomModifiers, parameterTypes, parameterRequiredCustomModifiers, parameterOptionalCustomModifiers);
            else if (_parentTypeBuilder is not null)
                _methodBuilder = _parentTypeBuilder.DefineMethod(_builder.Name, _builder.Attributes, _builder.CallingConvention, returnType, returnRequiredCustomModifiers, returnOptionalCustomModifiers, parameterTypes, parameterRequiredCustomModifiers, parameterOptionalCustomModifiers);
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Executes the finish phase.
        /// </summary>
        void Emit()
        {
            if (_state != ReflectionSymbolState.Defined)
                throw new InvalidOperationException();
            if (_methodBuilder == null)
                throw new InvalidOperationException();

            // lock builder and set state
            _builder.Freeze();
            _state = ReflectionSymbolState.Emitted;

            // define various properties
            _methodBuilder.InitLocals = _builder.InitLocals;
            _methodBuilder.SetImplementationFlags(_builder.MethodImplementationFlags);

            // freeze the set of generic parameter builders
            var genericParameterBuilders = _builder.GenericParameters.CastArray<GenericMethodParameterTypeSymbolBuilder>();
            for (int i = 0; i < genericParameterBuilders.Length; i++)
                genericParameterBuilders[i].Freeze();

            // define generic parameters
            var realGenericParameters = _methodBuilder.DefineGenericParameters(genericParameterBuilders.Select(i => i.Name).ToArray());
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

            // define return parameter
            var realReturnBuilder = _methodBuilder.DefineParameter(0, _builder.ReturnParameter.Attributes, _builder.ReturnParameter.Name);
            foreach (var customAttribute in _builder.ReturnParameter.GetDeclaredCustomAttributes())
                realReturnBuilder.SetCustomAttribute(_context.CreateCustomAttributeBuilder(customAttribute));

            // define parameters
            var parameterBuilders = _builder.Parameters.CastArray<ParameterSymbolBuilder>();
            for (int i = 0; i < parameterBuilders.Length; i++)
            {
                var parameterBuilder = parameterBuilders[i];
                var realParameterBuilder = _methodBuilder.DefineParameter(i + 1, parameterBuilder.Attributes, parameterBuilder.Name);
                realParameterBuilder.SetConstant(parameterBuilder.DefaultValue);

                // set custom attributes
                foreach (var customAttribute in parameterBuilder.GetDeclaredCustomAttributes())
                    realParameterBuilder.SetCustomAttribute(_context.CreateCustomAttributeBuilder(customAttribute));
            }

            // emit method body
            _builder._il?.Write(new ReflectionILGenerationWriter(_context, _methodBuilder.GetILGenerator()));
        }

        /// <summary>
        /// Executes the complete phase.
        /// </summary>
        void Finish()
        {
            if (_state != ReflectionSymbolState.Emitted)
                throw new InvalidOperationException();

            // set state
            _state = ReflectionSymbolState.Finished;

            if (_parentModule is not null)
                _method = _parentModule.GetMethod(_builder.Name, TypeSymbol.DeclaredOnlyLookup, null, CallingConventions.Any, _context.ResolveTypes(_builder.ParameterTypes), null) ?? throw new InvalidOperationException();
            else if (_parentType is not null)
                _method = _parentType.GetMethod(_builder.Name, TypeSymbol.DeclaredOnlyLookup, null, CallingConventions.Any, _context.ResolveTypes(_builder.ParameterTypes), null) ?? throw new InvalidOperationException();
            else
                throw new InvalidOperationException();
        }

    }

}
