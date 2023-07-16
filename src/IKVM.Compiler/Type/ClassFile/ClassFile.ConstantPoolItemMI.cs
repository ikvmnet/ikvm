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

namespace IKVM.Compiler.Type.ClassFile
{

    sealed partial class ClassFile
    {

        internal class ConstantPoolItemMI : ConstantPoolItemFMI
        {

            JavaType[] argTypeWrappers;
            JavaType retTypeWrapper;
            protected JavaMethod method;
            protected JavaMethod invokespecialMethod;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="classIndex"></param>
            /// <param name="nameAndTypeIndex"></param>
            internal ConstantPoolItemMI(ushort classIndex, ushort nameAndTypeIndex) : base(classIndex, nameAndTypeIndex)
            {

            }

            protected override void Validate(string name, string descriptor, int majorVersion)
            {
                if (!IsValidMethodSig(descriptor))
                {
                    throw new ClassFormatError("Method {0} has invalid signature {1}", name, descriptor);
                }
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

            internal override void Link(JavaClassType thisType, LoadMode mode)
            {
                base.Link(thisType, mode);
                lock (this)
                {
                    if (argTypeWrappers != null)
                    {
                        return;
                    }
                }
                ClassLoaderWrapper classLoader = thisType.GetClassLoader();
                JavaType[] args = classLoader.ArgTypeWrapperListFromSig(this.Signature, mode);
                JavaType ret = classLoader.RetTypeWrapperFromSig(this.Signature, mode);
                lock (this)
                {
                    if (argTypeWrappers == null)
                    {
                        argTypeWrappers = args;
                        retTypeWrapper = ret;
                    }
                }
            }

            internal JavaType[] GetArgTypes()
            {
                return argTypeWrappers;
            }

            internal JavaType GetRetType()
            {
                return retTypeWrapper;
            }

            internal JavaMethod GetMethod()
            {
                return method;
            }

            internal JavaMethod GetMethodForInvokespecial()
            {
                return invokespecialMethod != null ? invokespecialMethod : method;
            }

            internal override MemberWrapper GetMember()
            {
                return method;
            }
        }
    }

}
