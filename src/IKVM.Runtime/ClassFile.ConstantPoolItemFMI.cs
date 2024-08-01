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

using IKVM.ByteCode;

namespace IKVM.Runtime
{

    sealed partial class ClassFile
    {

        internal abstract class ConstantPoolItemFMI : ConstantPoolItem
        {

            readonly ClassConstantHandle classHandle;
            readonly NameAndTypeConstantHandle nameAndTypeHandle;

            ConstantPoolItemClass clazz;
            string name;
            string descriptor;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="clazz"></param>
            /// <param name="nameAndType"></param>
            internal ConstantPoolItemFMI(RuntimeContext context, ClassConstantHandle clazz, NameAndTypeConstantHandle nameAndType) :
                base(context)
            {
                classHandle = clazz;
                nameAndTypeHandle = nameAndType;
            }

            internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
            {
                var nameAndType = (ConstantPoolItemNameAndType)classFile.GetConstantPoolItem(nameAndTypeHandle);
                clazz = (ConstantPoolItemClass)classFile.GetConstantPoolItem(classHandle);

                // if the constant pool items referred to were strings, GetConstantPoolItem returns null
                if (nameAndType == null || clazz == null)
                    throw new ClassFormatError("Bad index in constant pool");

                name = string.Intern(classFile.GetConstantPoolUtf8String(utf8_cp, nameAndType.Name));
                descriptor = classFile.GetConstantPoolUtf8String(utf8_cp, nameAndType.Descriptor);
                Validate(name, descriptor, classFile.reader.Version);
                descriptor = string.Intern(descriptor.Replace('/', '.'));
            }

            protected abstract void Validate(string name, string descriptor, ClassFormatVersion version);

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

            internal abstract RuntimeJavaMember GetMember();
        }
    }

}
