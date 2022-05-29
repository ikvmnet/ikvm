using System;
using System.Collections.Concurrent;

namespace IKVM.Tests.JNI
{
    /// <summary>
    /// Provides methods to compile a set of sources into a Java classes.
    /// </summary>
    class InMemoryCompiler
    {

        /// <summary>
        /// Captures the output files from the compilation process.
        /// </summary>
        class InMemoryForwardingJavaFileManager : global::javax.tools.ForwardingJavaFileManager
        {

            readonly ConcurrentDictionary<string, global::java.io.ByteArrayOutputStream> classes = new();

            /// <summary>
            /// Class loader which reads from the output class files.
            /// </summary>
            class InMemoryForwardingJavaFileManagerClassLoader : global::java.lang.ClassLoader
            {

                readonly ConcurrentDictionary<string, global::java.io.ByteArrayOutputStream> classes;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="classes"></param>
                public InMemoryForwardingJavaFileManagerClassLoader(ConcurrentDictionary<string, global::java.io.ByteArrayOutputStream> classes)
                {
                    this.classes = classes ?? throw new ArgumentNullException(nameof(classes));
                }

                /// <summary>
                /// Finds the class with the specified name.
                /// </summary>
                /// <param name="className"></param>
                /// <returns></returns>
                protected override global::java.lang.Class findClass(string className)
                {
                    if (classes.TryGetValue(className, out var bos))
                    {
                        var b = bos.toByteArray();
                        return base.defineClass(className, b, 0, b.Length);
                    }

                    return null;
                }

            }

            /// <summary>
            /// Represents an output file to be written into.
            /// </summary>
            class InMemoryClassFileObject : global::javax.tools.SimpleJavaFileObject
            {

                readonly ConcurrentDictionary<string, global::java.io.ByteArrayOutputStream> classes;
                readonly string className;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="files"></param>
                public InMemoryClassFileObject(ConcurrentDictionary<string, global::java.io.ByteArrayOutputStream> files, string className, global::javax.tools.JavaFileObject.Kind kind) :
                    base(global::java.net.URI.create("string:///" + className.Replace('.', '/') + kind), kind)
                {
                    this.classes = files ?? throw new ArgumentNullException(nameof(files));
                    this.className = className ?? throw new ArgumentNullException(nameof(className));
                }

                /// <summary>
                /// Gets the output stream into which to write the class file.
                /// </summary>
                /// <returns></returns>
                public override global::java.io.OutputStream openOutputStream()
                {
                    return classes.GetOrAdd(className, _ => new global::java.io.ByteArrayOutputStream());
                }

            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="manager"></param>
            public InMemoryForwardingJavaFileManager(global::javax.tools.JavaFileManager manager) :
                base(manager)
            {

            }

            /// <summary>
            /// Gets the class loader capable of loading generated classes.
            /// </summary>
            /// <param name="location"></param>
            /// <returns></returns>
            public override global::java.lang.ClassLoader getClassLoader(global::javax.tools.JavaFileManager.Location location)
            {
                return new InMemoryForwardingJavaFileManagerClassLoader(classes);
            }

            /// <summary>
            /// Gets the file object into which to output the given class.
            /// </summary>
            /// <param name="location"></param>
            /// <param name="className"></param>
            /// <param name="kind"></param>
            /// <param name="sibling"></param>
            /// <returns></returns>
            public override global::javax.tools.JavaFileObject getJavaFileForOutput(global::javax.tools.JavaFileManager.Location location, string className, global::javax.tools.JavaFileObject.Kind kind, global::javax.tools.FileObject sibling)
            {
                return new InMemoryClassFileObject(classes, className, kind);
            }

        }

        /// <summary>
        /// Represents an input file to be read from.
        /// </summary>
        class InMemorySourceFileObject : global::javax.tools.SimpleJavaFileObject
        {

            readonly InMemoryCodeUnit unit;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="uri"></param>
            public InMemorySourceFileObject(InMemoryCodeUnit unit, global::java.net.URI uri) :
                base(uri, global::javax.tools.JavaFileObject.Kind.SOURCE)
            {
                this.unit = unit ?? throw new ArgumentNullException(nameof(unit));
            }

            /// <summary>
            /// Gets the character content of the source file.
            /// </summary>
            /// <param name="ignoreEncodingErrors"></param>
            /// <returns></returns>
            public override global::java.lang.CharSequence getCharContent(bool ignoreEncodingErrors)
            {
                return unit.Code;
            }

        }

        readonly InMemoryCodeUnit[] units;
        readonly global::javax.tools.JavaFileManager files;
        readonly global::javax.tools.JavaCompiler compiler;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="source"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public InMemoryCompiler(InMemoryCodeUnit[] source)
        {
            this.compiler = global::javax.tools.ToolProvider.getSystemJavaCompiler() ?? throw new Exception();

            this.units = source ?? throw new ArgumentNullException(nameof(source));
            this.files = new InMemoryForwardingJavaFileManager(compiler.getStandardFileManager(null, null, null));
        }

        /// <summary>
        /// Compiles the added java code in memory.
        /// </summary>
        public void Compile()
        {
            // add each unit as a source item
            var l = new global::java.util.ArrayList();
            foreach (var unit in units)
            {
                var uri = global::java.net.URI.create("string:///" + unit.Name.Replace('.', '/') + global::javax.tools.JavaFileObject.Kind.SOURCE.extension);
                var src = new InMemorySourceFileObject(unit, uri);
                l.add(src);
            }

            // get compiler and invoke
            if (l.size() > 0)
            {
                var d = new global::javax.tools.DiagnosticCollector();
                var s = compiler.getTask(null, files, d, null, null, l).call();
                if (s.booleanValue() == false)
                    throw new Exception();
            }
        }

        /// <summary>
        /// Gets the compiled class with the given name.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        /// <exception cref="ClassNotFoundException"></exception>
        public global::java.lang.Class getCompiledClass(string className)
        {
            var cld = files.getClassLoader(null);
            var cls = cld.loadClass(className);
            if (cls == null)
                throw new ClassNotFoundException("Class returned by ClassLoader was null!");

            return cls;
        }

    }

}
