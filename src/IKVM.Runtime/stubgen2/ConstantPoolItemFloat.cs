/*
  Copyright (C) 2002-2013 Jeroen Frijters

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

namespace IKVM.StubGen
{
    sealed class ConstantPoolItemFloat : ConstantPoolItem
	{
		private float v;

		public ConstantPoolItemFloat(float v)
		{
			this.v = v;
		}

		public override int GetHashCode()
		{
			return BitConverter.ToInt32(BitConverter.GetBytes(v), 0);
		}

		public override bool Equals(object o)
		{
			if (o != null && o.GetType() == typeof(ConstantPoolItemFloat))
			{
				return ((ConstantPoolItemFloat)o).v == v;
			}
			return false;
		}

		public override void Write(BigEndianStream bes)
		{
			bes.WriteByte((byte)Constant.Float);
			bes.WriteFloat(v);
		}
	}
}
