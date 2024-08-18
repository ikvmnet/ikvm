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
using IKVM.ByteCode;
using IKVM.ByteCode.Decoding;

namespace IKVM.Runtime
{

    sealed partial class ClassFile
    {

        internal sealed class Field : FieldOrMethod
        {

            object constantValue;
            string[] propertyGetterSetter;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="classFile"></param>
            /// <param name="utf8_cp"></param>
            /// <param name="field"></param>
            /// <exception cref="ClassFormatError"></exception>
            internal Field(ClassFile classFile, string[] utf8_cp, IKVM.ByteCode.Decoding.Field field) :
                base(classFile, utf8_cp, field.AccessFlags, field.Name, field.Descriptor)
            {
                if ((IsPrivate && IsPublic) || (IsPrivate && IsProtected) || (IsPublic && IsProtected) || (IsFinal && IsVolatile) || (classFile.IsInterface && (!IsPublic || !IsStatic || !IsFinal || IsTransient)))
                    throw new ClassFormatError("{0} (Illegal field modifiers: 0x{1:X})", classFile.Name, accessFlags);

                for (int i = 0; i < field.Attributes.Count; i++)
                {
                    var attribute = field.Attributes[i];

                    switch (classFile.GetConstantPoolUtf8String(utf8_cp, attribute.Name))
                    {
                        case AttributeName.Deprecated:
                            attribute.AsDeprecated();
                            flags |= FLAG_MASK_DEPRECATED;
                            break;
                        case AttributeName.ConstantValue:
                            try
                            {
                                var _constantValue = (ConstantValueAttribute)attribute;
                                constantValue = Signature switch
                                {
                                    "I" => classFile.GetConstantPoolConstantInteger((IntegerConstantHandle)_constantValue.Value),
                                    "S" => (short)classFile.GetConstantPoolConstantInteger((IntegerConstantHandle)_constantValue.Value),
                                    "B" => (byte)classFile.GetConstantPoolConstantInteger((IntegerConstantHandle)_constantValue.Value),
                                    "C" => (char)classFile.GetConstantPoolConstantInteger((IntegerConstantHandle)_constantValue.Value),
                                    "Z" => classFile.GetConstantPoolConstantInteger((IntegerConstantHandle)_constantValue.Value) != 0,
                                    "J" => classFile.GetConstantPoolConstantLong((LongConstantHandle)_constantValue.Value),
                                    "F" => classFile.GetConstantPoolConstantFloat((FloatConstantHandle)_constantValue.Value),
                                    "D" => classFile.GetConstantPoolConstantDouble((DoubleConstantHandle)_constantValue.Value),
                                    "Ljava.lang.String;" => classFile.GetConstantPoolConstantString((StringConstantHandle)_constantValue.Value),
                                    _ => throw new ClassFormatError("{0} (Invalid signature for constant)", classFile.Name),
                                };
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
                            catch (ByteCodeException)
                            {
                                throw new ClassFormatError("{0} (Bad index into constant pool)", classFile.Name);
                            }
                            break;
                        case AttributeName.Signature:
                            if (classFile.MajorVersion < 49)
                                goto default;

                            var _signature = (IKVM.ByteCode.Decoding.SignatureAttribute)attribute;
                            signature = classFile.GetConstantPoolUtf8String(utf8_cp, _signature.Signature);
                            break;
                        case AttributeName.RuntimeVisibleAnnotations:
                            if (classFile.MajorVersion < 49)
                                goto default;

                            var _runtimeVisibleAnnotation = (RuntimeVisibleAnnotationsAttribute)attribute;
                            annotations = ReadAnnotations(_runtimeVisibleAnnotation.Annotations, classFile, utf8_cp);
                            break;
                        case AttributeName.RuntimeInvisibleAnnotations:
                            if (classFile.MajorVersion < 49)
                                goto default;

                            var _runtimeInvisibleAnnotations = (RuntimeInvisibleAnnotationsAttribute)attribute;

                            foreach (object[] annot in ReadAnnotations(_runtimeInvisibleAnnotations.Annotations, classFile, utf8_cp))
                            {
                                if (annot[1].Equals("Likvm/lang/Property;"))
                                    DecodePropertyAnnotation(classFile, annot);
#if IMPORTER
								else if(annot[1].Equals("Likvm/lang/Internal;"))
								{
									this.accessFlags &= ~Modifiers.AccessMask;
									flags |= FLAG_MASK_INTERNAL;
								}
#endif
                            }

                            break;
                        case AttributeName.RuntimeVisibleTypeAnnotations:
                            if (classFile.MajorVersion < 52)
                                goto default;

                            var _runtimeVisibleTypeAnnotations = (IKVM.ByteCode.Decoding.RuntimeVisibleTypeAnnotationsAttribute)attribute;
                            classFile.CreateUtf8ConstantPoolItems(utf8_cp);
                            runtimeVisibleTypeAnnotations = _runtimeVisibleTypeAnnotations.TypeAnnotations;
                            break;
                        default:
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
                    throw new ClassFormatError("{0} (Field \"{1}\" has invalid signature \"{2}\")", classFile.Name, this.Name, descriptor);
            }

            internal object ConstantValue => constantValue;

            internal void PatchConstantValue(object value) => constantValue = value;

            internal bool IsStaticFinalConstant => (accessFlags & (Modifiers.Final | Modifiers.Static)) == (Modifiers.Final | Modifiers.Static) && constantValue != null;

            internal bool IsProperty => propertyGetterSetter != null;

            internal string PropertyGetter => propertyGetterSetter[0];

            internal string PropertySetter => propertyGetterSetter[1];

        }

    }

}
