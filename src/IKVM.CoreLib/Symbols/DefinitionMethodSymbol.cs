using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols
{

    class DefinitionMethodSymbol : MethodSymbol
    {

        readonly string _name;
        readonly DefinitionModuleSymbol _moduleDef;
        readonly DefinitionTypeSymbol? _declaringTypeDef;
        MethodDefinition? _def;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="moduleDef"></param>
        /// <param name="declaringTypeDef"></param>
        /// <param name="def"></param>
        public DefinitionMethodSymbol(SymbolContext context, DefinitionModuleSymbol moduleDef, DefinitionTypeSymbol? declaringTypeDef, string name, MethodDefinition? def) :
            base(context, moduleDef, declaringTypeDef)
        {
            _moduleDef = moduleDef ?? throw new ArgumentNullException(nameof(moduleDef));
            _declaringTypeDef = declaringTypeDef;
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _def = def;
        }

        /// <summary>
        /// Gets the underlying source information. If the type source is missing, <c>null</c> is returned.
        /// </summary>
        MethodDefinition? Def => GetDef();

        /// <summary>
        /// Attempts to resolve the symbol definition source.
        /// </summary>
        /// <returns></returns>
        MethodDefinition? GetDef()
        {
            if (_def is null)
                if (_declaringTypeDef is { } dt)
                    Interlocked.CompareExchange(ref _def, dt.ResolveMethodDef(_name), null);
                else
                    Interlocked.CompareExchange(ref _def, _moduleDef.ResolveMethodDef(_name), null);

            return _def;
        }

        /// <summary>
        /// Attempts to resolve the symbol definition source, or throws.
        /// </summary>
        MethodDefinition DefOrThrow => Def ?? throw new MissingMethodSymbolException(this);

        /// <inheritdoc />
        public sealed override bool IsMissing => Def == null;

        /// <inheritdoc />
        public sealed override string Name => _name;

        /// <inheritdoc />
        public sealed override MethodAttributes Attributes => DefOrThrow.GetAttributes();

        /// <inheritdoc />
        public sealed override CallingConventions CallingConvention => DefOrThrow.GetCallingConvention();

        /// <inheritdoc />
        public sealed override bool IsGenericMethodDefinition => GenericArguments.IsEmpty == false;

        /// <inheritdoc />
        public sealed override bool IsConstructedGenericMethod => false;

        /// <inheritdoc />
        public sealed override MethodImplAttributes MethodImplementationFlags => DefOrThrow.GetMethodImplementationFlags();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericArguments => DefOrThrow.GetGenericArguments();

        /// <inheritdoc />
        public sealed override ImmutableArray<ParameterSymbol> Parameters => DefOrThrow.GetParameters();

        /// <inheritdoc />
        public sealed override ParameterSymbol ReturnParameter => DefOrThrow.GetReturnParameter();

        /// <inheritdoc />
        public sealed override MethodSymbol? BaseDefinition => throw new System.NotImplementedException();

        /// <inheritdoc />
        public sealed override MethodSymbol? GenericMethodDefinition => IsGenericMethodDefinition ? this : throw new InvalidOperationException();

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => DefOrThrow.GetCustomAttributes();

    }

}
