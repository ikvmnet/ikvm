using System;
using System.Collections.Immutable;
using System.Reflection;

using IKVM.CoreLib.Collections;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Defines a unique method signature.
    /// </summary>
    public readonly struct MethodSymbolSignature
    {

        static readonly ImmutableExtensions.ImmutableArrayValueComparer<TypeSymbol, ImmutableExtensions.ValueReferenceEqualityComparer<TypeSymbol>> typeArrayComparer = new(new());
        static readonly ImmutableExtensions.ImmutableArrayValueComparer<ImmutableArray<TypeSymbol>, ImmutableExtensions.ImmutableArrayValueComparer<TypeSymbol, ImmutableExtensions.ValueReferenceEqualityComparer<TypeSymbol>>> nestedTypeArrayComparer = new(typeArrayComparer);

        readonly TypeSymbol returnType;
        readonly ImmutableArray<TypeSymbol> returnRequiredCustomModifiers;
        readonly ImmutableArray<TypeSymbol> returnOptionalCustomModifiers;
        readonly ImmutableArray<TypeSymbol> parameterTypes;
        readonly ImmutableArray<ImmutableArray<TypeSymbol>> parameterRequiredCustomModifiers;
        readonly ImmutableArray<ImmutableArray<TypeSymbol>> parameterOptionalCustomModifiers;
        readonly CallingConventions callingConvention;
        readonly int genericParameterCount;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="returnRequiredCustomModifiers"></param>
        /// <param name="returnOptionalCustomModifiers"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="parameterRequiredCustomModifiers"></param>
        /// <param name="parameterOptionalCustomModifiers"></param>
        /// <param name="callingConvention"></param>
        /// <param name="genericParameterCount"></param>
        public MethodSymbolSignature(TypeSymbol returnType, ImmutableArray<TypeSymbol> returnRequiredCustomModifiers, ImmutableArray<TypeSymbol> returnOptionalCustomModifiers, ImmutableArray<TypeSymbol> parameterTypes, ImmutableArray<ImmutableArray<TypeSymbol>> parameterRequiredCustomModifiers, ImmutableArray<ImmutableArray<TypeSymbol>> parameterOptionalCustomModifiers, CallingConventions callingConvention, int genericParameterCount)
        {
            this.returnType = returnType;
            this.returnRequiredCustomModifiers = returnRequiredCustomModifiers;
            this.returnOptionalCustomModifiers = returnOptionalCustomModifiers;
            this.parameterTypes = parameterTypes;
            this.parameterRequiredCustomModifiers = parameterRequiredCustomModifiers;
            this.parameterOptionalCustomModifiers = parameterOptionalCustomModifiers;
            this.callingConvention = callingConvention;
            this.genericParameterCount = genericParameterCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal MethodSymbolSignature Specialize(GenericContext genericContext)
        {
            return new MethodSymbolSignature(
                returnType.Specialize(genericContext),
                Specialize(returnRequiredCustomModifiers, genericContext),
                Specialize(returnOptionalCustomModifiers, genericContext),
                Specialize(parameterTypes, genericContext),
                Specialize(parameterRequiredCustomModifiers, genericContext),
                Specialize(parameterOptionalCustomModifiers, genericContext),
                callingConvention,
                genericParameterCount);
        }

        /// <summary>
        /// Specializes the given type array.
        /// </summary>
        /// <param name="types"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> Specialize(ImmutableArray<TypeSymbol> types, GenericContext genericContext)
        {
            var b = ImmutableArray.CreateBuilder<TypeSymbol>(types.Length);
            foreach (var i in types)
                b.Add(i.Specialize(genericContext));

            return b.DrainToImmutable();
        }

        /// <summary>
        /// Specializes the given array of type arrays.
        /// </summary>
        /// <param name="types"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        ImmutableArray<ImmutableArray<TypeSymbol>> Specialize(ImmutableArray<ImmutableArray<TypeSymbol>> types, GenericContext genericContext)
        {
            var b = ImmutableArray.CreateBuilder<ImmutableArray<TypeSymbol>>(types.Length);
            foreach (var i in types)
                b.Add(Specialize(i, genericContext));

            return b.DrainToImmutable();
        }

        /// <summary>
        /// Returns <c>true</c> if this signature is equal to the specified signature.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(MethodSymbolSignature other)
        {
            return other.callingConvention == callingConvention
                && other.genericParameterCount == genericParameterCount
                && other.returnType.Equals(returnType)
                && other.parameterTypes.ImmutableArrayReferenceEquals(parameterTypes)
                && other.returnRequiredCustomModifiers.ImmutableArrayReferenceEquals(returnRequiredCustomModifiers)
                && other.returnOptionalCustomModifiers.ImmutableArrayReferenceEquals(returnOptionalCustomModifiers)
                && other.parameterTypes.ImmutableArrayReferenceEquals(parameterTypes)
                && other.parameterRequiredCustomModifiers.ImmutableArrayEquals(parameterRequiredCustomModifiers, typeArrayComparer)
                && other.parameterOptionalCustomModifiers.ImmutableArrayEquals(parameterOptionalCustomModifiers, typeArrayComparer);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is MethodSymbolSignature s && Equals(s);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var h = new HashCode();
            h.Add(genericParameterCount);
            h.Add(callingConvention);
            h.Add(returnType);
            h.Add(parameterTypes, typeArrayComparer);
            h.Add(returnRequiredCustomModifiers, typeArrayComparer);
            h.Add(returnOptionalCustomModifiers, typeArrayComparer);
            h.Add(parameterRequiredCustomModifiers, nestedTypeArrayComparer);
            h.Add(parameterOptionalCustomModifiers, nestedTypeArrayComparer);
            return h.ToHashCode();
        }

    }

}
