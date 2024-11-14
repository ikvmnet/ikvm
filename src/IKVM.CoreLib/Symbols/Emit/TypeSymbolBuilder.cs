using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace IKVM.CoreLib.Symbols.Emit
{

    class TypeSymbolBuilder : TypeSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringModule"></param>
        public TypeSymbolBuilder(SymbolContext context, ModuleSymbol declaringModule) :
            base(context, declaringModule)
        {

        }

        /// <inheritdoc />
        public override TypeAttributes Attributes => throw new NotImplementedException();

        /// <inheritdoc />
        public override MethodBaseSymbol? DeclaringMethod => throw new NotImplementedException();

        /// <inheritdoc />
        public override string? FullName => throw new NotImplementedException();

        /// <inheritdoc />
        public override string? Namespace => throw new NotImplementedException();

        /// <inheritdoc />
        public override TypeCode TypeCode => throw new NotImplementedException();

        /// <inheritdoc />
        public override TypeSymbol? BaseType => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool ContainsGenericParameters => throw new NotImplementedException();

        /// <inheritdoc />
        public override GenericParameterAttributes GenericParameterAttributes => throw new NotImplementedException();

        /// <inheritdoc />
        public override int GenericParameterPosition => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsTypeDefinition => true;

        /// <inheritdoc />
        public override bool IsGenericTypeDefinition => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsConstructedGenericType => false;

        /// <inheritdoc />
        public override bool IsGenericTypeParameter => false;

        /// <inheritdoc />
        public override bool IsGenericMethodParameter => false;

        /// <inheritdoc />
        public override bool HasElementType => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsPrimitive => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsSZArray => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsArray => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsEnum => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsPointer => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsFunctionPointer => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsUnmanagedFunctionPointer => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsByRef => throw new NotImplementedException();

        /// <inheritdoc />
        public override string Name => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsMissing => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool ContainsMissing => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsComplete => throw new NotImplementedException();

        /// <summary>
        /// Sets the base type of the type currently under construction.
        /// </summary>
        /// <param name="parent"></param>
        void SetParent(TypeSymbol? parent)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines the generic type parameters for the current type, specifying their number and their names, and returns an array of GenericTypeParameterBuilder objects that can be used to set their constraints.
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        ImmutableList<GenericTypeParameterTypeSymbolBuilder> DefineGenericParameters(ImmutableList<string> names)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines the initializer for this type.
        /// </summary>
        /// <returns></returns>
        ConstructorSymbolBuilder DefineTypeInitializer()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds an interface that this type implements.
        /// </summary>
        /// <param name="interfaceType"></param>
        void AddInterfaceImplementation(TypeSymbol interfaceType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new constructor to the type, with the given attributes and signature and the standard calling convention.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        ConstructorSymbolBuilder DefineConstructor(MethodAttributes attributes, ImmutableList<TypeSymbol> parameterTypes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new constructor to the type, with the given attributes and signature.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        ConstructorSymbolBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, ImmutableList<TypeSymbol> parameterTypes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new constructor to the type, with the given attributes, signature, and custom modifiers.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="requiredCustomModifiers"></param>
        /// <param name="optionalCustomModifiers"></param>
        /// <returns></returns>
        ConstructorSymbolBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, ImmutableList<TypeSymbol> parameterTypes, ImmutableList<ImmutableList<TypeSymbol>> requiredCustomModifiers, ImmutableList<ImmutableList<TypeSymbol>> optionalCustomModifiers)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines the parameterless constructor. The constructor defined here will simply call the parameterless constructor of the parent.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        ConstructorSymbolBuilder DefineDefaultConstructor(MethodAttributes attributes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new event to the type, with the given name, attributes and event type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="eventtype"></param>
        /// <returns></returns>
        EventSymbolBuilder DefineEvent(string name, EventAttributes attributes, TypeSymbol eventtype)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new field to the type, with the given name, attributes, and field type.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="type"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        FieldSymbolBuilder DefineField(string fieldName, TypeSymbol type, FieldAttributes attributes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new field to the type, with the given name, attributes, field type, and custom modifiers.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="type"></param>
        /// <param name="requiredCustomModifiers"></param>
        /// <param name="optionalCustomModifiers"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        FieldSymbolBuilder DefineField(string fieldName, TypeSymbol type, ImmutableList<TypeSymbol> requiredCustomModifiers, ImmutableList<TypeSymbol> optionalCustomModifiers, FieldAttributes attributes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new method to the type, with the specified name, method attributes, calling convention, method signature, and custom modifiers.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="returnTypeRequiredCustomModifiers"></param>
        /// <param name="returnTypeOptionalCustomModifiers"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="parameterTypeRequiredCustomModifiers"></param>
        /// <param name="parameterTypeOptionalCustomModifiers"></param>
        /// <returns></returns>
        MethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, TypeSymbol? returnType, ImmutableList<TypeSymbol> returnTypeRequiredCustomModifiers, ImmutableList<TypeSymbol> returnTypeOptionalCustomModifiers, ImmutableList<TypeSymbol> parameterTypes, ImmutableList<ImmutableList<TypeSymbol>> parameterTypeRequiredCustomModifiers, ImmutableList<ImmutableList<TypeSymbol>> parameterTypeOptionalCustomModifiers)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new method to the type, with the specified name, method attributes, calling convention, and method signature.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        MethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, TypeSymbol? returnType, ImmutableList<TypeSymbol> parameterTypes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new method to the type, with the specified name, method attributes, and calling convention.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <returns></returns>
        MethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new method to the type, with the specified name and method attributes.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        MethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new method to the type, with the specified name, method attributes, and method signature.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        MethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, TypeSymbol? returnType, ImmutableList<TypeSymbol> parameterTypes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Specifies a given method body that implements a given method declaration, potentially with a different name.
        /// </summary>
        /// <param name="methodInfoBody"></param>
        /// <param name="methodInfoDeclaration"></param>
        void DefineMethodOverride(MethodSymbol methodInfoBody, MethodSymbol methodInfoDeclaration)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a nested type, given its name, attributes, the type that it extends, and the interfaces that it implements.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="interfaces"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, TypeSymbol? parent, ImmutableList<TypeSymbol> interfaces)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a nested type, given its name, attributes, size, and the type that it extends.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="packSize"></param>
        /// <param name="typeSize"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, TypeSymbol? parent, PackingSize packSize, int typeSize)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a nested type, given its name, attributes, the type that it extends, and the packing size.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="packSize"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, TypeSymbol? parent, PackingSize packSize)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a nested type, given its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineNestedType(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a nested type, given its name, attributes, and the type that it extends.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, TypeSymbol? parent)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a nested type, given its name and attributes.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a nested type, given its name, attributes, the total size of the type, and the type that it extends.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="typeSize"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, TypeSymbol? parent, int typeSize)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a PInvoke method given its name, the name of the DLL in which the method is defined, the attributes of the method, the calling convention of the method, the return type of the method, the types of the parameters of the method, and the PInvoke flags.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dllName"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="nativeCallConv"></param>
        /// <param name="nativeCharSet"></param>
        /// <returns></returns>
        MethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, TypeSymbol? returnType, ImmutableList<TypeSymbol> parameterTypes, System.Runtime.InteropServices.CallingConvention nativeCallConv, System.Runtime.InteropServices.CharSet nativeCharSet)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a PInvoke method given its name, the name of the DLL in which the method is defined, the name of the entry point, the attributes of the method, the calling convention of the method, the return type of the method, the types of the parameters of the method, and the PInvoke flags.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dllName"></param>
        /// <param name="entryName"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="nativeCallConv"></param>
        /// <param name="nativeCharSet"></param>
        /// <returns></returns>
        MethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, TypeSymbol? returnType, ImmutableList<TypeSymbol> parameterTypes, System.Runtime.InteropServices.CallingConvention nativeCallConv, System.Runtime.InteropServices.CharSet nativeCharSet)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a PInvoke method given its name, the name of the DLL in which the method is defined, the name of the entry point, the attributes of the method, the calling convention of the method, the return type of the method, the types of the parameters of the method, the PInvoke flags, and custom modifiers for the parameters and return type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dllName"></param>
        /// <param name="entryName"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="returnTypeRequiredCustomModifiers"></param>
        /// <param name="returnTypeOptionalCustomModifiers"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="parameterTypeRequiredCustomModifiers"></param>
        /// <param name="parameterTypeOptionalCustomModifiers"></param>
        /// <param name="nativeCallConv"></param>
        /// <param name="nativeCharSet"></param>
        /// <returns></returns>
        MethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, TypeSymbol? returnType, ImmutableList<TypeSymbol> returnTypeRequiredCustomModifiers, ImmutableList<TypeSymbol> returnTypeOptionalCustomModifiers, ImmutableList<TypeSymbol> parameterTypes, ImmutableList<ImmutableList<TypeSymbol>> parameterTypeRequiredCustomModifiers, ImmutableList<ImmutableList<TypeSymbol>> parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new property to the type, with the given name and property signature.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        PropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, TypeSymbol returnType, ImmutableList<TypeSymbol> parameterTypes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new property to the type, with the given name, attributes, calling convention, and property signature.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        PropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, TypeSymbol returnType, ImmutableList<TypeSymbol> parameterTypes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new property to the type, with the given name, property signature, and custom modifiers.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="returnType"></param>
        /// <param name="returnTypeRequiredCustomModifiers"></param>
        /// <param name="returnTypeOptionalCustomModifiers"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="parameterTypeRequiredCustomModifiers"></param>
        /// <param name="parameterTypeOptionalCustomModifiers"></param>
        /// <returns></returns>
        PropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, TypeSymbol returnType, ImmutableList<TypeSymbol> returnTypeRequiredCustomModifiers, ImmutableList<TypeSymbol> returnTypeOptionalCustomModifiers, ImmutableList<TypeSymbol> parameterTypes, ImmutableList<ImmutableList<TypeSymbol>> parameterTypeRequiredCustomModifiers, ImmutableList<ImmutableList<TypeSymbol>> parameterTypeOptionalCustomModifiers)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new property to the type, with the given name, calling convention, property signature, and custom modifiers.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="returnTypeRequiredCustomModifiers"></param>
        /// <param name="returnTypeOptionalCustomModifiers"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="parameterTypeRequiredCustomModifiers"></param>
        /// <param name="parameterTypeOptionalCustomModifiers"></param>
        /// <returns></returns>
        PropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, TypeSymbol returnType, ImmutableList<TypeSymbol> returnTypeRequiredCustomModifiers, ImmutableList<TypeSymbol> returnTypeOptionalCustomModifiers, ImmutableList<TypeSymbol> parameterTypes, ImmutableList<ImmutableList<TypeSymbol>> parameterTypeRequiredCustomModifiers, ImmutableList<ImmutableList<TypeSymbol>> parameterTypeOptionalCustomModifiers)
        {
            throw new NotImplementedException();
        }

        public override int GetArrayRank()
        {
            throw new NotImplementedException();
        }

        internal override ImmutableArray<ConstructorSymbol> GetDeclaredConstructors()
        {
            throw new NotImplementedException();
        }

        public override TypeSymbol? GetElementType()
        {
            throw new NotImplementedException();
        }

        public override bool IsEnumDefined(object value)
        {
            throw new NotImplementedException();
        }

        public override string? GetEnumName(object value)
        {
            throw new NotImplementedException();
        }

        public override ImmutableArray<string> GetEnumNames()
        {
            throw new NotImplementedException();
        }

        public override TypeSymbol GetEnumUnderlyingType()
        {
            throw new NotImplementedException();
        }

        internal override ImmutableArray<EventSymbol> GetDeclaredEvents()
        {
            throw new NotImplementedException();
        }

        internal override ImmutableArray<FieldSymbol> GetDeclaredFields()
        {
            throw new NotImplementedException();
        }

        public override ImmutableArray<TypeSymbol> GetGenericArguments()
        {
            throw new NotImplementedException();
        }

        public override ImmutableArray<TypeSymbol> GetGenericParameterConstraints()
        {
            throw new NotImplementedException();
        }

        public override TypeSymbol GetGenericTypeDefinition()
        {
            throw new NotImplementedException();
        }

        public override ImmutableArray<TypeSymbol> GetInterfaces()
        {
            throw new NotImplementedException();
        }

        public override InterfaceMapping GetInterfaceMap(TypeSymbol interfaceType)
        {
            throw new NotImplementedException();
        }

        internal override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            throw new NotImplementedException();
        }

        internal override ImmutableArray<TypeSymbol> GetDeclaredNestedTypes()
        {
            throw new NotImplementedException();
        }

        internal override ImmutableArray<PropertySymbol> GetDeclaredProperties()
        {
            throw new NotImplementedException();
        }

        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            throw new NotImplementedException();
        }

    }

}
