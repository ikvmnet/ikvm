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
using IKVM.ByteCode.Reading;

namespace IKVM.Runtime
{

    sealed partial class ClassFile
    {

        internal sealed class ConstantPoolItemMethodHandle : ConstantPoolItem
        {

            readonly MethodHandleConstantData data;
            ConstantPoolItemFMI cpi;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="data"></param>
            internal ConstantPoolItemMethodHandle(RuntimeContext context, MethodHandleConstantData data) :
                base(context)
            {
                this.data = data;
            }

            internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
            {
                switch (data.Kind)
                {
                    case MethodHandleKind.GetField:
                    case MethodHandleKind.GetStatic:
                    case MethodHandleKind.PutField:
                    case MethodHandleKind.PutStatic:
                        cpi = classFile.GetConstantPoolItem(data.Reference) as ConstantPoolItemFieldref;
                        break;
                    case MethodHandleKind.InvokeSpecial:
                    case MethodHandleKind.InvokeVirtual:
                    case MethodHandleKind.InvokeStatic:
                    case MethodHandleKind.NewInvokeSpecial:
                        cpi = classFile.GetConstantPoolItem(data.Reference) as ConstantPoolItemMethodref;
                        if (cpi == null && classFile.MajorVersion >= 52 && (data.Kind is MethodHandleKind.InvokeStatic or MethodHandleKind.InvokeSpecial))
                            goto case MethodHandleKind.InvokeInterface;
                        break;
                    case MethodHandleKind.InvokeInterface:
                        cpi = classFile.GetConstantPoolItem(data.Reference) as ConstantPoolItemInterfaceMethodref;
                        break;
                }

                if (cpi == null)
                    throw new ClassFormatError("Invalid constant pool item MethodHandle");

                if (ReferenceEquals(cpi.Name, StringConstants.INIT) && data.Kind != MethodHandleKind.NewInvokeSpecial)
                    throw new ClassFormatError("Bad method name");
            }

            internal override void MarkLinkRequired()
            {
                cpi.MarkLinkRequired();
            }

            internal string Class
            {
                get { return cpi.Class; }
            }

            internal string Name
            {
                get { return cpi.Name; }
            }

            internal string Signature
            {
                get { return cpi.Signature; }
            }

            internal ConstantPoolItemFMI MemberConstantPoolItem
            {
                get { return cpi; }
            }

            internal MethodHandleKind Kind
            {
                get { return data.Kind; }
            }

            internal RuntimeJavaMember Member
            {
                get { return cpi.GetMember(); }
            }

            internal RuntimeJavaType GetClassType()
            {
                return cpi.GetClassType();
            }

            internal override void Link(RuntimeJavaType thisType, LoadMode mode)
            {
                cpi.Link(thisType, mode);
            }

            internal override ConstantType GetConstantType()
            {
                return ConstantType.MethodHandle;
            }

        }

    }

}
