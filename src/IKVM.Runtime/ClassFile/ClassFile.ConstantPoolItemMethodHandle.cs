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

namespace IKVM.Internal
{

    sealed partial class ClassFile
    {
        internal sealed class ConstantPoolItemMethodHandle : ConstantPoolItem
        {
            private byte ref_kind;
            private ushort method_index;
            private ConstantPoolItemFMI cpi;

            internal ConstantPoolItemMethodHandle(BigEndianBinaryReader br)
            {
                ref_kind = br.ReadByte();
                method_index = br.ReadUInt16();
            }

            internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
            {
                switch ((RefKind)ref_kind)
                {
                    case RefKind.getField:
                    case RefKind.getStatic:
                    case RefKind.putField:
                    case RefKind.putStatic:
                        cpi = classFile.GetConstantPoolItem(method_index) as ConstantPoolItemFieldref;
                        break;
                    case RefKind.invokeSpecial:
                    case RefKind.invokeVirtual:
                    case RefKind.invokeStatic:
                    case RefKind.newInvokeSpecial:
                        cpi = classFile.GetConstantPoolItem(method_index) as ConstantPoolItemMethodref;
                        if (cpi == null && classFile.MajorVersion >= 52 && ((RefKind)ref_kind == RefKind.invokeStatic || (RefKind)ref_kind == RefKind.invokeSpecial))
                            goto case RefKind.invokeInterface;
                        break;
                    case RefKind.invokeInterface:
                        cpi = classFile.GetConstantPoolItem(method_index) as ConstantPoolItemInterfaceMethodref;
                        break;
                }
                if (cpi == null)
                {
                    throw new ClassFormatError("Invalid constant pool item MethodHandle");
                }
                if (ReferenceEquals(cpi.Name, StringConstants.INIT) && Kind != RefKind.newInvokeSpecial)
                {
                    throw new ClassFormatError("Bad method name");
                }
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

            internal RefKind Kind
            {
                get { return (RefKind)ref_kind; }
            }

            internal MemberWrapper Member
            {
                get { return cpi.GetMember(); }
            }

            internal TypeWrapper GetClassType()
            {
                return cpi.GetClassType();
            }

            internal override void Link(TypeWrapper thisType, LoadMode mode)
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
