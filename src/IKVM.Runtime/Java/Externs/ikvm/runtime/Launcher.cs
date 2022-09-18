using System;
using System.Collections;
using System.Collections.Generic;

namespace IKVM.Java.Externs.ikvm.runtime
{

    /// <summary>
    /// Implements the backing support for <see cref="ikvm.runtime.Launcher"/>.
    /// </summary>
    static class Launcher
    {

        /// <summary>
        /// Implements the backing support for <see cref="ikvm.runtime.Launcher.run(java.lang.Class, string[], string, java.util.Properties)"/>.
        /// </summary>
        /// <param name="main"></param>
        /// <param name="args"></param>
        /// <param name="jvmArgPrefix"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static int run(global::java.lang.Class main, string[] args, string jvmArgPrefix, global::java.util.Properties properties)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (main is null)
                throw new ArgumentNullException(nameof(main));
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            // copy properties to a CLR type
            var p = new Dictionary<string, string>();
            if (properties != null)
                foreach (global::java.util.Map.Entry entry in (IEnumerable)properties.entrySet())
                    p.Add((string)entry.getKey(), (string)entry.getValue());

            return IKVM.Runtime.Launcher.Run(main.getName(), false, args, jvmArgPrefix, p);
#endif
        }

    }

}
