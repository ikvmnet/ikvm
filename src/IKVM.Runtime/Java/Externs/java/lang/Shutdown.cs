using System;

namespace IKVM.Java.Externs.java.lang
{

    /// <summary>
    /// Implements the native methods for 'java.lang.Shutdown'.
    /// </summary>
    static class Shutdown
    {

        /// <summary>
        /// Implements the native method 'beforeHalt'.
        /// </summary>
        public static void beforeHalt()
        {

        }

        /// <summary>
        /// Implements the native method 'halt0'.
        /// </summary>
        public static void halt0(int status)
        {
            Environment.Exit(status);
        }

        /// <summary>
        /// Implements the native method 'runAllFinalizers'.
        /// </summary>
        public static void runAllFinalizers()
        {

        }

    }

}
