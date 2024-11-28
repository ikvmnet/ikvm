using System;
using System.Collections.Immutable;

using IKVM.CoreLib.Text;

namespace IKVM.CoreLib.Symbols
{

    public readonly record struct CustomAttribute(TypeSymbol AttributeType, MethodSymbol Constructor, ImmutableArray<CustomAttributeTypedArgument> ConstructorArguments, ImmutableArray<CustomAttributeNamedArgument> NamedArguments)
    {

        /// <summary>
        /// Initializes an instance of the <see cref="CustomAttribute"/> interface given the constructor for the custom attribute and the arguments to the constructor.
        /// </summary>
        /// <param name="ctor"></param>
        /// <param name="constructorArgs"></param>
        /// <returns></returns>
        public static CustomAttribute Create(MethodSymbol ctor, ImmutableArray<object?> constructorArgs)
        {
            return new CustomAttribute(
                ctor.DeclaringType ?? throw new InvalidOperationException(),
                ctor,
                PackTypedArgs(ctor.ParameterTypes, constructorArgs),
                ImmutableArray<CustomAttributeNamedArgument>.Empty);
        }

        /// <summary>
        /// Initializes an instance of the <see cref="CustomAttribute"/> interface given the constructor for the custom attribute, the arguments to the constructor, and a set of named field/value pairs.
        /// </summary>
        /// <param name="ctor"></param>
        /// <param name="constructorArgs"></param>
        /// <param name="namedFields"></param>
        /// <param name="fieldValues"></param>
        public static CustomAttribute Create(MethodSymbol ctor, ImmutableArray<object?> constructorArgs, ImmutableArray<FieldSymbol> namedFields, ImmutableArray<object?> fieldValues)
        {
            return new CustomAttribute(
                ctor.DeclaringType ?? throw new InvalidOperationException(),
                ctor,
                PackTypedArgs(ctor.ParameterTypes, constructorArgs),
                PackNamedArgs([], [], namedFields, fieldValues));
        }

        /// <summary>
        /// Initializes an instance of the <see cref="CustomAttribute"/> interface given the constructor for the custom attribute, the arguments to the constructor, and a set of named property or value pairs.
        /// </summary>
        /// <param name="ctor"></param>
        /// <param name="constructorArgs"></param>
        /// <param name="namedProperties"></param>
        /// <param name="propertyValues"></param>
        public static CustomAttribute Create(MethodSymbol ctor, ImmutableArray<object?> constructorArgs, ImmutableArray<PropertySymbol> namedProperties, ImmutableArray<object?> propertyValues)
        {
            return new CustomAttribute(
                ctor.DeclaringType ?? throw new InvalidOperationException(),
                ctor,
                PackTypedArgs(ctor.ParameterTypes, constructorArgs),
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
        public static CustomAttribute Create(MethodSymbol ctor, ImmutableArray<object?> constructorArgs, ImmutableArray<PropertySymbol> namedProperties, ImmutableArray<object?> propertyValues, ImmutableArray<FieldSymbol> namedFields, ImmutableArray<object?> fieldValues)
        {
            return new CustomAttribute(
                ctor.DeclaringType ?? throw new InvalidOperationException(),
                ctor,
                PackTypedArgs(ctor.ParameterTypes, constructorArgs),
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
        static ImmutableArray<CustomAttributeTypedArgument> PackTypedArgs(ImmutableArray<TypeSymbol> types, ImmutableArray<object?> values)
        {
            if (types.IsDefault)
                throw new ArgumentNullException(nameof(types));
            if (values.IsDefault)
                throw new ArgumentNullException(nameof(values));
            if (types.Length != values.Length)
                throw new ArgumentException();

            var a = ImmutableArray.CreateBuilder<CustomAttributeTypedArgument>(types.Length);
            for (int i = 0; i < types.Length; i++)
                a.Add(PackTypedArg(types[i], values[i]));

            return a.DrainToImmutable();
        }

        /// <summary>
        /// Packs the type as a typed argument.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        static CustomAttributeTypedArgument PackTypedArg(TypeSymbol type, object? value)
        {
            return new CustomAttributeTypedArgument(type, value);
        }

        /// <summary>
        /// Packages the members and args as a named argument.
        /// </summary>
        /// <param name="namedProperties"></param>
        /// <param name="propertyValues"></param>
        /// <returns></returns>
        static ImmutableArray<CustomAttributeNamedArgument> PackNamedArgs(ImmutableArray<PropertySymbol> namedProperties, ImmutableArray<object?> propertyValues, ImmutableArray<FieldSymbol> namedFields, ImmutableArray<object?> fieldValues)
        {
            var a = ImmutableArray.CreateBuilder<CustomAttributeNamedArgument>(namedProperties.Length + namedFields.Length);
            for (int i = 0; i < namedProperties.Length; i++)
                a.Add(PackNamedArg(namedProperties[i], propertyValues[i]));
            for (int i = 0; i < namedFields.Length; i++)
                a.Add(PackNamedArg(namedFields[i], fieldValues[i]));

            return a.DrainToImmutable();
        }

        /// <summary>
        /// Packs the property and arg as a named argument.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        static CustomAttributeNamedArgument PackNamedArg(PropertySymbol property, object? v)
        {
            return new CustomAttributeNamedArgument(property, PackTypedArg(property.PropertyType, v));
        }

        /// <summary>
        /// Packs the field and arg as a named argument.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        static CustomAttributeNamedArgument PackNamedArg(FieldSymbol field, object? v)
        {
            return new CustomAttributeNamedArgument(field, PackTypedArg(field.FieldType, v));
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var vsb = new ValueStringBuilder(stackalloc char[256]);

            vsb.Append('[');
            vsb.Append(Constructor.DeclaringType!.FullName);
            vsb.Append('(');

            var first = true;

            var constructorArguments = ConstructorArguments;
            var constructorArgumentsCount = constructorArguments.Length;
            for (int i = 0; i < constructorArgumentsCount; i++)
            {
                if (!first)
                    vsb.Append(", ");

                vsb.Append(constructorArguments[i].ToString());
                first = false;
            }

            var namedArguments = NamedArguments;
            var namedArgumentsCount = namedArguments.Length;
            for (int i = 0; i < namedArgumentsCount; i++)
            {
                if (!first)
                    vsb.Append(", ");

                vsb.Append(namedArguments[i].ToString());
                first = false;
            }

            vsb.Append(")]");

            return vsb.ToString();
        }

    }


}
