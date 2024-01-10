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
using System;
using System.Diagnostics;

namespace IKVM.Reflection.Writer
{

    abstract class Heap
	{

		protected bool frozen;
		protected int unalignedlength;

		internal void Write(MetadataWriter mw)
		{
			var pos = mw.Position;
			WriteImpl(mw);

			Debug.Assert(mw.Position == pos + unalignedlength);
			var align = Length - unalignedlength;
			for (int i = 0; i < align; i++)
				mw.Write((byte)0);
		}

        internal bool IsBig => Length > 65535;

        internal int Length
		{
			get
			{
				if (!frozen)
					throw new InvalidOperationException();

				return (unalignedlength + 3) & ~3;
			}
		}

		protected abstract void WriteImpl(MetadataWriter mw);

	}

}
