/*
  Copyright (C) 2009-2012 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System.Collections.Generic;

namespace IKVM.Reflection
{

    abstract class EventInfo : MemberInfo
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected EventInfo()
        {
        }

        public sealed override MemberTypes MemberType => MemberTypes.Event;

        public abstract EventAttributes Attributes { get; }

        public abstract MethodInfo GetAddMethod(bool nonPublic);

        public abstract MethodInfo GetRaiseMethod(bool nonPublic);

        public abstract MethodInfo GetRemoveMethod(bool nonPublic);

        public abstract MethodInfo[] GetOtherMethods(bool nonPublic);

        public abstract Type EventHandlerType { get; }

        internal abstract bool IsPublic { get; }

        internal abstract bool IsNonPrivate { get; }

        internal abstract bool IsStatic { get; }

        public bool IsSpecialName => (Attributes & EventAttributes.SpecialName) != 0;

        public MethodInfo GetAddMethod() => GetAddMethod(false);

        public MethodInfo GetRaiseMethod() => GetRaiseMethod(false);

        public MethodInfo GetRemoveMethod() => GetRemoveMethod(false);

        public MethodInfo[] GetOtherMethods() => GetOtherMethods(false);

        public MethodInfo AddMethod => GetAddMethod(true);

        public MethodInfo RaiseMethod => GetRaiseMethod(true);

        public MethodInfo RemoveMethod => GetRemoveMethod(true);

        internal virtual EventInfo BindTypeParameters(Type type) => new GenericEventInfo(DeclaringType.BindTypeParameters(type), this);

        public override string ToString() => DeclaringType.ToString() + " " + Name;

        internal sealed override bool BindingFlagsMatch(BindingFlags flags)
        {
            return BindingFlagsMatch(IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(IsStatic, flags, BindingFlags.Static, BindingFlags.Instance);
        }

        internal sealed override bool BindingFlagsMatchInherited(BindingFlags flags)
        {
            return IsNonPrivate
                && BindingFlagsMatch(IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(IsStatic, flags, BindingFlags.Static | BindingFlags.FlattenHierarchy, BindingFlags.Instance);
        }

        internal sealed override MemberInfo SetReflectedType(Type type) => new EventInfoWithReflectedType(type, this);

        internal sealed override List<CustomAttributeData> GetPseudoCustomAttributes(Type attributeType) =>
            // events don't have pseudo custom attributes
            null;

    }

}
