using System.Reflection;

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
        IGenericTypeParameterSymbolBuilder[] DefineGenericParameters(params string[] names);

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
        /// Adds a new constructor to the type, with the given attributes and signature.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        IConstructorSymbolBuilder DefineConstructor(System.Reflection.MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol[]? parameterTypes);

        /// <summary>
        /// Adds a new constructor to the type, with the given attributes, signature, and custom modifiers.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="requiredCustomModifiers"></param>
        /// <param name="optionalCustomModifiers"></param>
        /// <returns></returns>
        IConstructorSymbolBuilder DefineConstructor(MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? requiredCustomModifiers, ITypeSymbol[][]? optionalCustomModifiers);

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
        IFieldSymbolBuilder DefineField(string fieldName, ITypeSymbol type, ITypeSymbol[]? requiredCustomModifiers, ITypeSymbol[]? optionalCustomModifiers, FieldAttributes attributes);

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
        IMethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers);

        /// <summary>
        /// Adds a new method to the type, with the specified name, method attributes, calling convention, and method signature.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        IMethodSymbolBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes);

        /// <summary>
        /// Adds a new method to the type, with the specified name, method attributes, and calling convention.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <returns></returns>
        IMethodSymbolBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention);

        /// <summary>
        /// Adds a new method to the type, with the specified name and method attributes.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        IMethodSymbolBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes);

        /// <summary>
        /// Adds a new method to the type, with the specified name, method attributes, and method signature.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        IMethodSymbolBuilder DefineMethod(string name, System.Reflection.MethodAttributes attributes, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes);

        /// <summary>
        /// Specifies a given method body that implements a given method declaration, potentially with a different name.
        /// </summary>
        /// <param name="methodInfoBody"></param>
        /// <param name="methodInfoDeclaration"></param>
        void DefineMethodOverride(IMethodSymbolBuilder methodInfoBody, IMethodSymbol methodInfoDeclaration);

        /// <summary>
        /// Defines a nested type, given its name, attributes, the type that it extends, and the interfaces that it implements.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="interfaces"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, ITypeSymbol[]? interfaces);

        /// <summary>
        /// Defines a nested type, given its name, attributes, size, and the type that it extends.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="packSize"></param>
        /// <param name="typeSize"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, System.Reflection.Emit.PackingSize packSize, int typeSize);

        /// <summary>
        /// Defines a nested type, given its name, attributes, the type that it extends, and the packing size.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="packSize"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, System.Reflection.Emit.PackingSize packSize);

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
        ITypeSymbolBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent);

        /// <summary>
        /// Defines a nested type, given its name and attributes.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr);

        /// <summary>
        /// Defines a nested type, given its name, attributes, the total size of the type, and the type that it extends.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="typeSize"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineNestedType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, int typeSize);

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
        IMethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes, System.Runtime.InteropServices.CallingConvention nativeCallConv, System.Runtime.InteropServices.CharSet nativeCharSet);

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
        IMethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes, System.Runtime.InteropServices.CallingConvention nativeCallConv, System.Runtime.InteropServices.CharSet nativeCharSet);

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
        IMethodSymbolBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers, System.Runtime.InteropServices.CallingConvention nativeCallConv, System.Runtime.InteropServices.CharSet nativeCharSet);

        /// <summary>
        /// Adds a new property to the type, with the given name and property signature.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        IPropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, ITypeSymbol returnType, ITypeSymbol[]? parameterTypes);

        /// <summary>
        /// Adds a new property to the type, with the given name, attributes, calling convention, and property signature.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        IPropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, ITypeSymbol returnType, ITypeSymbol[]? parameterTypes);

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
        IPropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, ITypeSymbol returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers);

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
        IPropertySymbolBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, ITypeSymbol returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers);

        /// <summary>
        /// Set a custom attribute using a custom attribute builder.
        /// </summary>
        /// <param name="customBuilder"></param>
        void SetCustomAttribute(ICustomAttributeBuilder customBuilder);

        /// <summary>
        /// Sets a custom attribute using a specified custom attribute blob.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="binaryAttribute"></param>
        void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute);

        /// <summary>
        /// Finishes the type, updating the associated symbol.
        /// </summary>
        void Complete();

    }

}
