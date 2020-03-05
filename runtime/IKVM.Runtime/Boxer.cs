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

namespace IKVM.Internal
{
	static class Boxer
	{
		private static readonly TypeWrapper javaLangByte;
		private static readonly MethodWrapper byteValue;
		private static readonly MethodWrapper valueOfByte;
		private static readonly TypeWrapper javaLangBoolean;
		private static readonly MethodWrapper booleanValue;
		private static readonly MethodWrapper valueOfBoolean;
		private static readonly TypeWrapper javaLangShort;
		private static readonly MethodWrapper shortValue;
		private static readonly MethodWrapper valueOfShort;
		private static readonly TypeWrapper javaLangCharacter;
		private static readonly MethodWrapper charValue;
		private static readonly MethodWrapper valueOfCharacter;
		private static readonly TypeWrapper javaLangInteger;
		private static readonly MethodWrapper intValue;
		private static readonly MethodWrapper valueOfInteger;
		private static readonly TypeWrapper javaLangFloat;
		private static readonly MethodWrapper floatValue;
		private static readonly MethodWrapper valueOfFloat;
		private static readonly TypeWrapper javaLangLong;
		private static readonly MethodWrapper longValue;
		private static readonly MethodWrapper valueOfLong;
		private static readonly TypeWrapper javaLangDouble;
		private static readonly MethodWrapper doubleValue;
		private static readonly MethodWrapper valueOfDouble;

		static Boxer()
		{
			ClassLoaderWrapper bootClassLoader = ClassLoaderWrapper.GetBootstrapClassLoader();
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

		internal static void EmitUnbox(CodeEmitter ilgen, TypeWrapper tw, bool cast)
		{
			if (tw == PrimitiveTypeWrapper.BYTE)
			{
				if (cast)
				{
					javaLangByte.EmitCheckcast(ilgen);
				}
				byteValue.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.BOOLEAN)
			{
				if (cast)
				{
					javaLangBoolean.EmitCheckcast(ilgen);
				}
				booleanValue.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.SHORT)
			{
				if (cast)
				{
					javaLangShort.EmitCheckcast(ilgen);
				}
				shortValue.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.CHAR)
			{
				if (cast)
				{
					javaLangCharacter.EmitCheckcast(ilgen);
				}
				charValue.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.INT)
			{
				if (cast)
				{
					javaLangInteger.EmitCheckcast(ilgen);
				}
				intValue.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.FLOAT)
			{
				if (cast)
				{
					javaLangFloat.EmitCheckcast(ilgen);
				}
				floatValue.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.LONG)
			{
				if (cast)
				{
					javaLangLong.EmitCheckcast(ilgen);
				}
				longValue.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.DOUBLE)
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

		internal static void EmitBox(CodeEmitter ilgen, TypeWrapper tw)
		{
			if (tw == PrimitiveTypeWrapper.BYTE)
			{
				valueOfByte.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.BOOLEAN)
			{
				valueOfBoolean.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.SHORT)
			{
				valueOfShort.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.CHAR)
			{
				valueOfCharacter.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.INT)
			{
				valueOfInteger.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.FLOAT)
			{
				valueOfFloat.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.LONG)
			{
				valueOfLong.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.DOUBLE)
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
