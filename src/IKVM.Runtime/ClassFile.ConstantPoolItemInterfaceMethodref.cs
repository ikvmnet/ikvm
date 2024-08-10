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

        internal sealed class ConstantPoolItemInterfaceMethodref : ConstantPoolItemMI
        {

            /// <summary>
            /// initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="data"></param>
            internal ConstantPoolItemInterfaceMethodref(RuntimeContext context, InterfaceMethodrefConstantData data) :
                base(context, data.Class, data.NameAndType)
            {

            }

            internal override void Link(RuntimeJavaType thisType, LoadMode mode)
            {
                base.Link(thisType, mode);
                RuntimeJavaType wrapper = GetClassType();
                if (wrapper != null)
                {
                    if (!wrapper.IsUnloadable)
                    {
                        method = wrapper.GetInterfaceMethod(Name, Signature);
                    }
                    if (method == null)
                    {
                        // NOTE vmspec 5.4.3.4 clearly states that an interfacemethod may also refer to a method in Object
                        method = thisType.Context.JavaBase.TypeOfJavaLangObject.GetMethodWrapper(Name, Signature, false);
                    }
                    if (method != null)
                    {
                        method.Link(mode);
                    }
                }
            }

        }

    }

}
