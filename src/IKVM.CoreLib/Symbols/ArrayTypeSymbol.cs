using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class ArrayTypeSymbol : HasElementSymbol
    {

        readonly int _rank;

        string? _nameSuffix;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="elementType"></param>
        public ArrayTypeSymbol(ISymbolContext context, TypeSymbol elementType, int rank) :
            base(context, elementType)
        {
            _rank = rank;
        }

        /// <inheritdoc />
        protected override string NameSuffix => _nameSuffix ??= ComputeNameSuffix();

        /// <summary>
        /// Computes the value for <see cref="NameSuffix"/>.
        /// </summary>
        /// <returns></returns>
        string ComputeNameSuffix()
        {
            if (_rank == 1)
                return "[*]";
            else
                return "[" + new string(',', _rank - 1) + "]";
        }

        /// <inheritdoc />
        public sealed override TypeAttributes Attributes => TypeAttributes.AutoLayout | TypeAttributes.AnsiClass | TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Serializable;

        /// <inheritdoc />
        public sealed override bool IsArray => true;

        /// <inheritdoc />
        public sealed override TypeSymbol? BaseType => Context.ResolveCoreType("System.Array");

        /// <inheritdoc />
        public sealed override int GetArrayRank()
        {
            return _rank;
        }

    }

}
