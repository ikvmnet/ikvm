using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    abstract class MethodBaseSymbol : MemberSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        public MethodBaseSymbol(ISymbolContext context, IModuleSymbol module, TypeSymbol? declaringType) :
            base(context, module, declaringType)
        {

        }

        /// <summary>
        /// Gets the attributes associated with this method.
        /// </summary>
        public abstract MethodAttributes Attributes { get; }

        /// <summary>
        /// Gets a value indicating the calling conventions for this method.
        /// </summary>
        public abstract CallingConventions CallingConvention { get; }

        /// <summary>
        /// Gets a value indicating whether the generic method contains unassigned generic type parameters.
        /// </summary>
        public abstract bool ContainsGenericParameters { get; }

        /// <summary>
        /// Gets a value indicating whether the method is abstract.
        /// </summary>
        public bool IsAbstract => (Attributes & MethodAttributes.Abstract) != 0;

        /// <summary>
        /// Gets a value indicating whether the potential visibility of this method or constructor is described by Assembly; that is, the method or constructor is visible at most to other types in the same assembly, and is not visible to derived types outside the assembly.
        /// </summary>
        public bool IsAssembly => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Assembly;

        /// <summary>
        /// Gets a value indicating whether the method is a constructor.
        /// </summary>
        public bool IsConstructor => !IsStatic && (Attributes & MethodAttributes.RTSpecialName) == MethodAttributes.RTSpecialName;

        /// <summary>
        /// Gets a value indicating whether the visibility of this method or constructor is described by Family; that is, the method or constructor is visible only within its class and derived classes.
        /// </summary>
        public bool IsFamily => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Family;

        /// <summary>
        /// Gets a value indicating whether the visibility of this method or constructor is described by FamANDAssem; that is, the method or constructor can be called by derived classes, but only if they are in the same assembly.
        /// </summary>
        public bool IsFamilyAndAssembly => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamANDAssem;

        /// <summary>
        /// Gets a value indicating whether the potential visibility of this method or constructor is described by FamORAssem; that is, the method or constructor can be called by derived classes wherever they are, and by classes in the same assembly.
        /// </summary>
        public bool IsFamilyOrAssembly => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamORAssem;

        /// <summary>
        /// Gets a value indicating whether this method is final.
        /// </summary>
        public bool IsFinal => (Attributes & MethodAttributes.Final) != 0;

        /// <summary>
        /// Gets a value indicating whether the method is generic.
        /// </summary>
        public abstract bool IsGenericMethod { get; }

        /// <summary>
        /// Gets a value indicating whether the method is a generic method definition.
        /// </summary>
        public abstract bool IsGenericMethodDefinition { get; }

        /// <summary>
        /// Gets a value indicating whether only a member of the same kind with exactly the same signature is hidden in the derived class.
        /// </summary>
        public bool IsHideBySig => (Attributes & MethodAttributes.HideBySig) != 0;

        /// <summary>
        /// Gets a value indicating whether this member is private.
        /// </summary>
        public bool IsPrivate => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Private;

        /// <summary>
        /// Gets a value indicating whether this is a public method.
        /// </summary>
        public bool IsPublic => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public;

        /// <summary>
        /// Gets a value indicating whether the method is static.
        /// </summary>
        public bool IsStatic => (Attributes & MethodAttributes.Static) != 0;

        /// <summary>
        /// Gets a value indicating whether the method is virtual.
        /// </summary>
        public bool IsVirtual => (Attributes & MethodAttributes.Virtual) != 0;

        /// <summary>
        /// Gets a value indicating whether this method has a special name.
        /// </summary>
        public bool IsSpecialName => (Attributes & MethodAttributes.SpecialName) != 0;

        /// <summary>
        /// Gets the <see cref="MethodImplAttributes" /> flags that specify the attributes of a method implementation.
        /// </summary>
        public abstract MethodImplAttributes MethodImplementationFlags { get; }

        /// <summary>
        /// Returns an array of <see cref="ITypeSymbol" /> objects that represent the type arguments of a generic method or the type parameters of a generic method definition.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableList<TypeSymbol> GetGenericArguments();

        /// <summary>
        /// When overridden in a derived class, gets the parameters of the specified method or constructor.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableList<ParameterSymbol> GetParameters();

        /// <summary>
        /// When overridden in a derived class, returns the MethodImplAttributes flags.
        /// </summary>
        /// <returns></returns>
        public abstract MethodImplAttributes GetMethodImplementationFlags();

    }

}
