using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using IKVM.ByteCode;
using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Encoding;
using IKVM.CoreLib.Jvm;
using IKVM.CoreLib.Linking;
using IKVM.CoreLib.Runtime;
using IKVM.CoreLib.Tests.Linking;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ClassFile = IKVM.CoreLib.Linking.ClassFile<IKVM.CoreLib.Tests.Linking.TestJvmTypeSystem, IKVM.CoreLib.Tests.Linking.TestJavaType, IKVM.CoreLib.Tests.Linking.TestJavaMember, IKVM.CoreLib.Tests.Linking.TestJavaField, IKVM.CoreLib.Tests.Linking.TestJavaMethod>;
using CodeInfo = IKVM.CoreLib.Jvm.CodeInfo<IKVM.CoreLib.Tests.Linking.TestJvmTypeSystem, IKVM.CoreLib.Tests.Linking.TestJavaType, IKVM.CoreLib.Tests.Linking.TestJavaMember, IKVM.CoreLib.Tests.Linking.TestJavaField, IKVM.CoreLib.Tests.Linking.TestJavaMethod>;
using ConstantPoolItemMI = IKVM.CoreLib.Linking.ConstantPoolItemMI<IKVM.CoreLib.Tests.Linking.TestJvmTypeSystem, IKVM.CoreLib.Tests.Linking.TestJavaType, IKVM.CoreLib.Tests.Linking.TestJavaMember, IKVM.CoreLib.Tests.Linking.TestJavaField, IKVM.CoreLib.Tests.Linking.TestJavaMethod>;
using InstructionState = IKVM.CoreLib.Jvm.InstructionState<IKVM.CoreLib.Tests.Linking.TestJvmTypeSystem, IKVM.CoreLib.Tests.Linking.TestJavaType, IKVM.CoreLib.Tests.Linking.TestJavaMember, IKVM.CoreLib.Tests.Linking.TestJavaField, IKVM.CoreLib.Tests.Linking.TestJavaMethod>;
using Method = IKVM.CoreLib.Linking.Method<IKVM.CoreLib.Tests.Linking.TestJvmTypeSystem, IKVM.CoreLib.Tests.Linking.TestJavaType, IKVM.CoreLib.Tests.Linking.TestJavaMember, IKVM.CoreLib.Tests.Linking.TestJavaField, IKVM.CoreLib.Tests.Linking.TestJavaMethod>;
using MethodVerifier = IKVM.CoreLib.Jvm.MethodVerifier<IKVM.CoreLib.Tests.Linking.TestJvmTypeSystem, IKVM.CoreLib.Tests.Linking.TestJavaType, IKVM.CoreLib.Tests.Linking.TestJavaMember, IKVM.CoreLib.Tests.Linking.TestJavaField, IKVM.CoreLib.Tests.Linking.TestJavaMethod>;

namespace IKVM.CoreLib.Tests.Jvm
{

    /// <summary>
    /// Verifies real bytecode with the <see cref="MethodVerifier"/>, without a runtime: the classes are built with the
    /// bytecode encoder, parsed and linked against <see cref="TestJvmTypeSystem"/>.
    /// </summary>
    [TestClass]
    public class MethodVerifierTests
    {

        const byte T_INT = 10;

        static readonly TestJavaType Subject = new("Subject", TestTypes.Object);

        /// <summary>
        /// Builds a class named 'Subject' that extends 'java.lang.Object'.
        /// </summary>
        sealed class SubjectBuilder
        {

            readonly ClassFileBuilder _builder = new ClassFileBuilder(49, AccessFlag.Public, "Subject", "java/lang/Object");

            public ConstantBuilder Constants => _builder.Constants;

            public ClassConstantHandle Class(string name) => Constants.GetOrAddClass(name);

            public void AddMethod(AccessFlag flags, string name, string signature, ushort maxStack, ushort maxLocals, Action<CodeBuilder, List<(ushort Start, ushort End, ushort Handler, ClassConstantHandle CatchType)>> emit)
            {
                var buffer = new BlobBuilder();
                var handlers = new List<(ushort Start, ushort End, ushort Handler, ClassConstantHandle CatchType)>();
                emit(new CodeBuilder(buffer), handlers);

                var attributes = new AttributeTableBuilder(_builder.Constants);
                attributes.Code(maxStack, maxLocals, buffer, e =>
                {
                    foreach (var h in handlers)
                        e = e.Exception(h.Start, h.End, h.Handler, h.CatchType);
                }, null);

                _builder.AddMethod(flags, name, signature, attributes);
            }

            public void AddStatic(string name, string signature, ushort maxStack, ushort maxLocals, Action<CodeBuilder> emit)
            {
                AddMethod(AccessFlag.Public | AccessFlag.Static, name, signature, maxStack, maxLocals, (code, handlers) => emit(code));
            }

            public ClassFile Link(TestJvmTypeSystem typeSystem)
            {
                var buffer = new BlobBuilder();
                _builder.Serialize(buffer);
                var classFile = new ClassFile(typeSystem, IKVM.ByteCode.Decoding.ClassFile.Read(buffer.ToArray()), "Subject", ClassFileParseOptions.None, []);
                classFile.Link(Subject, LoadMode.Link);
                return classFile;
            }

        }

        /// <summary>
        /// A verifier that reports a fixed type for the result of every invocation, to test the intrinsic hook.
        /// </summary>
        sealed class IntrinsicVerifier : MethodVerifier
        {

            readonly TestJavaType _intrinsicReturnType;

            public IntrinsicVerifier(TestJvmTypeSystem typeSystem, ClassFile classFile, Method method, TestJavaType[] argTypes, TestJavaType returnType, TestJavaType intrinsicReturnType) :
                base(typeSystem, null, Subject, classFile, method, argTypes, returnType)
            {
                _intrinsicReturnType = intrinsicReturnType;
            }

            public List<string> Calls { get; } = new List<string>();

            protected override TestJavaType? GetIntrinsicReturnType(ConstantPoolItemMI cpi, ref InstructionState state)
            {
                Calls.Add(cpi.Name);
                return _intrinsicReturnType;
            }

        }

        static Method GetMethod(ClassFile classFile, string name, string signature)
        {
            signature = signature.Replace('/', '.');
            return classFile.Methods.Single(i => i.Name == name && i.Signature == signature);
        }

        /// <summary>
        /// Verifies the given method of the class, as a known method.
        /// </summary>
        static CodeInfo Verify(SubjectBuilder builder, string name, string signature)
        {
            var typeSystem = new TestJvmTypeSystem(false, Subject);
            var classFile = builder.Link(typeSystem);
            var method = GetMethod(classFile, name, signature);
            var argTypes = typeSystem.GetArgTypeListFromSignature(Subject, signature, LoadMode.Link);
            var returnType = typeSystem.GetReturnTypeFromSignature(Subject, signature, LoadMode.Link);
            var verifier = new MethodVerifier(typeSystem, null, Subject, classFile, method, argTypes, returnType);
            verifier.Verify();
            return verifier.GetCodeInfo();
        }

        /// <summary>
        /// Verifies a static method that consists of the given code.
        /// </summary>
        static CodeInfo VerifyStatic(string signature, ushort maxStack, ushort maxLocals, Action<CodeBuilder> emit)
        {
            var builder = new SubjectBuilder();
            builder.AddStatic("m", signature, maxStack, maxLocals, emit);
            return Verify(builder, "m", signature);
        }

        static Action Verifying(string signature, ushort maxStack, ushort maxLocals, Action<CodeBuilder> emit)
        {
            return () => VerifyStatic(signature, maxStack, maxLocals, emit);
        }

        [TestMethod]
        public void ArithmeticOnArgumentsVerifies()
        {
            var info = VerifyStatic("(II)I", 2, 2, code => code.Iload0().Iload1().Iadd().Ireturn());
            info.GetStackHeight(0).Should().Be(0);
            info.GetStackHeight(2).Should().Be(2);
            info.GetStackHeight(3).Should().Be(1);
            info.GetRawStackType(3, 0).Should().Be(VerificationType<TestJavaType>.Int);
            info.GetLocalType(0, 0).Should().Be(VerificationType<TestJavaType>.Int);
            info.GetLocalType(0, 1).Should().Be(VerificationType<TestJavaType>.Int);
        }

        [TestMethod]
        public void CategoryTwoArgumentsTakeTwoLocals()
        {
            var info = VerifyStatic("(JI)I", 1, 3, code => code.Iload2().Ireturn());
            info.GetLocalType(0, 0).Should().Be(VerificationType<TestJavaType>.Long);
            info.GetLocalType(0, 2).Should().Be(VerificationType<TestJavaType>.Int);
        }

        [TestMethod]
        public void ReferenceArgumentsAreTypedLocals()
        {
            var info = VerifyStatic("(Ljava/lang/String;)Ljava/lang/Object;", 1, 1, code => code.Aload0().Areturn());
            info.GetLocalType(0, 0).Type.Should().BeSameAs(TestTypes.String);
            info.GetRawStackType(1, 0).Type.Should().BeSameAs(TestTypes.String);
        }

        [TestMethod]
        public void ReturningNullAsIntFails()
        {
            Verifying("()I", 1, 0, code => code.AconstNull().Areturn()).Should().Throw<VerifyException>().WithMessage("Wrong return type in function *");
        }

        [TestMethod]
        public void ReturningIntFromVoidMethodFails()
        {
            Verifying("()V", 1, 0, code => code.Iconst0().Ireturn()).Should().Throw<VerifyException>().WithMessage("Wrong return type in function *");
        }

        [TestMethod]
        public void ReturningVoidFromIntMethodFails()
        {
            Verifying("()I", 0, 0, code => code.Return()).Should().Throw<VerifyException>().WithMessage("Wrong return type in function *");
        }

        [TestMethod]
        public void ReturningLongFromIntMethodFails()
        {
            Verifying("()I", 2, 0, code => code.Lconst0().Lreturn()).Should().Throw<VerifyException>().WithMessage("Wrong return type in function *");
        }

        [TestMethod]
        public void ReturningIntFromBooleanMethodVerifies()
        {
            VerifyStatic("()Z", 1, 0, code => code.Iconst1().Ireturn());
        }

        [TestMethod]
        public void ReturningIncompatibleReferenceFails()
        {
            Verifying("(Ljava/lang/Integer;)Ljava/lang/String;", 1, 1, code => code.Aload0().Areturn()).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void ReturningSubclassVerifies()
        {
            VerifyStatic("(Ljava/lang/Integer;)Ljava/lang/Number;", 1, 1, code => code.Aload0().Areturn());
        }

        [TestMethod]
        public void AddingIntToFloatFails()
        {
            Verifying("(IF)I", 2, 2, code => code.Iload0().Fload1().Iadd().Ireturn()).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void LoadingLocalOfWrongTypeFails()
        {
            Verifying("(I)F", 1, 1, code => code.Fload0().Freturn()).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void LoadingUnsetLocalFails()
        {
            Verifying("()I", 1, 1, code => code.Iload0().Ireturn()).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void ExceedingMaxStackFails()
        {
            Verifying("()I", 1, 0, code => code.Iconst0().Iconst1().Iadd().Ireturn()).Should().Throw<VerifyException>().WithMessage("Stack overflow *");
        }

        [TestMethod]
        public void PoppingEmptyStackFails()
        {
            Verifying("()V", 1, 0, code => code.Pop().Return()).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void FallingOffTheEndFails()
        {
            Verifying("()V", 1, 0, code => code.Iconst0().Pop()).Should().Throw<VerifyException>().WithMessage("Falling off the end of the code *");
        }

        [TestMethod]
        public void BranchesMergeToCommonBaseClass()
        {
            var info = VerifyStatic("(ZLjava/lang/Integer;Ljava/lang/Double;)Ljava/lang/Number;", 1, 3, code =>
            {
                var other = code.DefineLabel();
                var end = code.DefineLabel();
                code.Iload0();
                code.Ifeq(other);
                code.Aload1();
                code.Goto(end);
                code.MarkLabel(other);
                code.Aload2();
                code.MarkLabel(end);
                code.Areturn();
            });

            // iload_0, ifeq, aload_1, goto, aload_2, areturn
            info.GetRawStackType(5, 0).Type.Should().BeSameAs(TestTypes.Number);
        }

        [TestMethod]
        public void BranchesMergingIntAndFloatLeaveUnusableLocal()
        {
            Verifying("(Z)I", 1, 2, code =>
            {
                var other = code.DefineLabel();
                var end = code.DefineLabel();
                code.Iload0();
                code.Ifeq(other);
                code.Iconst0().Istore1();
                code.Goto(end);
                code.MarkLabel(other);
                code.Fconst0().Fstore1();
                code.MarkLabel(end);
                code.Iload1().Ireturn();
            }).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void BranchesMergingDifferentStackHeightsFail()
        {
            Verifying("(Z)V", 2, 1, code =>
            {
                var end = code.DefineLabel();
                code.Iconst0();
                code.Iload0();
                code.Ifeq(end);
                code.Pop();
                code.MarkLabel(end);
                code.Return();
            }).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void LoopVerifies()
        {
            var info = VerifyStatic("(I)I", 1, 1, code =>
            {
                var top = code.DefineLabel();
                var end = code.DefineLabel();
                code.MarkLabel(top);
                code.Iload0();
                code.Ifeq(end);
                code.Iinc(0, -1);
                code.Goto(top);
                code.MarkLabel(end);
                code.Iload0();
                code.Ireturn();
            });

            info.GetLocalType(0, 0).Should().Be(VerificationType<TestJavaType>.Int);
        }

        [TestMethod]
        public void NewObjectIsUninitializedUntilConstructed()
        {
            var builder = new SubjectBuilder();
            builder.AddStatic("m", "()Ljava/lang/Object;", 2, 0, code => code
                .New(builder.Class("java/lang/Object"))
                .Dup()
                .InvokeSpecial(builder.Constants.GetOrAddMethodref("java/lang/Object", "<init>", "()V"))
                .Areturn());

            var info = Verify(builder, "m", "()Ljava/lang/Object;");

            // new, dup, invokespecial, areturn
            var uninitialized = info.GetRawStackType(1, 0);
            uninitialized.IsUninitializedNew.Should().BeTrue();
            uninitialized.Index.Should().Be(0);
            uninitialized.Type.Should().BeSameAs(TestTypes.Object);

            info.GetRawStackType(3, 0).IsUninitializedNew.Should().BeFalse();
            info.GetRawStackType(3, 0).Type.Should().BeSameAs(TestTypes.Object);
        }

        [TestMethod]
        public void ReturningUninitializedObjectFails()
        {
            var builder = new SubjectBuilder();
            builder.AddStatic("m", "()Ljava/lang/Object;", 1, 0, code => code.New(builder.Class("java/lang/Object")).Areturn());
            builder.Invoking(b => Verify(b, "m", "()Ljava/lang/Object;")).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void ConstructingWithWrongClassFails()
        {
            var builder = new SubjectBuilder();
            builder.AddStatic("m", "()V", 2, 0, code => code
                .New(builder.Class("java/lang/Object"))
                .InvokeSpecial(builder.Constants.GetOrAddMethodref("java/lang/String", "<init>", "()V"))
                .Return());

            builder.Invoking(b => Verify(b, "m", "()V")).Should().Throw<VerifyException>().WithMessage("Call to wrong initialization method *");
        }

        [TestMethod]
        public void ConstructorThatCallsSuperVerifies()
        {
            var builder = new SubjectBuilder();
            builder.AddMethod(AccessFlag.Public, "<init>", "()V", 1, 1, (code, handlers) => code
                .Aload0()
                .InvokeSpecial(builder.Constants.GetOrAddMethodref("java/lang/Object", "<init>", "()V"))
                .Return());

            var info = Verify(builder, "<init>", "()V");
            info.GetLocalType(0, 0).IsUninitializedThis.Should().BeTrue();
            info.GetLocalType(2, 0).IsThis.Should().BeTrue();
            info.GetLocalType(2, 0).Type.Should().BeSameAs(Subject);
        }

        [TestMethod]
        public void ConstructorThatDoesNotCallSuperFails()
        {
            var builder = new SubjectBuilder();
            builder.AddMethod(AccessFlag.Public, "<init>", "()V", 0, 1, (code, handlers) => code.Return());
            builder.Invoking(b => Verify(b, "<init>", "()V")).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void InstanceMethodHasThisInFirstLocal()
        {
            var builder = new SubjectBuilder();
            builder.AddMethod(AccessFlag.Public, "m", "(I)Ljava/lang/Object;", 1, 2, (code, handlers) => code.Aload0().Areturn());

            var info = Verify(builder, "m", "(I)Ljava/lang/Object;");
            info.GetLocalType(0, 0).IsThis.Should().BeTrue();
            info.GetLocalType(0, 0).Type.Should().BeSameAs(Subject);
            info.GetLocalType(0, 1).Should().Be(VerificationType<TestJavaType>.Int);
        }

        [TestMethod]
        public void InvokingInitializerWithInvokevirtualFails()
        {
            var builder = new SubjectBuilder();
            builder.AddStatic("m", "(Ljava/lang/Object;)V", 1, 1, code => code
                .Aload0()
                .InvokeVirtual(builder.Constants.GetOrAddMethodref("java/lang/Object", "<init>", "()V"))
                .Return());

            builder.Invoking(b => Verify(b, "m", "(Ljava/lang/Object;)V")).Should().Throw<VerifyException>().WithMessage("Must call initializers using invokespecial *");
        }

        [TestMethod]
        public void InvokingWithIncompatibleArgumentFails()
        {
            var builder = new SubjectBuilder();
            builder.AddStatic("m", "(Ljava/lang/Integer;)I", 1, 1, code => code
                .Aload0()
                .InvokeVirtual(builder.Constants.GetOrAddMethodref("java/lang/String", "length", "()I"))
                .Ireturn());

            builder.Invoking(b => Verify(b, "m", "(Ljava/lang/Integer;)I")).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void InvocationPushesDeclaredReturnType()
        {
            var builder = new SubjectBuilder();
            builder.AddStatic("m", "(Ljava/lang/Object;)Ljava/lang/Object;", 1, 1, code => code
                .Aload0()
                .InvokeVirtual(builder.Constants.GetOrAddMethodref("java/lang/Object", "toString", "()Ljava/lang/String;"))
                .Areturn());

            var info = Verify(builder, "m", "(Ljava/lang/Object;)Ljava/lang/Object;");
            info.GetRawStackType(2, 0).Type.Should().BeSameAs(TestTypes.String);
        }

        [TestMethod]
        public void IntrinsicHookOverridesReturnType()
        {
            var builder = new SubjectBuilder();
            builder.AddStatic("m", "(Ljava/lang/Object;)Ljava/lang/Object;", 1, 1, code => code
                .Aload0()
                .InvokeVirtual(builder.Constants.GetOrAddMethodref("java/lang/Object", "toString", "()Ljava/lang/String;"))
                .Areturn());

            var typeSystem = new TestJvmTypeSystem(false, Subject);
            var classFile = builder.Link(typeSystem);
            var method = GetMethod(classFile, "m", "(Ljava/lang/Object;)Ljava/lang/Object;");
            var verifier = new IntrinsicVerifier(typeSystem, classFile, method, [TestTypes.Object], TestTypes.Object, TestTypes.Integer);
            verifier.Verify();

            verifier.Calls.Should().Contain("toString");
            verifier.GetCodeInfo().GetRawStackType(2, 0).Type.Should().BeSameAs(TestTypes.Integer);
        }

        [TestMethod]
        public void NewarrayPushesPrimitiveArray()
        {
            var info = VerifyStatic("()Ljava/lang/Object;", 1, 0, code => code.Iconst1().Newarray(T_INT).Areturn());
            info.GetRawStackType(2, 0).Type.Should().BeSameAs(TestTypes.PrimitiveInt.MakeArrayType());
        }

        [TestMethod]
        public void IaloadOnIntArrayVerifies()
        {
            VerifyStatic("([I)I", 2, 1, code => code.Aload0().Iconst0().Iaload().Ireturn());
        }

        [TestMethod]
        public void BaloadOnIntArrayFails()
        {
            Verifying("([I)I", 2, 1, code => code.Aload0().Iconst0().Baload().Ireturn()).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void AaloadPushesElementType()
        {
            var info = VerifyStatic("([Ljava/lang/String;)Ljava/lang/Object;", 2, 1, code => code.Aload0().Iconst0().Aaload().Areturn());
            info.GetRawStackType(3, 0).Type.Should().BeSameAs(TestTypes.String);
        }

        [TestMethod]
        public void AaloadOnPrimitiveArrayFails()
        {
            Verifying("([I)Ljava/lang/Object;", 2, 1, code => code.Aload0().Iconst0().Aaload().Areturn()).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void LdcStringPushesString()
        {
            var builder = new SubjectBuilder();
            builder.AddStatic("m", "()Ljava/lang/Object;", 1, 0, code => code.Ldc(builder.Constants.GetOrAddString("s")).Areturn());
            var info = Verify(builder, "m", "()Ljava/lang/Object;");
            info.GetRawStackType(1, 0).Type.Should().BeSameAs(TestTypes.String);
        }

        [TestMethod]
        public void CheckcastNarrowsType()
        {
            var builder = new SubjectBuilder();
            builder.AddStatic("m", "(Ljava/lang/Object;)Ljava/lang/String;", 1, 1, code => code.Aload0().Checkcast(builder.Class("java/lang/String")).Areturn());
            var info = Verify(builder, "m", "(Ljava/lang/Object;)Ljava/lang/String;");
            info.GetRawStackType(2, 0).Type.Should().BeSameAs(TestTypes.String);
        }

        [TestMethod]
        public void UnloadableTypesAreKeptByName()
        {
            var builder = new SubjectBuilder();
            builder.AddStatic("m", "(Ljava/lang/Object;)Ljava/lang/Object;", 1, 1, code => code.Aload0().Checkcast(builder.Class("not/There")).Areturn());
            var info = Verify(builder, "m", "(Ljava/lang/Object;)Ljava/lang/Object;");
            var type = info.GetRawStackType(2, 0);
            type.IsReference.Should().BeTrue();
            type.Type.IsUnloadable.Should().BeTrue();
            type.Type.Name.Should().Be("not.There");
        }

        [TestMethod]
        public void TypedHandlerStartsWithCaughtException()
        {
            var builder = new SubjectBuilder();
            builder.AddMethod(AccessFlag.Public | AccessFlag.Static, "m", "(Ljava/lang/Object;)V", 1, 1, (code, handlers) =>
            {
                var start = code.Offset;
                code.Aload0();
                code.InvokeVirtual(builder.Constants.GetOrAddMethodref("java/lang/Object", "hashCode", "()I"));
                code.Pop();
                var end = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.Pop();
                code.Return();
                handlers.Add((start, end, handler, builder.Class("java/lang/RuntimeException")));
            });

            // aload_0, invokevirtual, pop, return, pop (handler), return
            var info = Verify(builder, "m", "(Ljava/lang/Object;)V");
            info.GetStackHeight(4).Should().Be(1);
            info.GetRawStackType(4, 0).Type.Should().BeSameAs(TestTypes.RuntimeException);
            info.GetLocalType(4, 0).Type.Should().BeSameAs(TestTypes.Object);
        }

        [TestMethod]
        public void CatchAnyHandlerStartsWithFaultBlockException()
        {
            var builder = new SubjectBuilder();
            builder.AddMethod(AccessFlag.Public | AccessFlag.Static, "m", "(Ljava/lang/Object;)V", 1, 1, (code, handlers) =>
            {
                var start = code.Offset;
                code.Aload0();
                code.InvokeVirtual(builder.Constants.GetOrAddMethodref("java/lang/Object", "hashCode", "()I"));
                code.Pop();
                var end = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.Athrow();
                handlers.Add((start, end, handler, default));
            });

            var info = Verify(builder, "m", "(Ljava/lang/Object;)V");
            var exception = info.GetRawStackType(4, 0);
            exception.IsFaultBlockException.Should().BeTrue();
            exception.Index.Should().Be(4);
        }

        [TestMethod]
        public void HandlerLocalsMergeStatesFromWholeBlock()
        {
            var builder = new SubjectBuilder();
            builder.AddMethod(AccessFlag.Public | AccessFlag.Static, "m", "()I", 1, 1, (code, handlers) =>
            {
                var start = code.Offset;
                code.Iconst0().Istore0();
                code.Fconst0().Fstore0();
                var end = code.Offset;
                code.Iconst0().Ireturn();
                var handler = code.Offset;
                code.Pop();
                code.Iload0().Ireturn();
                handlers.Add((start, end, handler, builder.Class("java/lang/Throwable")));
            });

            // the handler can be entered with local 0 holding nothing, an int or a float, so it cannot load it as an int
            builder.Invoking(b => Verify(b, "m", "()I")).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void ExceptionBlockEndingInsideInstructionIsClassFormatError()
        {
            var builder = new SubjectBuilder();
            builder.AddMethod(AccessFlag.Public | AccessFlag.Static, "m", "()V", 1, 0, (code, handlers) =>
            {
                code.Sipush(1000);
                code.Pop();
                var handler = code.Offset;
                code.Return();

                // sipush is three bytes long, so offset 1 is in the middle of it
                handlers.Add((0, 1, handler, default));
            });

            builder.Invoking(b => Verify(b, "m", "()V")).Should().Throw<ClassFormatException>().WithMessage("Illegal exception table*");
        }

        [TestMethod]
        public void HasValidExceptionTableAcceptsWellFormedTable()
        {
            var builder = new SubjectBuilder();
            builder.AddMethod(AccessFlag.Public | AccessFlag.Static, "m", "()V", 1, 0, (code, handlers) =>
            {
                var start = code.Offset;
                code.Iconst0().Pop();
                var end = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.Pop().Return();
                handlers.Add((start, end, handler, default));
            });

            var classFile = builder.Link(new TestJvmTypeSystem(false, Subject));
            MethodVerifier.HasValidExceptionTable(GetMethod(classFile, "m", "()V")).Should().BeTrue();
        }

        [TestMethod]
        public void UnknownMethodSkipsReturnTypeChecks()
        {
            var builder = new SubjectBuilder();
            builder.AddStatic("m", "()I", 0, 0, code => code.Return());

            var typeSystem = new TestJvmTypeSystem(false, Subject);
            var classFile = builder.Link(typeSystem);
            var verifier = new MethodVerifier(typeSystem, null, Subject, classFile, GetMethod(classFile, "m", "()I"), null, null);
            verifier.Verify();
            verifier.GetCodeInfo().HasState(0).Should().BeTrue();
        }

        [TestMethod]
        public void UnknownMethodStillVerifiesStack()
        {
            var builder = new SubjectBuilder();
            builder.AddStatic("m", "()V", 1, 0, code => code.Pop().Return());

            var typeSystem = new TestJvmTypeSystem(false, Subject);
            var classFile = builder.Link(typeSystem);
            var verifier = new MethodVerifier(typeSystem, null, Subject, classFile, GetMethod(classFile, "m", "()V"), null, null);
            verifier.Invoking(v => v.Verify()).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void UnreachableCodeHasNoState()
        {
            var info = VerifyStatic("()V", 1, 0, code => code.Return().Iconst0().Pop().Return());
            info.HasState(0).Should().BeTrue();
            info.HasState(1).Should().BeFalse();
            info.HasState(2).Should().BeFalse();
        }

        [TestMethod]
        public void DupX1ReordersStack()
        {
            var builder = new SubjectBuilder();
            builder.AddStatic("m", "(ILjava/lang/String;)V", 3, 2, code => code.Iload0().Aload1().DupX1().Pop().Pop().Pop().Return());
            var info = Verify(builder, "m", "(ILjava/lang/String;)V");

            // after dup_x1 the stack is string, int, string from the bottom; position 0 is the top
            info.GetRawStackType(3, 0).Type.Should().BeSameAs(TestTypes.String);
            info.GetRawStackType(3, 1).Should().Be(VerificationType<TestJavaType>.Int);
            info.GetRawStackType(3, 2).Type.Should().BeSameAs(TestTypes.String);
        }

        [TestMethod]
        public void SwappingCategoryTwoValueFails()
        {
            Verifying("(J)V", 3, 2, code => code.Iconst0().Lload0().Swap().Return()).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void ArraylengthOnNonArrayFails()
        {
            Verifying("(Ljava/lang/Object;)I", 1, 1, code => code.Aload0().Arraylength().Ireturn()).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void AthrowOfNonThrowableFails()
        {
            Verifying("(Ljava/lang/String;)V", 1, 1, code => code.Aload0().Athrow()).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void AthrowOfExceptionVerifies()
        {
            VerifyStatic("(Ljava/lang/RuntimeException;)V", 1, 1, code => code.Aload0().Athrow());
        }

    }

}
