using System;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Extension methods for working with <see cref="ManagedSignatureData"/> instances.
    /// </summary>
    internal static class ManagedSignatureDataExtensions
    {

        /// <summary>
        /// Gets the last code in the data structure.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ManagedTypeException"></exception>
        public static ref readonly ManagedSignatureCode GetLastCode(this in ManagedSignatureData self)
        {
            switch (self.Length)
            {
                case 0:
                    throw new ManagedTypeException("Invalid signature data.");
                case 1:
                    return ref self.Local0;
                case 2:
                    return ref self.Local1;
                case 3:
                    return ref self.Local2;
                case 4:
                    return ref self.Local3;
                default:
                    var m = self.Memory[self.Memory.Count - 1].Span;
                    return ref m[m.Length - 1];
            }
        }

        /// <summary>
        /// Gets the code in the data structure at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static ref readonly ManagedSignatureCode GetCodeRef(this in ManagedSignatureData self, int index)
        {
            if (index >= self.Length)
                throw new IndexOutOfRangeException();

            switch (index)
            {
                case 0:
                    return ref self.Local0;
                case 1:
                    return ref self.Local1;
                case 2:
                    return ref self.Local2;
                case 3:
                    return ref self.Local3;
                default:
                    var p = 4;
                    for (int i = 0; i < self.Memory.Count; i++)
                    {
                        var m = self.Memory[i];
                        if (m.Length < index - p)
                            return ref m.Span[index - p];
                        else
                            p += m.Length;
                    }
                    break;
            }

            // should not happen due to length check
            throw new InvalidOperationException();
        }

    }

}
