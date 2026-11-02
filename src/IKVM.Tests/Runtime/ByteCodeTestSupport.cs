using System;
using System.Collections.Generic;
using System.Linq;

using IKVM.ByteCode;
using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Encoding;
using IKVM.Runtime;

using IKVM.CoreLib.Linking;
using IKVM.CoreLib.Runtime;

using ClassFile = IKVM.CoreLib.Linking.ClassFile<IKVM.Runtime.RuntimeJvmTypeSystem, IKVM.Runtime.RuntimeJavaType, IKVM.Runtime.RuntimeJavaMember, IKVM.Runtime.RuntimeJavaField, IKVM.Runtime.RuntimeJavaMethod>;
using Instruction = IKVM.CoreLib.Linking.Instruction<IKVM.Runtime.RuntimeJvmTypeSystem, IKVM.Runtime.RuntimeJavaType, IKVM.Runtime.RuntimeJavaMember, IKVM.Runtime.RuntimeJavaField, IKVM.Runtime.RuntimeJavaMethod>;
using Method = IKVM.CoreLib.Linking.Method<IKVM.Runtime.RuntimeJvmTypeSystem, IKVM.Runtime.RuntimeJavaType, IKVM.Runtime.RuntimeJavaMember, IKVM.Runtime.RuntimeJavaField, IKVM.Runtime.RuntimeJavaMethod>;

namespace IKVM.Tests.Runtime
{

    /// <summary>
    /// Simple <see cref="java.lang.ClassLoader"/> implementation that defines a class from bytes.
    /// </summary>
    class ByteArrayClassLoader : java.lang.ClassLoader
    {

        public java.lang.Class Define(string name, byte[] bytes)
        {
            return defineClass(name, bytes, 0, bytes.Length);
        }

    }

    /// <summary>
    /// Builds a test class. Every class gets a static method 'f()V' that can be called to produce an instruction that
    /// can throw, and a static method 'h(Ljava/lang/Throwable;)V' that consumes an exception.
    /// </summary>
    class TestClassBuilder
    {

        readonly ClassFileBuilder builder;

        public TestClassBuilder(string name)
        {
            Name = name;
            builder = new ClassFileBuilder(49, AccessFlag.Public, name, "java/lang/Object");
            AddMethod("f", "()V", 0, 0, (code, handlers) => code.Return());
            AddMethod("h", "(Ljava/lang/Throwable;)V", 0, 1, (code, handlers) => code.Return());
        }

        public string Name { get; }

        public ConstantBuilder Constants => builder.Constants;

        public MethodrefConstantHandle F => Constants.GetOrAddMethodref(Name, "f", "()V");

        public MethodrefConstantHandle H => Constants.GetOrAddMethodref(Name, "h", "(Ljava/lang/Throwable;)V");

        public ClassConstantHandle Class(string name) => Constants.GetOrAddClass(name);

        /// <summary>
        /// Adds a public static method.
        /// </summary>
        public void AddMethod(string name, string signature, ushort maxStack, ushort maxLocals, Action<CodeBuilder, List<(ushort Start, ushort End, ushort Handler, ClassConstantHandle CatchType)>> emit)
        {
            AddMethod(AccessFlag.Public | AccessFlag.Static, name, signature, maxStack, maxLocals, emit);
        }

        public void AddMethod(AccessFlag flags, string name, string signature, ushort maxStack, ushort maxLocals, Action<CodeBuilder, List<(ushort Start, ushort End, ushort Handler, ClassConstantHandle CatchType)>> emit)
        {
            var buffer = new BlobBuilder();
            var handlers = new List<(ushort Start, ushort End, ushort Handler, ClassConstantHandle CatchType)>();
            emit(new CodeBuilder(buffer), handlers);

            var attributes = new AttributeTableBuilder(builder.Constants);
            attributes.Code(maxStack, maxLocals, buffer, e =>
            {
                foreach (var h in handlers)
                    e = e.Exception(h.Start, h.End, h.Handler, h.CatchType);
            }, null);

            builder.AddMethod(flags, name, signature, attributes);
        }

        /// <summary>
        /// Adds a field.
        /// </summary>
        public void AddField(AccessFlag flags, string name, string descriptor)
        {
            builder.AddField(flags, name, descriptor, new AttributeTableBuilder(builder.Constants));
        }

        /// <summary>
        /// Adds a public default constructor.
        /// </summary>
        public void AddDefaultConstructor()
        {
            AddMethod(AccessFlag.Public, "<init>", "()V", 1, 1, (code, handlers) => code
                .Aload0()
                .InvokeSpecial(Constants.GetOrAddMethodref("java/lang/Object", "<init>", "()V"))
                .Return());
        }

        public byte[] ToArray()
        {
            var buffer = new BlobBuilder();
            builder.Serialize(buffer);
            return buffer.ToArray();
        }

        public LoadedClass Load()
        {
            return LoadedClass.Load(Name, ToArray());
        }

    }

    /// <summary>
    /// A class that has been defined in the runtime, along with a separately parsed copy of its class file that the
    /// analyzer is free to patch.
    /// </summary>
    sealed class LoadedClass
    {

        public static LoadedClass Load(string name, byte[] bytes)
        {
            var javaClass = new ByteArrayClassLoader().Define(name, bytes);
            var type = RuntimeJavaType.FromClass(javaClass);
            var classFile = new ClassFile(new RuntimeJvmTypeSystem(JVM.Context), IKVM.ByteCode.Decoding.ClassFile.Read(bytes), name, type.ClassLoader.ClassFileParseOptions, []);
            classFile.Link(type, LoadMode.Link);
            return new LoadedClass(javaClass, type, classFile);
        }

        LoadedClass(java.lang.Class javaClass, RuntimeJavaType type, ClassFile classFile)
        {
            JavaClass = javaClass;
            Type = type;
            ClassFile = classFile;
        }

        public java.lang.Class JavaClass { get; }

        public RuntimeJavaType Type { get; }

        public ClassFile ClassFile { get; }

        /// <summary>
        /// Gets the compiled .NET type. This compiles every method of the class.
        /// </summary>
        public Type GetRuntimeType()
        {
            return global::ikvm.runtime.Util.getRuntimeTypeFromClass(JavaClass);
        }

        public AnalyzedMethod GetMethod(string name, string signature)
        {
            signature = signature.Replace('/', '.');
            var method = ClassFile.Methods.Single(i => i.Name == name && i.Signature == signature);
            var javaMethod = Type.GetMethod(name, signature, false);
            javaMethod.Link();
            return new AnalyzedMethod(this, method, javaMethod);
        }

    }

    /// <summary>
    /// A method of a <see cref="LoadedClass"/> and the results of analyzing it.
    /// </summary>
    sealed class AnalyzedMethod
    {

        public AnalyzedMethod(LoadedClass clazz, Method method, RuntimeJavaMethod javaMethod)
        {
            Class = clazz;
            Method = method;
            JavaMethod = javaMethod;
        }

        public LoadedClass Class { get; }

        public Method Method { get; }

        public RuntimeJavaMethod JavaMethod { get; }

        public Instruction[] Instructions => Method.Instructions;

        public CodeInfo CodeInfo { get; private set; }

        public UntangledExceptionTable Exceptions { get; private set; }

        public List<string> Errors { get; private set; }

        /// <summary>
        /// Creates the analyzer, which verifies the method.
        /// </summary>
        public MethodAnalyzer CreateAnalyzer()
        {
            return JVM.Context.MethodAnalyzerFactory.Create(Class.Type, Class.Type, JavaMethod, Class.ClassFile, Method, Class.Type.ClassLoader);
        }

        /// <summary>
        /// Runs the analysis in the same order as the compiler.
        /// </summary>
        public AnalyzedMethod Analyze()
        {
            var analyzer = CreateAnalyzer();
            Exceptions = MethodAnalyzer.UntangleExceptionBlocks(JVM.Context, Class.ClassFile, Method);
            CodeInfo = analyzer.GetCodeInfoAndErrors(Exceptions, out var errors);
            Errors = errors;
            return this;
        }

        /// <summary>
        /// Runs the local variable analysis the compiler performs after <see cref="Analyze"/>.
        /// </summary>
        public LocalVarInfo AnalyzeLocals()
        {
            return new LocalVarInfo(CodeInfo, Class.ClassFile, Method, Exceptions, JavaMethod, Class.Type.ClassLoader);
        }

        public InstructionFlags[] Reachability(int initialInstructionIndex = 0, bool skipFaultBlocks = false)
        {
            return MethodAnalyzer.ComputePartialReachability(CodeInfo, Instructions, Exceptions, initialInstructionIndex, skipFaultBlocks);
        }

        public (int Start, int End, int Handler, int Ordinal)[] ExceptionEntries()
        {
            var l = new (int, int, int, int)[Exceptions.Length];
            for (int i = 0; i < l.Length; i++)
                l[i] = (Exceptions[i].StartIndex, Exceptions[i].EndIndex, Exceptions[i].HandlerIndex, Exceptions[i].Ordinal);

            return l;
        }

        /// <summary>
        /// Describes the verifier state on entry to each instruction, as <c>stack|locals</c> with the stack listed from
        /// bottom to top, or <c>-</c> for an instruction that has no state. This is the only part of the tests that knows
        /// how the analyzer represents verification types, so that the tests survive changes to that representation.
        /// </summary>
        public string[] Frames()
        {
            var frames = new string[Instructions.Length];
            for (int pc = 0; pc < frames.Length; pc++)
            {
                if (CodeInfo.HasState(pc) == false)
                {
                    frames[pc] = "-";
                    continue;
                }

                var height = CodeInfo.GetStackHeight(pc);
                var stack = new string[height];
                for (int pos = 0; pos < height; pos++)
                {
                    var name = FormatType(CodeInfo.GetRawStackTypeWrapper(pc, pos));
                    if (CodeInfo.IsStackTypeExtendedDouble(pc, pos) || CodeInfo.IsStackTypeExtendedFloat(pc, pos))
                        name += "!";

                    stack[height - 1 - pos] = name;
                }

                var locals = new string[Method.MaxLocals];
                for (int i = 0; i < locals.Length; i++)
                    locals[i] = FormatType(CodeInfo.GetLocalTypeWrapper(pc, i));

                frames[pc] = string.Join(",", stack) + "|" + string.Join(",", locals);
            }

            return frames;
        }

        static string FormatType(RuntimeJavaType type)
        {
            var context = JVM.Context;
            if (type == context.VerifierJavaTypeFactory.Invalid)
                return "top";
            if (type == context.PrimitiveJavaTypeFactory.INT)
                return "I";
            if (type == context.PrimitiveJavaTypeFactory.LONG)
                return "J";
            if (type == context.PrimitiveJavaTypeFactory.FLOAT)
                return "F";
            if (type == context.PrimitiveJavaTypeFactory.DOUBLE)
                return "D";
            if (type == context.VerifierJavaTypeFactory.Null)
                return "null";
            if (type == context.VerifierJavaTypeFactory.UninitializedThis)
                return "uninitThis";
            if (RuntimeVerifierJavaType.IsNew(type))
                return "new@" + ((RuntimeVerifierJavaType)type).Index + ":" + ((RuntimeVerifierJavaType)type).UnderlyingType.Name;
            if (RuntimeVerifierJavaType.IsThis(type))
                return "this:" + ((RuntimeVerifierJavaType)type).UnderlyingType.Name;
            if (RuntimeVerifierJavaType.IsFaultBlockException(type))
                return "fault@" + ((RuntimeVerifierJavaType)type).Index;
            if (type.IsUnloadable)
                return "unloadable:" + type.Name;
            if (type.IsPrimitive)
                throw new System.InvalidOperationException($"Unexpected primitive {type.SignatureName} in verifier state.");

            return type.Name;
        }

    }

}
