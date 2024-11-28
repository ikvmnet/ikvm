using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace IKVM.CoreLib.Symbols.Emit
{

    public sealed class MethodSymbolBuilder : MethodSymbol, ICustomAttributeBuilder
    {

        static ImmutableArray<TypeSymbol> GetIndex(ImmutableArray<ImmutableArray<TypeSymbol>> a, int i) => a.Length > i ? a[i] : [];

        ParameterSymbolBuilder _returnParameter;
        readonly ImmutableArray<ParameterSymbolBuilder>.Builder _parameters = ImmutableArray.CreateBuilder<ParameterSymbolBuilder>();
        ImmutableArray<ParameterSymbol> _parametersCache = default;
        readonly string _name;
        readonly CallingConventions _callingConvention;
        MethodAttributes _attributes;
        MethodImplAttributes _methodImplFlags;
        ImmutableArray<GenericMethodParameterTypeSymbolBuilder> _typeParameters;
        readonly ImmutableArray<CustomAttribute>.Builder _customAttributes = ImmutableArray.CreateBuilder<CustomAttribute>();
        (bool set, string dllName, string entryName, CallingConvention nativeCallConv, CharSet nativeCharSet) _dllImportData;
        bool _initLocals = true;
        ILGenerator? _il;

        bool _frozen;
        object? _state1;
        object? _state2;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringModule"></param>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="returnRequiredCustomModifiers"></param>
        /// <param name="returnOptionalCustomModifiers"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="parameterRequiredCustomModifiers"></param>
        /// <param name="parameterOptionalCustomModifiers"></param>
        internal MethodSymbolBuilder(SymbolContext context, ModuleSymbol declaringModule, TypeSymbolBuilder? declaringType, string name, MethodAttributes attributes, CallingConventions callingConvention, TypeSymbol returnType, ImmutableArray<TypeSymbol> returnRequiredCustomModifiers, ImmutableArray<TypeSymbol> returnOptionalCustomModifiers, ImmutableArray<TypeSymbol> parameterTypes, ImmutableArray<ImmutableArray<TypeSymbol>> parameterRequiredCustomModifiers, ImmutableArray<ImmutableArray<TypeSymbol>> parameterOptionalCustomModifiers) :
            base(context, declaringModule, declaringType)
        {
            _name = name;
            _attributes = attributes;
            _callingConvention = callingConvention;
            _returnParameter = new ParameterSymbolBuilder(Context, this, returnType, -1, returnRequiredCustomModifiers, returnOptionalCustomModifiers);
            _parameters.AddRange(parameterTypes.Select((i, j) => new ParameterSymbolBuilder(Context, this, i, j, GetIndex(parameterRequiredCustomModifiers, j), GetIndex(parameterOptionalCustomModifiers, j))));
        }

        new TypeSymbolBuilder? DeclaringType => (TypeSymbolBuilder?)base.DeclaringType;

        /// <inheritdoc />
        public sealed override ParameterSymbol ReturnParameter => _returnParameter;

        /// <inheritdoc />
        public sealed override TypeSymbol ReturnType => _returnParameter.ParameterType;

        /// <inheritdoc />
        public sealed override ICustomAttributeProvider ReturnTypeCustomAttributes => _returnParameter;

        /// <inheritdoc />
        public sealed override MethodAttributes Attributes => _attributes;

        /// <inheritdoc />
        public sealed override CallingConventions CallingConvention => _callingConvention;

        /// <inheritdoc />
        public sealed override bool IsGenericMethodDefinition => _typeParameters.Length > 0;

        /// <inheritdoc />
        public sealed override bool IsConstructedGenericMethod => false;

        /// <inheritdoc />
        public sealed override MethodImplAttributes MethodImplementationFlags => _methodImplFlags;

        /// <inheritdoc />
        public sealed override string Name => _name;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool IsComplete => false;

        /// <inheritdoc />
        public sealed override MethodSymbol? BaseDefinition => null;

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericArguments => _typeParameters.CastArray<TypeSymbol>();

        /// <inheritdoc />
        public sealed override MethodSymbol? GenericMethodDefinition => null;

        /// <inheritdoc />
        public sealed override ImmutableArray<ParameterSymbol> Parameters => ComputeParameters();

        ImmutableArray<ParameterSymbol> ComputeParameters()
        {
            if (_parametersCache == default)
                ImmutableInterlocked.InterlockedInitialize(ref _parametersCache, _parameters.ToImmutable().CastArray<ParameterSymbol>());

            return _parametersCache;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return _customAttributes.ToImmutable();
        }

        /// <summary>
        /// Sets the implementation flags for this method.
        /// </summary>
        /// <param name="attributes"></param>
        public void SetImplementationFlags(MethodImplAttributes attributes)
        {
            _methodImplFlags = attributes;
        }

        /// <summary>
        /// Defines a parameter of this method.
        /// </summary>
        /// <param name="iSequence"></param>
        /// <param name="attributes"></param>
        /// <param name="strParamName"></param>
        /// <returns></returns>
        public ParameterSymbolBuilder DefineParameter(int iSequence, ParameterAttributes attributes, string? strParamName)
        {
            _parameters[iSequence - 1]._attributes = attributes;
            _parameters[iSequence - 1]._name = strParamName;
            OnParametersChanged();
            return _parameters[iSequence - 1];
        }

        /// <summary>
        /// Sets the number and types of parameters for a method.
        /// </summary>
        /// <param name="parameterTypes"></param>
        public void SetParameters(ImmutableArray<TypeSymbol> parameterTypes)
        {
            _parameters.Clear();
            _parameters.AddRange(parameterTypes.Select((i, j) => new ParameterSymbolBuilder(Context, this, i, j, [], [])));
            _parametersCache = default;
            OnParametersChanged();
        }

        /// <summary>
        /// Sets the return type of the method.
        /// </summary>
        /// <param name="returnType"></param>
        public void SetReturnType(TypeSymbol? returnType)
        {
            _returnParameter = new ParameterSymbolBuilder(Context, this, returnType ?? Context.ResolveCoreType("System.Void"), -1, [], []);
        }

        /// <summary>
        /// Sets the method signature, including the return type, the parameter types, and the required and optional custom modifiers of the return type and parameter types.
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="returnRequiredCustomModifiers"></param>
        /// <param name="returnOptionalCustomModifiers"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="parameterRequiredCustomModifiers"></param>
        /// <param name="parameterOptionalCustomModifiers"></param>
        public void SetSignature(TypeSymbol? returnType, ImmutableArray<TypeSymbol> returnRequiredCustomModifiers, ImmutableArray<TypeSymbol> returnOptionalCustomModifiers, ImmutableArray<TypeSymbol> parameterTypes, ImmutableArray<ImmutableArray<TypeSymbol>> parameterRequiredCustomModifiers, ImmutableArray<ImmutableArray<TypeSymbol>> parameterOptionalCustomModifiers)
        {
            _returnParameter = new ParameterSymbolBuilder(Context, this, returnType ?? Context.ResolveCoreType("System.Void"), -1, returnRequiredCustomModifiers, returnOptionalCustomModifiers);
            _parameters.Clear();
            _parameters.AddRange(parameterTypes.Select((i, j) => new ParameterSymbolBuilder(Context, this, i, j, GetIndex(parameterRequiredCustomModifiers, j), GetIndex(parameterOptionalCustomModifiers, j))));
            _parametersCache = default;
            OnParametersChanged();
        }

        /// <summary>
        /// Sets the number of generic type parameters for the current method, specifies their names, and returns an array of <see cref="GenericMethodParameterTypeSymbolBuilder"/> objects that can be used to define their constraints.
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public ImmutableArray<GenericMethodParameterTypeSymbolBuilder> DefineGenericParameters(params string[] names)
        {
            return _typeParameters = names.Select((i, j) => new GenericMethodParameterTypeSymbolBuilder(Context, this, i, GenericParameterAttributes.None, j)).ToImmutableArray();
        }

        /// <summary>
        /// Gets an ILGenerator for this method.
        /// </summary>
        /// <returns></returns>
        public ILGenerator GetILGenerator()
        {
            return _il ?? new ILGenerator(Context);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            _customAttributes.Add(attribute);
        }

        /// <summary>
        /// Sets the native information on the method.
        /// </summary>
        /// <param name="dllName"></param>
        /// <param name="entryName"></param>
        /// <param name="nativeCallConv"></param>
        /// <param name="nativeCharSet"></param>
        internal void SetDllImportData(string dllName, string entryName, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            _dllImportData = (true, dllName, entryName, nativeCallConv, nativeCharSet);
        }

    }

}
