using System.Collections.Immutable;
using System.Reflection;

using IKVM.CoreLib.Text;

namespace IKVM.CoreLib.Symbols
{

    public abstract class MethodSymbol : MemberSymbol
    {

        readonly TypeSymbol? _declaringType;

        MethodSpecTable _specTable;
        ImmutableArray<TypeSymbol> _parameterTypes;
        ImmutableArray<ImmutableArray<TypeSymbol>> _parameterRequiredCustomModifiers;
        ImmutableArray<ImmutableArray<TypeSymbol>> _parameterOptionalCustomModifiers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        protected MethodSymbol(SymbolContext context, ModuleSymbol module, TypeSymbol? declaringType) :
            base(context, module)
        {
            _declaringType = declaringType;
            _specTable = new MethodSpecTable(context, module, declaringType, this, []);
        }

        /// <inheritdoc />
        public sealed override TypeSymbol? DeclaringType => _declaringType;

        /// <inheritdoc />
        public sealed override MemberTypes MemberType => Name is ".ctor" or "..ctor" ? MemberTypes.Constructor : MemberTypes.Method;

        /// <summary>
        /// Gets the attributes associated with this method.
        /// </summary>
        public abstract MethodAttributes Attributes { get; }

        /// <summary>
        /// Gets a value indicating the calling conventions for this method.
        /// </summary>
        public abstract CallingConventions CallingConvention { get; }

        /// <inheritdoc />
        public bool ContainsGenericParameters => ComputeContainsGenericParameters();

        /// <summary>
        /// Computes the value for <see cref="ContainsGenericParameters"/>.
        /// </summary>
        /// <returns></returns>
        bool ComputeContainsGenericParameters()
        {
            if (DeclaringType is { ContainsGenericParameters: true })
                return true;

            foreach (var type in GenericArguments)
                if (type.ContainsGenericParameters)
                    return true;

            return false;
        }

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
        public bool IsConstructor => (Attributes & MethodAttributes.RTSpecialName) == MethodAttributes.RTSpecialName && (Attributes & MethodAttributes.SpecialName) == MethodAttributes.SpecialName && (Name == ConstructorInfo.ConstructorName || Name == ConstructorInfo.TypeConstructorName);

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
        /// Gets a value indicating whether this method requires a new slot.
        /// </summary>
        public bool IsNewSlot => (Attributes & MethodAttributes.NewSlot) != 0;

        /// <summary>
        /// Gets a value indicating whether the method is generic.
        /// </summary>
        public bool IsGenericMethod => IsGenericMethodDefinition || IsConstructedGenericMethod;

        /// <summary>
        /// Gets a value indicating whether the method is a generic method definition.
        /// </summary>
        public abstract bool IsGenericMethodDefinition { get; }

        /// <summary>
        /// Gets a value indicating whether the method is a constructed method.
        /// </summary>
        public abstract bool IsConstructedGenericMethod { get; }

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
        /// Gets the method signature of the method.
        /// </summary>
        public MethodSymbolSignature Signature => new MethodSymbolSignature(
            ReturnType,
            ReturnParameter.GetRequiredCustomModifiers(),
            ReturnParameter.GetOptionalCustomModifiers(),
            ComputeParameterTypes(),
            GetParameterRequiredCustomModifiers(),
            GetParameterOptionalCustomModifiers(),
            CallingConvention,
            GenericArguments.Length);

        /// <summary>
        /// Gets the <see cref="MethodImplAttributes" /> flags that specify the attributes of a method implementation.
        /// </summary>
        public abstract MethodImplAttributes MethodImplementationFlags { get; }

        /// <summary>
        /// Returns an array of <see cref="TypeSymbol" /> objects that represent the type arguments of a generic method or the type parameters of a generic method definition.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeSymbol> GenericArguments { get; }

        /// <summary>
        /// When overridden in a derived class, gets the parameters of the specified method or constructor.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<ParameterSymbol> Parameters { get; }

        /// <summary>
        /// When overridden in a derived class, gets the parameters of the specified method or constructor.
        /// </summary>
        /// <returns></returns>
        public ImmutableArray<TypeSymbol> ParameterTypes => ComputeParameterTypes();

        /// <summary>
        /// Gets the types of the parameters.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> ComputeParameterTypes()
        {
            if (_parameterTypes.IsDefault)
            {
                var l = Parameters;
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                for (int i = 0; i < l.Length; i++)
                    b.Add(l[i].ParameterType);

                ImmutableInterlocked.InterlockedInitialize(ref _parameterTypes, b.DrainToImmutable());
            }

            return _parameterTypes;
        }

        /// <summary>
        /// Gets the required custom modifiers applied to the parameters.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<ImmutableArray<TypeSymbol>> GetParameterRequiredCustomModifiers()
        {
            if (_parameterRequiredCustomModifiers.IsDefault)
            {
                var l = Parameters;
                var b = ImmutableArray.CreateBuilder<ImmutableArray<TypeSymbol>>(l.Length);
                for (int i = 0; i < l.Length; i++)
                    b.Add(l[i].GetRequiredCustomModifiers());

                ImmutableInterlocked.InterlockedInitialize(ref _parameterRequiredCustomModifiers, b.DrainToImmutable());
            }

            return _parameterRequiredCustomModifiers;
        }

        /// <summary>
        /// Gets the optional custom modifiers applied to the parameters.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<ImmutableArray<TypeSymbol>> GetParameterOptionalCustomModifiers()
        {
            if (_parameterOptionalCustomModifiers.IsDefault)
            {
                var l = Parameters;
                var b = ImmutableArray.CreateBuilder<ImmutableArray<TypeSymbol>>(l.Length);
                for (int i = 0; i < l.Length; i++)
                    b.Add(l[i].GetOptionalCustomModifiers());

                ImmutableInterlocked.InterlockedInitialize(ref _parameterOptionalCustomModifiers, b.DrainToImmutable());
            }

            return _parameterOptionalCustomModifiers;
        }

        /// <summary>
        /// Invoke when the parameter types may have changed to regenerate the parameter type cache.
        /// </summary>
        protected void OnParametersChanged()
        {
            _parameterTypes = default;
            _parameterRequiredCustomModifiers = default;
            _parameterOptionalCustomModifiers = default;
        }

        /// <summary>
        /// Gets a <see cref="ParameterSymbol"/> object that contains information about the return type of the method, such as whether the return type has custom modifiers.
        /// </summary>
        public abstract ParameterSymbol ReturnParameter { get; }

        /// <summary>
        /// Gets the return type of this method.
        /// </summary>
        public abstract TypeSymbol ReturnType { get; }

        /// <summary>
        /// Gets the custom attributes for the return type.
        /// </summary>
        public abstract ICustomAttributeProvider ReturnTypeCustomAttributes { get; }

        /// <summary>
        /// When overridden in a derived class, returns the <see cref="MethodSymbol"/> object for the method on the direct or indirect base class in which the method represented by this instance was first declared.
        /// </summary>
        /// <returns></returns>
        public abstract MethodSymbol? BaseDefinition { get; }

        /// <summary>
        /// Returns a <see cref="MethodSymbol"/> object that represents a generic method definition from which the current method can be constructed.
        /// </summary>
        /// <returns></returns>
        public abstract MethodSymbol? GenericMethodDefinition { get; }

        /// <summary>
        /// Substitutes the elements of an array of types for the type parameters of the current generic method definition, and returns a MethodInfo object representing the resulting constructed method.
        /// </summary>
        /// <param name="typeArguments"></param>
        /// <returns></returns>
        public MethodSymbol MakeGenericMethod(ImmutableArray<TypeSymbol> typeArguments) => _specTable.GetOrCreateGenericMethodSymbol(typeArguments);

        /// <inheritdoc />
        public override string? ToString()
        {
            using var sb = new ValueStringBuilder(stackalloc char[256]);
            sb.Append(ReturnType.Name);
            sb.Append(' ');
            sb.Append(Name);

            string sep;
            if (IsGenericMethod)
            {
                sb.Append('[');
                sep = "";

                foreach (var arg in GenericArguments)
                {
                    sb.Append(sep);
                    sb.Append(arg.ToString());
                    sep = ", ";
                }

                sb.Append(']');
            }

            sb.Append('(');
            sep = "";

            foreach (var arg in Parameters)
            {
                sb.Append(sep);
                sb.Append(arg.ParameterType.ToString());
                sep = ", ";
            }

            sb.Append(')');
            return sb.ToString();
        }

    }

}
