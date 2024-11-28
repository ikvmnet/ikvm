using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class SyntheticMethodSymbol : MethodSymbol
    {

        readonly TypeSymbol? _declaringType;
        readonly string _name;
        readonly MethodAttributes _attributes;
        readonly CallingConventions _callingConventions;
        readonly ParameterSymbol _returnParameter;
        readonly ImmutableArray<ParameterSymbol> _parameters;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConventions"></param>
        /// <param name="returnParameter"></param>
        /// <param name="parameters"></param>
        public SyntheticMethodSymbol(SymbolContext context, ModuleSymbol module, TypeSymbol? declaringType, string name, MethodAttributes attributes, CallingConventions callingConventions, ParameterSymbol? returnParameter, ImmutableArray<ParameterSymbol> parameters) :
            base(context, module, declaringType)
        {
            _declaringType = declaringType;
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _attributes = attributes;
            _callingConventions = callingConventions;
            _returnParameter = returnParameter ?? new SyntheticParameterSymbol(context, this, null, ParameterAttributes.Retval, context.ResolveCoreType("System.Void"), -1);
            _parameters = parameters;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConventions"></param>
        /// <param name="returnType"></param>
        /// <param name="parameters"></param>
        public SyntheticMethodSymbol(SymbolContext context, ModuleSymbol module, TypeSymbol? declaringType, string name, MethodAttributes attributes, CallingConventions callingConventions, TypeSymbol? returnType, ImmutableArray<TypeSymbol> parameters) :
            base(context, module, declaringType)
        {
            _declaringType = declaringType;
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _attributes = attributes;
            _callingConventions = callingConventions;
            _returnParameter = new SyntheticParameterSymbol(context, this, null, ParameterAttributes.Retval, returnType ?? context.ResolveCoreType("System.Void"), -1);
            _parameters = parameters.Select((i, j) => new SyntheticParameterSymbol(context, this, null, ParameterAttributes.None, i, j)).ToImmutableArray<ParameterSymbol>();
        }

        /// <inheritdoc />
        public sealed override ParameterSymbol ReturnParameter => _returnParameter;

        /// <inheritdoc />
        public sealed override TypeSymbol ReturnType => ReturnParameter.ParameterType;

        /// <inheritdoc />
        public sealed override ICustomAttributeProvider ReturnTypeCustomAttributes => throw new NotImplementedException();

        /// <inheritdoc />
        public sealed override MethodAttributes Attributes => _attributes;

        /// <inheritdoc />
        public sealed override CallingConventions CallingConvention => _callingConventions;

        /// <inheritdoc />
        public sealed override bool IsGenericMethodDefinition => false;

        /// <inheritdoc />
        public sealed override bool IsConstructedGenericMethod => false;

        /// <inheritdoc />
        public sealed override MethodImplAttributes MethodImplementationFlags => MethodImplAttributes.Managed;

        /// <inheritdoc />
        public sealed override string Name => _name;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool IsComplete => true;

        /// <inheritdoc />
        public sealed override MethodSymbol? BaseDefinition => null;

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericArguments => [];

        /// <inheritdoc />
        public sealed override MethodSymbol? GenericMethodDefinition => null;

        /// <inheritdoc />
        public sealed override ImmutableArray<ParameterSymbol> Parameters => _parameters;

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            throw new NotImplementedException();
        }

    }

}
