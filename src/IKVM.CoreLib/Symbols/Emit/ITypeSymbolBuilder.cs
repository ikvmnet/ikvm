using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace IKVM.CoreLib.Symbols.Emit
{

    interface ITypeSymbolBuilder : ISymbolBuilder<ITypeSymbol>, IMemberSymbolBuilder, ITypeSymbol
    {

        /// <summary>
        /// Sets the base type of the type currently under construction.
        /// </summary>
        /// <param name="parent"></param>
        void SetParent(ITypeSymbol? parent);

        /// <summary>
        /// Defines the generic type parameters for the current type, specifying their number and their names, and returns an array of GenericTypeParameterBuilder objects that can be used to set their constraints.
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        IImmutableList<IGenericTypeParameterSymbolBuilder> DefineGenericParameters(IImmutableList<string> names);

        /// <summary>
        /// Defines the initializer for this type.
        /// </summary>
        /// <returns></returns>
        IConstructorSymbolBuilder DefineTypeInitializer();

        /// <summary>
        /// Adds an interface that this type implements.
        /// </summary>
        /// <param name="interfaceType"></param>
        void AddInterfaceImplementation(ITypeSymbol interfaceType);

        /// <summary>
        /// Adds a new constructor to the type, with the given attributes and signature and the standard calling convention.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        IConstructorSymbolBuilder DefineConstructor(MethodAttributes attributes, IImmutableList<ITypeSymbol> parameterTypes);

        /// <summary>
        /// Adds a new constructor to the type, with the given attributes and signature.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        IConstructorSymbolBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, IImmutableList<ITypeSymbol> parameterTypes);

        /// <summary>
        /// Adds a new constructor to the type, with the given attributes, signature, and custom modifiers.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="requiredCustomModifiers"></param>
        /// <param name="optionalCustomModifiers"></param>
        /// <returns></returns>
        IConstructorSymbolBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, IImmutableList<ITypeSymbol> parameterTypes, IImmutableList<IImmutableList<ITypeSymbol>> requiredCustomModifiers, IImmutableList<IImmutableList<ITypeSymbol>> optionalCustomModifiers);

        /// <summary>
        /// Defines the parameterless constructor. The constructor defined here will simply call the parameterless constructor of the parent.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        IConstructorSymbolBuilder DefineDefaultConstructor(MethodAttributes attributes);

        /// <summary>
        /// Adds a new event to the type, with the given name, attributes and event type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="eventtype"></param>
        /// <returns></returns>
        IEventSymbolBuilder DefineEvent(string name, EventAttributes attributes, ITypeSymbol eventtype);

        /// <summary>
        /// Adds a new field to the type, with the given name, attributes, and field type.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="type"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        IFieldSymbolBuilder DefineField(string fieldName, ITypeSymbol type, FieldAttributes attributes);

        /// <summary>
        /// Adds a new field to the type, with the given name, attributes, field type, and custom modifiers.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="type"></param>
        /// <param name="requiredCustomModifiers"></param>
        /// <param name="optionalCustomModifiers"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        IFieldSymbolBuilder DefineField(string fieldName, ITypeSymbol type, IImmutableList<ITypeSymbol> requiredCustomModifiers, IImmutableList<ITypeSymbol> optionalCustomModifiers, FieldAttributes attributes);

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
        IMethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> returnTypeRequiredCustomModifiers, IImmutableList<ITypeSymbol> returnTypeOptionalCustomModifiers, IImmutableList<ITypeSymbol> parameterTypes, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeRequiredCustomModifiers, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeOptionalCustomModifiers);

        /// <summary>
        /// Adds a new method to the type, with the specified name, method attributes, calling convention, and method signature.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        IMethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> parameterTypes);

        /// <summary>
        /// Adds a new method to the type, with the specified name, method attributes, and calling convention.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <returns></returns>
        IMethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention);

        /// <summary>
        /// Adds a new method to the type, with the specified name and method attributes.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        IMethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes);

        /// <summary>
        /// Adds a new method to the type, with the specified name, method attributes, and method signature.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        IMethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> parameterTypes);

        /// <summary>
        /// Specifies a given method body that implements a given method declaration, potentially with a different name.
        /// </summary>
        /// <param name="methodInfoBody"></param>
        /// <param name="methodInfoDeclaration"></param>
        void DefineMethodOverride(IMethodSymbol methodInfoBody, IMethodSymbol methodInfoDeclaration);

        /// <summary>
        /// Defines a nested type, given its name, attributes, the type that it extends, and the interfaces that it implements.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="interfaces"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, ITypeSymbol? parent, IImmutableList<ITypeSymbol> interfaces);

        /// <summary>
        /// Defines a nested type, given its name, attributes, size, and the type that it extends.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="packSize"></param>
        /// <param name="typeSize"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, ITypeSymbol? parent, PackingSize packSize, int typeSize);

        /// <summary>
        /// Defines a nested type, given its name, attributes, the type that it extends, and the packing size.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="packSize"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, ITypeSymbol? parent, PackingSize packSize);

        /// <summary>
        /// Defines a nested type, given its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineNestedType(string name);

        /// <summary>
        /// Defines a nested type, given its name, attributes, and the type that it extends.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, ITypeSymbol? parent);

        /// <summary>
        /// Defines a nested type, given its name and attributes.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr);

        /// <summary>
        /// Defines a nested type, given its name, attributes, the total size of the type, and the type that it extends.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="typeSize"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineNestedType(string name, TypeAttributes attr, ITypeSymbol? parent, int typeSize);

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
        IMethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> parameterTypes, System.Runtime.InteropServices.CallingConvention nativeCallConv, System.Runtime.InteropServices.CharSet nativeCharSet);

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
        IMethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> parameterTypes, System.Runtime.InteropServices.CallingConvention nativeCallConv, System.Runtime.InteropServices.CharSet nativeCharSet);

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
        IMethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> returnTypeRequiredCustomModifiers, IImmutableList<ITypeSymbol> returnTypeOptionalCustomModifiers, IImmutableList<ITypeSymbol> parameterTypes, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeRequiredCustomModifiers, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet);

        /// <summary>
        /// Adds a new property to the type, with the given name and property signature.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        IPropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, ITypeSymbol returnType, IImmutableList<ITypeSymbol> parameterTypes);

        /// <summary>
        /// Adds a new property to the type, with the given name, attributes, calling convention, and property signature.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        IPropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, ITypeSymbol returnType, IImmutableList<ITypeSymbol> parameterTypes);

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
        IPropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, ITypeSymbol returnType, IImmutableList<ITypeSymbol> returnTypeRequiredCustomModifiers, IImmutableList<ITypeSymbol> returnTypeOptionalCustomModifiers, IImmutableList<ITypeSymbol> parameterTypes, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeRequiredCustomModifiers, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeOptionalCustomModifiers);

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
        IPropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, ITypeSymbol returnType, IImmutableList<ITypeSymbol> returnTypeRequiredCustomModifiers, IImmutableList<ITypeSymbol> returnTypeOptionalCustomModifiers, IImmutableList<ITypeSymbol> parameterTypes, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeRequiredCustomModifiers, IImmutableList<IImmutableList<ITypeSymbol>> parameterTypeOptionalCustomModifiers);

        /// <summary>
        /// Finishes the type, updating the associated symbol.
        /// </summary>
        void Complete();

    }

}
