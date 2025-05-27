using System;
using System.Collections.Immutable;
using System.Reflection;

using IKVM.CoreLib.Symbols.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class DefinitionMethodSymbol : MethodSymbol
    {

        readonly IMethodLoader _loader;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loader"></param>
        public DefinitionMethodSymbol(SymbolContext context, IMethodLoader loader) :
            base(context)
        {
            _loader = loader ?? throw new ArgumentNullException(nameof(loader));
        }

        /// <summary>
        /// Gets the associated loader.
        /// </summary>
        public IMethodLoader Loader => _loader;

        /// <inheritdoc />
        public override ModuleSymbol Module => _loader.GetModule();

        /// <inheritdoc />
        public override TypeSymbol? DeclaringType => _loader.GetDeclaringType();

        /// <inheritdoc />
        public sealed override bool IsMissing => _loader.GetIsMissing();

        /// <inheritdoc />
        public sealed override string Name => _loader.GetName();

        /// <inheritdoc />
        public sealed override MethodAttributes Attributes => _loader.GetAttributes();

        /// <inheritdoc />
        public sealed override CallingConventions CallingConvention => _loader.GetCallingConvention();

        /// <inheritdoc />
        public sealed override bool IsGenericMethodDefinition => GenericParameters.IsEmpty == false;

        /// <inheritdoc />
        public sealed override bool IsConstructedGenericMethod => false;

        /// <inheritdoc />
        public sealed override MethodImplAttributes MethodImplementationFlags => _loader.GetMethodImplementationFlags();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericParameters => _loader.GetGenericArguments();

        /// <inheritdoc />
        public sealed override ImmutableArray<ParameterSymbol> Parameters => _loader.GetParameters();

        /// <inheritdoc />
        public sealed override ParameterSymbol ReturnParameter => _loader.GetReturnParameter();

        /// <inheritdoc />
        public sealed override MethodSymbol? BaseDefinition => throw new NotImplementedException();

        /// <inheritdoc />
        public sealed override MethodSymbol? GenericMethodDefinition => IsGenericMethodDefinition ? this : throw new InvalidOperationException();

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => _loader.GetCustomAttributes();

    }

}
