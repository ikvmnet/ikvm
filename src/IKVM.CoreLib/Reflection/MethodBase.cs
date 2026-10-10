/*
  Copyright (C) 2009 Jeroen Frijters

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
namespace IKVM.Reflection
{

    abstract class MethodBase : MemberInfo
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal MethodBase()
        {
        }

        internal abstract MethodSignature MethodSignature { get; }
        internal abstract int ParameterCount { get; }
        public abstract ParameterInfo[] GetParameters();

        internal abstract Type[] GetParameterTypes();

        public abstract MethodAttributes Attributes { get; }
        public abstract MethodImplAttributes GetMethodImplementationFlags();
        public abstract CallingConventions CallingConvention { get; }

        public bool IsConstructor
        {
            get
            {
                if ((Attributes & MethodAttributes.RTSpecialName) != 0)
                {
                    var name = Name;
                    return name == ConstructorInfo.ConstructorName || name == ConstructorInfo.TypeConstructorName;
                }

                return false;
            }
        }

        public bool IsStatic => (Attributes & MethodAttributes.Static) != 0;

        public bool IsVirtual => (Attributes & MethodAttributes.Virtual) != 0;

        public bool IsAbstract => (Attributes & MethodAttributes.Abstract) != 0;

        public bool IsFinal => (Attributes & MethodAttributes.Final) != 0;

        public bool IsPublic => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public;

        public bool IsFamily => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Family;

        public bool IsFamilyOrAssembly => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamORAssem;

        public bool IsAssembly => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Assembly;

        public bool IsFamilyAndAssembly => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamANDAssem;

        public bool IsPrivate => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Private;

        public bool IsSpecialName => (Attributes & MethodAttributes.SpecialName) != 0;

        public bool IsHideBySig => (Attributes & MethodAttributes.HideBySig) != 0;

        public MethodImplAttributes MethodImplementationFlags => GetMethodImplementationFlags();

        public virtual Type[] GetGenericArguments() => Type.EmptyTypes;

        public virtual bool IsGenericMethod => false;

        public virtual bool IsGenericMethodDefinition => false;

        public virtual bool ContainsGenericParameters => IsGenericMethodDefinition;

        internal abstract int ImportTo(Emit.ModuleBuilder module);

        internal abstract MethodBase BindTypeParameters(Type type);

        internal sealed override bool BindingFlagsMatch(BindingFlags flags)
        {
            return BindingFlagsMatch(IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(IsStatic, flags, BindingFlags.Static, BindingFlags.Instance);
        }

        internal sealed override bool BindingFlagsMatchInherited(BindingFlags flags)
        {
            return (Attributes & MethodAttributes.MemberAccessMask) > MethodAttributes.Private
                && BindingFlagsMatch(IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(IsStatic, flags, BindingFlags.Static | BindingFlags.FlattenHierarchy, BindingFlags.Instance);
        }

    }

}
