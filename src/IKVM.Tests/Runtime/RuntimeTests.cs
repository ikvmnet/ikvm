using System;

using FluentAssertions;

using IKVM.ByteCode;
using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Encoding;

using java.lang;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime
{

    [TestClass]
    public class RuntimeTests
    {

        /// <summary>
        /// Simple <see cref="ClassLoader"/> implementation that loads a class from a builder.
        /// </summary>
        class ByteArrayClassLoader : ClassLoader
        {

            /// <summary>
            /// Loads the specified <see cref="ClassFileBuilder"/> as a class.
            /// </summary>
            /// <param name="clazz"></param>
            /// <param name="name"></param>
            /// <returns></returns>
            public Class Load(ClassFileBuilder clazz, string name)
            {
                var clazzBuffer = new BlobBuilder();
                clazz.Serialize(clazzBuffer);
                return defineClass(name, clazzBuffer.ToArray(), 0, clazzBuffer.Count);
            }

        }

        /// <summary>
        /// Adds a method and code.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="flags"></param>
        /// <param name="name"></param>
        /// <param name="signature"></param>
        /// <param name="code"></param>
        /// <param name="maxStack"></param>
        /// <param name="maxLocals"></param>
        void AddMethod(ClassFileBuilder clazz, AccessFlag flags, string name, string signature, Action<CodeBuilder> code, ushort maxStack, ushort maxLocals)
        {
            var attributes = new AttributeTableBuilder(clazz.Constants);
            var buffer = new BlobBuilder();
            code(new CodeBuilder(buffer));
            attributes.Code(maxStack, maxLocals, buffer, e => { }, null);
            clazz.AddMethod(flags, name, signature, attributes);
        }

        /// <summary>
        /// Adds a default constrctor.
        /// </summary>
        /// <param name="clazz"></param>
        void AddDefaultConstructor(ClassFileBuilder clazz)
        {
            AddMethod(clazz, AccessFlag.Public, "<init>", "()V", c => c
                .Aload0()
                .InvokeSpecial(clazz.Constants.GetOrAddMethodref(clazz.Constants.GetOrAddClass("java/lang/Object"), clazz.Constants.GetOrAddNameAndType("<init>", "()V")))
                .Return(), 1, 1);
        }

        [TestMethod]
        public void NewDupInvokeSpecialAReturn()
        {
            var builder = new ClassFileBuilder(52, AccessFlag.Public, "Test", "java/lang/Object");
            AddDefaultConstructor(builder);
            AddMethod(builder, AccessFlag.Public | AccessFlag.Static, "CreateObject", "()Ljava/lang/Object;", c => c
                .New(builder.Constants.GetOrAddClass("java/lang/Object"))
                .Dup()
                .InvokeSpecial(builder.Constants.GetOrAddMethodref(builder.Constants.GetOrAddClass("java/lang/Object"), builder.Constants.GetOrAddNameAndType("<init>", "()V")))
                .Areturn(), 2, 0);

            var cldr = new ByteArrayClassLoader();
            var clazz = cldr.Load(builder, "Test");

            var instance = clazz.newInstance();
            var method = clazz.getMethod("CreateObject", []);
            var result = method.invoke(instance, []);
            result.Should().BeOfType<global::java.lang.Object>();
        }

    }

}
