/*
  Copyright (C) 2010 Jeroen Frijters

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
using System.Collections.Generic;
using System.IO;
using System.Text;
using IKVM.Attributes;
using IKVM.Internal;
#if STUB_GENERATOR
using Type = IKVM.Reflection.Type;
#endif

namespace IKVM.StubGen
{
	static class SerialVersionUID
	{
		private readonly static System.Security.Cryptography.SHA1Managed sha1 = new System.Security.Cryptography.SHA1Managed();

		internal static long Compute(TypeWrapper tw)
		{
			MemoryStream mem = new MemoryStream();
			BigEndianStream bes = new BigEndianStream(mem);
			WriteClassName(bes, tw);
			WriteModifiers(bes, tw);
			WriteInterfaces(bes, tw);
			WriteFields(bes, tw);
			WriteStaticInitializer(bes, tw);
			WriteConstructors(bes, tw);
			WriteMethods(bes, tw);
			byte[] buf = sha1.ComputeHash(mem.ToArray());
			long hash = 0;
			for (int i = 7; i >= 0; i--)
			{
				hash <<= 8;
				hash |= buf[i];
			}
			return hash;
		}

		private static void WriteClassName(BigEndianStream bes, TypeWrapper tw)
		{
			bes.WriteUtf8(tw.Name);
		}

		private static void WriteModifiers(BigEndianStream bes, TypeWrapper tw)
		{
			Modifiers mods = tw.ReflectiveModifiers & (Modifiers.Public | Modifiers.Final | Modifiers.Interface | Modifiers.Abstract);
			if ((mods & Modifiers.Interface) != 0)
			{
				mods &= ~Modifiers.Abstract;
				if (HasJavaMethods(tw))
				{
					mods |= Modifiers.Abstract;
				}
			}
			bes.WriteUInt32((uint)mods);
		}

		private static bool HasJavaMethods(TypeWrapper tw)
		{
			foreach (MethodWrapper mw in tw.GetMethods())
			{
				if (!mw.IsHideFromReflection && !mw.IsClassInitializer)
				{
					return true;
				}
			}
			return false;
		}

		private static void WriteInterfaces(BigEndianStream bes, TypeWrapper tw)
		{
			TypeWrapper[] interfaces = (TypeWrapper[])tw.Interfaces.Clone();
			Array.Sort(interfaces, delegate(TypeWrapper tw1, TypeWrapper tw2) { return String.CompareOrdinal(tw1.Name, tw2.Name); });
			foreach (TypeWrapper iface in interfaces)
			{
				bes.WriteUtf8(iface.Name);
			}
		}

		private static void WriteFields(BigEndianStream bes, TypeWrapper tw)
		{
			List<FieldWrapper> list = new List<FieldWrapper>();
			foreach (FieldWrapper fw in tw.GetFields())
			{
				if (!fw.IsHideFromReflection)
				{
					list.Add(fw);
				}
			}
			list.Sort(delegate(FieldWrapper fw1, FieldWrapper fw2) { return String.CompareOrdinal(fw1.Name, fw2.Name); });
			foreach (FieldWrapper fw in list)
			{
				Modifiers mods = fw.Modifiers & (Modifiers.Public | Modifiers.Private | Modifiers.Protected | Modifiers.Static | Modifiers.Final | Modifiers.Volatile | Modifiers.Transient);
				if (((mods & Modifiers.Private) == 0) || ((mods & (Modifiers.Static | Modifiers.Transient)) == 0))
				{
					bes.WriteUtf8(fw.Name);
					bes.WriteUInt32((uint)mods);
					bes.WriteUtf8(fw.Signature.Replace('.', '/'));
				}
			}
		}

		private static void WriteStaticInitializer(BigEndianStream bes, TypeWrapper tw)
		{
			Type type = tw.TypeAsTBD;
			if (!type.IsArray && type.TypeInitializer != null)
			{
				if (!AttributeHelper.IsHideFromJava(type.TypeInitializer))
				{
					bes.WriteUtf8("<clinit>");
					bes.WriteUInt32((uint)Modifiers.Static);
					bes.WriteUtf8("()V");
				}
			}
		}

		private static void WriteConstructors(BigEndianStream bes, TypeWrapper tw)
		{
			List<MethodWrapper> list = new List<MethodWrapper>();
			foreach (MethodWrapper mw in tw.GetMethods())
			{
				if (mw.IsConstructor && !mw.IsHideFromReflection && !mw.IsPrivate)
				{
					list.Add(mw);
				}
			}
			list.Sort(delegate(MethodWrapper mw1, MethodWrapper mw2) { return String.CompareOrdinal(mw1.Signature, mw2.Signature); });
			foreach (MethodWrapper mw in list)
			{
				Modifiers mods = mw.Modifiers & (Modifiers.Public | Modifiers.Private | Modifiers.Protected | Modifiers.Static | Modifiers.Final | Modifiers.Synchronized | Modifiers.Native | Modifiers.Abstract | Modifiers.Strictfp);
				bes.WriteUtf8(mw.Name);
				bes.WriteUInt32((uint)mods);
				bes.WriteUtf8(mw.Signature);
			}
		}

		private static void WriteMethods(BigEndianStream bes, TypeWrapper tw)
		{
			List<MethodWrapper> list = new List<MethodWrapper>();
			foreach (MethodWrapper mw in tw.GetMethods())
			{
				if (!mw.IsConstructor && !mw.IsHideFromReflection && !mw.IsPrivate)
				{
					list.Add(mw);
				}
			}
			list.Sort(delegate(MethodWrapper mw1, MethodWrapper mw2) {
				if (mw1.Name == mw2.Name)
					return String.CompareOrdinal(mw1.Signature, mw2.Signature);
				return String.CompareOrdinal(mw1.Name, mw2.Name); 
			});
			foreach (MethodWrapper mw in list)
			{
				Modifiers mods = mw.Modifiers & (Modifiers.Public | Modifiers.Private | Modifiers.Protected | Modifiers.Static | Modifiers.Final | Modifiers.Synchronized | Modifiers.Native | Modifiers.Abstract | Modifiers.Strictfp);
				bes.WriteUtf8(mw.Name);
				bes.WriteUInt32((uint)mods);
				bes.WriteUtf8(mw.Signature);
			}
		}
	}
}
