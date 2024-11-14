using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Emit
{

    class ConstructorSymbolBuilder : ConstructorSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        public ConstructorSymbolBuilder(SymbolContext context, TypeSymbolBuilder declaringType) :
            base(context, declaringType)
        {

        }

        /// <inheritdoc />
        public override MethodAttributes Attributes => throw new NotImplementedException();

        /// <inheritdoc />
        public override CallingConventions CallingConvention => throw new NotImplementedException();

        /// <inheritdoc />
        public override MethodImplAttributes MethodImplementationFlags => throw new NotImplementedException();

        /// <inheritdoc />
        public override string Name => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsMissing => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool ContainsMissing => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsComplete => throw new NotImplementedException();

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
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            throw new NotImplementedException();
        }

    }

}