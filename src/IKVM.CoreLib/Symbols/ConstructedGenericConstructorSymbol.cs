using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a constructor of a <see cref="ConstructedGenericTypeSymbol"/>.
    /// </summary>
    class ConstructedGenericConstructorSymbol : ConstructorSymbol
    {

        readonly ConstructorSymbol _definition;
        readonly GenericContext _genericContext;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="definition"></param>
        /// <param name="genericContext"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstructedGenericConstructorSymbol(SymbolContext context, TypeSymbol declaringType, ConstructorSymbol definition, GenericContext genericContext) :
            base(context, declaringType)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public override MethodAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public override CallingConventions CallingConvention => _definition.CallingConvention;

        /// <inheritdoc />
        public override MethodImplAttributes MethodImplementationFlags => _definition.MethodImplementationFlags;

        /// <inheritdoc />
        public override string Name => _definition.Name;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool ContainsMissing => false;

        /// <inheritdoc />
        public sealed override bool IsComplete => true;

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetGenericArguments()
        {
            return _definition.GetGenericArguments();
        }

        /// <inheritdoc />
        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return _definition.GetMethodImplementationFlags();
        }

        /// <inheritdoc />
        public override ImmutableArray<ParameterSymbol> GetParameters()
        {
            return _definition.GetParameters();
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            throw new NotImplementedException();
        }

    }

}