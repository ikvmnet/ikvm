using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionTypeSymbolBuilder : ReflectionTypeSymbolBase, IReflectionTypeSymbolBuilder
    {

        readonly TypeBuilder _builder;
        Type? _type;

        ITypeSymbol _parentType;
        List<IGenericTypeParameterSymbolBuilder>? _genericTypeParameters;
        List<IReflectionTypeSymbol> _interfaces;
        List<IReflectionFieldSymbolBuilder> _fields;
        List<IReflectionConstructorSymbolBuilder>? _constructors;
        List<IReflectionMethodSymbolBuilder>? _methods;
        List<IReflectionPropertySymbolBuilder>? _properties;
        List<IReflectionEventSymbolBuilder>? _events;
        List<CustomAttribute>? _customAttributes;

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
        public TypeBuilder UnderlyingTypeBuilder => _builder;

        /// <inheritdoc />
        public override Type UnderlyingType => _builder;

        /// <inheritdoc />
        public override Type UnderlyingRuntimeType => _type ?? throw new InvalidOperationException();

        /// <inheritdoc />
        public IReflectionModuleSymbolBuilder ResolvingModuleBuilder => (IReflectionModuleSymbolBuilder)ResolvingModule;

        #region ITypeSymbolBuilder

        /// <inheritdoc />
        public void SetParent(ITypeSymbol? parent)
        {
            _parentType = (IReflectionTypeSymbol?)parent;
        }

        /// <inheritdoc />
        public IGenericTypeParameterSymbolBuilder[] DefineGenericParameters(params string[] names)
        {
            var a = new IGenericTypeParameterSymbolBuilder[l.Length];
            for (int i = 0; i < l.Length; i++)
                a[i] = new ReflectionGenericTypeParameterSymbolBuilder(Context, ResolvingModuleBuilder, this);

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
            return ResolveFieldSymbol(UnderlyingTypeBuilder.DefineField(fieldName, type.Unpack(), attributes), [], []);
        }

        /// <inheritdoc />
        public IFieldSymbolBuilder DefineField(string fieldName, ITypeSymbol type, ITypeSymbol[]? requiredCustomModifiers, ITypeSymbol[]? optionalCustomModifiers, FieldAttributes attributes)
        {
            return ResolveFieldSymbol(UnderlyingTypeBuilder.DefineField(fieldName, type.Unpack(), requiredCustomModifiers?.Unpack(), optionalCustomModifiers?.Unpack(), attributes), requiredCustomModifiers, optionalCustomModifiers);
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers)
        {
            var m = ResolveMethodSymbol(UnderlyingTypeBuilder.DefineMethod(name, (MethodAttributes)attributes, (CallingConventions)callingConvention, returnType?.Unpack(), returnTypeRequiredCustomModifiers?.Unpack(), returnTypeOptionalCustomModifiers?.Unpack(), parameterTypes?.Unpack(), parameterTypeRequiredCustomModifiers?.Unpack(), parameterTypeOptionalCustomModifiers?.Unpack()));
            _methods ??= [];
            _methods.Add(m);
            return m;
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes)
        {
            var m = ResolveMethodSymbol(UnderlyingTypeBuilder.DefineMethod(name, (MethodAttributes)attributes, (CallingConventions)callingConvention, returnType?.Unpack(), parameterTypes?.Unpack()));
            _methods ??= [];
            _methods.Add(m);
            return m;
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention)
        {
            var m = ResolveMethodSymbol(UnderlyingTypeBuilder.DefineMethod(name, (MethodAttributes)attributes, (CallingConventions)callingConvention));
            _methods ??= [];
            _methods.Add(m);
            return m;
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes)
        {
            var m = ResolveMethodSymbol(UnderlyingTypeBuilder.DefineMethod(name, (MethodAttributes)attributes));
            _methods ??= [];
            _methods.Add(m);
            return m;
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes)
        {
            var m = ResolveMethodSymbol(UnderlyingTypeBuilder.DefineMethod(name, (MethodAttributes)attributes, returnType?.Unpack(), parameterTypes?.Unpack()));
            _methods ??= [];
            _methods.Add(m);
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
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            UnderlyingTypeBuilder.SetCustomAttribute(attribute.Unpack());
            _customAttributes ??= [];
            _customAttributes.Add(attribute);
        }

        #endregion

        #region ITypeSymbol

        /// <inheritdoc />
        public override bool IsComplete => _type != null;

        /// <inheritdoc />
        public override IMethodSymbol? GetMethod(string name)
        {
            if (IsComplete)
                return base.GetMethod(name);
            else
                return GetIncompleteMethods().FirstOrDefault(i => i.Name == name);
        }

        /// <inheritdoc />
        public override IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            if (IsComplete)
                return base.GetMethod(name, bindingAttr, types, modifiers);
            else
                throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, CallingConventions callConvention, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            if (IsComplete)
                return base.GetMethod(name, bindingAttr, callConvention, types, modifiers);
            else
                throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override IMethodSymbol[] GetMethods()
        {
            if (IsComplete)
                return base.GetMethods();
            else
                return GetIncompleteMethods();
        }

        /// <inheritdoc />
        public override IMethodSymbol[] GetMethods(BindingFlags bindingAttr)
        {
            if (IsComplete)
                return base.GetMethods(bindingAttr);
            else
                return GetIncompleteMethods(bindingAttr);
        }

        /// <summary>
        /// Gets the set of incomplete methods.
        /// </summary>
        /// <returns></returns>
        IMethodSymbol[] GetIncompleteMethods(BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
        {
            if (_methods == null)
                return [];
            else
                return SymbolUtil.SelectMethods(this, _methods, bindingAttr).Cast<IMethodSymbol>().ToArray();
        }

        #endregion

        #region ICustomAttributeProvider

        /// <inheritdoc />
        public override CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            if (IsComplete)
                return ResolveCustomAttributes(UnderlyingRuntimeMember.GetCustomAttributesData(inherit).ToArray());
            else if (inherit == false || BaseType == null)
                return _customAttributes?.ToArray() ?? [];
            else
                return Enumerable.Concat(_customAttributes?.ToArray() ?? [], ResolveCustomAttributes(BaseType.Unpack().GetInheritedCustomAttributesData())).ToArray();
        }

        /// <inheritdoc />
        public override CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(inherit).FirstOrDefault(i => i.AttributeType == attributeType);
        }

        /// <inheritdoc />
        public override CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(inherit).Where(i => i.AttributeType == attributeType).ToArray();
        }

        #endregion

        /// <inheritdoc />
        public void Complete()
        {
            if (_builder != null)
            {
                // complete type
                if (_builder.IsCreated() == false)
                {
                    _type = _builder.CreateType() ?? throw new InvalidOperationException();
                    _methods = null;
                    _customAttributes = null;
                }

                // force module to reresolve
                Context.GetOrCreateModuleSymbol(ResolvingModule.UnderlyingModule);
                OnComplete();
            }
        }

        /// <inheritdoc />
        public void OnComplete()
        {
            const BindingFlags DefaultBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            // apply the runtime generic type parameters and pass them to the symbols for completion
            var srcGenericArgs = UnderlyingRuntimeType.GetGenericArguments() ?? [];
            var dstGenericArgs = GetGenericArguments() ?? [];
            Debug.Assert(srcGenericArgs.Length == dstGenericArgs.Length);
            for (int i = 0; i < srcGenericArgs.Length; i++)
                if (dstGenericArgs[i] is IReflectionGenericTypeParameterSymbolBuilder b)
                    b.OnComplete(srcGenericArgs[i]);

            foreach (var i in GetConstructors(DefaultBindingFlags) ?? [])
                if (i is IReflectionConstructorSymbolBuilder b)
                    b.OnComplete();

            foreach (var i in GetMethods(DefaultBindingFlags) ?? [])
                if (i is IReflectionMethodSymbolBuilder b)
                    b.OnComplete();

            foreach (var i in GetFields(DefaultBindingFlags) ?? [])
                if (i is IReflectionFieldSymbolBuilder b)
                    b.OnComplete();

            foreach (var i in GetProperties(DefaultBindingFlags) ?? [])
                if (i is IReflectionPropertySymbolBuilder b)
                    b.OnComplete();

            foreach (var m in GetEvents(DefaultBindingFlags) ?? [])
                if (m is IReflectionPropertySymbolBuilder b)
                    b.OnComplete();
        }

    }

}
