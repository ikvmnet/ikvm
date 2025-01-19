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
using IKVM.ByteCode.Decoding;

namespace IKVM.Runtime
{

    sealed partial class ClassFile
    {

        internal sealed class ConstantPoolItemInvokeDynamic : ConstantPoolItem
        {

            readonly ushort _bootstrapMethodAttributeIndex;
            readonly NameAndTypeConstantHandle _nameAndTypeHandle;

            string _name;
            string _descriptor;
            RuntimeJavaType[] _argTypes;
            RuntimeJavaType _returnType;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="data"></param>
            internal ConstantPoolItemInvokeDynamic(RuntimeContext context, InvokeDynamicConstantData data) :
                base(context)
            {
                _bootstrapMethodAttributeIndex = data.BootstrapMethodAttributeIndex;
                _nameAndTypeHandle = data.NameAndType;
            }

            internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
            {
                var nameAndType = (ConstantPoolItemNameAndType)classFile.GetConstantPoolItem(_nameAndTypeHandle);
                if (nameAndType == null)
                    throw new ClassFormatError("Bad index in constant pool");

                _name = string.Intern(classFile.GetConstantPoolUtf8String(utf8_cp, nameAndType.NameHandle));
                _descriptor = string.Intern(classFile.GetConstantPoolUtf8String(utf8_cp, nameAndType.DescriptorHandle).Replace('/', '.'));
            }

            internal override void Link(RuntimeJavaType thisType, LoadMode mode)
            {
                lock (this)
                    if (_argTypes != null)
                        return;

                var classLoader = thisType.ClassLoader;
                var args = classLoader.ArgJavaTypeListFromSig(_descriptor, mode);
                var ret = classLoader.RetTypeWrapperFromSig(_descriptor, mode);

                lock (this)
                {
                    if (_argTypes == null)
                    {
                        _argTypes = args;
                        _returnType = ret;
                    }
                }
            }

            internal RuntimeJavaType[] GetArgTypes()
            {
                return _argTypes;
            }

            internal RuntimeJavaType GetRetType()
            {
                return _returnType;
            }

            internal string Name => _name;

            internal string Signature => _descriptor;

            internal ushort BootstrapMethod => _bootstrapMethodAttributeIndex;

        }

    }

}
