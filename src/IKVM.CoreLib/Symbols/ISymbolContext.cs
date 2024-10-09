using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols
{

    interface ISymbolContext
    {

        /// <summary>
        /// Defines a dynamic assembly that has the specified name and access rights.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="collectable"></param>
        /// <param name="saveable"></param>
        /// <returns></returns>
        IAssemblySymbolBuilder DefineAssembly(AssemblyIdentity name, bool collectable = true, bool saveable = false);

        /// <summary>
        /// Defines a dynamic assembly that has the specified name and access rights.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="assemblyAttributes"></param>
        /// <param name="collectable"></param>
        /// <param name="saveable"></param>
        /// <returns></returns>
        IAssemblySymbolBuilder DefineAssembly(AssemblyIdentity name, ICustomAttributeBuilder[]? assemblyAttributes, bool collectable = true, bool saveable = false);

        /// <summary>
        /// Initializes an instance of the <see cref="ICustomAttributeBuilder"/> interface given the constructor for the custom attribute and the arguments to the constructor.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="constructorArgs"></param>
        /// <returns></returns>
        ICustomAttributeBuilder CreateCustomAttribute(IConstructorSymbol con, object?[] constructorArgs);

        /// <summary>
        /// Initializes an instance of the <see cref="ICustomAttributeBuilder"/> interface given the constructor for the custom attribute, the arguments to the constructor, and a set of named field/value pairs.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="constructorArgs"></param>
        /// <param name="namedFields"></param>
        /// <param name="fieldValues"></param>
        ICustomAttributeBuilder CreateCustomAttribute(IConstructorSymbol con, object?[] constructorArgs, IFieldSymbol[] namedFields, object?[] fieldValues);

        /// <summary>
        /// Initializes an instance of the <see cref="ICustomAttributeBuilder"/> interface given the constructor for the custom attribute, the arguments to the constructor, and a set of named property or value pairs.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="constructorArgs"></param>
        /// <param name="namedProperties"></param>
        /// <param name="propertyValues"></param>
        ICustomAttributeBuilder CreateCustomAttribute(IConstructorSymbol con, object?[] constructorArgs, IPropertySymbol[] namedProperties, object?[] propertyValues);

        /// <summary>
        /// Initializes an instance of the <see cref="ICustomAttributeBuilder"/> interface given the constructor for the custom attribute, the arguments to the constructor, a set of named property or value pairs, and a set of named field or value pairs.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="constructorArgs"></param>
        /// <param name="namedProperties"></param>
        /// <param name="propertyValues"></param>
        /// <param name="namedFields"></param>
        /// <param name="fieldValues"></param>
        ICustomAttributeBuilder CreateCustomAttribute(IConstructorSymbol con, object?[] constructorArgs, IPropertySymbol[] namedProperties, object?[] propertyValues, IFieldSymbol[] namedFields, object?[] fieldValues);

    }

}
