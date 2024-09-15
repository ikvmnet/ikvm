using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionTypeSymbolBuilder : ReflectionSymbolBuilder<ITypeSymbol, ReflectionTypeSymbol>, ITypeSymbolBuilder
    {

        readonly ReflectionModuleSymbolBuilder _containingModuleBuilder;
        readonly TypeBuilder _builder;
        ReflectionTypeSymbol? _symbol;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingModuleBuilder"></param>
        /// <param name="builder"></param>
        public ReflectionTypeSymbolBuilder(ReflectionSymbolContext context, ReflectionModuleSymbolBuilder containingModuleBuilder, TypeBuilder builder) :
            base(context)
        {
            _containingModuleBuilder = containingModuleBuilder ?? throw new ArgumentNullException(nameof(containingModuleBuilder));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the containing <see cref="ReflectionModuleSymbolBuilder"/>.
        /// </summary>
        internal ReflectionModuleSymbolBuilder ContainingModuleBuilder => _containingModuleBuilder;

        /// <summary>
        /// Gets the underlying <see cref="TypeBuilder"/>.
        /// </summary>
        internal TypeBuilder ReflectionBuilder => _builder;

        /// <inheritdoc />
        internal override ReflectionTypeSymbol ReflectionSymbol => _symbol ??= Context.GetOrCreateTypeSymbol(_builder);

        /// <inheritdoc />
        public void SetParent(ITypeSymbol? parent)
        {
            _builder.SetParent(parent?.Unpack());
        }

        /// <inheritdoc />
        public IGenericTypeParameterSymbolBuilder[] DefineGenericParameters(params string[] names)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void AddInterfaceImplementation(ITypeSymbol interfaceType)
        {
            _builder.AddInterfaceImplementation(interfaceType.Unpack());
        }

        /// <inheritdoc />
        public IConstructorSymbolBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol[]? parameterTypes)
        {
            return new ReflectionConstructorSymbolBuilder(Context, this, _builder.DefineConstructor(attributes, callingConvention, parameterTypes?.Unpack()));
        }

        /// <inheritdoc />
        public IConstructorSymbolBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? requiredCustomModifiers, ITypeSymbol[][]? optionalCustomModifiers)
        {
            return new ReflectionConstructorSymbolBuilder(Context, this, _builder.DefineConstructor(attributes, callingConvention, parameterTypes?.Unpack(), requiredCustomModifiers?.Unpack(), optionalCustomModifiers?.Unpack()));
        }

        /// <inheritdoc />
        public IConstructorSymbolBuilder DefineDefaultConstructor(MethodAttributes attributes)
        {
            return new ReflectionConstructorSymbolBuilder(Context, this, _builder.DefineDefaultConstructor(attributes));
        }

        /// <inheritdoc />
        public IEventSymbolBuilder DefineEvent(string name, EventAttributes attributes, ITypeSymbol eventtype)
        {
            return new ReflectionEventSymbolBuilder(Context, this, _builder.DefineEvent(name, attributes, eventtype.Unpack()));
        }

        /// <inheritdoc />
        public IFieldSymbolBuilder DefineField(string fieldName, ITypeSymbol type, FieldAttributes attributes)
        {
            return new ReflectionFieldSymbolBuilder(Context, this, _builder.DefineField(fieldName, type.Unpack(), attributes));
        }

        /// <inheritdoc />
        public IFieldSymbolBuilder DefineField(string fieldName, ITypeSymbol type, ITypeSymbol[]? requiredCustomModifiers, ITypeSymbol[]? optionalCustomModifiers, FieldAttributes attributes)
        {
            return new ReflectionFieldSymbolBuilder(Context, this, _builder.DefineField(fieldName, type.Unpack(), requiredCustomModifiers?.Unpack(), optionalCustomModifiers?.Unpack(), attributes));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers)
        {
            return new ReflectionMethodSymbolBuilder(Context, this, _builder.DefineMethod(name, attributes, callingConvention, returnType?.Unpack(), returnTypeRequiredCustomModifiers?.Unpack(), returnTypeOptionalCustomModifiers?.Unpack(), parameterTypes?.Unpack(), parameterTypeRequiredCustomModifiers?.Unpack(), parameterTypeOptionalCustomModifiers?.Unpack()));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes)
        {
            return new ReflectionMethodSymbolBuilder(Context, this, _builder.DefineMethod(name, attributes, callingConvention, returnType?.Unpack(), parameterTypes?.Unpack()));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention)
        {
            return new ReflectionMethodSymbolBuilder(Context, this, _builder.DefineMethod(name, attributes, callingConvention));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes)
        {
            return new ReflectionMethodSymbolBuilder(Context, this, _builder.DefineMethod(name, attributes));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes)
        {
            return new ReflectionMethodSymbolBuilder(Context, this, _builder.DefineMethod(name, attributes, returnType?.Unpack(), parameterTypes?.Unpack()));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, ITypeSymbol? parent, ITypeSymbol[]? interfaces)
        {
            return new ReflectionTypeSymbolBuilder(Context, ContainingModuleBuilder, _builder.DefineNestedType(name, attr, parent?.Unpack(), interfaces?.Unpack()));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, ITypeSymbol? parent, PackingSize packSize, int typeSize)
        {
            return new ReflectionTypeSymbolBuilder(Context, ContainingModuleBuilder, _builder.DefineNestedType(name, attr, parent?.Unpack(), packSize, typeSize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, ITypeSymbol? parent, PackingSize packSize)
        {
            return new ReflectionTypeSymbolBuilder(Context, ContainingModuleBuilder, _builder.DefineNestedType(name, attr, parent?.Unpack(), packSize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name)
        {
            return new ReflectionTypeSymbolBuilder(Context, ContainingModuleBuilder, _builder.DefineNestedType(name));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, ITypeSymbol? parent)
        {
            return new ReflectionTypeSymbolBuilder(Context, ContainingModuleBuilder, _builder.DefineNestedType(name, attr, parent?.Unpack()));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr)
        {
            return new ReflectionTypeSymbolBuilder(Context, ContainingModuleBuilder, _builder.DefineNestedType(name, attr));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, ITypeSymbol? parent, int typeSize)
        {
            return new ReflectionTypeSymbolBuilder(Context, ContainingModuleBuilder, _builder.DefineNestedType(name, attr, parent?.Unpack(), typeSize));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            return new ReflectionMethodSymbolBuilder(Context, ContainingModuleBuilder, _builder.DefinePInvokeMethod(name, dllName, attributes, callingConvention, returnType?.Unpack(), parameterTypes?.Unpack(), nativeCallConv, nativeCharSet));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            return new ReflectionMethodSymbolBuilder(Context, ContainingModuleBuilder, _builder.DefinePInvokeMethod(name, dllName, entryName, attributes, callingConvention, returnType?.Unpack(), parameterTypes?.Unpack(), nativeCallConv, nativeCharSet));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            return new ReflectionMethodSymbolBuilder(Context, ContainingModuleBuilder, _builder.DefinePInvokeMethod(name, dllName, entryName, attributes, callingConvention, returnType?.Unpack(), returnTypeRequiredCustomModifiers?.Unpack(), returnTypeOptionalCustomModifiers?.Unpack(), parameterTypes?.Unpack(), parameterTypeRequiredCustomModifiers?.Unpack(), parameterTypeOptionalCustomModifiers?.Unpack(), nativeCallConv, nativeCharSet));
        }

        /// <inheritdoc />
        public IPropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, ITypeSymbol returnType, ITypeSymbol[]? parameterTypes)
        {
            return new ReflectionPropertySymbolBuilder(Context, this, _builder.DefineProperty(name, attributes, returnType.Unpack(), parameterTypes?.Unpack()));
        }

        /// <inheritdoc />
        public IPropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, ITypeSymbol returnType, ITypeSymbol[]? parameterTypes)
        {
            return new ReflectionPropertySymbolBuilder(Context, this, _builder.DefineProperty(name, attributes, callingConvention, returnType.Unpack(), parameterTypes?.Unpack()));
        }

        /// <inheritdoc />
        public IPropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, ITypeSymbol returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers)
        {
            return new ReflectionPropertySymbolBuilder(Context, this, _builder.DefineProperty(name, attributes, returnType.Unpack(), returnTypeRequiredCustomModifiers?.Unpack(), returnTypeOptionalCustomModifiers?.Unpack(), parameterTypes?.Unpack(), parameterTypeRequiredCustomModifiers?.Unpack(), parameterTypeOptionalCustomModifiers?.Unpack()));
        }

        /// <inheritdoc />
        public IPropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, ITypeSymbol returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers)
        {
            return new ReflectionPropertySymbolBuilder(Context, this, _builder.DefineProperty(name, attributes, callingConvention, returnType.Unpack(), returnTypeRequiredCustomModifiers?.Unpack(), returnTypeOptionalCustomModifiers?.Unpack(), parameterTypes?.Unpack(), parameterTypeRequiredCustomModifiers?.Unpack(), parameterTypeOptionalCustomModifiers?.Unpack()));
        }

        /// <inheritdoc />
        public IConstructorSymbolBuilder DefineTypeInitializer()
        {
            return new ReflectionConstructorSymbolBuilder(Context, this, _builder.DefineTypeInitializer());
        }
        /// <inheritdoc />
        public void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            _builder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            _builder.SetCustomAttribute(((ReflectionCustomAttributeBuilder)customBuilder).ReflectionBuilder);
        }

        /// <inheritdoc />
        public void Finish()
        {
            var sym = ReflectionSymbol;
            sym.FinishType(_builder.CreateType());
        }

    }

}
