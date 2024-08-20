using System.Linq;

using IKVM.Attributes;

namespace IKVM.Runtime.StubGen
{

    /// <summary>
    /// Generates a SerialVerisonUID value by hashing data related to the type.
    /// </summary>
    static class SerialVersionUID
    {

        /// <summary>
        /// Computes the SerialVerisonUID for the given Java type.
        /// </summary>
        /// <param name="javaType"></param>
        /// <returns></returns>
        internal static long Compute(RuntimeJavaType javaType)
        {
            // dump class structure into hash writer
            var writer = new SHA1HashWriter();
            WriteClassName(writer, javaType);
            WriteModifiers(writer, javaType);
            WriteInterfaces(writer, javaType);
            WriteFields(writer, javaType);
            WriteStaticInitializer(writer, javaType);
            WriteConstructors(writer, javaType);
            WriteMethods(writer, javaType);

            // finalize and convert to long
            var hash = writer.Finalize();
            var uuid = 0;
            for (var i = 7; i >= 0; i--)
            {
                uuid <<= 8;
                uuid |= hash[i];
            }

            return uuid;
        }

        static void WriteClassName(SHA1HashWriter writer, RuntimeJavaType javaType)
        {
            writer.WriteUtf8(javaType.Name);
        }

        static void WriteModifiers(SHA1HashWriter writer, RuntimeJavaType javaType)
        {
            var mods = javaType.ReflectiveModifiers & (Modifiers.Public | Modifiers.Final | Modifiers.Interface | Modifiers.Abstract);
            if ((mods & Modifiers.Interface) != 0)
            {
                mods &= ~Modifiers.Abstract;
                if (HasJavaMethods(javaType))
                    mods |= Modifiers.Abstract;
            }

            writer.WriteUInt32((uint)mods);
        }

        static bool HasJavaMethods(RuntimeJavaType javaType)
        {
            return javaType.GetMethods().Any(i => !i.IsHideFromReflection && !i.IsClassInitializer);
        }

        static void WriteInterfaces(SHA1HashWriter writer, RuntimeJavaType javaType)
        {
            foreach (var i in javaType.Interfaces.OrderBy(i => i.Name))
                writer.WriteUtf8(i.Name);
        }

        static void WriteFields(SHA1HashWriter writer, RuntimeJavaType javaType)
        {
            foreach (var fw in javaType.GetFields().Where(i => !i.IsHideFromReflection).OrderBy(i => i.Name))
            {
                var mods = fw.Modifiers & (Modifiers.Public | Modifiers.Private | Modifiers.Protected | Modifiers.Static | Modifiers.Final | Modifiers.Volatile | Modifiers.Transient);
                if (((mods & Modifiers.Private) == 0) || ((mods & (Modifiers.Static | Modifiers.Transient)) == 0))
                {
                    writer.WriteUtf8(fw.Name);
                    writer.WriteUInt32((uint)mods);
                    writer.WriteUtf8(fw.Signature.Replace('.', '/'));
                }
            }
        }

        static void WriteStaticInitializer(SHA1HashWriter writer, RuntimeJavaType javaType)
        {
            var type = javaType.TypeAsTBD;
            if (!type.IsArray && type.TypeInitializer != null)
            {
                if (!javaType.Context.AttributeHelper.IsHideFromJava(type.TypeInitializer))
                {
                    writer.WriteUtf8("<clinit>");
                    writer.WriteUInt32((uint)Modifiers.Static);
                    writer.WriteUtf8("()V");
                }
            }
        }

        static void WriteConstructors(SHA1HashWriter writer, RuntimeJavaType javaType)
        {
            var ctors = javaType.GetMethods()
                .Where(i => i.IsConstructor && !i.IsHideFromReflection && !i.IsPrivate)
                .OrderBy(i => i.Signature);

            foreach (var ctor in ctors)
            {
                var mods = ctor.Modifiers & (Modifiers.Public | Modifiers.Private | Modifiers.Protected | Modifiers.Static | Modifiers.Final | Modifiers.Synchronized | Modifiers.Native | Modifiers.Abstract | Modifiers.Strictfp);
                writer.WriteUtf8(ctor.Name);
                writer.WriteUInt32((uint)mods);
                writer.WriteUtf8(ctor.Signature);
            }
        }

        static void WriteMethods(SHA1HashWriter writer, RuntimeJavaType javaType)
        {
            var methods = javaType.GetMethods()
                .Where(i => !i.IsConstructor && !i.IsHideFromReflection && !i.IsPrivate)
                .OrderBy(i => i.Name)
                .ThenBy(i => i.Signature);

            foreach (var method in methods)
            {
                var mods = method.Modifiers & (Modifiers.Public | Modifiers.Private | Modifiers.Protected | Modifiers.Static | Modifiers.Final | Modifiers.Synchronized | Modifiers.Native | Modifiers.Abstract | Modifiers.Strictfp);
                writer.WriteUtf8(method.Name);
                writer.WriteUInt32((uint)mods);
                writer.WriteUtf8(method.Signature);
            }
        }

    }

}
