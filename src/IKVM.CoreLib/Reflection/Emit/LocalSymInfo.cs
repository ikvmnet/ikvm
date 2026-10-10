using System;
using System.Diagnostics.SymbolStore;

namespace IKVM.Reflection.Emit
{

    internal sealed class LocalSymInfo
    {

        // This class tracks the local variable's debugging information
        // and namespace information with a given active lexical scope.

        #region Internal Data Members
        string[] _name = null!;  // All these arrys initialized in helper method
        byte[][] _signature = null!;
        int[] _localSlot = null!;
        int[] _startOffset = null!;
        int[] _endOffset = null!;
        int _localSymCount;         // how many entries in the arrays are occupied
        string[] _namespace = null!;
        int _nameSpaceCount;
        internal const int InitialSize = 16;
        #endregion

        #region Constructor
        internal LocalSymInfo()
        {
            // initialize data variables
            _localSymCount = 0;
            _nameSpaceCount = 0;
        }

#endregion
#region Private Members

        private void EnsureCapacity()
        {
            if (_localSymCount == 0)
            {
                // First time. Allocate the arrays.
                _name = new string[InitialSize];
                _signature = new byte[InitialSize][];
                _localSlot = new int[InitialSize];
                _startOffset = new int[InitialSize];
                _endOffset = new int[InitialSize];
            }
            else if (_localSymCount == _name.Length)
            {
                // the arrays are full. Enlarge the arrays
                // why aren't we just using lists here?
                int newSize = checked(_localSymCount * 2);
                int[] temp = new int[newSize];
                Array.Copy(_localSlot, temp, _localSymCount);
                _localSlot = temp;

                temp = new int[newSize];
                Array.Copy(_startOffset, temp, _localSymCount);
                _startOffset = temp;

                temp = new int[newSize];
                Array.Copy(_endOffset, temp, _localSymCount);
                _endOffset = temp;

                string[] strTemp = new string[newSize];
                Array.Copy(_name, strTemp, _localSymCount);
                _name = strTemp;

                byte[][] ubTemp = new byte[newSize][];
                Array.Copy(_signature, ubTemp, _localSymCount);
                _signature = ubTemp;
            }
        }

        #endregion

        #region Internal Members
        internal void AddLocalSymInfo(string strName, byte[] signature, int slot, int startOffset, int endOffset)
        {
            // make sure that arrays are large enough to hold addition info
            EnsureCapacity();
            _startOffset[_localSymCount] = startOffset;
            _endOffset[_localSymCount] = endOffset;
            _localSlot[_localSymCount] = slot;
            _name[_localSymCount] = strName;
            _signature[_localSymCount] = signature;
            checked { _localSymCount++; }
        }

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
