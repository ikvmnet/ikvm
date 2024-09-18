using System.Reflection;
using System.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Emit
{

    interface IModuleSymbolBuilder : ISymbolBuilder<IModuleSymbol>, IModuleSymbol
    {

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
        ITypeSymbolBuilder DefineType(string name, TypeAttributes attr, ITypeSymbol? parent, ITypeSymbol[]? interfaces);

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

    }

}
