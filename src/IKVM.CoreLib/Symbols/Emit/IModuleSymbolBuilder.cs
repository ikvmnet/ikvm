using System;
using System.Collections.Immutable;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Resources;

namespace IKVM.CoreLib.Symbols.Emit
{

    interface IModuleSymbolBuilder : ISymbolBuilder<IModuleSymbol>, IModuleSymbol, ICustomAttributeProviderBuilder
    {

        /// <summary>
        /// Gets or sets the preferred address of the first byte of the image when it is loaded into memory.
        /// </summary>
        ulong ImageBase { get; set; }

        /// <summary>
        /// Gets or sets the alignment factor (in bytes) that is used to align the raw data of sections in the image file.
        /// </summary>
        uint FileAlignment { get; set; }

        /// <summary>
        /// Gets or sets the characteristics of a dynamic link library.
        /// </summary>
        DllCharacteristics DllCharacteristics { get; set; }

        /// <summary>
        /// Defines a document for source.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="language"></param>
        /// <param name="languageVendor"></param>
        /// <param name="documentType"></param>
        /// <returns></returns>
        ISymbolDocumentWriter? DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType);

        /// <summary>
        /// Defines a binary large object (BLOB) that represents a manifest resource to be embedded in the dynamic assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        /// <param name="attribute"></param>
        void DefineManifestResource(string name, Stream stream, ResourceAttributes attribute);

        /// <summary>
        /// Defines the named managed embedded resource to be stored in this module.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        IResourceWriter DefineResource(string name, string description);

        /// <summary>
        /// Defines the named managed embedded resource with the given attributes that is to be stored in this module.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        IResourceWriter DefineResource(string name, string description, ResourceAttributes attribute);

        /// <summary>
        /// Defines a global method with the specified name, attributes, calling convention, return type, and parameter types.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        IMethodSymbolBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> parameterTypes);

        /// <summary>
        /// Defines a global method with the specified name, attributes, calling convention, return type, custom modifiers for the return type, parameter types, and custom modifiers for the parameter types.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="requiredReturnTypeCustomModifiers"></param>
        /// <param name="optionalReturnTypeCustomModifiers"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="requiredParameterTypeCustomModifiers"></param>
        /// <param name="optionalParameterTypeCustomModifiers"></param>
        /// <returns></returns>
        IMethodSymbolBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> requiredReturnTypeCustomModifiers, IImmutableList<ITypeSymbol> optionalReturnTypeCustomModifiers, IImmutableList<ITypeSymbol> parameterTypes, IImmutableList<IImmutableList<ITypeSymbol>> requiredParameterTypeCustomModifiers, IImmutableList<IImmutableList<ITypeSymbol>> optionalParameterTypeCustomModifiers);

        /// <summary>
        /// Defines a global method with the specified name, attributes, return type, and parameter types.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        IMethodSymbolBuilder DefineGlobalMethod(string name, MethodAttributes attributes, ITypeSymbol? returnType, IImmutableList<ITypeSymbol> parameterTypes);

        /// <summary>
        /// Constructs a TypeBuilder for a private type with the specified name in this module.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineType(string name);

        /// <summary>
        /// Constructs a TypeBuilder given the type name, the attributes, the type that the defined type extends, and the total size of the type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="typesize"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineType(string name, TypeAttributes attr, ITypeSymbol? parent, int typesize);

        /// <summary>
        /// Constructs a TypeBuilder given type name, its attributes, and the type that the defined type extends.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineType(string name, TypeAttributes attr, ITypeSymbol? parent);

        /// <summary>
        /// Constructs a TypeBuilder given the type name and the type attributes.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineType(string name, TypeAttributes attr);

        /// <summary>
        /// Constructs a TypeBuilder given the type name, the attributes, the type that the defined type extends, and the packing size of the type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="packsize"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineType(string name, TypeAttributes attr, ITypeSymbol? parent, PackingSize packsize);

        /// <summary>
        /// Constructs a TypeBuilder given the type name, attributes, the type that the defined type extends, the packing size of the defined type, and the total size of the defined type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="packingSize"></param>
        /// <param name="typesize"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineType(string name, TypeAttributes attr, ITypeSymbol? parent, PackingSize packingSize, int typesize);

        /// <summary>
        /// Constructs a TypeBuilder given the type name, attributes, the type that the defined type extends, and the interfaces that the defined type implements.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="interfaces"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineType(string name, TypeAttributes attr, ITypeSymbol? parent, IImmutableList<ITypeSymbol> interfaces);

        /// <summary>
        /// Defines a nested type, given its name.
        /// </summary>
        /// <param name="enclosingType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineNestedType(ITypeSymbolBuilder enclosingType, string name);

        /// <summary>
        /// Defines a nested type, given its name and attributes.
        /// </summary>
        /// <param name="enclosingType"></param>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineNestedType(ITypeSymbolBuilder enclosingType, string name, TypeAttributes attr);

        /// <summary>
        /// Defines a nested type, given its name, attributes, and the type that it extends.
        /// </summary>
        /// <param name="enclosingType"></param>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineNestedType(ITypeSymbolBuilder enclosingType, string name, TypeAttributes attr, ITypeSymbol? parent);

        /// <summary>
        /// Defines a nested type, given its name, attributes, the type that it extends, and the interfaces that it implements.
        /// </summary>
        /// <param name="enclosingType"></param>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="interfaces"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineNestedType(ITypeSymbolBuilder enclosingType, string name, TypeAttributes attr, ITypeSymbol? parent, IImmutableList<ITypeSymbol> interfaces);

        /// <summary>
        /// Defines a nested type, given its name, attributes, size, and the type that it extends.
        /// </summary>
        /// <param name="enclosingType"></param>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="typeSize"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineNestedType(ITypeSymbolBuilder enclosingType, string name, TypeAttributes attr, ITypeSymbol? parent, int typeSize);

        /// <summary>
        /// Defines a nested type, given its name, attributes, the type that it extends, and the packing size.
        /// </summary>
        /// <param name="enclosingType"></param>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="packSize"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineNestedType(ITypeSymbolBuilder enclosingType, string name, TypeAttributes attr, ITypeSymbol? parent, PackingSize packSize);

        /// <summary>
        /// Defines a nested type, given its name, attributes, size, and the type that it extends.
        /// </summary>
        /// <param name="enclosingType"></param>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="packSize"></param>
        /// <param name="typeSize"></param>
        /// <returns></returns>
        ITypeSymbolBuilder DefineNestedType(ITypeSymbolBuilder enclosingType, string name, TypeAttributes attr, ITypeSymbol? parent, PackingSize packSize, int typeSize);

        /// <summary>
        /// Explicitely adds a reference to the specified assembly.
        /// </summary>
        /// <param name="assembly"></param>
        void AddReference(IAssemblySymbol assembly);

        /// <summary>
        /// Finishes the module and all types. Required before save.
        /// </summary>
        void Complete();

        /// <summary>
        /// Saves this module assembly to disk. Module must first be completed.
        /// </summary>
        /// <param name="portableExecutableKind"></param>
        /// <param name="imageFileMachine"></param>
        void Save(PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine);

    }

}
