using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Policy for type symbols.
    /// </summary>
    class TypeTypeSymbolPolicy : MemberSymbolPolicy<TypeSymbol, TypeSymbol>
    {

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetDeclaredMembers(TypeSymbol type) => type.GetDeclaredNestedTypes();

        /// <inheritdoc />
        public override TypeSymbol? GetInheritedParent(TypeSymbol parent) => parent.BaseType;

        /// <inheritdoc />
        public sealed override bool AlwaysTreatAsDeclaredOnly => true;

        /// <inheritdoc />
        public sealed override void GetMemberAttributes(TypeSymbol member, out MethodAttributes visibility, out bool isStatic, out bool isVirtual, out bool isNewSlot)
        {
            isStatic = true;
            isVirtual = false;
            isNewSlot = false;

            // Since we never search base types for nested types, we don't need to map every visibility value one to one.
            // We just need to distinguish between "public" and "everything else."
            visibility = member.IsNestedPublic ? MethodAttributes.Public : MethodAttributes.Private;
        }

        /// <inheritdoc />
        public sealed override bool ImplicitlyOverrides(TypeSymbol? baseMember, TypeSymbol? derivedMember) => false;

        /// <inheritdoc />
        public sealed override bool IsSuppressedByMoreDerivedMember(TypeSymbol member, List<TypeSymbol> priorMembers) => false;

        /// <inheritdoc />
        public sealed override BindingFlags ModifyBindingFlags(BindingFlags bindingFlags)
        {
            bindingFlags &= BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase;
            bindingFlags |= BindingFlags.Static | BindingFlags.DeclaredOnly;
            return bindingFlags;
        }

        /// <inheritdoc />
        public sealed override bool OkToIgnoreAmbiguity(TypeSymbol m1, TypeSymbol m2)
        {
            return false;
        }

    }

}
