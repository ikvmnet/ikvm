using System;

namespace IKVM.CoreLib.Symbols.Reflection
{

    abstract class ReflectionTypeSpecSymbol : ReflectionTypeSymbolBase
    {

        readonly IReflectionTypeSymbol _elementType;

        ReflectionMethodTable _methodTable;
        ReflectionFieldTable _fieldTable;
        ReflectionPropertyTable _propertyTable;
        ReflectionEventTable _eventTable;
        ReflectionGenericTypeParameterTable _genericTypeParameterTable;
        ReflectionTypeSpecTable _specTable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="elementType"></param>
        public ReflectionTypeSpecSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionTypeSymbol elementType) :
            base(context, resolvingModule)
        {
            _elementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
        }

        /// <summary>
        /// Gets the element type.
        /// </summary>
        protected IReflectionTypeSymbol ElementType => _elementType;

    }

}
