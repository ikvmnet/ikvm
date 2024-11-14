using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Emit
{

    class MethodSymbolBuilder : MethodSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        public MethodSymbolBuilder(SymbolContext context, TypeSymbolBuilder declaringType) :
            base(context, declaringType.Module, declaringType)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringModule"></param>
        public MethodSymbolBuilder(SymbolContext context, ModuleSymbolBuilder declaringModule) :
            base(context, declaringModule, null)
        {

        }

        /// <inheritdoc />
        public override ParameterSymbol ReturnParameter => throw new NotImplementedException();

        /// <inheritdoc />
        public override TypeSymbol ReturnType => throw new NotImplementedException();

        /// <inheritdoc />
        public override ICustomAttributeProvider ReturnTypeCustomAttributes => throw new NotImplementedException();

        /// <inheritdoc />
        public override MethodAttributes Attributes => throw new NotImplementedException();

        /// <inheritdoc />
        public override CallingConventions CallingConvention => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool ContainsGenericParameters => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsGenericMethod => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsGenericMethodDefinition => throw new NotImplementedException();

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
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            throw new NotImplementedException();
        }

    }

}
