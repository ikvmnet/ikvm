/*
  Copyright (C) 2007-2014 Jeroen Frijters

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
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Exceptions;
using IKVM.Runtime;

namespace IKVM.Java.Externs.java.io
{

    static class ObjectStreamClass
    {

        static class IOHelpers
        {

            public static void WriteByte(byte[] buf, int offset, byte value)
            {
                buf[offset] = value;
            }

            public static void WriteBoolean(byte[] buf, int offset, bool value)
            {
                buf[offset] = value ? (byte)1 : (byte)0;
            }

            public static void WriteChar(byte[] buf, int offset, char value)
            {
                buf[offset + 0] = (byte)(value >> 8);
                buf[offset + 1] = (byte)(value >> 0);
            }

            public static void WriteShort(byte[] buf, int offset, short value)
            {
                buf[offset + 0] = (byte)(value >> 8);
                buf[offset + 1] = (byte)(value >> 0);
            }

            public static void WriteInt(byte[] buf, int offset, int value)
            {
                buf[offset + 0] = (byte)(value >> 24);
                buf[offset + 1] = (byte)(value >> 16);
                buf[offset + 2] = (byte)(value >> 8);
                buf[offset + 3] = (byte)(value >> 0);
            }

            public static void WriteFloat(byte[] buf, int offset, float value)
            {
#if !FIRST_PASS && !IMPORTER
                global::java.io.Bits.putFloat(buf, offset, value);
#endif
            }

            public static void WriteLong(byte[] buf, int offset, long value)
            {
                WriteInt(buf, offset, (int)(value >> 32));
                WriteInt(buf, offset + 4, (int)value);
            }

            public static void WriteDouble(byte[] buf, int offset, double value)
            {
#if !FIRST_PASS && !IMPORTER
                global::java.io.Bits.putDouble(buf, offset, value);
#endif
            }

            public static byte ReadByte(byte[] buf, int offset)
            {
                return buf[offset];
            }

            public static bool ReadBoolean(byte[] buf, int offset)
            {
                return buf[offset] != 0;
            }

            public static char ReadChar(byte[] buf, int offset)
            {
                return (char)((buf[offset] << 8) + buf[offset + 1]);
            }

            public static short ReadShort(byte[] buf, int offset)
            {
                return (short)((buf[offset] << 8) + buf[offset + 1]);
            }

            public static int ReadInt(byte[] buf, int offset)
            {
                return (buf[offset + 0] << 24)
                     + (buf[offset + 1] << 16)
                     + (buf[offset + 2] << 8)
                     + (buf[offset + 3] << 0);
            }

            public static float ReadFloat(byte[] buf, int offset)
            {
#if FIRST_PASS || IMPORTER
			    return 0;
#else
                return global::java.lang.Float.intBitsToFloat(ReadInt(buf, offset));
#endif
            }

            public static long ReadLong(byte[] buf, int offset)
            {
                long hi = (uint)ReadInt(buf, offset);
                long lo = (uint)ReadInt(buf, offset + 4);
                return lo + (hi << 32);
            }

            public static double ReadDouble(byte[] buf, int offset)
            {
#if FIRST_PASS || IMPORTER
			    return 0;
#else
                return global::java.lang.Double.longBitsToDouble(ReadLong(buf, offset));
#endif
            }
        }

        public static void initNative()
        {
        }

        public static bool isDynamicTypeWrapper(global::java.lang.Class cl)
        {
            RuntimeJavaType wrapper = RuntimeJavaType.FromClass(cl);
            return !wrapper.IsFastClassLiteralSafe;
        }

        public static bool hasStaticInitializer(global::java.lang.Class cl)
        {
            var wrapper = RuntimeJavaType.FromClass(cl);
            try
            {
                wrapper.Finish();
            }
            catch (TranslatableJavaException e)
            {
                throw wrapper.Context.ExceptionHelper.MapException<Exception>(e, true, false);
            }

            var type = wrapper.TypeAsTBD;
            if (!type.IsArray && type.TypeInitializer != null)
            {
                wrapper.RunClassInit();
                return !wrapper.Context.AttributeHelper.IsHideFromJava(type.TypeInitializer);
            }

            return false;
        }

#if !FIRST_PASS && !NO_REF_EMIT

        private sealed class FastFieldReflector : global::ikvm.@internal.FieldReflectorBase
        {

            private static readonly MethodInfo ReadByteMethod = typeof(IOHelpers).GetMethod("ReadByte");
            private static readonly MethodInfo ReadBooleanMethod = typeof(IOHelpers).GetMethod("ReadBoolean");
            private static readonly MethodInfo ReadCharMethod = typeof(IOHelpers).GetMethod("ReadChar");
            private static readonly MethodInfo ReadShortMethod = typeof(IOHelpers).GetMethod("ReadShort");
            private static readonly MethodInfo ReadIntMethod = typeof(IOHelpers).GetMethod("ReadInt");
            private static readonly MethodInfo ReadFloatMethod = typeof(IOHelpers).GetMethod("ReadFloat");
            private static readonly MethodInfo ReadLongMethod = typeof(IOHelpers).GetMethod("ReadLong");
            private static readonly MethodInfo ReadDoubleMethod = typeof(IOHelpers).GetMethod("ReadDouble");
            private static readonly MethodInfo WriteByteMethod = typeof(IOHelpers).GetMethod("WriteByte");
            private static readonly MethodInfo WriteBooleanMethod = typeof(IOHelpers).GetMethod("WriteBoolean");
            private static readonly MethodInfo WriteCharMethod = typeof(IOHelpers).GetMethod("WriteChar");
            private static readonly MethodInfo WriteShortMethod = typeof(IOHelpers).GetMethod("WriteShort");
            private static readonly MethodInfo WriteIntMethod = typeof(IOHelpers).GetMethod("WriteInt");
            private static readonly MethodInfo WriteFloatMethod = typeof(IOHelpers).GetMethod("WriteFloat");
            private static readonly MethodInfo WriteLongMethod = typeof(IOHelpers).GetMethod("WriteLong");
            private static readonly MethodInfo WriteDoubleMethod = typeof(IOHelpers).GetMethod("WriteDouble");
            private delegate void ObjFieldGetterSetter(object obj, object[] objarr);
            private delegate void PrimFieldGetterSetter(object obj, byte[] objarr);
            private static readonly ObjFieldGetterSetter objDummy = new ObjFieldGetterSetter(Dummy);
            private static readonly PrimFieldGetterSetter primDummy = new PrimFieldGetterSetter(Dummy);
            private global::java.io.ObjectStreamField[] fields;
            private ObjFieldGetterSetter objFieldGetter;
            private PrimFieldGetterSetter primFieldGetter;
            private ObjFieldGetterSetter objFieldSetter;
            private PrimFieldGetterSetter primFieldSetter;

            private static void Dummy(object obj, object[] objarr)
            {
            }

            private static void Dummy(object obj, byte[] barr)
            {
            }

            internal FastFieldReflector(global::java.io.ObjectStreamField[] fields)
            {
                this.fields = fields;
                RuntimeJavaType tw = null;
                foreach (global::java.io.ObjectStreamField field in fields)
                {
                    RuntimeJavaField fw = GetFieldWrapper(field);
                    if (fw != null)
                    {
                        if (tw == null)
                        {
                            tw = fw.DeclaringType;
                        }
                        else if (tw != fw.DeclaringType)
                        {
                            // pre-condition is that all fields are from the same Type!
                            throw new global::java.lang.InternalError();
                        }
                    }
                }
                if (tw == null)
                {
                    objFieldGetter = objFieldSetter = objDummy;
                    primFieldGetter = primFieldSetter = primDummy;
                }
                else
                {
                    try
                    {
                        tw.Finish();
                    }
                    catch (TranslatableJavaException e)
                    {
                        throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
                    }

                    var dmObjGetter = DynamicMethodUtil.Create("__<ObjFieldGetter>", tw.TypeAsBaseType, true, null, new Type[] { typeof(object), typeof(object[]) });
                    var dmPrimGetter = DynamicMethodUtil.Create("__<PrimFieldGetter>", tw.TypeAsBaseType, true, null, new Type[] { typeof(object), typeof(byte[]) });
                    var dmObjSetter = DynamicMethodUtil.Create("__<ObjFieldSetter>", tw.TypeAsBaseType, true, null, new Type[] { typeof(object), typeof(object[]) });
                    var dmPrimSetter = DynamicMethodUtil.Create("__<PrimFieldSetter>", tw.TypeAsBaseType, true, null, new Type[] { typeof(object), typeof(byte[]) });
                    var ilgenObjGetter = JVM.Context.CodeEmitterFactory.Create(dmObjGetter);
                    var ilgenPrimGetter = JVM.Context.CodeEmitterFactory.Create(dmPrimGetter);
                    var ilgenObjSetter = JVM.Context.CodeEmitterFactory.Create(dmObjSetter);
                    var ilgenPrimSetter = JVM.Context.CodeEmitterFactory.Create(dmPrimSetter);

                    // we want the getters to be verifiable (because writeObject can be used from partial trust),
                    // so we create a local to hold the properly typed object reference
                    var objGetterThis = ilgenObjGetter.DeclareLocal(tw.TypeAsBaseType);
                    var primGetterThis = ilgenPrimGetter.DeclareLocal(tw.TypeAsBaseType);
                    ilgenObjGetter.Emit(OpCodes.Ldarg_0);
                    ilgenObjGetter.Emit(OpCodes.Castclass, tw.TypeAsBaseType);
                    ilgenObjGetter.Emit(OpCodes.Stloc, objGetterThis);
                    ilgenPrimGetter.Emit(OpCodes.Ldarg_0);
                    ilgenPrimGetter.Emit(OpCodes.Castclass, tw.TypeAsBaseType);
                    ilgenPrimGetter.Emit(OpCodes.Stloc, primGetterThis);

                    foreach (global::java.io.ObjectStreamField field in fields)
                    {
                        var fw = GetFieldWrapper(field);
                        if (fw == null)
                            continue;

                        fw.ResolveField();

                        var fieldType = fw.FieldTypeWrapper;
                        try
                        {
                            fieldType = fieldType.EnsureLoadable(tw.ClassLoader);
                            fieldType.Finish();
                        }
                        catch (TranslatableJavaException e)
                        {
                            throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
                        }

                        if (fieldType.IsPrimitive)
                        {
                            // Getter
                            ilgenPrimGetter.Emit(OpCodes.Ldarg_1);
                            ilgenPrimGetter.EmitLdc_I4(field.getOffset());
                            ilgenPrimGetter.Emit(OpCodes.Ldloc, primGetterThis);
                            fw.EmitGet(ilgenPrimGetter);
                            if (fieldType == ilgenPrimGetter.Context.PrimitiveJavaTypeFactory.BYTE)
                            {
                                ilgenPrimGetter.Emit(OpCodes.Call, WriteByteMethod);
                            }
                            else if (fieldType == ilgenPrimGetter.Context.PrimitiveJavaTypeFactory.BOOLEAN)
                            {
                                ilgenPrimGetter.Emit(OpCodes.Call, WriteBooleanMethod);
                            }
                            else if (fieldType == ilgenPrimGetter.Context.PrimitiveJavaTypeFactory.CHAR)
                            {
                                ilgenPrimGetter.Emit(OpCodes.Call, WriteCharMethod);
                            }
                            else if (fieldType == ilgenPrimGetter.Context.PrimitiveJavaTypeFactory.SHORT)
                            {
                                ilgenPrimGetter.Emit(OpCodes.Call, WriteShortMethod);
                            }
                            else if (fieldType == ilgenPrimGetter.Context.PrimitiveJavaTypeFactory.INT)
                            {
                                ilgenPrimGetter.Emit(OpCodes.Call, WriteIntMethod);
                            }
                            else if (fieldType == ilgenPrimGetter.Context.PrimitiveJavaTypeFactory.FLOAT)
                            {
                                ilgenPrimGetter.Emit(OpCodes.Call, WriteFloatMethod);
                            }
                            else if (fieldType == ilgenPrimGetter.Context.PrimitiveJavaTypeFactory.LONG)
                            {
                                ilgenPrimGetter.Emit(OpCodes.Call, WriteLongMethod);
                            }
                            else if (fieldType == ilgenPrimGetter.Context.PrimitiveJavaTypeFactory.DOUBLE)
                            {
                                ilgenPrimGetter.Emit(OpCodes.Call, WriteDoubleMethod);
                            }
                            else
                            {
                                throw new global::java.lang.InternalError();
                            }

                            // Setter
                            ilgenPrimSetter.Emit(OpCodes.Ldarg_0);
                            ilgenPrimSetter.Emit(OpCodes.Castclass, tw.TypeAsBaseType);
                            ilgenPrimSetter.Emit(OpCodes.Ldarg_1);
                            ilgenPrimSetter.EmitLdc_I4(field.getOffset());
                            if (fieldType == ilgenPrimSetter.Context.PrimitiveJavaTypeFactory.BYTE)
                            {
                                ilgenPrimSetter.Emit(OpCodes.Call, ReadByteMethod);
                            }
                            else if (fieldType == ilgenPrimSetter.Context.PrimitiveJavaTypeFactory.BOOLEAN)
                            {
                                ilgenPrimSetter.Emit(OpCodes.Call, ReadBooleanMethod);
                            }
                            else if (fieldType == ilgenPrimSetter.Context.PrimitiveJavaTypeFactory.CHAR)
                            {
                                ilgenPrimSetter.Emit(OpCodes.Call, ReadCharMethod);
                            }
                            else if (fieldType == ilgenPrimSetter.Context.PrimitiveJavaTypeFactory.SHORT)
                            {
                                ilgenPrimSetter.Emit(OpCodes.Call, ReadShortMethod);
                            }
                            else if (fieldType == ilgenPrimSetter.Context.PrimitiveJavaTypeFactory.INT)
                            {
                                ilgenPrimSetter.Emit(OpCodes.Call, ReadIntMethod);
                            }
                            else if (fieldType == ilgenPrimSetter.Context.PrimitiveJavaTypeFactory.FLOAT)
                            {
                                ilgenPrimSetter.Emit(OpCodes.Call, ReadFloatMethod);
                            }
                            else if (fieldType == ilgenPrimSetter.Context.PrimitiveJavaTypeFactory.LONG)
                            {
                                ilgenPrimSetter.Emit(OpCodes.Call, ReadLongMethod);
                            }
                            else if (fieldType == ilgenPrimSetter.Context.PrimitiveJavaTypeFactory.DOUBLE)
                            {
                                ilgenPrimSetter.Emit(OpCodes.Call, ReadDoubleMethod);
                            }
                            else
                            {
                                throw new global::java.lang.InternalError();
                            }
                            fw.EmitSet(ilgenPrimSetter);
                        }
                        else
                        {
                            // Getter
                            ilgenObjGetter.Emit(OpCodes.Ldarg_1);
                            ilgenObjGetter.EmitLdc_I4(field.getOffset());
                            ilgenObjGetter.Emit(OpCodes.Ldloc, objGetterThis);
                            fw.EmitGet(ilgenObjGetter);
                            fieldType.EmitConvSignatureTypeToStackType(ilgenObjGetter);
                            ilgenObjGetter.Emit(OpCodes.Stelem_Ref);

                            // Setter
                            ilgenObjSetter.Emit(OpCodes.Ldarg_0);
                            ilgenObjSetter.Emit(OpCodes.Ldarg_1);
                            ilgenObjSetter.EmitLdc_I4(field.getOffset());
                            ilgenObjSetter.Emit(OpCodes.Ldelem_Ref);
                            fieldType.EmitCheckcast(ilgenObjSetter);
                            fieldType.EmitConvStackTypeToSignatureType(ilgenObjSetter, null);
                            fw.EmitSet(ilgenObjSetter);
                        }
                    }
                    ilgenObjGetter.Emit(OpCodes.Ret);
                    ilgenPrimGetter.Emit(OpCodes.Ret);
                    ilgenObjSetter.Emit(OpCodes.Ret);
                    ilgenPrimSetter.Emit(OpCodes.Ret);
                    ilgenObjGetter.DoEmit();
                    ilgenPrimGetter.DoEmit();
                    ilgenObjSetter.DoEmit();
                    ilgenPrimSetter.DoEmit();
                    objFieldGetter = (ObjFieldGetterSetter)dmObjGetter.CreateDelegate(typeof(ObjFieldGetterSetter));
                    primFieldGetter = (PrimFieldGetterSetter)dmPrimGetter.CreateDelegate(typeof(PrimFieldGetterSetter));
                    objFieldSetter = (ObjFieldGetterSetter)dmObjSetter.CreateDelegate(typeof(ObjFieldGetterSetter));
                    primFieldSetter = (PrimFieldGetterSetter)dmPrimSetter.CreateDelegate(typeof(PrimFieldGetterSetter));
                }
            }

            private static RuntimeJavaField GetFieldWrapper(global::java.io.ObjectStreamField field)
            {
                global::java.lang.reflect.Field f = field.getField();
                return f == null ? null : RuntimeJavaField.FromField(f);
            }

            public override global::java.io.ObjectStreamField[] getFields()
            {
                return fields;
            }

            public override void getObjFieldValues(object obj, object[] objarr)
            {
                objFieldGetter(obj, objarr);
            }

            public override void setObjFieldValues(object obj, object[] objarr)
            {
                objFieldSetter(obj, objarr);
            }

            public override void getPrimFieldValues(object obj, byte[] barr)
            {
                primFieldGetter(obj, barr);
            }

            public override void setPrimFieldValues(object obj, byte[] barr)
            {
                primFieldSetter(obj, barr);
            }
        }
#endif // !FIRST_PASS && !NO_REF_EMIT

        public static object getFastFieldReflector(global::java.io.ObjectStreamField[] fieldsObj)
        {
#if FIRST_PASS || NO_REF_EMIT
        return null;
#else
            return new FastFieldReflector(fieldsObj);
#endif
        }
    }

}