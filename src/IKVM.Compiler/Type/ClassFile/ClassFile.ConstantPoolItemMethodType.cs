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
using IKVM.Compiler.Type.ClassFile.Test;

namespace IKVM.Compiler.Type.ClassFile
{

    sealed partial class ClassFile
    {

        internal sealed class ConstantPoolItemMethodType : ConstantPoolItem
        {

            readonly ushort signature_index;

            string descriptor;
            JavaType[] argTypeWrappers;
            JavaType retTypeWrapper;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="reader"></param>
            internal ConstantPoolItemMethodType(MethodTypeConstantReader reader)
            {
                signature_index = reader.Record.DescriptorIndex;
            }

            internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
            {
                var descriptor = classFile.GetConstantPoolUtf8String(utf8_cp, signature_index);
                if (descriptor == null || !IsValidMethodSig(descriptor))
                    throw new ClassFormatError("Invalid MethodType signature");

                this.descriptor = string.Intern(descriptor.Replace('/', '.'));
            }

            internal override void Link(JavaClassType thisType, LoadMode mode)
            {
                lock (this)
                {
                    if (argTypeWrappers != null)
                    {
                        return;
                    }
                }

                var classLoader = thisType.GetClassLoader();
                var args = classLoader.ArgTypeWrapperListFromSig(descriptor, mode);
                var ret = classLoader.RetTypeWrapperFromSig(descriptor, mode);

                lock (this)
                {
                    if (argTypeWrappers == null)
                    {
                        argTypeWrappers = args;
                        retTypeWrapper = ret;
                    }
                }
            }

            internal string Signature
            {
                get { return descriptor; }
            }

            internal JavaType[] GetArgTypes()
            {
                return argTypeWrappers;
            }

            internal JavaType GetRetType()
            {
                return retTypeWrapper;
            }

            internal override ConstantType GetConstantType()
            {
                return ConstantType.MethodType;
            }

        }

    }

}
