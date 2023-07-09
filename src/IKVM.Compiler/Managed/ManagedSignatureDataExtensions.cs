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
            switch (self.length)
            {
                case 0:
                    throw new ManagedTypeException("Invalid signature data.");
                case 1:
                    return ref self.local0;
                case 2:
                    return ref self.local1;
                case 3:
                    return ref self.local2;
                case 4:
                    return ref self.local3;
                default:
                    var m = self.memory[self.memory.Count - 1].Span;
                    return ref m[m.Length - 1];
            }
        }

        /// <summary>
        /// Gets the code in the data structure at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static ref readonly ManagedSignatureCode GetCode(this in ManagedSignatureData self, int index)
        {
            if (index >= self.length)
                throw new IndexOutOfRangeException();

            switch (index)
            {
                case 0:
                    return ref self.local0;
                case 1:
                    return ref self.local1;
                case 2:
                    return ref self.local2;
                case 3:
                    return ref self.local3;
                default:
                    var p = 4;
                    for (int i = 0; i < self.memory.Count; i++)
                    {
                        var m = self.memory[i];
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

        /// <summary>
        /// Extracts a data structure for the code at the specified index.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static ManagedSignatureData GetCodeData(this in ManagedSignatureData self, int index)
        {
            int length = 0;
            ManagedSignatureCode local0;
            ManagedSignatureCode local1;
            ManagedSignatureCode local2;
            ManagedSignatureCode local3;
            FixedValueList4<Memory<ManagedSignatureCode>> memory;

            // get code at index
            var c = GetCode(self, index);

            // get starting position by navigating backwards
            var p = index;

            // follow code to earliest reference: this will be the starting position of the copy
            while (true)
            {
                var o = c.Arg0;
                for (int i = 0; i < c.Argv.Count; i++)
                    o = Math.Min(o, c.Argv[i]);

                // code has no args
                if (o == 0)
                    break;

                // caculate new offset, resolve code at position
                p = p + o;
                c = GetCode(self, p);
            }

            throw new NotImplementedException();
        }

    }

}
