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
            public Type Load(ClassFileBuilder builder, string name)
            {
                var buffer = new BlobBuilder();
                builder.Serialize(buffer);

                var clazz = defineClass(name, buffer.ToArray(), 0, buffer.Count);
                return global::ikvm.runtime.Util.getRuntimeTypeFromClass(clazz);
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
            AddMethod(builder, AccessFlag.Public | AccessFlag.Static, "Method", "()Ljava/lang/Object;", c => c
                .New(builder.Constants.GetOrAddClass("java/lang/Object"))
                .Dup()
                .InvokeSpecial(builder.Constants.GetOrAddMethodref(builder.Constants.GetOrAddClass("java/lang/Object"), builder.Constants.GetOrAddNameAndType("<init>", "()V")))
                .Areturn(), 2, 0);

            var cldr = new ByteArrayClassLoader();
            var type = cldr.Load(builder, "Test");

            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod("Method", []);
            var result = method.Invoke(instance, []);
            result.Should().BeOfType<global::java.lang.Object>();
        }

        [TestMethod]
        public void InvokeDynamicOneSameArg()
        {
            var builder = new ClassFileBuilder(52, AccessFlag.Public, "Test", "java/lang/Object");
            AddDefaultConstructor(builder);

            // bootstrap method invokes LambdaMetadataFactory.metafactory, passing the Test.lambda method as a reference
            builder.Attributes.BootstrapMethods(e => e.Method(
                builder.Constants.GetOrAddMethodHandle(
                    MethodHandleKind.InvokeStatic,
                    builder.Constants.GetOrAddMethodref(
                        "java/lang/invoke/LambdaMetafactory",
                        "metafactory",
                        "(Ljava/lang/invoke/MethodHandles$Lookup;Ljava/lang/String;Ljava/lang/invoke/MethodType;Ljava/lang/invoke/MethodType;Ljava/lang/invoke/MethodHandle;Ljava/lang/invoke/MethodType;)Ljava/lang/invoke/CallSite;")),
                    c => c
                        .Constant(builder.Constants.GetOrAddMethodType("(Ljava/lang/Object;)V"))
                        .Constant(builder.Constants.GetOrAddMethodHandle(
                            MethodHandleKind.InvokeStatic,
                            builder.Constants.GetOrAddMethodref(
                                "Test",
                                "Lambda",
                                "(Ljava/lang/Object;)V")))
                        .Constant(builder.Constants.GetOrAddMethodType("(Ljava/lang/Object;)V"))));

            // the InvokeDynamic method uses the actual InvokeDynamic instruction to create and return the Consumer
            AddMethod(builder, AccessFlag.Public | AccessFlag.Static, "CreateFunc", "()Ljava/util/function/Consumer;", code => code
                .InvokeDynamic(builder.Constants.GetOrAddInvokeDynamic(0, "accept", "()Ljava/util/function/Consumer;"), 0, 0)
                .Areturn(), 1, 0);

            // the lambda method is the actual Consumer accept impl
            AddMethod(builder, AccessFlag.Public | AccessFlag.Static, "Lambda", "(Ljava/lang/Object;)V", code => code
                .Aload0()
                .Pop()
                .Return(), 1, 1);

            var cldr = new ByteArrayClassLoader();
            var type = cldr.Load(builder, "Test");

            // create an instance of the type and invoke CreateFunc, which should return a Consumer
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod("CreateFunc", []);
            var func = method.Invoke(instance, []);
            func.Should().BeAssignableTo<global::java.util.function.Consumer>();

            // invoke the accept method on the Consumer
            var r = func.GetType().GetMethod("accept").Invoke(func, [new object()]);
            r.Should().BeNull();
        }

        [TestMethod]
        public void InvokeDynamicOneConvArg()
        {
            var builder = new ClassFileBuilder(52, AccessFlag.Public, "Test", "java/lang/Object");
            AddDefaultConstructor(builder);

            // bootstrap method invokes LambdaMetadataFactory.metafactory, passing the Test.lambda method as a reference
            builder.Attributes.BootstrapMethods(e => e.Method(
                builder.Constants.GetOrAddMethodHandle(
                    MethodHandleKind.InvokeStatic,
                    builder.Constants.GetOrAddMethodref(
                        "java/lang/invoke/LambdaMetafactory",
                        "metafactory",
                        "(Ljava/lang/invoke/MethodHandles$Lookup;Ljava/lang/String;Ljava/lang/invoke/MethodType;Ljava/lang/invoke/MethodType;Ljava/lang/invoke/MethodHandle;Ljava/lang/invoke/MethodType;)Ljava/lang/invoke/CallSite;")),
                    c => c
                        .Constant(builder.Constants.GetOrAddMethodType("(Ljava/lang/Object;)V"))
                        .Constant(builder.Constants.GetOrAddMethodHandle(
                            MethodHandleKind.InvokeStatic,
                            builder.Constants.GetOrAddMethodref(
                                "Test",
                                "Lambda",
                                "(Ljava/lang/String;)V")))
                        .Constant(builder.Constants.GetOrAddMethodType("(Ljava/lang/String;)V"))));

            // the InvokeDynamic method uses the actual InvokeDynamic instruction to create and return the Consumer
            AddMethod(builder, AccessFlag.Public | AccessFlag.Static, "CreateFunc", "()Ljava/util/function/Consumer;", code => code
                .InvokeDynamic(builder.Constants.GetOrAddInvokeDynamic(0, "accept", "()Ljava/util/function/Consumer;"), 0, 0)
                .Areturn(), 1, 0);

            // the lambda method is the actual Consumer accept impl
            AddMethod(builder, AccessFlag.Public | AccessFlag.Static, "Lambda", "(Ljava/lang/String;)V", code => code
                .Aload0()
                .Pop()
                .Return(), 1, 1);

            var cldr = new ByteArrayClassLoader();
            var type = cldr.Load(builder, "Test");

            // create an instance of the type and invoke CreateFunc, which should return a Consumer
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod("CreateFunc", []);
            var func = method.Invoke(instance, []);
            func.Should().BeAssignableTo<global::java.util.function.Consumer>();

            // invoke the accept method on the Consumer
            var r = func.GetType().GetMethod("accept").Invoke(func, ["TEST"]);
            r.Should().BeNull();
        }

        [TestMethod]
        public void InvokeDynamicOneGhostInstantiationArg()
        {
            var builder = new ClassFileBuilder(52, AccessFlag.Public, "Test", "java/lang/Object");
            AddDefaultConstructor(builder);

            // bootstrap method invokes LambdaMetadataFactory.metafactory, passing the Test.lambda method as a reference
            builder.Attributes.BootstrapMethods(e => e.Method(
                builder.Constants.GetOrAddMethodHandle(
                    MethodHandleKind.InvokeStatic,
                    builder.Constants.GetOrAddMethodref(
                        "java/lang/invoke/LambdaMetafactory",
                        "metafactory",
                        "(Ljava/lang/invoke/MethodHandles$Lookup;Ljava/lang/String;Ljava/lang/invoke/MethodType;Ljava/lang/invoke/MethodType;Ljava/lang/invoke/MethodHandle;Ljava/lang/invoke/MethodType;)Ljava/lang/invoke/CallSite;")),
                    c => c
                        .Constant(builder.Constants.GetOrAddMethodType("(Ljava/lang/Object;)V"))
                        .Constant(builder.Constants.GetOrAddMethodHandle(
                            MethodHandleKind.InvokeStatic,
                            builder.Constants.GetOrAddMethodref(
                                "Test",
                                "Lambda",
                                "(Ljava/lang/CharSequence;)V")))
                        .Constant(builder.Constants.GetOrAddMethodType("(Ljava/lang/String;)V"))));

            // the InvokeDynamic method uses the actual InvokeDynamic instruction to create and return the Consumer
            AddMethod(builder, AccessFlag.Public | AccessFlag.Static, "CreateFunc", "()Ljava/util/function/Consumer;", code => code
                .InvokeDynamic(builder.Constants.GetOrAddInvokeDynamic(0, "accept", "()Ljava/util/function/Consumer;"), 0, 0)
                .Areturn(), 1, 0);

            // the lambda method is the actual Consumer accept impl
            AddMethod(builder, AccessFlag.Public | AccessFlag.Static, "Lambda", "(Ljava/lang/CharSequence;)V", code => code
                .Aload0()
                .Pop()
                .GetStatic(builder.Constants.GetOrAddFieldref("java/lang/System", "out", "Ljava/io/PrintStream;"))
                .LoadConstant(builder.Constants.GetOrAddString("HIT"))
                .InvokeVirtual(builder.Constants.GetOrAddMethodref("java/io/PrintStream", "println", "(Ljava/lang/String;)V"))
                .Return(), 2, 1);

            var cldr = new ByteArrayClassLoader();
            var type = cldr.Load(builder, "Test");

            // create an instance of the type and invoke CreateFunc, which should return a Consumer
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod("CreateFunc", []);
            var func = method.Invoke(instance, []);
            func.Should().BeAssignableTo<global::java.util.function.Consumer>();

            // invoke the accept method on the Consumer
            var r = func.GetType().GetMethod("accept").Invoke(func, ["TEST"]);
            r.Should().BeNull();
        }

    }

}
