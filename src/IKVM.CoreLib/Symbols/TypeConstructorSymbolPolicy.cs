using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Policy for constructor symbols.
    /// </summary>
    class TypeConstructorSymbolPolicy : MemberSymbolPolicy<TypeSymbol, ConstructorSymbol>
    {

        /// <inheritdoc />
        public sealed override ImmutableArray<ConstructorSymbol> GetDeclaredMembers(TypeSymbol type) => type.GetDeclaredConstructors();

        /// <inheritdoc />
        public sealed override TypeSymbol? GetInheritedParent(TypeSymbol parent) => parent.BaseType;

        public sealed override BindingFlags ModifyBindingFlags(BindingFlags bindingFlags)
        {
            return bindingFlags | BindingFlags.DeclaredOnly;
        }

        /// <inheritdoc />
        public sealed override bool AlwaysTreatAsDeclaredOnly => true;

        /// <inheritdoc />
        public sealed override void GetMemberAttributes(ConstructorSymbol member, out MethodAttributes visibility, out bool isStatic, out bool isVirtual, out bool isNewSlot)
        {
            var methodAttributes = member.Attributes;
            visibility = methodAttributes & MethodAttributes.MemberAccessMask;
            isStatic = 0 != (methodAttributes & MethodAttributes.Static);
            isVirtual = false;
            isNewSlot = false;
        }

        /// <inheritdoc />
        public sealed override bool ImplicitlyOverrides(ConstructorSymbol? baseMember, ConstructorSymbol? derivedMember) => false;

        /// <inheritdoc />
        public sealed override bool IsSuppressedByMoreDerivedMember(ConstructorSymbol member, List<ConstructorSymbol> priorMembers) => false;

        /// <inheritdoc />
        public sealed override bool OkToIgnoreAmbiguity(ConstructorSymbol m1, ConstructorSymbol m2)
        {
            throw new NotSupportedException();
        }

    }

}
