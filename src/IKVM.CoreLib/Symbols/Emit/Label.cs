namespace IKVM.CoreLib.Symbols.Emit
{

    public readonly struct Label
    {

        readonly int _index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="index"></param>
        public Label(int index)
        {
            _index = index;
        }

        /// <summary>
        /// Gets the label index.
        /// </summary>
        internal int Index => _index;

    }

}