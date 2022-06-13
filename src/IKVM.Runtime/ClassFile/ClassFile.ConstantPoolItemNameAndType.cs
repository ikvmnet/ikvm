/*
  Copyright (C) 2002-2015 Jeroen Frijters

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

namespace IKVM.Internal
{

    sealed partial class ClassFile
	{
        private sealed class ConstantPoolItemNameAndType : ConstantPoolItem
		{
			internal ushort name_index;
			internal ushort descriptor_index;

			internal ConstantPoolItemNameAndType(BigEndianBinaryReader br)
			{
				name_index = br.ReadUInt16();
				descriptor_index = br.ReadUInt16();
			}

			internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
			{
				if(classFile.GetConstantPoolUtf8String(utf8_cp, name_index) == null
					|| classFile.GetConstantPoolUtf8String(utf8_cp, descriptor_index) == null)
				{
					throw new ClassFormatError("Illegal constant pool index");
				}
			}
		}
	}

}
