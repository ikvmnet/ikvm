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

    sealed class CodeAttribute : ClassFileAttribute
	{

		private ClassFileWriter classFile;
		private ushort max_stack;
		private ushort max_locals;
		private byte[] code;

		public CodeAttribute(ClassFileWriter classFile)
			: base(classFile.AddUtf8("Code"))
		{
			this.classFile = classFile;
		}

		public ushort MaxStack
		{
			get { return max_stack; }
			set { max_stack = value; }
		}

		public ushort MaxLocals
		{
			get { return max_locals; }
			set { max_locals = value; }
		}

		public byte[] ByteCode
		{
			get { return code; }
			set { code = value; }
		}

		public override void Write(BigEndianStream bes)
		{
			base.Write(bes);
			bes.WriteUInt32((uint)(2 + 2 + 4 + code.Length + 2 + 2));
			bes.WriteUInt16(max_stack);
			bes.WriteUInt16(max_locals);
			bes.WriteUInt32((uint)code.Length);
			for (int i = 0; i < code.Length; i++)
			{
				bes.WriteByte(code[i]);
			}
			bes.WriteUInt16(0);	// no exceptions
			bes.WriteUInt16(0); // no attributes
		}

	}

}
