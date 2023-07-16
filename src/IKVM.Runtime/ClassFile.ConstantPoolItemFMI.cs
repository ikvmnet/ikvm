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
using System;

namespace IKVM.Runtime
{

    sealed partial class ClassFile
    {

        internal abstract class ConstantPoolItemFMI : ConstantPoolItem
        {

            readonly ushort class_index;
            readonly ushort name_and_type_index;

            ConstantPoolItemClass clazz;
            string name;
            string descriptor;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="classIndex"></param>
            /// <param name="nameAndTypeIndex"></param>
            internal ConstantPoolItemFMI(ushort classIndex, ushort nameAndTypeIndex)
            {
                class_index = classIndex;
                name_and_type_index = nameAndTypeIndex;
            }

            internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
            {
                ConstantPoolItemNameAndType name_and_type = (ConstantPoolItemNameAndType)classFile.GetConstantPoolItem(name_and_type_index);
                clazz = (ConstantPoolItemClass)classFile.GetConstantPoolItem(class_index);
                // if the constant pool items referred to were strings, GetConstantPoolItem returns null
                if (name_and_type == null || clazz == null)
                {
                    throw new ClassFormatError("Bad index in constant pool");
                }
                name = String.Intern(classFile.GetConstantPoolUtf8String(utf8_cp, name_and_type.nameIndex));
                descriptor = classFile.GetConstantPoolUtf8String(utf8_cp, name_and_type.descriptorIndex);
                Validate(name, descriptor, classFile.MajorVersion);
                descriptor = String.Intern(descriptor.Replace('/', '.'));
            }

            protected abstract void Validate(string name, string descriptor, int majorVersion);

            internal override void MarkLinkRequired()
            {
                clazz.MarkLinkRequired();
            }

            internal override void Link(RuntimeJavaType thisType, LoadMode mode)
            {
                clazz.Link(thisType, mode);
            }

            internal string Name
            {
                get
                {
                    return name;
                }
            }

            internal string Signature
            {
                get
                {
                    return descriptor;
                }
            }

            internal string Class
            {
                get
                {
                    return clazz.Name;
                }
            }

            internal RuntimeJavaType GetClassType()
            {
                return clazz.GetClassType();
            }

            internal abstract MemberWrapper GetMember();
        }
    }

}
