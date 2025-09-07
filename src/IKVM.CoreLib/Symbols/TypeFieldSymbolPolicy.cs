using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Policy for field symbols.
    /// </summary>
    class TypeFieldSymbolPolicy : MemberSymbolPolicy<TypeSymbol, FieldSymbol>
    {

        /// <inheritdoc />
        public sealed override ImmutableArray<FieldSymbol> GetDeclaredMembers(TypeSymbol type) => type.GetDeclaredFields();

        /// <inheritdoc />
        public sealed override TypeSymbol? GetInheritedParent(TypeSymbol parent) => parent.BaseType;

        /// <inheritdoc />
        public sealed override bool AlwaysTreatAsDeclaredOnly => false;

        /// <inheritdoc />
        public sealed override void GetMemberAttributes(FieldSymbol member, out MethodAttributes visibility, out bool isStatic, out bool isVirtual, out bool isNewSlot)
        {
            var fieldAttributes = member.Attributes;
            visibility = (MethodAttributes)(fieldAttributes & FieldAttributes.FieldAccessMask);
            isStatic = (0 != (fieldAttributes & FieldAttributes.Static));
            isVirtual = false;
            isNewSlot = false;
        }

        /// <inheritdoc />
        public sealed override bool ImplicitlyOverrides(FieldSymbol? baseMember, FieldSymbol? derivedMember) => false;

        /// <inheritdoc />
        public sealed override bool IsSuppressedByMoreDerivedMember(FieldSymbol member, List<FieldSymbol> priorMembers) => false;

        /// <inheritdoc />
        public sealed override bool OkToIgnoreAmbiguity(FieldSymbol m1, FieldSymbol m2) => true;

    }

}
