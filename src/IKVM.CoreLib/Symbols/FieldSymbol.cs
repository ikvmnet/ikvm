using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    abstract class FieldSymbol : MemberSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        public FieldSymbol(ISymbolContext context, IModuleSymbol module, TypeSymbol? declaringType) :
            base(context, module, declaringType)
        {

        }

        /// <summary>
        /// Gets the attributes associated with this field.
        /// </summary>
        public abstract FieldAttributes Attributes { get; }

        /// <summary>
        /// Gets the type of this field object.
        /// </summary>
        public abstract TypeSymbol FieldType { get; }

        /// <inheritdoc />
        public sealed override MemberTypes MemberType => MemberTypes.Field;

        /// <summary>
        /// Gets a value indicating whether the potential visibility of this field is described by <see cref="FieldAttributes.Assembly"/>; that is, the field is visible at most to other types in the same assembly, and is not visible to derived types outside the assembly.
        /// </summary>
        public bool IsAssembly => (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Assembly;

        /// <summary>
        /// Gets a value indicating whether the visibility of this field is described by <see cref="FieldAttributes.Family"/>; that is, the field is visible only within its class and derived classes.
        /// </summary>
        public bool IsFamily => (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Family;

        /// <summary>
        /// Gets a value indicating whether the visibility of this field is described by <see cref="FieldAttributes.FamANDAssem"/>; that is, the field can be accessed from derived classes, but only if they are in the same assembly.
        /// </summary>
        public bool IsFamilyAndAssembly => (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamANDAssem;

        /// <summary>
        /// Gets a value indicating whether the potential visibility of this field is described by <see cref="FieldAttributes.FamORAssem"/>; that is, the field can be accessed by derived classes wherever they are, and by classes in the same assembly.
        /// </summary>
        public bool IsFamilyOrAssembly => (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamORAssem;

        /// <summary>
        /// Gets a value indicating whether the field can only be set in the body of the constructor.
        /// </summary>
        public bool IsInitOnly => (Attributes & FieldAttributes.InitOnly) != 0;

        /// <summary>
        /// Gets a value indicating whether the value is written at compile time and cannot be changed.
        /// </summary>
        public bool IsLiteral => (Attributes & FieldAttributes.Literal) != 0;

        /// <summary>
        /// Gets a value indicating whether this field has the NotSerialized attribute.
        /// </summary>
        public bool IsNotSerialized => (Attributes & FieldAttributes.NotSerialized) != 0;

        /// <summary>
        /// Gets a value indicating whether the corresponding PinvokeImpl attribute is set in FieldAttributes.
        /// </summary>
        public bool IsPinvokeImpl => (Attributes & FieldAttributes.PinvokeImpl) != 0;

        /// <summary>
        /// Gets a value indicating whether the field is private.
        /// </summary>
        public bool IsPrivate => (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Private;

        /// <summary>
        /// Gets a value indicating whether the field is public.
        /// </summary>
        public bool IsPublic => (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public;

        /// <summary>
        /// Gets a value indicating whether the corresponding SpecialName attribute is set in the FieldAttributes enumerator.
        /// </summary>
        public bool IsSpecialName => (Attributes & FieldAttributes.SpecialName) != 0;

        /// <summary>
        /// Gets a value indicating whether the field is static.
        /// </summary>
        public bool IsStatic => (Attributes & FieldAttributes.Static) != 0;

        /// <summary>
        /// Returns a literal value associated with the field by a compiler.
        /// </summary>
        /// <returns></returns>
        public abstract object? GetRawConstantValue();

        /// <summary>
        /// Gets an array of types that identify the optional custom modifiers of the field.
        /// </summary>
        /// <returns></returns>
        public abstract IImmutableList<TypeSymbol> GetOptionalCustomModifiers();

        /// <summary>
        /// Gets an array of types that identify the required custom modifiers of the property.
        /// </summary>
        /// <returns></returns>
        public abstract IImmutableList<TypeSymbol> GetRequiredCustomModifiers();

    }

}
