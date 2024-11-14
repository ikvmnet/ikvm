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

    class ModuleSymbolBuilder : ModuleSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        public ModuleSymbolBuilder(SymbolContext context, AssemblySymbolBuilder assembly) :
            base(context, assembly)
        {

        }

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

        /// <inheritdoc />
        public override string FullyQualifiedName => throw new NotImplementedException();

        /// <inheritdoc />
        public override string Name => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsMissing => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool ContainsMissing => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsComplete => throw new NotImplementedException();

        /// <summary>
        /// Defines a document for source.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="language"></param>
        /// <param name="languageVendor"></param>
        /// <param name="documentType"></param>
        /// <returns></returns>
        ISymbolDocumentWriter? DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a binary large object (BLOB) that represents a manifest resource to be embedded in the dynamic assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        /// <param name="attribute"></param>
        void DefineManifestResource(string name, Stream stream, ResourceAttributes attribute)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines the named managed embedded resource to be stored in this module.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        IResourceWriter DefineResource(string name, string description)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines the named managed embedded resource with the given attributes that is to be stored in this module.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        IResourceWriter DefineResource(string name, string description, ResourceAttributes attribute)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a global method with the specified name, attributes, calling convention, return type, and parameter types.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        MethodSymbolBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, TypeSymbol? returnType, IImmutableList<TypeSymbol> parameterTypes)
        {
            throw new NotImplementedException();
        }

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
        MethodSymbolBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, TypeSymbol? returnType, IImmutableList<TypeSymbol> requiredReturnTypeCustomModifiers, IImmutableList<TypeSymbol> optionalReturnTypeCustomModifiers, IImmutableList<TypeSymbol> parameterTypes, IImmutableList<IImmutableList<TypeSymbol>> requiredParameterTypeCustomModifiers, IImmutableList<IImmutableList<TypeSymbol>> optionalParameterTypeCustomModifiers)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a global method with the specified name, attributes, return type, and parameter types.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        MethodSymbolBuilder DefineGlobalMethod(string name, MethodAttributes attributes, TypeSymbol? returnType, IImmutableList<TypeSymbol> parameterTypes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constructs a TypeBuilder for a private type with the specified name in this module.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineType(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constructs a TypeBuilder given the type name, the attributes, the type that the defined type extends, and the total size of the type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="typesize"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineType(string name, TypeAttributes attr, TypeSymbol? parent, int typesize)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constructs a TypeBuilder given type name, its attributes, and the type that the defined type extends.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineType(string name, TypeAttributes attr, TypeSymbol? parent)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constructs a TypeBuilder given the type name and the type attributes.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineType(string name, TypeAttributes attr)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constructs a TypeBuilder given the type name, the attributes, the type that the defined type extends, and the packing size of the type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="packsize"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineType(string name, TypeAttributes attr, TypeSymbol? parent, PackingSize packsize)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constructs a TypeBuilder given the type name, attributes, the type that the defined type extends, the packing size of the defined type, and the total size of the defined type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="packingSize"></param>
        /// <param name="typesize"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineType(string name, TypeAttributes attr, TypeSymbol? parent, PackingSize packingSize, int typesize)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constructs a TypeBuilder given the type name, attributes, the type that the defined type extends, and the interfaces that the defined type implements.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="interfaces"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineType(string name, TypeAttributes attr, TypeSymbol? parent, ImmutableList<TypeSymbol> interfaces)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a nested type, given its name.
        /// </summary>
        /// <param name="enclosingType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineNestedType(TypeSymbolBuilder enclosingType, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a nested type, given its name and attributes.
        /// </summary>
        /// <param name="enclosingType"></param>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineNestedType(TypeSymbolBuilder enclosingType, string name, TypeAttributes attr)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a nested type, given its name, attributes, and the type that it extends.
        /// </summary>
        /// <param name="enclosingType"></param>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineNestedType(TypeSymbolBuilder enclosingType, string name, TypeAttributes attr, TypeSymbol? parent)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a nested type, given its name, attributes, the type that it extends, and the interfaces that it implements.
        /// </summary>
        /// <param name="enclosingType"></param>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="interfaces"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineNestedType(TypeSymbolBuilder enclosingType, string name, TypeAttributes attr, TypeSymbol? parent, IImmutableList<TypeSymbol> interfaces)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a nested type, given its name, attributes, size, and the type that it extends.
        /// </summary>
        /// <param name="enclosingType"></param>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="typeSize"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineNestedType(TypeSymbolBuilder enclosingType, string name, TypeAttributes attr, TypeSymbol? parent, int typeSize)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a nested type, given its name, attributes, the type that it extends, and the packing size.
        /// </summary>
        /// <param name="enclosingType"></param>
        /// <param name="name"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="packSize"></param>
        /// <returns></returns>
        TypeSymbolBuilder DefineNestedType(TypeSymbolBuilder enclosingType, string name, TypeAttributes attr, TypeSymbol? parent, PackingSize packSize)
        {
            throw new NotImplementedException();
        }

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
        TypeSymbolBuilder DefineNestedType(TypeSymbolBuilder enclosingType, string name, TypeAttributes attr, TypeSymbol? parent, PackingSize packSize, int typeSize)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Explicitely adds a reference to the specified assembly.
        /// </summary>
        /// <param name="assembly"></param>
        void AddReference(AssemblySymbol assembly)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finishes the module and all types. Required before save.
        /// </summary>
        void Complete()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves this module assembly to disk. Module must first be completed.
        /// </summary>
        /// <param name="portableExecutableKind"></param>
        /// <param name="imageFileMachine"></param>
        void Save(PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool IsResource()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        internal override ImmutableArray<FieldSymbol> GetDeclaredFields()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        internal override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        internal override ImmutableArray<TypeSymbol> GetDeclaredTypes()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            throw new NotImplementedException();
        }

    }

}
