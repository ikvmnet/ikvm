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
using IKVM.ByteCode;

namespace IKVM.Runtime
{

    sealed partial class ClassFile
    {

        internal abstract class ConstantPoolItemFMI : ConstantPoolItem
        {

            readonly ClassConstantHandle _clazzHandle;
            readonly NameAndTypeConstantHandle _nameAndType;

            ConstantPoolItemClass _clazz;
            string _name;
            string _descriptor;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="clazz"></param>
            /// <param name="nameAndType"></param>
            internal ConstantPoolItemFMI(RuntimeContext context, ClassConstantHandle clazz, NameAndTypeConstantHandle nameAndType) :
                base(context)
            {
                this._clazzHandle = clazz;
                this._nameAndType = nameAndType;
            }

            internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
            {
                var name_and_type = (ConstantPoolItemNameAndType)classFile.GetConstantPoolItem(_nameAndType);

                _clazz = (ConstantPoolItemClass)classFile.GetConstantPoolItem(_clazzHandle);
                // if the constant pool items referred to were strings, GetConstantPoolItem returns null
                if (name_and_type == null || _clazz == null)
                    throw new ClassFormatError("Bad index in constant pool");

                _name = string.Intern(classFile.GetConstantPoolUtf8String(utf8_cp, name_and_type.NameHandle));
                _descriptor = classFile.GetConstantPoolUtf8String(utf8_cp, name_and_type.DescriptorHandle);
                Validate(_name, _descriptor, classFile.MajorVersion);
                _descriptor = string.Intern(_descriptor.Replace('/', '.'));
            }

            protected abstract void Validate(string name, string descriptor, int majorVersion);

            internal override void MarkLinkRequired()
            {
                _clazz.MarkLinkRequired();
            }

            internal override void Link(RuntimeJavaType thisType, LoadMode mode)
            {
                _clazz.Link(thisType, mode);
            }

            internal string Name => _name;

            internal string Signature => _descriptor;

            internal string Class => _clazz.Name;

            internal RuntimeJavaType GetClassType() => _clazz.GetClassType();

            internal abstract RuntimeJavaMember GetMember();

        }

    }

}
