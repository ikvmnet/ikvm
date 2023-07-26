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

namespace IKVM.StubGen
{

    sealed class InnerClassesAttribute : ClassFileAttribute
	{

		private ClassFileWriter classFile;
		private List<Item> classes = new List<Item>();

		public InnerClassesAttribute(ClassFileWriter classFile)
			: base(classFile.AddUtf8("InnerClasses"))
		{
			this.classFile = classFile;
		}

		public override void Write(BigEndianStream bes)
		{
			base.Write(bes);
			bes.WriteUInt32((uint)(2 + 8 * classes.Count));
			bes.WriteUInt16((ushort)classes.Count);
			foreach (Item i in classes)
			{
				bes.WriteUInt16(i.inner_class_info_index);
				bes.WriteUInt16(i.outer_class_info_index);
				bes.WriteUInt16(i.inner_name_index);
				bes.WriteUInt16(i.inner_class_access_flags);
			}
		}

		private class Item
		{
			internal ushort inner_class_info_index;
			internal ushort outer_class_info_index;
			internal ushort inner_name_index;
			internal ushort inner_class_access_flags;
		}

		public void Add(string inner, string outer, string name, ushort access)
		{
			Item i = new Item();
			i.inner_class_info_index = classFile.AddClass(inner);
			i.outer_class_info_index = classFile.AddClass(outer);
			if (name != null)
			{
				i.inner_name_index = classFile.AddUtf8(name);
			}
			i.inner_class_access_flags = access;
			classes.Add(i);
		}

	}

}
