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
        public override ParameterSymbol ReturnParameter => _returnParameter;

        /// <inheritdoc />
        public override TypeSymbol ReturnType => ReturnParameter.ParameterType;

        /// <inheritdoc />
        public override ICustomAttributeProvider ReturnTypeCustomAttributes => throw new NotImplementedException();

        /// <inheritdoc />
        public override MethodAttributes Attributes => _attributes;

        /// <inheritdoc />
        public override CallingConventions CallingConvention => _callingConventions;

        /// <inheritdoc />
        public override bool ContainsGenericParameters => false;

        /// <inheritdoc />
        public override bool IsGenericMethod => false;

        /// <inheritdoc />
        public override bool IsGenericMethodDefinition => false;

        /// <inheritdoc />
        public override MethodImplAttributes MethodImplementationFlags => MethodImplAttributes.Managed;

        /// <inheritdoc />
        public override string Name => _name;

        /// <inheritdoc />
        public override bool IsMissing => false;

        /// <inheritdoc />
        public override bool ContainsMissing => false;

        /// <inheritdoc />
        public override bool IsComplete => true;

        /// <inheritdoc />
        public override MethodSymbol GetBaseDefinition()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetGenericArguments()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override MethodSymbol GetGenericMethodDefinition()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ImmutableArray<ParameterSymbol> GetParameters()
        {
            return _parameters;
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            throw new NotImplementedException();
        }

    }

}
