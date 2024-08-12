/*
  Copyright (C) 2002-2014 Jeroen Frijters

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
using System;
using System.Collections.Generic;

using IKVM.Runtime;

namespace IKVM.Attributes
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class ConstantPoolAttribute : Attribute
    {

        internal static object[] Decompress(object[] constantPool)
        {
            var list = new List<object>();

            foreach (var obj in constantPool)
            {
                int emptySlots = obj as byte? ?? obj as ushort? ?? 0;
                if (emptySlots == 0)
                {
                    list.Add(Unescape(obj));
                }
                else
                {
                    for (int i = 0; i < emptySlots; i++)
                        list.Add(null);
                }
            }

            return list.ToArray();
        }

        static object Unescape(object obj)
        {
            if (obj is string str)
                obj = UnicodeUtil.UnescapeInvalidSurrogates(str);

            return obj;
        }

        internal static object[] Compress(object[] constantPool, bool[] inUse)
        {
            int length = constantPool.Length;
            while (!inUse[length - 1])
                length--;

            int write = 0;
            for (int read = 0; read < length; read++)
            {
                int start = read;
                while (!inUse[read])
                    read++;

                int emptySlots = read - start;
                if (emptySlots > 255)
                {
                    constantPool[write++] = (ushort)emptySlots;
                }
                else if (emptySlots > 0)
                {
                    constantPool[write++] = (byte)emptySlots;
                }

                constantPool[write++] = Escape(constantPool[read]);
            }

            Array.Resize(ref constantPool, write);
            return constantPool;
        }

        static object Escape(object obj)
        {
            if (obj is string str)
                obj = UnicodeUtil.EscapeInvalidSurrogates(str);

            return obj;
        }

        internal readonly object[] constantPool;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="constantPool"></param>
        public ConstantPoolAttribute(object[] constantPool)
        {
            this.constantPool = Decompress(constantPool);
        }

    }

}
