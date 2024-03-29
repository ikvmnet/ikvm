﻿/*
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
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace IKVM.Reflection.Writer
{

    sealed class GuidHeap : SimpleHeap
    {

        readonly List<Guid> list = new List<Guid>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal GuidHeap()
        {

        }

        internal int Add(Guid guid)
        {
            Debug.Assert(!frozen);
            list.Add(guid);
            return list.Count;
        }

        protected override int GetLength()
        {
            return list.Count * 16;
        }

        protected override void WriteImpl(MetadataWriter mw)
        {
            foreach (var guid in list)
                mw.Write(guid.ToByteArray());
        }

    }

}
