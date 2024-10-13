using System;
using System.Diagnostics.CodeAnalysis;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionTypeSymbol : ReflectionTypeSymbolBase
    {

        readonly Type _type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="type"></param>
        public ReflectionTypeSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, Type type) :
            base(context, resolvingModule)
        {
            _type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <inheritdoc />
        public override Type UnderlyingType => _type;

        /// <inheritdoc />
        public override Type UnderlyingRuntimeType => _type;

        #region IReflectionSymbol

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(type))]
        public override IReflectionTypeSymbol? ResolveTypeSymbol(Type? type)
        {
            if (type == _type)
                return this;
            else
                return base.ResolveTypeSymbol(type);
        }

        #endregion

    }

}
