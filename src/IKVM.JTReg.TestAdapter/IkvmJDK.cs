using System;

using com.sun.javatest.regtest.agent;
using com.sun.javatest.regtest.config;

using java.io;
using java.lang;
using java.util;

using static com.sun.javatest.regtest.agent.GetJDKProperties;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// Custom <see cref="JDK"/> implementation suitable for IKVM implementation.
    /// </summary>
    class IkvmJDK : JDK
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="jdk"></param>
        public IkvmJDK(File jdk) : base(jdk)
        {

        }

        public override JDK_Version getVersion(RegressionParameters @params)
        {
            return JDK_Version.forThisJVM();
        }

        public override Set getDefaultModules(RegressionParameters @params)
        {
            return base.getDefaultModules(@params);
        }

        public override Set getSystemModules(RegressionParameters @params)
        {
            return base.getSystemModules(@params);
        }

        public override Properties getProperties(RegressionParameters @params)
        {
            var p = new Properties();
            p.putAll(java.lang.System.getProperties());
            return p;
        }

    }

}