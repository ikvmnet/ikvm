using System;
using System.Diagnostics.SymbolStore;

namespace IKVM.Reflection.Emit
{

    internal sealed class LocalSymInfo
    {

        // This class tracks the local variable's debugging information
        // and namespace information with a given active lexical scope.

        #region Internal Data Members
        internal string[] m_strName = null!;  // All these arrys initialized in helper method
        internal byte[][] m_ubSignature = null!;
        internal int[] m_iLocalSlot = null!;
        internal int[] m_iStartOffset = null!;
        internal int[] m_iEndOffset = null!;
        internal int m_iLocalSymCount;         // how many entries in the arrays are occupied
        internal string[] m_namespace = null!;
        internal int m_iNameSpaceCount;
        internal const int InitialSize = 16;
        #endregion

        #region Constructor
        internal LocalSymInfo()
        {
            // initialize data variables
            m_iLocalSymCount = 0;
            m_iNameSpaceCount = 0;
        }
        #endregion

        #region Private Members
        private void EnsureCapacityNamespace()
        {
            if (m_iNameSpaceCount == 0)
            {
                m_namespace = new string[InitialSize];
            }
            else if (m_iNameSpaceCount == m_namespace.Length)
            {
                string[] strTemp = new string[checked(m_iNameSpaceCount * 2)];
                Array.Copy(m_namespace, strTemp, m_iNameSpaceCount);
                m_namespace = strTemp;
            }
        }

        private void EnsureCapacity()
        {
            if (m_iLocalSymCount == 0)
            {
                // First time. Allocate the arrays.
                m_strName = new string[InitialSize];
                m_ubSignature = new byte[InitialSize][];
                m_iLocalSlot = new int[InitialSize];
                m_iStartOffset = new int[InitialSize];
                m_iEndOffset = new int[InitialSize];
            }
            else if (m_iLocalSymCount == m_strName.Length)
            {
                // the arrays are full. Enlarge the arrays
                // why aren't we just using lists here?
                int newSize = checked(m_iLocalSymCount * 2);
                int[] temp = new int[newSize];
                Array.Copy(m_iLocalSlot, temp, m_iLocalSymCount);
                m_iLocalSlot = temp;

                temp = new int[newSize];
                Array.Copy(m_iStartOffset, temp, m_iLocalSymCount);
                m_iStartOffset = temp;

                temp = new int[newSize];
                Array.Copy(m_iEndOffset, temp, m_iLocalSymCount);
                m_iEndOffset = temp;

                string[] strTemp = new string[newSize];
                Array.Copy(m_strName, strTemp, m_iLocalSymCount);
                m_strName = strTemp;

                byte[][] ubTemp = new byte[newSize][];
                Array.Copy(m_ubSignature, ubTemp, m_iLocalSymCount);
                m_ubSignature = ubTemp;
            }
        }

        #endregion

        #region Internal Members
        internal void AddLocalSymInfo(string strName, byte[] signature, int slot, int startOffset, int endOffset)
        {
            // make sure that arrays are large enough to hold addition info
            EnsureCapacity();
            m_iStartOffset[m_iLocalSymCount] = startOffset;
            m_iEndOffset[m_iLocalSymCount] = endOffset;
            m_iLocalSlot[m_iLocalSymCount] = slot;
            m_strName[m_iLocalSymCount] = strName;
            m_ubSignature[m_iLocalSymCount] = signature;
            checked { m_iLocalSymCount++; }
        }

        internal void AddUsingNamespace(string strNamespace)
        {
            EnsureCapacityNamespace();
            m_namespace[m_iNameSpaceCount] = strNamespace;
            checked { m_iNameSpaceCount++; }
        }
        internal void EmitLocalSymInfo(ISymbolWriter symWriter)
        {
            int i;

            for (i = 0; i < m_iLocalSymCount; i++)
            {
                symWriter.DefineLocalVariable(
                            m_strName[i],
                            System.Reflection.FieldAttributes.PrivateScope,
                            m_ubSignature[i],
                            SymAddressKind.ILOffset,
                            m_iLocalSlot[i],
                            0,          // addr2 is not used yet
                            0,          // addr3 is not used
                            m_iStartOffset[i],
                            m_iEndOffset[i]);
            }
            for (i = 0; i < m_iNameSpaceCount; i++)
            {
                symWriter.UsingNamespace(m_namespace[i]);
            }
        }

        #endregion

    }

}
