using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Provides information about methods and constructors.
    /// </summary>
    interface IMethodBaseSymbol : IMemberSymbol
    {

        /// <summary>
        /// Gets the attributes associated with this method.
        /// </summary>
        MethodAttributes Attributes { get; }

        /// <summary>
        /// Gets a value indicating the calling conventions for this method.
        /// </summary>
        CallingConventions CallingConvention { get; }

        /// <summary>
        /// Gets a value indicating whether the generic method contains unassigned generic type parameters.
        /// </summary>
        bool ContainsGenericParameters { get; }

        /// <summary>
        /// Gets a value indicating whether the method is abstract.
        /// </summary>
        bool IsAbstract { get; }

        /// <summary>
        /// Gets a value indicating whether the potential visibility of this method or constructor is described by Assembly; that is, the method or constructor is visible at most to other types in the same assembly, and is not visible to derived types outside the assembly.
        /// </summary>
        bool IsAssembly { get; }

        /// <summary>
        /// Gets a value indicating whether the method is a constructor.
        /// </summary>
        bool IsConstructor { get; }

        /// <summary>
        /// Gets a value indicating whether the visibility of this method or constructor is described by Family; that is, the method or constructor is visible only within its class and derived classes.
        /// </summary>
        bool IsFamily { get; }

        /// <summary>
        /// Gets a value indicating whether the visibility of this method or constructor is described by FamANDAssem; that is, the method or constructor can be called by derived classes, but only if they are in the same assembly.
        /// </summary>
        bool IsFamilyAndAssembly { get; }

        /// <summary>
        /// Gets a value indicating whether the potential visibility of this method or constructor is described by FamORAssem; that is, the method or constructor can be called by derived classes wherever they are, and by classes in the same assembly.
        /// </summary>
        bool IsFamilyOrAssembly { get; }

        /// <summary>
        /// Gets a value indicating whether this method is final.
        /// </summary>
        bool IsFinal { get; }

        /// <summary>
        /// Gets a value indicating whether the method is generic.
        /// </summary>
        bool IsGenericMethod { get; }

        /// <summary>
        /// Gets a value indicating whether the method is a generic method definition.
        /// </summary>
        bool IsGenericMethodDefinition { get; }

        /// <summary>
        /// Gets a value indicating whether only a member of the same kind with exactly the same signature is hidden in the derived class.
        /// </summary>
        bool IsHideBySig { get; }

        /// <summary>
        /// Gets a value indicating whether this member is private.
        /// </summary>
        bool IsPrivate { get; }

        /// <summary>
        /// Gets a value indicating whether this is a public method.
        /// </summary>
        bool IsPublic { get; }

        /// <summary>
        /// Gets a value indicating whether the method is static.
        /// </summary>
        bool IsStatic { get; }

        /// <summary>
        /// Gets a value indicating whether the method is virtual.
        /// </summary>
        bool IsVirtual { get; }

        /// <summary>
        /// Gets a value indicating whether this method has a special name.
        /// </summary>
        bool IsSpecialName { get; }

        /// <summary>
        /// Gets the <see cref="MethodImplAttributes" /> flags that specify the attributes of a method implementation.
        /// </summary>
        MethodImplAttributes MethodImplementationFlags { get; }

        /// <summary>
        /// Returns an array of <see cref="ITypeSymbol" /> objects that represent the type arguments of a generic method or the type parameters of a generic method definition.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<ITypeSymbol> GetGenericArguments();

        /// <summary>
        /// When overridden in a derived class, gets the parameters of the specified method or constructor.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<IParameterSymbol> GetParameters();

        /// <summary>
        /// When overridden in a derived class, returns the MethodImplAttributes flags.
        /// </summary>
        /// <returns></returns>
        MethodImplAttributes GetMethodImplementationFlags();

    }

}