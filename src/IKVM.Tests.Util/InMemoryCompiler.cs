using System;
using System.Collections.Concurrent;
using System.IO;

using java.io;
using java.lang;
using java.util;

using javax.tools;

namespace IKVM.Tests.Util
{

    /// <summary>
    /// Provides methods to compile a set of sources into a Java classes.
    /// </summary>
    public class InMemoryCompiler
    {

        readonly ConcurrentDictionary<string, ByteArrayOutputStream> streams = new();

        /// <summary>
        /// Captures the output files from the compilation process.
        /// </summary>
        class InMemoryForwardingJavaFileManager : ForwardingJavaFileManager
        {

            /// <summary>
            /// Class loader which reads from the output class files.
            /// </summary>
            class InMemoryForwardingJavaFileManagerClassLoader : ClassLoader
            {

                readonly ConcurrentDictionary<string, ByteArrayOutputStream> streams;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="streams"></param>
                public InMemoryForwardingJavaFileManagerClassLoader(ConcurrentDictionary<string, ByteArrayOutputStream> streams)
                {
                    this.streams = streams ?? throw new ArgumentNullException(nameof(streams));
                }

                /// <summary>
                /// Finds the class with the specified name.
                /// </summary>
                /// <param name="className"></param>
                /// <returns></returns>
                protected override Class findClass(string className)
                {
                    if (streams.TryGetValue(className, out var stream))
                    {
                        var b = stream.toByteArray();
                        return defineClass(className, b, 0, b.Length);
                    }

                    return null;
                }

            }

            /// <summary>
            /// Represents an output file to be written into.
            /// </summary>
            class InMemoryClassFileObject : SimpleJavaFileObject
            {

                readonly ConcurrentDictionary<string, ByteArrayOutputStream> streams;
                readonly string className;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="streams"></param>
                public InMemoryClassFileObject(ConcurrentDictionary<string, ByteArrayOutputStream> streams, string className, JavaFileObject.Kind kind) :
                    base(java.net.URI.create("string:///" + className.Replace('.', '/') + kind), kind)
                {
                    this.streams = streams ?? throw new ArgumentNullException(nameof(streams));
                    this.className = className ?? throw new ArgumentNullException(nameof(className));
                }

                /// <summary>
                /// Gets the output stream into which to write the class file.
                /// </summary>
                /// <returns></returns>
                public override OutputStream openOutputStream()
                {
                    return streams.GetOrAdd(className, _ => new ByteArrayOutputStream());
                }

            }

            readonly ConcurrentDictionary<string, ByteArrayOutputStream> streams;
            readonly ClassLoader classLoader;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="manager"></param>
            /// <param name="streams"></param>
            public InMemoryForwardingJavaFileManager(JavaFileManager manager, ConcurrentDictionary<string, ByteArrayOutputStream> streams) :
                base(manager)
            {
                this.streams = streams ?? throw new ArgumentNullException(nameof(streams));
                this.classLoader = new InMemoryForwardingJavaFileManagerClassLoader(streams);
            }

            /// <summary>
            /// Gets the class loader capable of loading generated classes.
            /// </summary>
            /// <param name="location"></param>
            /// <returns></returns>
            public override ClassLoader getClassLoader(JavaFileManager.Location location)
            {
                return classLoader;
            }

            /// <summary>
            /// Gets the file object into which to output the given class.
            /// </summary>
            /// <param name="location"></param>
            /// <param name="className"></param>
            /// <param name="kind"></param>
            /// <param name="sibling"></param>
            /// <returns></returns>
            public override JavaFileObject getJavaFileForOutput(JavaFileManager.Location location, string className, JavaFileObject.Kind kind, FileObject sibling)
            {
                return new InMemoryClassFileObject(streams, className, kind);
            }

        }

        /// <summary>
        /// Represents an input file to be read from.
        /// </summary>
        class InMemorySourceFileObject : SimpleJavaFileObject
        {

            readonly InMemoryCodeUnit unit;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="uri"></param>
            public InMemorySourceFileObject(InMemoryCodeUnit unit, java.net.URI uri) :
                base(uri, JavaFileObject.Kind.SOURCE)
            {
                this.unit = unit ?? throw new ArgumentNullException(nameof(unit));
            }

            /// <summary>
            /// Gets the character content of the source file.
            /// </summary>
            /// <param name="ignoreEncodingErrors"></param>
            /// <returns></returns>
            public override CharSequence getCharContent(bool ignoreEncodingErrors)
            {
                return unit.Code;
            }

        }

        readonly InMemoryCodeUnit[] units;
        readonly JavaFileManager files;
        readonly JavaCompiler compiler;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="source"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public InMemoryCompiler(InMemoryCodeUnit[] source)
        {
            this.compiler = ToolProvider.getSystemJavaCompiler() ?? throw new System.Exception();

            this.units = source ?? throw new ArgumentNullException(nameof(source));
            this.files = new InMemoryForwardingJavaFileManager(compiler.getStandardFileManager(null, null, null), streams);
        }

        /// <summary>
        /// Compiles the added java code in memory.
        /// </summary>
        public void Compile()
        {
            // add each unit as a source item
            var l = new java.util.ArrayList();
            foreach (var unit in units)
            {
                var uri = java.net.URI.create("string:///" + unit.Name.Replace('.', '/') + JavaFileObject.Kind.SOURCE.extension);
                var src = new InMemorySourceFileObject(unit, uri);
                l.add(src);
            }

            // get compiler and invoke
            if (l.size() > 0)
            {
                var o = new ArrayList();
                o.add("-verbose");
                o.add("-g");

                var d = new DiagnosticCollector();
                var s = compiler.getTask(null, files, d, o, null, l).call();
                if (s.booleanValue() == false)
                {
                    var m = new StringBuilder();
                    var a = d.getDiagnostics();
                    for (int i = 0; i < a.size(); i++)
                        m.append(((Diagnostic)a.get(i)).getMessage(Locale.US));

                    throw new System.Exception(m.toString());
                }
            }
        }

        /// <summary>
        /// Gets the compiled class with the given name.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        /// <exception cref="java.lang.ClassNotFoundException"></exception>
        public Class GetClass(string className)
        {
            var cld = files.getClassLoader(null);
            var cls = cld.loadClass(className);
            if (cls == null)
                throw new ClassNotFoundException("Class returned by ClassLoader was null!");

            return cls;
        }

        /// <summary>
        /// Writes the compiled classes to a JAR.
        /// </summary>
        /// <param name="path"></param>
        public void WriteJar(string path)
        {
            using var jar = System.IO.File.OpenWrite(path);
            WriteJar(jar);
        }

        /// <summary>
        /// Writes the compiled classes to a JAR on the given stream.
        /// </summary>
        /// <param name="stream"></param>
        public void WriteJar(Stream stream)
        {
            // write to temporary stream
            var buf = new ByteArrayOutputStream();
            WriteJar(buf);
            stream.Write(buf.toByteArray(), 0, buf.size());
        }

        /// <summary>
        /// Writes the compiled classes to a JAR on the given stream.
        /// </summary>
        /// <param name="stream"></param>
        public void WriteJar(OutputStream stream)
        {
            var man = new java.util.jar.Manifest();
            man.getMainAttributes().put(java.util.jar.Attributes.Name.MANIFEST_VERSION, "1.0");
            var jar = new java.util.jar.JarOutputStream(stream, man);
            WriteJar(jar);
            jar.close();
        }

        /// <summary>
        /// Writes the compiled classes to a JAR.
        /// </summary>
        /// <param name="jar"></param>
        public void WriteJar(java.util.jar.JarOutputStream jar)
        {
            foreach (var i in streams)
            {
                var e = new java.util.jar.JarEntry(i.Key.Replace(".", "/") + ".class");
                jar.putNextEntry(e);
                i.Value.writeTo(jar);
                jar.closeEntry();
            }
        }

    }

}