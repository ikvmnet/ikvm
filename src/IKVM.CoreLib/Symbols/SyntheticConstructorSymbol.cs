using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class SyntheticConstructorSymbol : ConstructorSymbol
    {

        readonly TypeSymbol? _declaringType;
        readonly MethodAttributes _attributes;
        readonly CallingConventions _callingConventions;
        readonly ImmutableArray<ParameterSymbol> _parameters;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConventions"></param>
        /// <param name="parameters"></param>
        public SyntheticConstructorSymbol(SymbolContext context, ModuleSymbol module, TypeSymbol declaringType, MethodAttributes attributes, CallingConventions callingConventions, ImmutableArray<ParameterSymbol> parameters) :
            base(context, declaringType)
        {
            _declaringType = declaringType;
            _attributes = attributes;
            _callingConventions = callingConventions;
            _parameters = parameters;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConventions"></param>
        /// <param name="parameters"></param>
        public SyntheticConstructorSymbol(SymbolContext context, ModuleSymbol module, TypeSymbol declaringType, MethodAttributes attributes, CallingConventions callingConventions, ImmutableArray<TypeSymbol> parameters) :
            base(context, declaringType)
        {
            _declaringType = declaringType;
            _attributes = attributes;
            _callingConventions = callingConventions;
            _parameters = parameters.Select((i, j) => new SyntheticParameterSymbol(context, this, null, ParameterAttributes.None, i, j)).ToImmutableArray<ParameterSymbol>();
        }

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
        public override string Name => IsStatic ? ConstructorInfo.TypeConstructorName : ConstructorInfo.ConstructorName;

        /// <inheritdoc />
        public override bool IsMissing => false;

        /// <inheritdoc />
        public override bool ContainsMissing => false;

        /// <inheritdoc />
        public override bool IsComplete => true;

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetGenericArguments()
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
