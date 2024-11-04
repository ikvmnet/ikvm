using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    abstract class GenericParameterTypeSymbol : TypeSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        protected GenericParameterTypeSymbol(ISymbolContext context, IModuleSymbol module, TypeSymbol? declaringType) : 
            base(context, module, declaringType)
        {

        }

        /// <inheritdoc />
        public sealed override TypeAttributes Attributes => TypeAttributes.Public;

        /// <inheritdoc />
        public override MethodBaseSymbol? DeclaringMethod => null;

        /// <inheritdoc />
        public sealed override bool IsGenericTypeDefinition => false;

        /// <inheritdoc />
        public sealed override bool HasElementType => false;

        /// <inheritdoc />
        public sealed override bool IsArray => false;

        /// <inheritdoc />
        public sealed override bool IsSZArray => false;

        /// <inheritdoc />
        public sealed override bool IsByRef => false;

        /// <inheritdoc />
        public sealed override bool IsPointer => false;

        /// <inheritdoc />
        public sealed override bool IsFunctionPointer => false;

        /// <inheritdoc />
        public sealed override bool IsUnmanagedFunctionPointer => false;

        /// <inheritdoc />
        public sealed override bool IsConstructedGenericType => false;

        /// <inheritdoc />
        public sealed override bool IsGenericParameter => true;

        /// <inheritdoc />
        public sealed override bool ContainsGenericParameters => true;

        /// <inheritdoc />
        public sealed override TypeCode TypeCode => TypeCode.Object;

        /// <inheritdoc />
        public sealed override TypeSymbol? GetElementType()
        {
            return null;
        }

        /// <inheritdoc />
        public sealed override int GetArrayRank()
        {
            throw new NotSupportedException();
        }

    }

}
