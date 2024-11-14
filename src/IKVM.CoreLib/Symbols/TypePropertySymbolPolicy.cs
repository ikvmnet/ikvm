using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Policy for property symbols.
    /// </summary>
    class TypePropertySymbolPolicy : MemberSymbolPolicy<TypeSymbol, PropertySymbol>
    {

        /// <inheritdoc />
        public sealed override ImmutableArray<PropertySymbol> GetDeclaredMembers(TypeSymbol type) => type.GetDeclaredProperties();

        /// <inheritdoc />
        public sealed override TypeSymbol? GetInheritedParent(TypeSymbol parent) => parent.BaseType;

        /// <inheritdoc />
        public sealed override bool AlwaysTreatAsDeclaredOnly => false;

        /// <inheritdoc />
        public sealed override void GetMemberAttributes(PropertySymbol member, out MethodAttributes visibility, out bool isStatic, out bool isVirtual, out bool isNewSlot)
        {
            var accessorMethod = GetAccessorMethod(member);
            if (accessorMethod == null)
            {
                // If we got here, this is a inherited PropertySymbol that only had private accessors and is now refusing to give them out
                // because that's what the rules of inherited PropertySymbol's are. Such a PropertySymbol is also considered private and will never be
                // given out of a Type.GetProperty() call. So all we have to do is set its visibility to Private and it will get filtered out.
                // Other values need to be set to satisfy C# but they are meaningless.
                visibility = MethodAttributes.Private;
                isStatic = false;
                isVirtual = false;
                isNewSlot = true;
                return;
            }

            var methodAttributes = accessorMethod.Attributes;
            visibility = methodAttributes & MethodAttributes.MemberAccessMask;
            isStatic = (0 != (methodAttributes & MethodAttributes.Static));
            isVirtual = (0 != (methodAttributes & MethodAttributes.Virtual));
            isNewSlot = (0 != (methodAttributes & MethodAttributes.NewSlot));
        }

        /// <inheritdoc />
        public sealed override bool ImplicitlyOverrides(PropertySymbol? baseMember, PropertySymbol? derivedMember)
        {
            var baseAccessor = GetAccessorMethod(baseMember!);
            var derivedAccessor = GetAccessorMethod(derivedMember!);
            return MemberSymbolPolicy<TypeSymbol, MethodSymbol>.Default.ImplicitlyOverrides(baseAccessor, derivedAccessor);
        }

        /// <inheritdoc />
        public sealed override bool IsSuppressedByMoreDerivedMember(PropertySymbol member, List<PropertySymbol> priorMembers)
        {
            var baseAccessor = GetAccessorMethod(member)!;

            foreach (var prior in priorMembers)
            {
                var derivedAccessor = GetAccessorMethod(prior)!;
                if (!AreNamesAndSignaturesEqual(baseAccessor, derivedAccessor))
                    continue;
                if (derivedAccessor.IsStatic != baseAccessor.IsStatic)
                    continue;
                if (!(prior.PropertyType.Equals(member.PropertyType)))
                    continue;

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public sealed override bool OkToIgnoreAmbiguity(PropertySymbol m1, PropertySymbol m2)
        {
            return false;
        }

        /// <inheritdoc />
        static MethodSymbol? GetAccessorMethod(PropertySymbol property) => property.GetMethod ?? property.SetMethod;

    }

}
