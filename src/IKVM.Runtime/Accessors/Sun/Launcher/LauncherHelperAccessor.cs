using System;

namespace IKVM.Runtime.Accessors.Sun.Launcher
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'sun.launcher.LauncherHelper' type.
    /// </summary>
    internal sealed class LauncherHelperAccessor : Accessor<object>
    {

        MethodAccessor<Func<bool, int, string, object>> checkAndLoadMain;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public LauncherHelperAccessor(AccessorTypeResolver resolver) :
            base(resolver, "sun.launcher.LauncherHelper")
        {

        }

        /// <summary>
        /// Invokes the 'checkAndLoadMain' method.
        /// </summary>
        /// <param name="printToStderr"></param>
        /// <param name="mode"></param>
        /// <param name="what"></param>
        /// <returns></returns>
        public object InvokeCheckAndLoadMain(bool printToStderr, int mode, string what) => GetMethod(ref checkAndLoadMain, nameof(checkAndLoadMain), Resolve("java.lang.Class"), typeof(bool), typeof(int), typeof(string)).Invoker(printToStderr, mode, what);

    }

#endif

}
