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

namespace IKVM.StubGen
{
    sealed class MethodParametersAttribute : ClassFileAttribute
	{
		private readonly ClassFileWriter classFile;
		private readonly ushort[] names;
		private readonly ushort[] flags;

		internal MethodParametersAttribute(ClassFileWriter classFile, ushort[] names, ushort[] flags)
			: base(classFile.AddUtf8("MethodParameters"))
		{
			this.classFile = classFile;
			this.names = names;
			this.flags = flags;
		}

		public override void Write(BigEndianStream bes)
		{
			base.Write(bes);
			if (flags == null || names == null || flags.Length != names.Length)
			{
				// write a malformed MethodParameters attribute
				bes.WriteUInt32(0);
				return;
			}
			bes.WriteUInt32((uint)(1 + names.Length * 4));
			bes.WriteByte((byte)names.Length);
			for (int i = 0; i < names.Length; i++)
			{
				bes.WriteUInt16(names[i]);
				bes.WriteUInt16(flags[i]);
			}
		}
	}
}
