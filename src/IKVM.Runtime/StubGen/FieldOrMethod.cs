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
using System.Collections.Generic;

using IKVM.Attributes;

namespace IKVM.StubGen
{
    sealed class FieldOrMethod : IAttributeOwner
	{
		private Modifiers access_flags;
		private ushort name_index;
		private ushort descriptor_index;
		private List<ClassFileAttribute> attribs = new List<ClassFileAttribute>();

		public FieldOrMethod(Modifiers access_flags, ushort name_index, ushort descriptor_index)
		{
			this.access_flags = access_flags;
			this.name_index = name_index;
			this.descriptor_index = descriptor_index;
		}

		public void AddAttribute(ClassFileAttribute attrib)
		{
			attribs.Add(attrib);
		}

		public void Write(BigEndianStream bes)
		{
			bes.WriteUInt16((ushort)access_flags);
			bes.WriteUInt16(name_index);
			bes.WriteUInt16(descriptor_index);
			bes.WriteUInt16((ushort)attribs.Count);
			for (int i = 0; i < attribs.Count; i++)
			{
				attribs[i].Write(bes);
			}
		}
	}
}
