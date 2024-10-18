using System;
using System.Collections.Immutable;
using System.Linq;

namespace IKVM.CoreLib.Symbols
{

    readonly record struct CustomAttribute(
        ITypeSymbol AttributeType,
        IConstructorSymbol Constructor,
        ImmutableArray<CustomAttributeTypedArgument> ConstructorArguments,
        ImmutableArray<CustomAttributeNamedArgument> NamedArguments)
    {

        /// <summary>
        /// Initializes an instance of the <see cref="CustomAttribute"/> interface given the constructor for the custom attribute and the arguments to the constructor.
        /// </summary>
        /// <param name="ctor"></param>
        /// <param name="constructorArgs"></param>
        /// <returns></returns>
        public static CustomAttribute Create(IConstructorSymbol ctor, object?[] constructorArgs)
        {
            return new CustomAttribute(
                ctor.DeclaringType ?? throw new InvalidOperationException(),
                ctor,
                PackTypedArgs(ctor.GetParameters().Select(i => i.ParameterType).ToArray(), constructorArgs),
                ImmutableArray<CustomAttributeNamedArgument>.Empty);
        }

        /// <summary>
        /// Initializes an instance of the <see cref="CustomAttribute"/> interface given the constructor for the custom attribute, the arguments to the constructor, and a set of named field/value pairs.
        /// </summary>
        /// <param name="ctor"></param>
        /// <param name="constructorArgs"></param>
        /// <param name="namedFields"></param>
        /// <param name="fieldValues"></param>
        public static CustomAttribute Create(IConstructorSymbol ctor, object?[] constructorArgs, IFieldSymbol[] namedFields, object?[] fieldValues)
        {
            return new CustomAttribute(
                ctor.DeclaringType ?? throw new InvalidOperationException(),
                ctor,
                PackTypedArgs(ctor.GetParameters().Select(i => i.ParameterType).ToArray(), constructorArgs),
                PackNamedArgs([], [], namedFields, fieldValues));
        }

        /// <summary>
        /// Initializes an instance of the <see cref="CustomAttribute"/> interface given the constructor for the custom attribute, the arguments to the constructor, and a set of named property or value pairs.
        /// </summary>
        /// <param name="ctor"></param>
        /// <param name="constructorArgs"></param>
        /// <param name="namedProperties"></param>
        /// <param name="propertyValues"></param>
        public static CustomAttribute Create(IConstructorSymbol ctor, object?[] constructorArgs, IPropertySymbol[] namedProperties, object?[] propertyValues)
        {
            return new CustomAttribute(
                ctor.DeclaringType ?? throw new InvalidOperationException(),
                ctor,
                PackTypedArgs(ctor.GetParameters().Select(i => i.ParameterType).ToArray(), constructorArgs),
                PackNamedArgs(namedProperties, propertyValues, [], []));
        }

        /// <summary>
        /// Initializes an instance of the <see cref="CustomAttribute"/> interface given the constructor for the custom attribute, the arguments to the constructor, a set of named property or value pairs, and a set of named field or value pairs.
        /// </summary>
        /// <param name="ctor"></param>
        /// <param name="constructorArgs"></param>
        /// <param name="namedProperties"></param>
        /// <param name="propertyValues"></param>
        /// <param name="namedFields"></param>
        /// <param name="fieldValues"></param>
        public static CustomAttribute Create(IConstructorSymbol ctor, object?[] constructorArgs, IPropertySymbol[] namedProperties, object?[] propertyValues, IFieldSymbol[] namedFields, object?[] fieldValues)
        {
            return new CustomAttribute(
                ctor.DeclaringType ?? throw new InvalidOperationException(),
                ctor,
                PackTypedArgs(ctor.GetParameters().Select(i => i.ParameterType).ToArray(), constructorArgs),
                PackNamedArgs(namedProperties, propertyValues, namedFields, fieldValues));
        }

        /// <summary>
        /// Packs the types as typed arguments.
        /// </summary>
        /// <param name="types"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        static ImmutableArray<CustomAttributeTypedArgument> PackTypedArgs(ITypeSymbol[] types, object?[] values)
        {
            if (types is null)
                throw new ArgumentNullException(nameof(types));
            if (values is null)
                throw new ArgumentNullException(nameof(values));
            if (types.Length != values.Length)
                throw new ArgumentException();

            var a = ImmutableArray.CreateBuilder<CustomAttributeTypedArgument>(types.Length);
            for (int i = 0; i < types.Length; i++)
                a.Add(PackTypedArg(types[i], values[i]));

            return a.ToImmutable();
        }

        /// <summary>
        /// Packs the type as a typed argument.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        static CustomAttributeTypedArgument PackTypedArg(ITypeSymbol type, object? value)
        {
            return new CustomAttributeTypedArgument(type, value);
        }

        /// <summary>
        /// Packages the members and args as a named argument.
        /// </summary>
        /// <param name="namedProperties"></param>
        /// <param name="propertyValues"></param>
        /// <returns></returns>
        static ImmutableArray<CustomAttributeNamedArgument> PackNamedArgs(IPropertySymbol[] namedProperties, object?[] propertyValues, IFieldSymbol[] namedFields, object?[] fieldValues)
        {
            var a = ImmutableArray.CreateBuilder<CustomAttributeNamedArgument>(namedProperties.Length + namedFields.Length);
            for (int i = 0; i < namedProperties.Length; i++)
                a.Add(PackNamedArg(namedProperties[i], propertyValues[i]));
            for (int i = 0; i < namedFields.Length; i++)
                a.Add(PackNamedArg(namedFields[i], fieldValues[i]));

            return a.ToImmutable();
        }

        /// <summary>
        /// Packs the property and arg as a named argument.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        static CustomAttributeNamedArgument PackNamedArg(IPropertySymbol property, object? v)
        {
            return new CustomAttributeNamedArgument(false, property, property.Name, PackTypedArg(property.PropertyType, v));
        }

        /// <summary>
        /// Packs the field and arg as a named argument.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        static CustomAttributeNamedArgument PackNamedArg(IFieldSymbol field, object? v)
        {
            return new CustomAttributeNamedArgument(false, field, field.Name, PackTypedArg(field.FieldType, v));
        }

    }


}
