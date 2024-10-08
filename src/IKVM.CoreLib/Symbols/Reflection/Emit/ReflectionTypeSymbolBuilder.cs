using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionTypeSymbolBuilder : ReflectionTypeSymbolBase, IReflectionTypeSymbolBuilder
    {

        readonly TypeBuilder _builder;
        Type? _type;

        List<IReflectionMethodSymbolBuilder>? _incompleteMethods;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="builder"></param>
        public ReflectionTypeSymbolBuilder(ReflectionSymbolContext context, IReflectionModuleSymbolBuilder module, TypeBuilder builder) :
            base(context, module)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc />
        public override Type UnderlyingType => _type ?? _builder;

        /// <inheritdoc />
        public override Type UnderlyingEmitType => _builder;

        /// <inheritdoc />
        public override Type UnderlyingDynamicEmitType => _type ?? throw new InvalidOperationException();

        /// <inheritdoc />
        public TypeBuilder UnderlyingTypeBuilder => _builder;

        /// <inheritdoc />
        public IReflectionModuleSymbolBuilder ResolvingModuleBuilder => (IReflectionModuleSymbolBuilder)ResolvingModule;

        #region IReflectionSymbolBuilder

        /// <inheritdoc />
        [return: NotNullIfNotNull("genericTypeParameter")]
        public IReflectionGenericTypeParameterSymbolBuilder? ResolveGenericTypeParameterSymbol(GenericTypeParameterBuilder genericTypeParameter)
        {
            return Context.GetOrCreateGenericTypeParameterSymbol(genericTypeParameter);
        }

        #endregion

        #region IReflectionTypeSymbolBuilder

        /// <inheritdoc />
        public IReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor)
        {
            return _methodTable.GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method)
        {
            return _methodTable.GetOrCreateMethodSymbol(method);
        }

        /// <inheritdoc />
        public IReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field)
        {
            return _fieldTable.GetOrCreateFieldSymbol(field);
        }

        /// <inheritdoc />
        public IReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property)
        {
            return _propertyTable.GetOrCreatePropertySymbol(property);
        }

        /// <inheritdoc />
        public IReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event)
        {
            return _eventTable.GetOrCreateEventSymbol(@event);
        }

        #endregion

        #region ITypeSymbolBuilder

        /// <inheritdoc />
        public void SetParent(ITypeSymbol? parent)
        {
            UnderlyingTypeBuilder.SetParent(parent?.Unpack());
        }

        /// <inheritdoc />
        public IGenericTypeParameterSymbolBuilder[] DefineGenericParameters(params string[] names)
        {
            var l = UnderlyingTypeBuilder.DefineGenericParameters(names);
            var a = new IGenericTypeParameterSymbolBuilder[l.Length];
            for (int i = 0; i < l.Length; i++)
                a[i] = ResolveGenericTypeParameterSymbol(l[i]);

            return a;
        }

        /// <inheritdoc />
        public void AddInterfaceImplementation(ITypeSymbol interfaceType)
        {
            UnderlyingTypeBuilder.AddInterfaceImplementation(interfaceType.Unpack());
        }

        /// <inheritdoc />
        public IConstructorSymbolBuilder DefineConstructor(System.Reflection.MethodAttributes attributes, ITypeSymbol[]? parameterTypes)
        {
            return ResolveConstructorSymbol(UnderlyingTypeBuilder.DefineConstructor((MethodAttributes)attributes, CallingConventions.Standard, parameterTypes?.Unpack()));
        }

        /// <inheritdoc />
        public IConstructorSymbolBuilder DefineConstructor(System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol[]? parameterTypes)
        {
            return ResolveConstructorSymbol(UnderlyingTypeBuilder.DefineConstructor((MethodAttributes)attributes, (CallingConventions)callingConvention, parameterTypes?.Unpack()));
        }

        /// <inheritdoc />
        public IConstructorSymbolBuilder DefineConstructor(System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? requiredCustomModifiers, ITypeSymbol[][]? optionalCustomModifiers)
        {
            return ResolveConstructorSymbol(UnderlyingTypeBuilder.DefineConstructor((MethodAttributes)attributes, (CallingConventions)callingConvention, parameterTypes?.Unpack(), requiredCustomModifiers?.Unpack(), optionalCustomModifiers?.Unpack()));
        }

        /// <inheritdoc />
        public IConstructorSymbolBuilder DefineDefaultConstructor(System.Reflection.MethodAttributes attributes)
        {
            return ResolveConstructorSymbol(UnderlyingTypeBuilder.DefineDefaultConstructor((MethodAttributes)attributes));
        }

        /// <inheritdoc />
        public IEventSymbolBuilder DefineEvent(string name, System.Reflection.EventAttributes attributes, ITypeSymbol eventtype)
        {
            return ResolveEventSymbol(UnderlyingTypeBuilder.DefineEvent(name, (EventAttributes)attributes, eventtype.Unpack()));
        }

        /// <inheritdoc />
        public IFieldSymbolBuilder DefineField(string fieldName, ITypeSymbol type, System.Reflection.FieldAttributes attributes)
        {
            return ResolveFieldSymbol(UnderlyingTypeBuilder.DefineField(fieldName, type.Unpack(), (FieldAttributes)attributes));
        }

        /// <inheritdoc />
        public IFieldSymbolBuilder DefineField(string fieldName, ITypeSymbol type, ITypeSymbol[]? requiredCustomModifiers, ITypeSymbol[]? optionalCustomModifiers, System.Reflection.FieldAttributes attributes)
        {
            return ResolveFieldSymbol(UnderlyingTypeBuilder.DefineField(fieldName, type.Unpack(), requiredCustomModifiers?.Unpack(), optionalCustomModifiers?.Unpack(), (FieldAttributes)attributes));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers)
        {
            var m = ResolveMethodSymbol(UnderlyingTypeBuilder.DefineMethod(name, (MethodAttributes)attributes, (CallingConventions)callingConvention, returnType?.Unpack(), returnTypeRequiredCustomModifiers?.Unpack(), returnTypeOptionalCustomModifiers?.Unpack(), parameterTypes?.Unpack(), parameterTypeRequiredCustomModifiers?.Unpack(), parameterTypeOptionalCustomModifiers?.Unpack()));
            _incompleteMethods ??= [];
            _incompleteMethods.Add(m);
            return m;
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes)
        {
            var m = ResolveMethodSymbol(UnderlyingTypeBuilder.DefineMethod(name, (MethodAttributes)attributes, (CallingConventions)callingConvention, returnType?.Unpack(), parameterTypes?.Unpack()));
            _incompleteMethods ??= [];
            _incompleteMethods.Add(m);
            return m;
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention)
        {
            var m = ResolveMethodSymbol(UnderlyingTypeBuilder.DefineMethod(name, (MethodAttributes)attributes, (CallingConventions)callingConvention));
            _incompleteMethods ??= [];
            _incompleteMethods.Add(m);
            return m;
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes)
        {
            var m = ResolveMethodSymbol(UnderlyingTypeBuilder.DefineMethod(name, (MethodAttributes)attributes));
            _incompleteMethods ??= [];
            _incompleteMethods.Add(m);
            return m;
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes)
        {
            var m = ResolveMethodSymbol(UnderlyingTypeBuilder.DefineMethod(name, (MethodAttributes)attributes, returnType?.Unpack(), parameterTypes?.Unpack()));
            _incompleteMethods ??= [];
            _incompleteMethods.Add(m);
            return m;
        }

        /// <inheritdoc />
        public void DefineMethodOverride(IMethodSymbol methodInfoBody, IMethodSymbol methodInfoDeclaration)
        {
            UnderlyingTypeBuilder.DefineMethodOverride(methodInfoBody.Unpack(), methodInfoDeclaration.Unpack());
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, ITypeSymbol[]? interfaces)
        {
            return ResolveTypeSymbol(UnderlyingTypeBuilder.DefineNestedType(name, (TypeAttributes)attr, parent?.Unpack(), interfaces?.Unpack()));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, System.Reflection.Emit.PackingSize packSize, int typeSize)
        {
            return ResolveTypeSymbol(UnderlyingTypeBuilder.DefineNestedType(name, (TypeAttributes)attr, parent?.Unpack(), (PackingSize)packSize, typeSize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, System.Reflection.Emit.PackingSize packSize)
        {
            return (ITypeSymbolBuilder)ResolveTypeSymbol(UnderlyingTypeBuilder.DefineNestedType(name, (TypeAttributes)attr, parent?.Unpack(), (PackingSize)packSize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name)
        {
            return (ITypeSymbolBuilder)ResolveTypeSymbol(UnderlyingTypeBuilder.DefineNestedType(name));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent)
        {
            return (ITypeSymbolBuilder)ResolveTypeSymbol(UnderlyingTypeBuilder.DefineNestedType(name, (TypeAttributes)attr, parent?.Unpack()));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr)
        {
            return (ITypeSymbolBuilder)ResolveTypeSymbol(UnderlyingTypeBuilder.DefineNestedType(name, (TypeAttributes)attr));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, int typeSize)
        {
            return (ITypeSymbolBuilder)ResolveTypeSymbol(UnderlyingTypeBuilder.DefineNestedType(name, (TypeAttributes)attr, parent?.Unpack(), typeSize));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            return (IMethodSymbolBuilder)ResolveMethodSymbol(UnderlyingTypeBuilder.DefinePInvokeMethod(name, dllName, (MethodAttributes)attributes, (CallingConventions)callingConvention, returnType?.Unpack(), parameterTypes?.Unpack(), nativeCallConv, nativeCharSet));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, string entryName, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            return (IMethodSymbolBuilder)ResolveMethodSymbol(UnderlyingTypeBuilder.DefinePInvokeMethod(name, dllName, entryName, (MethodAttributes)attributes, (CallingConventions)callingConvention, returnType?.Unpack(), parameterTypes?.Unpack(), nativeCallConv, nativeCharSet));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, string entryName, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            return (IMethodSymbolBuilder)ResolveMethodSymbol(UnderlyingTypeBuilder.DefinePInvokeMethod(name, dllName, entryName, (MethodAttributes)attributes, (CallingConventions)callingConvention, returnType?.Unpack(), returnTypeRequiredCustomModifiers?.Unpack(), returnTypeOptionalCustomModifiers?.Unpack(), parameterTypes?.Unpack(), parameterTypeRequiredCustomModifiers?.Unpack(), parameterTypeOptionalCustomModifiers?.Unpack(), nativeCallConv, nativeCharSet));
        }

        /// <inheritdoc />
        public IPropertySymbolBuilder DefineProperty(string name, System.Reflection.PropertyAttributes attributes, ITypeSymbol returnType, ITypeSymbol[]? parameterTypes)
        {
            return (IPropertySymbolBuilder)ResolvePropertySymbol(UnderlyingTypeBuilder.DefineProperty(name, (PropertyAttributes)attributes, returnType.Unpack(), parameterTypes?.Unpack()));
        }

        /// <inheritdoc />
        public IPropertySymbolBuilder DefineProperty(string name, System.Reflection.PropertyAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol returnType, ITypeSymbol[]? parameterTypes)
        {
            return (IPropertySymbolBuilder)ResolvePropertySymbol(UnderlyingTypeBuilder.DefineProperty(name, (PropertyAttributes)attributes, (CallingConventions)callingConvention, returnType.Unpack(), parameterTypes?.Unpack()));
        }

        /// <inheritdoc />
        public IPropertySymbolBuilder DefineProperty(string name, System.Reflection.PropertyAttributes attributes, ITypeSymbol returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers)
        {
            return (IPropertySymbolBuilder)ResolvePropertySymbol(UnderlyingTypeBuilder.DefineProperty(name, (PropertyAttributes)attributes, returnType.Unpack(), returnTypeRequiredCustomModifiers?.Unpack(), returnTypeOptionalCustomModifiers?.Unpack(), parameterTypes?.Unpack(), parameterTypeRequiredCustomModifiers?.Unpack(), parameterTypeOptionalCustomModifiers?.Unpack()));
        }

        /// <inheritdoc />
        public IPropertySymbolBuilder DefineProperty(string name, System.Reflection.PropertyAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers)
        {
            return (IPropertySymbolBuilder)ResolvePropertySymbol(UnderlyingTypeBuilder.DefineProperty(name, (PropertyAttributes)attributes, (CallingConventions)callingConvention, returnType.Unpack(), returnTypeRequiredCustomModifiers?.Unpack(), returnTypeOptionalCustomModifiers?.Unpack(), parameterTypes?.Unpack(), parameterTypeRequiredCustomModifiers?.Unpack(), parameterTypeOptionalCustomModifiers?.Unpack()));
        }

        /// <inheritdoc />
        public IConstructorSymbolBuilder DefineTypeInitializer()
        {
            return (IConstructorSymbolBuilder)ResolveConstructorSymbol(UnderlyingTypeBuilder.DefineTypeInitializer());
        }
        /// <inheritdoc />
        public void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            UnderlyingTypeBuilder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            UnderlyingTypeBuilder.SetCustomAttribute(((ReflectionCustomAttributeBuilder)customBuilder).UnderlyingBuilder);
        }

        #endregion

        /// <inheritdoc />
        public void Complete()
        {
            if (_builder != null)
            {
                // complete type
                if (_builder.IsCreated() == false)
                    _type = _builder.CreateType()!;

                // force module to reresolve
                Context.GetOrCreateModuleSymbol(ResolvingModule.UnderlyingModule);
                OnComplete();
            }
        }

        /// <inheritdoc />
        public void OnComplete()
        {
            const BindingFlags DefaultBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            foreach (var i in GetGenericArguments())
                if (i is IReflectionGenericTypeParameterSymbolBuilder b)
                    b.OnComplete();

            foreach (var i in GetConstructors(DefaultBindingFlags))
                if (i is IReflectionConstructorSymbolBuilder b)
                    b.OnComplete();

            foreach (var i in GetMethods(DefaultBindingFlags))
                if (i is IReflectionMethodSymbolBuilder b)
                    b.OnComplete();

            foreach (var i in GetFields(DefaultBindingFlags))
                if (i is IReflectionFieldSymbolBuilder b)
                    b.OnComplete();

            foreach (var i in GetProperties(DefaultBindingFlags))
                if (i is IReflectionPropertySymbolBuilder b)
                    b.OnComplete();

            foreach (var m in GetEvents(DefaultBindingFlags))
                if (m is IReflectionPropertySymbolBuilder b)
                    b.OnComplete();
        }

    }

}
