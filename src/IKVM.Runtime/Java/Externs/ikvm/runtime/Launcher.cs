using System;
using System.Collections;
using System.Collections.Generic;

using IKVM.Internal;

namespace IKVM.Java.Externs.ikvm.runtime
{

    /// <summary>
    /// Implements the backing support for <see cref="ikvm.runtime.Launcher"/>.
    /// </summary>
    static class Launcher
    {

        /// <summary>
        /// Implements the backing support for <see cref="global::ikvm.runtime.Launcher.run(Type, string[], string, global::java.util.Properties)"/>.
        /// </summary>
        /// <param name="main"></param>
        /// <param name="args"></param>
        /// <param name="jvmArgPrefix"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static int run(Type main, string[] args, string jvmArgPrefix, global::java.util.Properties properties)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (main is null)
                throw new ArgumentNullException(nameof(main));
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            // statically compiled entry points need to be explicitly added to the classpath
            // this is sort of a hack and should be removed with a better class loader hierarchy
            if (main.Assembly.IsDynamic == false)
                ClassLoaderWrapper.GetBootstrapClassLoader().AddDelegate(IKVM.Internal.AssemblyClassLoader.FromAssembly(main.Assembly));

            // copy properties to a CLR type
            var p = new Dictionary<string, string>();
            if (properties != null)
                foreach (global::java.util.Map.Entry entry in (IEnumerable)properties.entrySet())
                    p.Add((string)entry.getKey(), (string)entry.getValue());

            return IKVM.Runtime.Launcher.Run(((global::java.lang.Class)main).getName(), false, args, jvmArgPrefix, p);
#endif
        }

    }

}
