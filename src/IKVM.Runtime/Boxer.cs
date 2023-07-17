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

    static class Boxer
    {

        private static readonly RuntimeJavaType javaLangByte;
        private static readonly RuntimeJavaMethod byteValue;
        private static readonly RuntimeJavaMethod valueOfByte;
        private static readonly RuntimeJavaType javaLangBoolean;
        private static readonly RuntimeJavaMethod booleanValue;
        private static readonly RuntimeJavaMethod valueOfBoolean;
        private static readonly RuntimeJavaType javaLangShort;
        private static readonly RuntimeJavaMethod shortValue;
        private static readonly RuntimeJavaMethod valueOfShort;
        private static readonly RuntimeJavaType javaLangCharacter;
        private static readonly RuntimeJavaMethod charValue;
        private static readonly RuntimeJavaMethod valueOfCharacter;
        private static readonly RuntimeJavaType javaLangInteger;
        private static readonly RuntimeJavaMethod intValue;
        private static readonly RuntimeJavaMethod valueOfInteger;
        private static readonly RuntimeJavaType javaLangFloat;
        private static readonly RuntimeJavaMethod floatValue;
        private static readonly RuntimeJavaMethod valueOfFloat;
        private static readonly RuntimeJavaType javaLangLong;
        private static readonly RuntimeJavaMethod longValue;
        private static readonly RuntimeJavaMethod valueOfLong;
        private static readonly RuntimeJavaType javaLangDouble;
        private static readonly RuntimeJavaMethod doubleValue;
        private static readonly RuntimeJavaMethod valueOfDouble;

        static Boxer()
        {
            RuntimeClassLoader bootClassLoader = RuntimeClassLoader.GetBootstrapClassLoader();
            javaLangByte = bootClassLoader.LoadClassByDottedNameFast("java.lang.Byte");
            byteValue = javaLangByte.GetMethodWrapper("byteValue", "()B", false);
            byteValue.Link();
            valueOfByte = javaLangByte.GetMethodWrapper("valueOf", "(B)Ljava.lang.Byte;", false);
            valueOfByte.Link();
            javaLangBoolean = bootClassLoader.LoadClassByDottedNameFast("java.lang.Boolean");
            booleanValue = javaLangBoolean.GetMethodWrapper("booleanValue", "()Z", false);
            booleanValue.Link();
            valueOfBoolean = javaLangBoolean.GetMethodWrapper("valueOf", "(Z)Ljava.lang.Boolean;", false);
            valueOfBoolean.Link();
            javaLangShort = bootClassLoader.LoadClassByDottedNameFast("java.lang.Short");
            shortValue = javaLangShort.GetMethodWrapper("shortValue", "()S", false);
            shortValue.Link();
            valueOfShort = javaLangShort.GetMethodWrapper("valueOf", "(S)Ljava.lang.Short;", false);
            valueOfShort.Link();
            javaLangCharacter = bootClassLoader.LoadClassByDottedNameFast("java.lang.Character");
            charValue = javaLangCharacter.GetMethodWrapper("charValue", "()C", false);
            charValue.Link();
            valueOfCharacter = javaLangCharacter.GetMethodWrapper("valueOf", "(C)Ljava.lang.Character;", false);
            valueOfCharacter.Link();
            javaLangInteger = bootClassLoader.LoadClassByDottedNameFast("java.lang.Integer");
            intValue = javaLangInteger.GetMethodWrapper("intValue", "()I", false);
            intValue.Link();
            valueOfInteger = javaLangInteger.GetMethodWrapper("valueOf", "(I)Ljava.lang.Integer;", false);
            valueOfInteger.Link();
            javaLangFloat = bootClassLoader.LoadClassByDottedNameFast("java.lang.Float");
            floatValue = javaLangFloat.GetMethodWrapper("floatValue", "()F", false);
            floatValue.Link();
            valueOfFloat = javaLangFloat.GetMethodWrapper("valueOf", "(F)Ljava.lang.Float;", false);
            valueOfFloat.Link();
            javaLangLong = bootClassLoader.LoadClassByDottedNameFast("java.lang.Long");
            longValue = javaLangLong.GetMethodWrapper("longValue", "()J", false);
            longValue.Link();
            valueOfLong = javaLangLong.GetMethodWrapper("valueOf", "(J)Ljava.lang.Long;", false);
            valueOfLong.Link();
            javaLangDouble = bootClassLoader.LoadClassByDottedNameFast("java.lang.Double");
            doubleValue = javaLangDouble.GetMethodWrapper("doubleValue", "()D", false);
            doubleValue.Link();
            valueOfDouble = javaLangDouble.GetMethodWrapper("valueOf", "(D)Ljava.lang.Double;", false);
            valueOfDouble.Link();
        }

        internal static void EmitUnbox(CodeEmitter ilgen, RuntimeJavaType tw, bool cast)
        {
            if (tw == RuntimePrimitiveJavaType.BYTE)
            {
                if (cast)
                {
                    javaLangByte.EmitCheckcast(ilgen);
                }
                byteValue.EmitCall(ilgen);
            }
            else if (tw == RuntimePrimitiveJavaType.BOOLEAN)
            {
                if (cast)
                {
                    javaLangBoolean.EmitCheckcast(ilgen);
                }
                booleanValue.EmitCall(ilgen);
            }
            else if (tw == RuntimePrimitiveJavaType.SHORT)
            {
                if (cast)
                {
                    javaLangShort.EmitCheckcast(ilgen);
                }
                shortValue.EmitCall(ilgen);
            }
            else if (tw == RuntimePrimitiveJavaType.CHAR)
            {
                if (cast)
                {
                    javaLangCharacter.EmitCheckcast(ilgen);
                }
                charValue.EmitCall(ilgen);
            }
            else if (tw == RuntimePrimitiveJavaType.INT)
            {
                if (cast)
                {
                    javaLangInteger.EmitCheckcast(ilgen);
                }
                intValue.EmitCall(ilgen);
            }
            else if (tw == RuntimePrimitiveJavaType.FLOAT)
            {
                if (cast)
                {
                    javaLangFloat.EmitCheckcast(ilgen);
                }
                floatValue.EmitCall(ilgen);
            }
            else if (tw == RuntimePrimitiveJavaType.LONG)
            {
                if (cast)
                {
                    javaLangLong.EmitCheckcast(ilgen);
                }
                longValue.EmitCall(ilgen);
            }
            else if (tw == RuntimePrimitiveJavaType.DOUBLE)
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

        internal static void EmitBox(CodeEmitter ilgen, RuntimeJavaType tw)
        {
            if (tw == RuntimePrimitiveJavaType.BYTE)
            {
                valueOfByte.EmitCall(ilgen);
            }
            else if (tw == RuntimePrimitiveJavaType.BOOLEAN)
            {
                valueOfBoolean.EmitCall(ilgen);
            }
            else if (tw == RuntimePrimitiveJavaType.SHORT)
            {
                valueOfShort.EmitCall(ilgen);
            }
            else if (tw == RuntimePrimitiveJavaType.CHAR)
            {
                valueOfCharacter.EmitCall(ilgen);
            }
            else if (tw == RuntimePrimitiveJavaType.INT)
            {
                valueOfInteger.EmitCall(ilgen);
            }
            else if (tw == RuntimePrimitiveJavaType.FLOAT)
            {
                valueOfFloat.EmitCall(ilgen);
            }
            else if (tw == RuntimePrimitiveJavaType.LONG)
            {
                valueOfLong.EmitCall(ilgen);
            }
            else if (tw == RuntimePrimitiveJavaType.DOUBLE)
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
