using System;
using System.Diagnostics.SymbolStore;

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Tracks the debugging information of the local variables, and the namespaces in use, within one lexical scope of
    /// a method body, and emits it to a symbol writer.
    /// </summary>
    sealed class LocalSymInfo
    {

        #region Internal Data Members

        // the per-local arrays are allocated and grown by EnsureCapacity
        string[] _name = null!;
        byte[][] _signature = null!;
        int[] _localSlot = null!;
        int[] _startOffset = null!;
        int[] _endOffset = null!;
        int _localSymCount; // how many entries in the arrays are occupied
        readonly string[] _namespace = null!;
        readonly int _nameSpaceCount;

        /// <summary>
        /// The initial capacity of the per-local arrays.
        /// </summary>
        internal const int InitialSize = 16;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal LocalSymInfo()
        {
            _localSymCount = 0;
            _nameSpaceCount = 0;
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Makes sure the per-local arrays have room for one more entry, allocating them on first use and doubling them
        /// when full.
        /// </summary>
        void EnsureCapacity()
        {
            if (_localSymCount == 0)
            {
                // first time, allocate the arrays
                _name = new string[InitialSize];
                _signature = new byte[InitialSize][];
                _localSlot = new int[InitialSize];
                _startOffset = new int[InitialSize];
                _endOffset = new int[InitialSize];
            }
            else if (_localSymCount == _name.Length)
            {
                // the arrays are full, so double their size
                var newSize = checked(_localSymCount * 2);
                var temp = new int[newSize];
                Array.Copy(_localSlot, temp, _localSymCount);
                _localSlot = temp;

                temp = new int[newSize];
                Array.Copy(_startOffset, temp, _localSymCount);
                _startOffset = temp;

                temp = new int[newSize];
                Array.Copy(_endOffset, temp, _localSymCount);
                _endOffset = temp;

                var strTemp = new string[newSize];
                Array.Copy(_name, strTemp, _localSymCount);
                _name = strTemp;

                var ubTemp = new byte[newSize][];
                Array.Copy(_signature, ubTemp, _localSymCount);
                _signature = ubTemp;
            }
        }

        #endregion

        #region Internal Members

        /// <summary>
        /// Records the debugging information of a local variable.
        /// </summary>
        /// <param name="strName">The name of the local variable.</param>
        /// <param name="signature">The signature of the local variable.</param>
        /// <param name="slot">The index of the local variable in the method's local variable signature.</param>
        /// <param name="startOffset">The IL offset at which the variable's scope begins.</param>
        /// <param name="endOffset">The IL offset at which the variable's scope ends.</param>
        internal void AddLocalSymInfo(string strName, byte[] signature, int slot, int startOffset, int endOffset)
        {
            // make sure that arrays are large enough to hold the additional info
            EnsureCapacity();
            _startOffset[_localSymCount] = startOffset;
            _endOffset[_localSymCount] = endOffset;
            _localSlot[_localSymCount] = slot;
            _name[_localSymCount] = strName;
            _signature[_localSymCount] = signature;
            checked
            { _localSymCount++; }
        }

        /// <summary>
        /// Writes the recorded local variables and namespaces to the symbol writer.
        /// </summary>
        /// <param name="symWriter">The symbol writer to write to.</param>
        internal void EmitLocalSymInfo(ISymbolWriter symWriter)
        {
            int i;

            for (i = 0; i < _localSymCount; i++)
            {
                symWriter.DefineLocalVariable(
                            _name[i],
                            System.Reflection.FieldAttributes.PrivateScope,
                            _signature[i],
                            SymAddressKind.ILOffset,
                            _localSlot[i],
                            0,          // addr2 is not used yet
                            0,          // addr3 is not used
                            _startOffset[i],
                            _endOffset[i]);
            }
            for (i = 0; i < _nameSpaceCount; i++)
            {
                symWriter.UsingNamespace(_namespace[i]);
            }
        }

        #endregion

    }

}
