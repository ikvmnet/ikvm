using System;
using System.IO;

namespace IKVM.Runtime
{

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

    /// <summary>
    /// Required native methods available in libjava.
    /// </summary>
    /// <remarks>
    /// OpenJDK's 'libjava' is named 'libiava' in IKVM due to the potential module name overlap on Windows, which loads
    /// .NET assemblies using LoadLibrary. The 'java.exe' application ends up being a Module, which conflicts with libjava.
    /// </remarks>
    internal unsafe class LibJava
    {

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        public static readonly LibJava Instance = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        LibJava()
        {
            // load libiava through libjvm as it is a JNI library
            if ((Handle = LibJvm.Instance.JVM_LoadLibrary(Path.Combine(JVM.Properties.HomePath, "bin", NativeLibrary.MapLibraryName("iava")))) == 0)
                throw new InternalException("Could not load libiava.");
        }

        /// <summary>
        /// Gets a handle to the loaded libjvm library.
        /// </summary>
        public nint Handle { get; private set; }

        /// <summary>
        /// Finalizes the instance.
        /// </summary>
        ~LibJava()
        {
            if (Environment.HasShutdownStarted == false)
            {
                if (Handle != 0)
                {
                    var h = Handle;
                    Handle = 0;
                    LibJvm.Instance.JVM_UnloadLibrary(h);
                }
            }
        }

    }

#endif

}
