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
            setup.ApplicationBase = Path.GetDirectoryName(typeof(JTRegTestManager).Assembly.Location);
            setup.ConfigurationFile = typeof(JTRegTestManager).Assembly.Location + ".config";
            appdomain = AppDomain.CreateDomain("JTReg", null, setup);
        }

#endif

        public static JTRegTestManager CreateManager()
        {
#if NETFRAMEWORK
            return (JTRegTestManager)appdomain.CreateInstanceAndUnwrap(typeof(JTRegTestManager).Assembly.FullName, typeof(JTRegTestManager).FullName);
#else
            return new JTRegTestManager();
#endif
        }

    }

}
