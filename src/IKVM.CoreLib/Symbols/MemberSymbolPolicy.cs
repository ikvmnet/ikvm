using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    abstract class MemberSymbolPolicy<TParentSymbol, TMemberSymbol>
        where TMemberSymbol : MemberSymbol
    {

        /// <summary>
        /// Gets the policy for the member symbol type.
        /// </summary>
        public static readonly MemberSymbolPolicy<TParentSymbol, TMemberSymbol> Default;

        /// <summary>
        /// Computes the default policy for the given member symbol type.
        /// </summary>
        /// <returns></returns>
        static MemberSymbolPolicy()
        {
            var parent = typeof(TParentSymbol);
            var member = typeof(TMemberSymbol);

            if (parent == typeof(ModuleSymbol))
            {
                if (member.Equals(typeof(FieldSymbol)))
                {
                    Default = (MemberSymbolPolicy<TParentSymbol, TMemberSymbol>)(object)new ModuleFieldSymbolPolicy();
                    return;
                }
                else if (member.Equals(typeof(MethodSymbol)))
                {
                    Default = (MemberSymbolPolicy<TParentSymbol, TMemberSymbol>)(object)new ModuleMethodSymbolPolicy();
                    return;
                }
                else if (member.Equals(typeof(TypeSymbol)))
                {
                    Default = (MemberSymbolPolicy<TParentSymbol, TMemberSymbol>)(object)new ModuleTypeSymbolPolicy();
                    return;
                }
                else
                {
                    throw new InvalidOperationException("Unknown member type.");
                }
            }

            if (parent == typeof(TypeSymbol))
            {
                if (member.Equals(typeof(FieldSymbol)))
                {
                    Default = (MemberSymbolPolicy<TParentSymbol, TMemberSymbol>)(object)new TypeFieldSymbolPolicy();
                    return;
                }
                else if (member.Equals(typeof(MethodSymbol)))
                {
                    Default = (MemberSymbolPolicy<TParentSymbol, TMemberSymbol>)(object)new TypeMethodSymbolPolicy();
                    return;
                }
                else if (member.Equals(typeof(PropertySymbol)))
                {
                    Default = (MemberSymbolPolicy<TParentSymbol, TMemberSymbol>)(object)new TypePropertySymbolPolicy();
                    return;
                }
                else if (member.Equals(typeof(EventSymbol)))
                {
                    Default = (MemberSymbolPolicy<TParentSymbol, TMemberSymbol>)(object)new TypeEventSymbolPolicy();
                    return;
                }
                else if (member.Equals(typeof(TypeSymbol)))
                {
                    Default = (MemberSymbolPolicy<TParentSymbol, TMemberSymbol>)(object)new TypeTypeSymbolPolicy();
                    return;
                }
                else
                {
                    throw new InvalidOperationException("Unknown member type.");
                }
            }

            throw new InvalidOperationException("Unknown parent type.");
        }

        /// <summary>
        /// Returns all of the directly declared members on the given <typeparamref name="TParentSymbol"/>.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public abstract ImmutableArray<TMemberSymbol> GetDeclaredMembers(TParentSymbol parent);

        /// <summary>
        /// Returns the base parent symbol that this parent is inherited from. Used for Type.BaseType.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public abstract TParentSymbol? GetInheritedParent(TParentSymbol parent);

        /// <summary>
        /// Policy to decide whether a member is considered "virtual", "virtual new" and what its member visibility is.
        /// (For "visibility", we reuse the MethodAttributes enum since Reflection lacks an element-agnostic enum for this.
        /// Only the MemberAccessMask bits are set.)
        /// </summary>
        public abstract void GetMemberAttributes(TMemberSymbol member, out MethodAttributes visibility, out bool isStatic, out bool isVirtual, out bool isNewSlot);

        /// <summary>
        /// Policy to decide whether "derivedMember" is a virtual override of "baseMember." Used to implement MethodInfo.GetBaseDefinition(),
        /// parent chain traversal for discovering inherited custom attributes, and suppressing lookup results in the Type.Get*() api family.
        ///
        /// Does not consider explicit overrides (methodimpls.) Does not consider "overrides" of interface methods.
        /// </summary>
        public abstract bool ImplicitlyOverrides(TMemberSymbol? baseMember, TMemberSymbol? derivedMember);

        /// <summary>
        /// Policy to decide how BindingFlags should be reinterpreted for a given member type.
        /// This is overridden for nested types which all match on any combination Instance | Static and are never inherited.
        /// It is also overridden for constructors which are never inherited.
        /// </summary>
        public virtual BindingFlags ModifyBindingFlags(BindingFlags bindingFlags)
        {
            return bindingFlags;
        }

        /// <summary>
        /// Policy to decide if BindingFlags is always interpreted as having set DeclaredOnly.
        /// </summary>
        public abstract bool AlwaysTreatAsDeclaredOnly { get; }

        /// <summary>
        /// Policy to decide how or if members in more derived types hide same-named members in base types.
        /// Due to .NET Framework compat concerns, the definitions are a bit more arbitrary than we'd like.
        /// </summary>
        public abstract bool IsSuppressedByMoreDerivedMember(TMemberSymbol member, List<TMemberSymbol> priorMembers);

        /// <summary>
        /// Policy to decide whether to throw an AmbiguousMatchException on an ambiguous Type.Get*() call.
        /// Does not apply to GetConstructor/GetMethod/GetProperty calls that have a non-null Type[] array passed to it.
        ///
        /// If method returns true, the Get() api will pick the member that's in the most derived type.
        /// If method returns false, the Get() api throws AmbiguousMatchException.
        /// </summary>
        public abstract bool OkToIgnoreAmbiguity(TMemberSymbol m1, TMemberSymbol m2);

        /// <summary>
        // Helper method for determining whether two methods are signature-compatible.
        /// </summary>
        /// <param name="method1"></param>
        /// <param name="method2"></param>
        /// <returns></returns>
        protected static bool AreNamesAndSignaturesEqual(MethodSymbol method1, MethodSymbol method2)
        {
            if (method1.Name != method2.Name)
                return false;

            var p1 = method1.Parameters;
            var p2 = method2.Parameters;
            if (p1.Length != p2.Length)
                return false;

            bool isGenericMethod1 = method1.IsGenericMethodDefinition;
            bool isGenericMethod2 = method2.IsGenericMethodDefinition;
            if (isGenericMethod1 != isGenericMethod2)
                return false;

            if (!isGenericMethod1)
            {
                for (int i = 0; i < p1.Length; i++)
                {
                    var parameterType1 = p1[i].ParameterType;
                    var parameterType2 = p2[i].ParameterType;
                    if (!(parameterType1.Equals(parameterType2)))
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (method1.GenericArguments.Length != method2.GenericArguments.Length)
                    return false;

                for (int i = 0; i < p1.Length; i++)
                {
                    var parameterType1 = p1[i].ParameterType;
                    var parameterType2 = p2[i].ParameterType;
                    if (!GenericMethodAwareAreParameterTypesEqual(parameterType1, parameterType2))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// This helper compares the types of the corresponding parameters of two methods to see if one method is signature equivalent to the other.
        /// This is needed when comparing the signatures of two generic methods as Type.Equals() is not up to that job.
        ///</summary>
        static bool GenericMethodAwareAreParameterTypesEqual(TypeSymbol t1, TypeSymbol t2)
        {
            // Fast-path - if Reflection has already deemed them equivalent, we can trust its result.
            if (t1.Equals(t2))
                return true;

            // If we got here, Reflection determined the types not equivalent. Most of the time, that's the result we want.
            // There is however, one wrinkle. If the type is or embeds a generic method parameter type, Reflection will always report them
            // non-equivalent, since generic parameter type comparison always compares both the position and the declaring method. For our purposes, though,
            // we only want to consider the position.

            // Fast-path: if the types don't embed any generic parameters, we can go ahead and use Reflection's result.
            if (!(t1.ContainsGenericParameters && t2.ContainsGenericParameters))
                return false;

            if ((t1.IsArray && t2.IsArray) || (t1.IsByRef && t2.IsByRef) || (t1.IsPointer && t2.IsPointer))
            {
                if (t1.IsSZArray != t2.IsSZArray)
                    return false;

                if (t1.IsArray && (t1.GetArrayRank() != t2.GetArrayRank()))
                    return false;

                return GenericMethodAwareAreParameterTypesEqual(t1.GetElementType()!, t2.GetElementType()!);
            }

            if (t1.IsConstructedGenericType)
            {
                // We can use regular old Equals() rather than recursing into GenericMethodAwareAreParameterTypesEqual() since the
                // generic type definition will always be a plain old named type and won't embed any generic method parameters.
                if (t1.GenericTypeDefinition.Equals(t2.GenericTypeDefinition) == false)
                    return false;

                var ga1 = t1.GenericArguments;
                var ga2 = t2.GenericArguments;
                if (ga1.Length != ga2.Length)
                    return false;

                for (int i = 0; i < ga1.Length; i++)
                    if (GenericMethodAwareAreParameterTypesEqual(ga1[i], ga2[i]) == false)
                        return false;

                return true;
            }

            if (t1.IsGenericMethodParameter && t2.IsGenericMethodParameter)
            {
                // A generic method parameter. The DeclaringMethods will be different but we don't care about that - we can assume that
                // the declaring method will be the method that declared the parameter's whose type we're testing. We only need to
                // compare the positions.
                return t1.GenericParameterPosition == t2.GenericParameterPosition;
            }

            // If we got here, either t1 and t2 are different flavors of types or they are both simple named types or both generic type parameters.
            // Either way, we can trust Reflection's result here.
            return false;
        }

    }

}
