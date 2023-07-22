/*
  Copyright (C) 2011-2014 Jeroen Frijters

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

namespace IKVM.Runtime
{

    class Boxer
    {

        readonly RuntimeJavaType javaLangByte;
        readonly RuntimeJavaMethod byteValue;
        readonly RuntimeJavaMethod valueOfByte;
        readonly RuntimeJavaType javaLangBoolean;
        readonly RuntimeJavaMethod booleanValue;
        readonly RuntimeJavaMethod valueOfBoolean;
        readonly RuntimeJavaType javaLangShort;
        readonly RuntimeJavaMethod shortValue;
        readonly RuntimeJavaMethod valueOfShort;
        readonly RuntimeJavaType javaLangCharacter;
        readonly RuntimeJavaMethod charValue;
        readonly RuntimeJavaMethod valueOfCharacter;
        readonly RuntimeJavaType javaLangInteger;
        readonly RuntimeJavaMethod intValue;
        readonly RuntimeJavaMethod valueOfInteger;
        readonly RuntimeJavaType javaLangFloat;
        readonly RuntimeJavaMethod floatValue;
        readonly RuntimeJavaMethod valueOfFloat;
        readonly RuntimeJavaType javaLangLong;
        readonly RuntimeJavaMethod longValue;
        readonly RuntimeJavaMethod valueOfLong;
        readonly RuntimeJavaType javaLangDouble;
        readonly RuntimeJavaMethod doubleValue;
        readonly RuntimeJavaMethod valueOfDouble;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public Boxer(RuntimeContext context)
        {
            RuntimeClassLoader bootClassLoader = context.ClassLoaderFactory.GetBootstrapClassLoader();
            javaLangByte = bootClassLoader.TryLoadClassByName("java.lang.Byte");
            byteValue = javaLangByte.GetMethodWrapper("byteValue", "()B", false);
            byteValue.Link();
            valueOfByte = javaLangByte.GetMethodWrapper("valueOf", "(B)Ljava.lang.Byte;", false);
            valueOfByte.Link();
            javaLangBoolean = bootClassLoader.TryLoadClassByName("java.lang.Boolean");
            booleanValue = javaLangBoolean.GetMethodWrapper("booleanValue", "()Z", false);
            booleanValue.Link();
            valueOfBoolean = javaLangBoolean.GetMethodWrapper("valueOf", "(Z)Ljava.lang.Boolean;", false);
            valueOfBoolean.Link();
            javaLangShort = bootClassLoader.TryLoadClassByName("java.lang.Short");
            shortValue = javaLangShort.GetMethodWrapper("shortValue", "()S", false);
            shortValue.Link();
            valueOfShort = javaLangShort.GetMethodWrapper("valueOf", "(S)Ljava.lang.Short;", false);
            valueOfShort.Link();
            javaLangCharacter = bootClassLoader.TryLoadClassByName("java.lang.Character");
            charValue = javaLangCharacter.GetMethodWrapper("charValue", "()C", false);
            charValue.Link();
            valueOfCharacter = javaLangCharacter.GetMethodWrapper("valueOf", "(C)Ljava.lang.Character;", false);
            valueOfCharacter.Link();
            javaLangInteger = bootClassLoader.TryLoadClassByName("java.lang.Integer");
            intValue = javaLangInteger.GetMethodWrapper("intValue", "()I", false);
            intValue.Link();
            valueOfInteger = javaLangInteger.GetMethodWrapper("valueOf", "(I)Ljava.lang.Integer;", false);
            valueOfInteger.Link();
            javaLangFloat = bootClassLoader.TryLoadClassByName("java.lang.Float");
            floatValue = javaLangFloat.GetMethodWrapper("floatValue", "()F", false);
            floatValue.Link();
            valueOfFloat = javaLangFloat.GetMethodWrapper("valueOf", "(F)Ljava.lang.Float;", false);
            valueOfFloat.Link();
            javaLangLong = bootClassLoader.TryLoadClassByName("java.lang.Long");
            longValue = javaLangLong.GetMethodWrapper("longValue", "()J", false);
            longValue.Link();
            valueOfLong = javaLangLong.GetMethodWrapper("valueOf", "(J)Ljava.lang.Long;", false);
            valueOfLong.Link();
            javaLangDouble = bootClassLoader.TryLoadClassByName("java.lang.Double");
            doubleValue = javaLangDouble.GetMethodWrapper("doubleValue", "()D", false);
            doubleValue.Link();
            valueOfDouble = javaLangDouble.GetMethodWrapper("valueOf", "(D)Ljava.lang.Double;", false);
            valueOfDouble.Link();
        }

        internal void EmitUnbox(CodeEmitter ilgen, RuntimeJavaType tw, bool cast)
        {
            if (tw == ilgen.Context.PrimitiveJavaTypeFactory.BYTE)
            {
                if (cast)
                {
                    javaLangByte.EmitCheckcast(ilgen);
                }
                byteValue.EmitCall(ilgen);
            }
            else if (tw == ilgen.Context.PrimitiveJavaTypeFactory.BOOLEAN)
            {
                if (cast)
                {
                    javaLangBoolean.EmitCheckcast(ilgen);
                }
                booleanValue.EmitCall(ilgen);
            }
            else if (tw == ilgen.Context.PrimitiveJavaTypeFactory.SHORT)
            {
                if (cast)
                {
                    javaLangShort.EmitCheckcast(ilgen);
                }
                shortValue.EmitCall(ilgen);
            }
            else if (tw == ilgen.Context.PrimitiveJavaTypeFactory.CHAR)
            {
                if (cast)
                {
                    javaLangCharacter.EmitCheckcast(ilgen);
                }
                charValue.EmitCall(ilgen);
            }
            else if (tw == ilgen.Context.PrimitiveJavaTypeFactory.INT)
            {
                if (cast)
                {
                    javaLangInteger.EmitCheckcast(ilgen);
                }
                intValue.EmitCall(ilgen);
            }
            else if (tw == ilgen.Context.PrimitiveJavaTypeFactory.FLOAT)
            {
                if (cast)
                {
                    javaLangFloat.EmitCheckcast(ilgen);
                }
                floatValue.EmitCall(ilgen);
            }
            else if (tw == ilgen.Context.PrimitiveJavaTypeFactory.LONG)
            {
                if (cast)
                {
                    javaLangLong.EmitCheckcast(ilgen);
                }
                longValue.EmitCall(ilgen);
            }
            else if (tw == ilgen.Context.PrimitiveJavaTypeFactory.DOUBLE)
            {
                if (cast)
                {
                    javaLangDouble.EmitCheckcast(ilgen);
                }
                doubleValue.EmitCall(ilgen);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        internal void EmitBox(CodeEmitter ilgen, RuntimeJavaType tw)
        {
            if (tw == ilgen.Context.PrimitiveJavaTypeFactory.BYTE)
            {
                valueOfByte.EmitCall(ilgen);
            }
            else if (tw == ilgen.Context.PrimitiveJavaTypeFactory.BOOLEAN)
            {
                valueOfBoolean.EmitCall(ilgen);
            }
            else if (tw == ilgen.Context.PrimitiveJavaTypeFactory.SHORT)
            {
                valueOfShort.EmitCall(ilgen);
            }
            else if (tw == ilgen.Context.PrimitiveJavaTypeFactory.CHAR)
            {
                valueOfCharacter.EmitCall(ilgen);
            }
            else if (tw == ilgen.Context.PrimitiveJavaTypeFactory.INT)
            {
                valueOfInteger.EmitCall(ilgen);
            }
            else if (tw == ilgen.Context.PrimitiveJavaTypeFactory.FLOAT)
            {
                valueOfFloat.EmitCall(ilgen);
            }
            else if (tw == ilgen.Context.PrimitiveJavaTypeFactory.LONG)
            {
                valueOfLong.EmitCall(ilgen);
            }
            else if (tw == ilgen.Context.PrimitiveJavaTypeFactory.DOUBLE)
            {
                valueOfDouble.EmitCall(ilgen);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

    }

}
