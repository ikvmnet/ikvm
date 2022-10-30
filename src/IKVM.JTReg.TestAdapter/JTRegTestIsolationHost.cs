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

#if NETFRAMEWORK
        AppDomain appdomain;
#endif

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        public JTRegTestIsolationHost(string source)
        {
#if NETFRAMEWORK
            var setup = new AppDomainSetup();
            setup.ApplicationBase = Path.GetDirectoryName(typeof(JTRegTestManagerProxy).Assembly.Location);
            setup.ConfigurationFile = File.Exists(source + ".config") ? source + ".config" : typeof(JTRegTestManagerProxy).Assembly.Location + ".config";
            appdomain = AppDomain.CreateDomain($"JTRegTestAdapter::{source}", null, setup);
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
                AppDomain.Unload(appdomain);
                appdomain = null;
            }
#endif
        }

    }

}
