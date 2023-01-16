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

using IKVM.Attributes;

namespace IKVM.Internal
{

    sealed partial class ClassFile
    {
        internal sealed class Field : FieldOrMethod
        {
            private object constantValue;
            private string[] propertyGetterSetter;

            internal Field(ClassFile classFile, string[] utf8_cp, BigEndianBinaryReader br) : base(classFile, utf8_cp, br)
            {
                if ((IsPrivate && IsPublic) || (IsPrivate && IsProtected) || (IsPublic && IsProtected)
                    || (IsFinal && IsVolatile)
                    || (classFile.IsInterface && (!IsPublic || !IsStatic || !IsFinal || IsTransient)))
                {
                    throw new ClassFormatError("{0} (Illegal field modifiers: 0x{1:X})", classFile.Name, access_flags);
                }
                int attributes_count = br.ReadUInt16();
                for (int i = 0; i < attributes_count; i++)
                {
                    switch (classFile.GetConstantPoolUtf8String(utf8_cp, br.ReadUInt16()))
                    {
                        case "Deprecated":
                            if (br.ReadUInt32() != 0)
                            {
                                throw new ClassFormatError("Invalid Deprecated attribute length");
                            }
                            flags |= FLAG_MASK_DEPRECATED;
                            break;
                        case "ConstantValue":
                            {
                                if (br.ReadUInt32() != 2)
                                {
                                    throw new ClassFormatError("Invalid ConstantValue attribute length");
                                }
                                ushort index = br.ReadUInt16();
                                try
                                {
                                    switch (Signature)
                                    {
                                        case "I":
                                            constantValue = classFile.GetConstantPoolConstantInteger(index);
                                            break;
                                        case "S":
                                            constantValue = (short)classFile.GetConstantPoolConstantInteger(index);
                                            break;
                                        case "B":
                                            constantValue = (byte)classFile.GetConstantPoolConstantInteger(index);
                                            break;
                                        case "C":
                                            constantValue = (char)classFile.GetConstantPoolConstantInteger(index);
                                            break;
                                        case "Z":
                                            constantValue = classFile.GetConstantPoolConstantInteger(index) != 0;
                                            break;
                                        case "J":
                                            constantValue = classFile.GetConstantPoolConstantLong(index);
                                            break;
                                        case "F":
                                            constantValue = classFile.GetConstantPoolConstantFloat(index);
                                            break;
                                        case "D":
                                            constantValue = classFile.GetConstantPoolConstantDouble(index);
                                            break;
                                        case "Ljava.lang.String;":
                                            constantValue = classFile.GetConstantPoolConstantString(index);
                                            break;
                                        default:
                                            throw new ClassFormatError("{0} (Invalid signature for constant)", classFile.Name);
                                    }
                                }
                                catch (InvalidCastException)
                                {
                                    throw new ClassFormatError("{0} (Bad index into constant pool)", classFile.Name);
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    throw new ClassFormatError("{0} (Bad index into constant pool)", classFile.Name);
                                }
                                catch (InvalidOperationException)
                                {
                                    throw new ClassFormatError("{0} (Bad index into constant pool)", classFile.Name);
                                }
                                catch (NullReferenceException)
                                {
                                    throw new ClassFormatError("{0} (Bad index into constant pool)", classFile.Name);
                                }
                                break;
                            }
                        case "Signature":
                            if (classFile.MajorVersion < 49)
                            {
                                goto default;
                            }
                            if (br.ReadUInt32() != 2)
                            {
                                throw new ClassFormatError("Signature attribute has incorrect length");
                            }
                            signature = classFile.GetConstantPoolUtf8String(utf8_cp, br.ReadUInt16());
                            break;
                        case "RuntimeVisibleAnnotations":
                            if (classFile.MajorVersion < 49)
                            {
                                goto default;
                            }
                            annotations = ReadAnnotations(br, classFile, utf8_cp);
                            break;
                        case "RuntimeInvisibleAnnotations":
                            if (classFile.MajorVersion < 49)
                            {
                                goto default;
                            }
                            foreach (object[] annot in ReadAnnotations(br, classFile, utf8_cp))
                            {
                                if (annot[1].Equals("Likvm/lang/Property;"))
                                {
                                    DecodePropertyAnnotation(classFile, annot);
                                }
#if IMPORTER
								else if(annot[1].Equals("Likvm/lang/Internal;"))
								{
									this.access_flags &= ~Modifiers.AccessMask;
									flags |= FLAG_MASK_INTERNAL;
								}
#endif
                            }
                            break;
                        case "RuntimeVisibleTypeAnnotations":
                            if (classFile.MajorVersion < 52)
                            {
                                goto default;
                            }
                            classFile.CreateUtf8ConstantPoolItems(utf8_cp);
                            runtimeVisibleTypeAnnotations = br.Section(br.ReadUInt32()).ToArray();
                            break;
                        default:
                            br.Skip(br.ReadUInt32());
                            break;
                    }
                }
            }

            private void DecodePropertyAnnotation(ClassFile classFile, object[] annot)
            {
                if (propertyGetterSetter != null)
                {
                    Tracer.Error(Tracer.ClassLoading, "Ignoring duplicate ikvm.lang.Property annotation on {0}.{1}", classFile.Name, this.Name);
                    return;
                }
                propertyGetterSetter = new string[2];
                for (int i = 2; i < annot.Length - 1; i += 2)
                {
                    string value = annot[i + 1] as string;
                    if (value == null)
                    {
                        propertyGetterSetter = null;
                        break;
                    }
                    if (annot[i].Equals("get") && propertyGetterSetter[0] == null)
                    {
                        propertyGetterSetter[0] = value;
                    }
                    else if (annot[i].Equals("set") && propertyGetterSetter[1] == null)
                    {
                        propertyGetterSetter[1] = value;
                    }
                    else
                    {
                        propertyGetterSetter = null;
                        break;
                    }
                }
                if (propertyGetterSetter == null || propertyGetterSetter[0] == null)
                {
                    propertyGetterSetter = null;
                    Tracer.Error(Tracer.ClassLoading, "Ignoring malformed ikvm.lang.Property annotation on {0}.{1}", classFile.Name, this.Name);
                    return;
                }
            }

            protected override void ValidateSig(ClassFile classFile, string descriptor)
            {
                if (!IsValidFieldSig(descriptor))
                {
                    throw new ClassFormatError("{0} (Field \"{1}\" has invalid signature \"{2}\")", classFile.Name, this.Name, descriptor);
                }
            }

            internal object ConstantValue
            {
                get
                {
                    return constantValue;
                }
            }

            internal void PatchConstantValue(object value)
            {
                constantValue = value;
            }

            internal bool IsStaticFinalConstant
            {
                get { return (access_flags & (Modifiers.Final | Modifiers.Static)) == (Modifiers.Final | Modifiers.Static) && constantValue != null; }
            }

            internal bool IsProperty
            {
                get
                {
                    return propertyGetterSetter != null;
                }
            }

            internal string PropertyGetter
            {
                get
                {
                    return propertyGetterSetter[0];
                }
            }

            internal string PropertySetter
            {
                get
                {
                    return propertyGetterSetter[1];
                }
            }
        }
    }

}
