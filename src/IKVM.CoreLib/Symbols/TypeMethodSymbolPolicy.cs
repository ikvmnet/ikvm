using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Policy for constructor symbols.
    /// </summary>
    class TypeMethodSymbolPolicy : MemberSymbolPolicy<TypeSymbol, MethodSymbol>
    {

        /// <inheritdoc />
        public sealed override ImmutableArray<MethodSymbol> GetDeclaredMembers(TypeSymbol type) => type.GetDeclaredMethods();

        /// <inheritdoc />
        public sealed override TypeSymbol? GetInheritedParent(TypeSymbol parent) => parent.BaseType;

        /// <inheritdoc />
        public sealed override bool AlwaysTreatAsDeclaredOnly => false;

        /// <inheritdoc />
        public sealed override void GetMemberAttributes(MethodSymbol member, out MethodAttributes visibility, out bool isStatic, out bool isVirtual, out bool isNewSlot)
        {
            var methodAttributes = member.Attributes;
            visibility = methodAttributes & MethodAttributes.MemberAccessMask;
            isStatic = (0 != (methodAttributes & MethodAttributes.Static));
            isVirtual = (0 != (methodAttributes & MethodAttributes.Virtual));
            isNewSlot = (0 != (methodAttributes & MethodAttributes.NewSlot));
        }

        /// <inheritdoc />
        public sealed override bool ImplicitlyOverrides(MethodSymbol? baseMember, MethodSymbol? derivedMember)
        {
            return AreNamesAndSignaturesEqual(baseMember!, derivedMember!);
        }

        /// <inheritdoc />
        public sealed override bool IsSuppressedByMoreDerivedMember(MethodSymbol member, List<MethodSymbol> priorMembers)
        {
            if (!member.IsVirtual)
                return false;

            foreach (var prior in priorMembers)
            {
                var attributes = prior.Attributes & (MethodAttributes.Virtual | MethodAttributes.VtableLayoutMask);
                if (attributes != (MethodAttributes.Virtual | MethodAttributes.ReuseSlot))
                    continue;
                if (!ImplicitlyOverrides(member, prior))
                    continue;

                return true;
            }
            return false;
        }

        public sealed override bool OkToIgnoreAmbiguity(MethodSymbol m1, MethodSymbol m2)
        {
            return DefaultBinder.CompareMethodSig(m1, m2);  // If all candidates have the same signature, pick the most derived one without throwing an AmbiguousMatchException.
        }

    }

}
