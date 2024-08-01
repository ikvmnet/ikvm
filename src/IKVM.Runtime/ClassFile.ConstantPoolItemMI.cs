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

        internal class ConstantPoolItemMI : ConstantPoolItemFMI
        {

            RuntimeJavaType[] argTypeWrappers;
            RuntimeJavaType retTypeWrapper;
            protected RuntimeJavaMethod method;
            protected RuntimeJavaMethod invokespecialMethod;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="clazz"></param>
            /// <param name="nameAndType"></param>
            internal ConstantPoolItemMI(RuntimeContext context, ClassConstantHandle clazz, NameAndTypeConstantHandle nameAndType) :
                base(context, clazz, nameAndType)
            {

            }

            protected override void Validate(string name, string descriptor, ClassFormatVersion majorVersion)
            {
                if (!IsValidMethodSig(descriptor))
                    throw new ClassFormatError("Method {0} has invalid signature {1}", name, descriptor);

                if (!IsValidMethodName(name, majorVersion))
                {
                    if (!ReferenceEquals(name, StringConstants.INIT))
                    {
                        throw new ClassFormatError("Invalid method name \"{0}\"", name);
                    }
                    if (!descriptor.EndsWith("V"))
                    {
                        throw new ClassFormatError("Method {0} has invalid signature {1}", name, descriptor);
                    }
                }
            }

            internal override void Link(RuntimeJavaType thisType, LoadMode mode)
            {
                base.Link(thisType, mode);
                lock (this)
                {
                    if (argTypeWrappers != null)
                    {
                        return;
                    }
                }
                RuntimeClassLoader classLoader = thisType.GetClassLoader();
                RuntimeJavaType[] args = classLoader.ArgJavaTypeListFromSig(this.Signature, mode);
                RuntimeJavaType ret = classLoader.RetTypeWrapperFromSig(this.Signature, mode);
                lock (this)
                {
                    if (argTypeWrappers == null)
                    {
                        argTypeWrappers = args;
                        retTypeWrapper = ret;
                    }
                }
            }

            internal RuntimeJavaType[] GetArgTypes()
            {
                return argTypeWrappers;
            }

            internal RuntimeJavaType GetRetType()
            {
                return retTypeWrapper;
            }

            internal RuntimeJavaMethod GetMethod()
            {
                return method;
            }

            internal RuntimeJavaMethod GetMethodForInvokespecial()
            {
                return invokespecialMethod != null ? invokespecialMethod : method;
            }

            internal override RuntimeJavaMember GetMember()
            {
                return method;
            }
        }
    }

}
