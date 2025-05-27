using System;

namespace IKVM.CoreLib.Symbols.Emit
{

    /// <summary>
    /// Represents a local variable within a method or constructor.
    /// </summary>
    public class LocalBuilder
    {

        readonly TypeSymbol _localType;
        readonly bool _pinned;
        readonly int _index;
        string? _name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="localType"></param>
        /// <param name="pinned"></param>
        /// <param name="index"></param>
        internal LocalBuilder(TypeSymbol localType, bool pinned, int index)
        {
            _localType = localType ?? throw new ArgumentNullException(nameof(localType));
            _pinned = pinned;
            _index = index;
        }

        /// <summary>
        /// Gets the type of the local variable.
        /// </summary>
        public TypeSymbol LocalType => _localType;

        /// <summary>
        /// Gets the zero-based index of the local variable within the method body.
        /// </summary>
        public int LocalIndex => _index;

        /// <summary>
        /// Gets a value indicating whether the object referred to by the local variable is pinned in memory.
        /// </summary>
        public bool IsPinned => _pinned;

        /// <summary>
        /// Sets the name of this local variable.
        /// </summary>
        /// <param name="name"></param>
        public void SetLocalSymInfo(string name)
        {
            _name = name;
        }

    }

}