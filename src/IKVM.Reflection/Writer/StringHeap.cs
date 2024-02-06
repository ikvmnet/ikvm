/*
  Copyright (C) 2008 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System.Collections.Generic;
using System.Diagnostics;

namespace IKVM.Reflection.Writer
{

    sealed class StringHeap : SimpleHeap
    {

        readonly List<string> list = new List<string>();
        readonly Dictionary<string, int> strings = new Dictionary<string, int>();
        int nextOffset;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal StringHeap()
        {
            Add("");
        }

        /// <summary>
        /// Adds a new string to the heap.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal int Add(string str)
        {
            Debug.Assert(frozen == false);

            if (strings.TryGetValue(str, out var offset) == false)
            {
                offset = nextOffset;
                nextOffset += System.Text.Encoding.UTF8.GetByteCount(str) + 1;
                list.Add(str);
                strings.Add(str, offset);
            }

            return offset;
        }

        internal string Find(int index)
        {
            foreach (var kv in strings)
                if (kv.Value == index)
                    return kv.Key;

            return null;
        }

        protected override int GetLength()
        {
            return nextOffset;
        }

        protected override void WriteImpl(MetadataWriter mw)
        {
            foreach (var str in list)
            {
                mw.Write(System.Text.Encoding.UTF8.GetBytes(str));
                mw.Write((byte)0);
            }
        }

    }

}
