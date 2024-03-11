using System;
using System.Diagnostics;

namespace IKVM.Runtime.Accessors.Java.Lang
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.lang.Win32Process' type.
    /// </summary>
    internal sealed class Win32ProcessAccessor : Accessor<object>
    {

        MethodAccessor<Func<Process, object, object, object, object>> init;
        FieldAccessor<object, Process> process;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public Win32ProcessAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.lang.Win32Process")
        {

        }

        public object Init(Process process, object outputStream, object inputStream, object errorStream) => GetConstructor(ref init, typeof(Process), Resolve("java.io.OutputStream"), Resolve("java.io.InputStream"), Resolve("java.io.InputStream")).Invoker(process, outputStream, inputStream, errorStream);

        public Process GetProcess(object self) => GetField(ref process, nameof(process)).GetValue(self);

    }

#endif

}
