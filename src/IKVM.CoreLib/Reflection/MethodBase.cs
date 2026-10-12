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

    /// <summary>
    /// Provides information about methods and constructors.
    /// </summary>
    abstract class MethodBase : MemberInfo
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal MethodBase()
        {
        }

        /// <summary>
        /// Gets the decoded signature of the method.
        /// </summary>
        internal abstract MethodSignature MethodSignature { get; }

        /// <summary>
        /// Gets the number of parameters of the method, without creating the <see cref="ParameterInfo"/> objects.
        /// </summary>
        internal abstract int ParameterCount { get; }

        /// <summary>
        /// When overridden in a derived class, gets the parameters of the specified method or constructor.
        /// </summary>
        /// <returns>An array of type <see cref="ParameterInfo"/> containing information that matches the signature of the method (or constructor) reflected by this <see cref="MethodBase"/> instance.</returns>
        public abstract ParameterInfo[] GetParameters();

        /// <summary>
        /// Gets the types of the parameters of the method, without creating the <see cref="ParameterInfo"/> objects.
        /// </summary>
        /// <returns>The parameter types, in declaration order.</returns>
        internal abstract Type[] GetParameterTypes();

        /// <summary>
        /// Gets the attributes associated with this method.
        /// </summary>
        public abstract MethodAttributes Attributes { get; }

        /// <summary>
        /// When overridden in a derived class, returns the <see cref="MethodImplAttributes"/> flags.
        /// </summary>
        /// <returns>The <see cref="MethodImplAttributes"/> flags.</returns>
        public abstract MethodImplAttributes GetMethodImplementationFlags();

        /// <summary>
        /// Gets a value indicating the calling conventions for this method.
        /// </summary>
        public abstract CallingConventions CallingConvention { get; }

        /// <summary>
        /// Gets a value indicating whether the method is a constructor or type initializer.
        /// </summary>
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

        /// <summary>
        /// Gets a value indicating whether the method is <see langword="static"/>.
        /// </summary>
        public bool IsStatic => (Attributes & MethodAttributes.Static) != 0;

        /// <summary>
        /// Gets a value indicating whether the method is <see langword="virtual"/>.
        /// </summary>
        public bool IsVirtual => (Attributes & MethodAttributes.Virtual) != 0;

        /// <summary>
        /// Gets a value indicating whether the method is abstract.
        /// </summary>
        public bool IsAbstract => (Attributes & MethodAttributes.Abstract) != 0;

        /// <summary>
        /// Gets a value indicating whether this method is <see langword="final"/>.
        /// </summary>
        public bool IsFinal => (Attributes & MethodAttributes.Final) != 0;

        /// <summary>
        /// Gets a value indicating whether this is a public method.
        /// </summary>
        public bool IsPublic => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public;

        /// <summary>
        /// Gets a value indicating whether the visibility of this method or constructor is described by
        /// <see cref="MethodAttributes.Family"/>; that is, the method or constructor is visible only within its class
        /// and derived classes.
        /// </summary>
        public bool IsFamily => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Family;

        /// <summary>
        /// Gets a value indicating whether the potential visibility of this method or constructor is described by
        /// <see cref="MethodAttributes.FamORAssem"/>; that is, the method or constructor can be called by derived
        /// classes wherever they are, and by classes in the same assembly.
        /// </summary>
        public bool IsFamilyOrAssembly => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamORAssem;

        /// <summary>
        /// Gets a value indicating whether the potential visibility of this method or constructor is described by
        /// <see cref="MethodAttributes.Assembly"/>; that is, the method or constructor is visible at most to other
        /// types in the same assembly, and is not visible to derived types outside the assembly.
        /// </summary>
        public bool IsAssembly => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Assembly;

        /// <summary>
        /// Gets a value indicating whether the visibility of this method or constructor is described by
        /// <see cref="MethodAttributes.FamANDAssem"/>; that is, the method or constructor can be called by derived
        /// classes, but only if they are in the same assembly.
        /// </summary>
        public bool IsFamilyAndAssembly => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamANDAssem;

        /// <summary>
        /// Gets a value indicating whether this member is private.
        /// </summary>
        public bool IsPrivate => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Private;

        /// <summary>
        /// Gets a value indicating whether this method has a special name.
        /// </summary>
        public bool IsSpecialName => (Attributes & MethodAttributes.SpecialName) != 0;

        /// <summary>
        /// Gets a value indicating whether only a member of the same kind with exactly the same signature is hidden in
        /// the derived class.
        /// </summary>
        public bool IsHideBySig => (Attributes & MethodAttributes.HideBySig) != 0;

        /// <summary>
        /// Gets the <see cref="MethodImplAttributes"/> flags that specify the attributes of a method implementation.
        /// </summary>
        public MethodImplAttributes MethodImplementationFlags => GetMethodImplementationFlags();

        /// <summary>
        /// Returns an array of <see cref="Type"/> objects that represent the type arguments of a generic method or the
        /// type parameters of a generic method definition.
        /// </summary>
        /// <returns>An array of <see cref="Type"/> objects that represent the type arguments of a generic method or the type parameters of a generic method definition. Returns an empty array if the current method is not a generic method.</returns>
        public virtual Type[] GetGenericArguments() => Type.EmptyTypes;

        /// <summary>
        /// Gets a value indicating whether the method is generic.
        /// </summary>
        public virtual bool IsGenericMethod => false;

        /// <summary>
        /// Gets a value indicating whether the method is a generic method definition.
        /// </summary>
        public virtual bool IsGenericMethodDefinition => false;

        /// <summary>
        /// Gets a value indicating whether the generic method contains unassigned generic type parameters.
        /// </summary>
        public virtual bool ContainsGenericParameters => IsGenericMethodDefinition;

        /// <summary>
        /// Gets a token that refers to this method from <paramref name="module"/>, adding a MemberRef or MethodSpec to
        /// it when the method is defined elsewhere.
        /// </summary>
        /// <param name="module">The module being built that references the method.</param>
        /// <returns>The token of the method in <paramref name="module"/>.</returns>
        internal abstract int ImportTo(Emit.ModuleBuilder module);

        /// <summary>
        /// Returns this method as a member of the instantiation of its declaring type whose generic arguments are
        /// supplied by <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The constructed generic type that supplies the type arguments for the declaring type.</param>
        /// <returns>The method on the bound declaring type.</returns>
        internal abstract MethodBase BindTypeParameters(Type type);

        /// <summary>
        /// Returns whether the method matches the visibility and static/instance criteria in the specified flags.
        /// </summary>
        /// <param name="flags">The binding flags to match.</param>
        /// <returns><see langword="true"/> if the method matches; otherwise <see langword="false"/>.</returns>
        internal sealed override bool BindingFlagsMatch(BindingFlags flags)
        {
            return BindingFlagsMatch(IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(IsStatic, flags, BindingFlags.Static, BindingFlags.Instance);
        }

        /// <summary>
        /// Returns whether the method, seen as inherited from a base type, matches the specified flags. Private methods
        /// never match, and static methods match only when <see cref="BindingFlags.FlattenHierarchy"/> is also set.
        /// </summary>
        /// <param name="flags">The binding flags to match.</param>
        /// <returns><see langword="true"/> if the inherited method matches; otherwise <see langword="false"/>.</returns>
        internal sealed override bool BindingFlagsMatchInherited(BindingFlags flags)
        {
            return (Attributes & MethodAttributes.MemberAccessMask) > MethodAttributes.Private
                && BindingFlagsMatch(IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(IsStatic, flags, BindingFlags.Static | BindingFlags.FlattenHierarchy, BindingFlags.Instance);
        }

    }

}
