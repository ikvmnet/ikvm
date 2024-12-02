using System;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a type definition.
    /// </summary>
    abstract class DefinitionTypeSymbol : TypeSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        protected DefinitionTypeSymbol(SymbolContext context, ModuleSymbol module) :
            base(context, module)
        {

        }

        /// <inheritdoc />
        public sealed override MethodSymbol? DeclaringMethod => null;

        /// <inheritdoc />
        public sealed override bool IsTypeDefinition => true;

        /// <inheritdoc />
        public sealed override bool IsArray => false;

        /// <inheritdoc />
        public sealed override bool IsByRef => false;

        /// <inheritdoc />
        public sealed override bool IsConstructedGenericType => false;

        /// <inheritdoc />
        public sealed override bool IsFunctionPointer => false;

        /// <inheritdoc />
        public sealed override int GenericParameterPosition => throw new InvalidOperationException();

        /// <inheritdoc />
        public sealed override bool HasElementType => false;

        /// <inheritdoc />
        public sealed override bool IsGenericTypeParameter => false;

        /// <inheritdoc />
        public sealed override bool IsGenericMethodParameter => false;

        /// <inheritdoc />
        public sealed override bool IsPointer => false;

        /// <inheritdoc />
        public sealed override bool IsSZArray => false;

        /// <inheritdoc />
        public sealed override bool IsUnmanagedFunctionPointer => false;

        /// <inheritdoc />
        public sealed override TypeSymbol? GetElementType()
        {
            return null;
        }

        /// <inheritdoc />
        public sealed override TypeSymbol GenericTypeDefinition => throw new InvalidOperationException();

        /// <inheritdoc />
        public sealed override int GetArrayRank()
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        internal override TypeSymbol Specialize(GenericContext context)
        {
            if (ContainsGenericParameters == false)
                return this;

            var args = GenericArguments;
            for (int i = 0; i < args.Length; i++)
                if (args[i].ContainsGenericParameters)
                    args = args.SetItem(i, args[i].Specialize(context));

            return MakeGenericType(args);
        }

    }

}
