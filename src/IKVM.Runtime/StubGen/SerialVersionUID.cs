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
using System.IO;
using System.Linq;
using System.Security.Cryptography;

using IKVM.Attributes;
using IKVM.Internal;

namespace IKVM.StubGen
{

    static class SerialVersionUID
    {

        readonly static SHA1 sha1 = SHA1.Create();

        internal static long Compute(TypeWrapper tw)
        {
            var mem = new MemoryStream();
            var bes = new BigEndianStream(mem);
            WriteClassName(bes, tw);
            WriteModifiers(bes, tw);
            WriteInterfaces(bes, tw);
            WriteFields(bes, tw);
            WriteStaticInitializer(bes, tw);
            WriteConstructors(bes, tw);
            WriteMethods(bes, tw);
            mem.Position = 0;
            var buf = sha1.ComputeHash(mem);
            var hash = 0;
            for (var i = 7; i >= 0; i--)
            {
                hash <<= 8;
                hash |= buf[i];
            }

            return hash;
        }

        static void WriteClassName(BigEndianStream bes, TypeWrapper tw)
        {
            bes.WriteUtf8(tw.Name);
        }

        static void WriteModifiers(BigEndianStream bes, TypeWrapper tw)
        {
            var mods = tw.ReflectiveModifiers & (Modifiers.Public | Modifiers.Final | Modifiers.Interface | Modifiers.Abstract);
            if ((mods & Modifiers.Interface) != 0)
            {
                mods &= ~Modifiers.Abstract;
                if (HasJavaMethods(tw))
                    mods |= Modifiers.Abstract;
            }

            bes.WriteUInt32((uint)mods);
        }

        static bool HasJavaMethods(TypeWrapper tw)
        {
            return tw.GetMethods().Any(i => !i.IsHideFromReflection && !i.IsClassInitializer);
        }

        static void WriteInterfaces(BigEndianStream bes, TypeWrapper tw)
        {
            foreach (var i in tw.Interfaces.OrderBy(i => i.Name))
                bes.WriteUtf8(i.Name);
        }

        static void WriteFields(BigEndianStream bes, TypeWrapper tw)
        {
            foreach (var fw in tw.GetFields().Where(i => !i.IsHideFromReflection).OrderBy(i => i.Name))
            {
                var mods = fw.Modifiers & (Modifiers.Public | Modifiers.Private | Modifiers.Protected | Modifiers.Static | Modifiers.Final | Modifiers.Volatile | Modifiers.Transient);
                if (((mods & Modifiers.Private) == 0) || ((mods & (Modifiers.Static | Modifiers.Transient)) == 0))
                {
                    bes.WriteUtf8(fw.Name);
                    bes.WriteUInt32((uint)mods);
                    bes.WriteUtf8(fw.Signature.Replace('.', '/'));
                }
            }
        }

        static void WriteStaticInitializer(BigEndianStream bes, TypeWrapper tw)
        {
            var type = tw.TypeAsTBD;
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

        static void WriteConstructors(BigEndianStream bes, TypeWrapper tw)
        {
            var ctors = tw.GetMethods()
                .Where(i => i.IsConstructor && !i.IsHideFromReflection && !i.IsPrivate)
                .OrderBy(i => i.Signature);

            foreach (var ctor in ctors)
            {
                var mods = ctor.Modifiers & (Modifiers.Public | Modifiers.Private | Modifiers.Protected | Modifiers.Static | Modifiers.Final | Modifiers.Synchronized | Modifiers.Native | Modifiers.Abstract | Modifiers.Strictfp);
                bes.WriteUtf8(ctor.Name);
                bes.WriteUInt32((uint)mods);
                bes.WriteUtf8(ctor.Signature);
            }
        }

        static void WriteMethods(BigEndianStream bes, TypeWrapper tw)
        {
            var methods = tw.GetMethods()
                .Where(i => !i.IsConstructor && !i.IsHideFromReflection && !i.IsPrivate)
                .OrderBy(i => i.Name)
                .ThenBy(i => i.Signature);

            foreach (var method in methods)
            {
                var mods = method.Modifiers & (Modifiers.Public | Modifiers.Private | Modifiers.Protected | Modifiers.Static | Modifiers.Final | Modifiers.Synchronized | Modifiers.Native | Modifiers.Abstract | Modifiers.Strictfp);
                bes.WriteUtf8(method.Name);
                bes.WriteUInt32((uint)mods);
                bes.WriteUtf8(method.Signature);
            }
        }

    }

}
