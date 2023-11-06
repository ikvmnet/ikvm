using System;
using System.IO;

using IKVM.JTReg.TestAdapter.Core;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// Manages the isolation requirements for the services of the JTReg test adapter.
    /// </summary>
    class JTRegTestIsolationHost : IDisposable
    {

        public static readonly JTRegTestIsolationHost Instance = new();

#if NETFRAMEWORK
        AppDomain appdomain;
#endif

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        public JTRegTestIsolationHost()
        {
#if NETFRAMEWORK
            var path = typeof(JTRegTestIsolationHost).Assembly.Location;
            var conf = Path.ChangeExtension(path, ".dll.config");

            var setup = new AppDomainSetup();
            setup.ApplicationBase = Path.GetDirectoryName(path);
            setup.ConfigurationFile = conf;
            appdomain = AppDomain.CreateDomain("JTRegTestAdapter", null, setup);
#endif
        }

        /// <summary>
        /// Creates a new <see cref="JTRegTestManagerProxy"/> for the given isolation host.
        /// </summary>
        /// <returns></returns>
        public JTRegTestManagerProxy CreateManager()
        {
#if NETFRAMEWORK
            return (JTRegTestManagerProxy)appdomain.CreateInstanceAndUnwrap(typeof(JTRegTestManagerProxy).Assembly.FullName, typeof(JTRegTestManagerProxy).FullName);
#else
            return new JTRegTestManagerProxy();
#endif
        }

        /// <summary>
        /// Disposes of the isolation host.
        /// </summary>
        public void Dispose()
        {
#if NETFRAMEWORK
            if (appdomain != null)
            {
                try
                {
                    AppDomain.Unload(appdomain);
                }
                catch
                {
                    // ignore any exceptions during unload
                }

                appdomain = null;
            }
#endif
        }

    }

}
