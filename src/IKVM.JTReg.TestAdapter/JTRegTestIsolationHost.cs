using System;
using System.IO;

using IKVM.JTReg.TestAdapter.Core;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// Manages the isolation requirements for the services of the JTReg test adapter.
    /// </summary>
    static class JTRegTestIsolationHost
    {

#if NETFRAMEWORK

        static readonly AppDomain appdomain;

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static JTRegTestIsolationHost()
        {
            var setup = new AppDomainSetup();
            setup.ApplicationBase = Path.GetDirectoryName(typeof(JTRegTestManagerProxy).Assembly.Location);
            setup.ConfigurationFile = typeof(JTRegTestManagerProxy).Assembly.Location + ".config";
            appdomain = AppDomain.CreateDomain("JTReg", null, setup);
        }

#endif

        public static IJTRegTestManager CreateManager()
        {
#if NETFRAMEWORK
            return (IJTRegTestManager)appdomain.CreateInstanceAndUnwrap(typeof(JTRegTestManagerProxy).Assembly.FullName, typeof(JTRegTestManagerProxy).FullName);
#else
            return new JTRegTestManager();
#endif
        }

    }

}
