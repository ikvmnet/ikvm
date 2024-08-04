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

using IKVM.ByteCode.Reading;

namespace IKVM.Runtime
{

    sealed partial class ClassFile
    {

        sealed class ConstantPoolItemNameAndType : ConstantPoolItem
        {

            internal ushort nameIndex;
            internal ushort descriptorIndex;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="reader"></param>
            internal ConstantPoolItemNameAndType(RuntimeContext context, NameAndTypeConstantReader reader) :
                base(context)
            {
                nameIndex = reader.Record.Name.Index;
                descriptorIndex = reader.Record.Descriptor.Index;
            }

            internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
            {
                if (classFile.GetConstantPoolUtf8String(utf8_cp, nameIndex) == null || classFile.GetConstantPoolUtf8String(utf8_cp, descriptorIndex) == null)
                    throw new ClassFormatError("Illegal constant pool index");
            }

        }

    }

}
