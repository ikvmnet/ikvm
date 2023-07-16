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

            readonly MethodHandleConstantReader reader;
            ConstantPoolItemFMI cpi;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="reader"></param>
            internal ConstantPoolItemMethodHandle(MethodHandleConstantReader reader)
            {
                this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
            }

            internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
            {
                switch (reader.ReferenceKind)
                {
                    case ReferenceKind.GetField:
                    case ReferenceKind.GetStatic:
                    case ReferenceKind.PutField:
                    case ReferenceKind.PutStatic:
                        cpi = classFile.GetConstantPoolItem(reader.Record.Index) as ConstantPoolItemFieldref;
                        break;
                    case ReferenceKind.InvokeSpecial:
                    case ReferenceKind.InvokeVirtual:
                    case ReferenceKind.InvokeStatic:
                    case ReferenceKind.NewInvokeSpecial:
                        cpi = classFile.GetConstantPoolItem(reader.Record.Index) as ConstantPoolItemMethodref;
                        if (cpi == null && classFile.MajorVersion >= 52 && (reader.ReferenceKind is ReferenceKind.InvokeStatic or ReferenceKind.InvokeSpecial))
                            goto case ReferenceKind.InvokeInterface;
                        break;
                    case ReferenceKind.InvokeInterface:
                        cpi = classFile.GetConstantPoolItem(reader.Record.Index) as ConstantPoolItemInterfaceMethodref;
                        break;
                }

                if (cpi == null)
                    throw new ClassFormatError("Invalid constant pool item MethodHandle");

                if (ReferenceEquals(cpi.Name, StringConstants.INIT) && reader.ReferenceKind != ReferenceKind.NewInvokeSpecial)
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

            internal ReferenceKind Kind
            {
                get { return reader.ReferenceKind; }
            }

            internal MemberWrapper Member
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
