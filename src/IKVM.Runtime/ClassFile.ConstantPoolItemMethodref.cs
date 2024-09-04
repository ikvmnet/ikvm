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
using IKVM.Attributes;
using IKVM.ByteCode.Decoding;
using IKVM.Runtime;

namespace IKVM.Runtime
{

    sealed partial class ClassFile
    {

        internal sealed class ConstantPoolItemMethodref : ConstantPoolItemMI
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="data"></param>
            internal ConstantPoolItemMethodref(RuntimeContext context, MethodrefConstantData data) :
                base(context, data.Class, data.NameAndType)
            {

            }

            internal override void Link(RuntimeJavaType thisJavaType, LoadMode mode)
            {
                base.Link(thisJavaType, mode);

                var javaType = GetClassType();
                if (javaType != null && javaType.IsUnloadable == false)
                {
                    method = javaType.GetMethodWrapper(Name, Signature, !ReferenceEquals(Name, StringConstants.INIT));
                    method?.Link(mode);

                    if (Name != StringConstants.INIT &&
                        thisJavaType.IsInterface == false &&
                        (JVM.AllowNonVirtualCalls == false || (thisJavaType.Modifiers & Modifiers.Super) == Modifiers.Super) &&
                        thisJavaType != javaType &&
                        thisJavaType.IsSubTypeOf(javaType))
                    {
                        invokespecialMethod = thisJavaType.BaseTypeWrapper.GetMethodWrapper(Name, Signature, true);
                        invokespecialMethod?.Link(mode);
                    }
                }
            }

        }

    }

}
