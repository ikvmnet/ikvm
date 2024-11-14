using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Policy for <see cref="EventSymbol"/> members on a <see cref="TypeSymbol"/>.
    /// </summary>
    class TypeEventSymbolPolicy : MemberSymbolPolicy<TypeSymbol, EventSymbol>
    {

        /// <inheritdoc />
        public sealed override ImmutableArray<EventSymbol> GetDeclaredMembers(TypeSymbol type) => type.GetDeclaredEvents();

        /// <inheritdoc />
        public override TypeSymbol? GetInheritedParent(TypeSymbol parent) => parent.BaseType;

        /// <inheritdoc />
        public sealed override bool AlwaysTreatAsDeclaredOnly => false;

        /// <inheritdoc />
        public sealed override void GetMemberAttributes(EventSymbol member, out MethodAttributes visibility, out bool isStatic, out bool isVirtual, out bool isNewSlot)
        {
            var accessorMethod = GetAccessorMethod(member);
            if (accessorMethod == null)
            {
                // If we got here, this is a inherited EventSymbol that only had private accessors and is now refusing to give them out
                // because that's what the rules of inherited EventSymbol's are. Such a EventSymbol is also considered private and will never be
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
        public sealed override bool IsSuppressedByMoreDerivedMember(EventSymbol member, List<EventSymbol> priorMembers)
        {
            foreach (var i in priorMembers)
                if (i.Name == member.Name)
                    return true;

            return false;
        }

        /// <inheritdoc />
        public sealed override bool ImplicitlyOverrides(EventSymbol? baseMember, EventSymbol? derivedMember)
        {
            var baseAccessor = GetAccessorMethod(baseMember!);
            var derivedAccessor = GetAccessorMethod(derivedMember!);
            return MemberSymbolPolicy<TypeSymbol, MethodSymbol>.Default.ImplicitlyOverrides(baseAccessor, derivedAccessor);
        }

        /// <inheritdoc />
        public sealed override bool OkToIgnoreAmbiguity(EventSymbol m1, EventSymbol m2)
        {
            return false;
        }

        /// <inheritdoc />
        static MethodSymbol? GetAccessorMethod(EventSymbol e)
        {
            return e.AddMethod;
        }

    }

}
