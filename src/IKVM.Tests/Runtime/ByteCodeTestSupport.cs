using System;
using System.Collections.Generic;
using System.Linq;

using IKVM.ByteCode;
using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Encoding;
using IKVM.Runtime;

using InstructionFlags = IKVM.Runtime.ClassFile.Method.InstructionFlags;

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
            var classFile = new IKVM.Runtime.ClassFile(JVM.Context, JVM.Context.Diagnostics, IKVM.ByteCode.Decoding.ClassFile.Read(bytes), name, type.ClassLoader.ClassFileParseOptions, null);
            classFile.Link(type, LoadMode.Link);
            return new LoadedClass(javaClass, type, classFile);
        }

        LoadedClass(java.lang.Class javaClass, RuntimeJavaType type, IKVM.Runtime.ClassFile classFile)
        {
            JavaClass = javaClass;
            Type = type;
            ClassFile = classFile;
        }

        public java.lang.Class JavaClass { get; }

        public RuntimeJavaType Type { get; }

        public IKVM.Runtime.ClassFile ClassFile { get; }

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

        public AnalyzedMethod(LoadedClass clazz, IKVM.Runtime.ClassFile.Method method, RuntimeJavaMethod javaMethod)
        {
            Class = clazz;
            Method = method;
            JavaMethod = javaMethod;
        }

        public LoadedClass Class { get; }

        public IKVM.Runtime.ClassFile.Method Method { get; }

        public RuntimeJavaMethod JavaMethod { get; }

        public IKVM.Runtime.ClassFile.Method.Instruction[] Instructions => Method.Instructions;

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
                l[i] = (Exceptions[i].startIndex, Exceptions[i].endIndex, Exceptions[i].handlerIndex, Exceptions[i].ordinal);

            return l;
        }

    }

}
